using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Ciudadania;
using eVote360Pro.Core.Application.Dtos.Elecciones;
using eVote360Pro.Core.Application.Helpers;
using eVote360Pro.Core.Application.Interfaces.Ciudadania;
using eVote360Pro.Core.Application.Interfaces.Elecciones;
using eVote360Pro.Core.Application.Interfaces.Infrastructure;
using eVote360Pro.Core.Application.ViewModels.Elector;
using eVote360Pro.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.Web.Controllers.Elector
{
    public class ElectorController : Controller
    {
        private readonly ICiudadanoService _ciudadanoService;
        private readonly IEleccionService _eleccionService;
        private readonly ICodigoVerificacionService _codigoVerificacionService;
        private readonly IVotoService _votoService;
        private readonly IEmailService _emailService;
        private readonly IOcrService _ocrService;
        private readonly IUploadService _uploadService;
        private readonly IMapper _mapper;

        public ElectorController(
            ICiudadanoService ciudadanoService,
            IEleccionService eleccionService,
            ICodigoVerificacionService codigoVerificacionService,
            IVotoService votoService,
            IEmailService emailService,
            IOcrService ocrService,
            IUploadService uploadService,
            IMapper mapper)
        {
            _ciudadanoService = ciudadanoService;
            _eleccionService = eleccionService;
            _codigoVerificacionService = codigoVerificacionService;
            _votoService = votoService;
            _emailService = emailService;
            _ocrService = ocrService;
            _uploadService = uploadService;
            _mapper = mapper;
        }

        public IActionResult ValidarIdentidad(string numeroDocumento, int ciudadanoId)
        {
            return View(new ValidarIdentidadViewModel { NumeroDocumento = numeroDocumento, CiudadanoId = ciudadanoId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidarIdentidad(ValidarIdentidadViewModel model)
        {
            if (model.ImagenCedula == null || model.ImagenCedula.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Debe subir una imagen de su cédula para validar su identidad.");
                return View(model);
            }

            var extension = Path.GetExtension(model.ImagenCedula.FileName);
            var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png" };
            if (string.IsNullOrWhiteSpace(extension) || !extensionesPermitidas.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(string.Empty, "El archivo seleccionado no tiene un formato de imagen válido.");
                return View(model);
            }

            if (!model.ImagenCedula.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(string.Empty, "El archivo seleccionado no tiene un formato de imagen válido.");
                return View(model);
            }

            var eleccionActiva = await _eleccionService.GetActiveElectionAsync();
            if (eleccionActiva == null)
            {
                TempData["Error"] = "No hay ningún proceso electoral en estos momentos.";
                return RedirectToAction("Index", "Home");
            }

            var ciudadano = await _ciudadanoService.GetByIdAsync(model.CiudadanoId);
            if (ciudadano == null)
            {
                TempData["Error"] = "No existe un ciudadano registrado con este número de documento.";
                return RedirectToAction("Index", "Home");
            }

            var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}{Path.GetExtension(model.ImagenCedula.FileName)}");
            await using (var fs = System.IO.File.Create(tempPath))
                await model.ImagenCedula.CopyToAsync(fs);

            var numeroExtraido = await _ocrService.ExtractDocumentNumberAsync(tempPath);
            System.IO.File.Delete(tempPath);

            if (string.IsNullOrWhiteSpace(numeroExtraido))
            {
                ModelState.AddModelError(string.Empty, "No fue posible leer correctamente el número de documento en la imagen cargada. Por favor, suba una imagen más clara.");
                return View(model);
            }

            if (!numeroExtraido.Trim().Equals(ciudadano.NumeroDocumento.Trim(), StringComparison.OrdinalIgnoreCase)
                && !numeroExtraido.Trim().Equals(model.NumeroDocumento.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(string.Empty, "Los datos extraídos de la foto no coinciden con los datos previamente ingresados por el elector.");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(ciudadano.CorreoElectronico))
            {
                TempData["Error"] = "Este ciudadano no tiene un correo electrónico registrado. No es posible continuar con la verificación de identidad.";
                return RedirectToAction("Index", "Home");
            }

            var codigo = await _codigoVerificacionService.GenerateCodeAsync(model.CiudadanoId, eleccionActiva.Id);
            if (codigo == null)
            {
                TempData["Error"] = "No fue posible generar el código de verificación. Intente nuevamente más tarde.";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                await _emailService.SendVerificationCodeAsync(ciudadano.CorreoElectronico, ciudadano.Nombre, codigo.Codigo);
            }
            catch
            {
                TempData["Error"] = "No fue posible enviar el código de verificación. Intente nuevamente más tarde.";
                return RedirectToAction("Index", "Home");
            }

            HttpContext.Session.SetInt32("CiudadanoId", model.CiudadanoId);
            HttpContext.Session.SetInt32("EleccionId", eleccionActiva.Id);
            HttpContext.Session.SetString("NumeroDocumento", model.NumeroDocumento);

            return RedirectToAction(nameof(VerificarCodigo));
        }

        public IActionResult VerificarCodigo()
        {
            var ciudadanoId = HttpContext.Session.GetInt32("CiudadanoId");
            if (!ciudadanoId.HasValue) return RedirectToAction("Index", "Home");

            return View(new VerificarCodigoViewModel
            {
                CiudadanoId = ciudadanoId.Value,
                NumeroDocumento = HttpContext.Session.GetString("NumeroDocumento") ?? string.Empty
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerificarCodigo(VerificarCodigoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var eleccionId = HttpContext.Session.GetInt32("EleccionId");
            if (!eleccionId.HasValue)
            {
                TempData["Error"] = "No hay ningún proceso electoral en estos momentos.";
                return RedirectToAction("Index", "Home");
            }

            var errorCodigo = await _codigoVerificacionService.ValidateCodeWithMessageAsync(
                model.CiudadanoId, eleccionId.Value, model.CodigoVerificacion);
            if (errorCodigo != null)
            {
                ModelState.AddModelError(string.Empty, errorCodigo);
                return View(model);
            }

            HttpContext.Session.SetString("CodigoValidado", "true");
            return RedirectToAction(nameof(PuestosElectivos));
        }

        public async Task<IActionResult> PuestosElectivos()
        {
            var errorFlujo = await ObtenerErrorFlujoActualAsync();
            if (errorFlujo != null)
            {
                TempData["Error"] = errorFlujo;
                return RedirectToAction("Index", "Home");
            }

            var eleccionId = HttpContext.Session.GetInt32("EleccionId")!.Value;
            var selecciones = HttpContext.Session.GetObject<Dictionary<int, int?>>("Selecciones") ?? new Dictionary<int, int?>();

            var puestos = await _eleccionService.GetPuestosVotacionAsync(eleccionId, selecciones);
            return View(_mapper.Map<List<PuestoElectivoVotacionViewModel>>(puestos));
        }

        public async Task<IActionResult> VotarPuesto(int puestoId)
        {
            var errorFlujo = await ObtenerErrorFlujoActualAsync();
            if (errorFlujo != null)
            {
                TempData["Error"] = errorFlujo;
                return RedirectToAction("Index", "Home");
            }

            var eleccionId = HttpContext.Session.GetInt32("EleccionId")!.Value;
            var selecciones = HttpContext.Session.GetObject<Dictionary<int, int?>>("Selecciones") ?? new Dictionary<int, int?>();
            var puestosDisponibles = await _eleccionService.GetPuestosVotacionAsync(eleccionId, selecciones);
            if (!puestosDisponibles.Any(p => p.Id == puestoId))
            {
                TempData["Error"] = "El puesto seleccionado no pertenece a la elección activa.";
                return RedirectToAction(nameof(PuestosElectivos));
            }

            int? seleccionActual = null;
            if (selecciones.TryGetValue(puestoId, out var valorActual))
                seleccionActual = valorActual ?? -1;

            var vm = await BuildVotarPuestoViewModelAsync(puestoId, eleccionId, seleccionActual);
            if (vm == null)
            {
                TempData["Error"] = "El puesto seleccionado no está disponible para votar.";
                return RedirectToAction(nameof(PuestosElectivos));
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VotarPuesto(VotarPuestoViewModel model)
        {
            var errorFlujo = await ObtenerErrorFlujoActualAsync();
            if (errorFlujo != null)
            {
                TempData["Error"] = errorFlujo;
                return RedirectToAction("Index", "Home");
            }

            var eleccionId = HttpContext.Session.GetInt32("EleccionId")!.Value;
            var selecciones = HttpContext.Session.GetObject<Dictionary<int, int?>>("Selecciones") ?? new Dictionary<int, int?>();
            var puestosDisponibles = await _eleccionService.GetPuestosVotacionAsync(eleccionId, selecciones);
            if (!puestosDisponibles.Any(p => p.Id == model.PuestoElectivoId))
            {
                TempData["Error"] = "El puesto seleccionado no pertenece a la elección activa.";
                return RedirectToAction(nameof(PuestosElectivos));
            }

            if (!model.CandidatoSeleccionadoId.HasValue)
            {
                ModelState.AddModelError(string.Empty, "Debe seleccionar un candidato antes de votar.");
                var vm = await BuildVotarPuestoViewModelAsync(model.PuestoElectivoId, eleccionId, model.CandidatoSeleccionadoId);
                return View(vm ?? model);
            }

            var candidatosDisponibles = await _eleccionService.GetCandidatosByPuestoAndEleccionAsync(model.PuestoElectivoId, eleccionId);
            var seleccionEsNinguno = model.CandidatoSeleccionadoId.Value <= 0;
            if (!seleccionEsNinguno && !candidatosDisponibles.Any(c => c.Id == model.CandidatoSeleccionadoId.Value))
            {
                ModelState.AddModelError(string.Empty, "La selección realizada no corresponde a un candidato válido para este puesto electivo.");
                var vm = await BuildVotarPuestoViewModelAsync(model.PuestoElectivoId, eleccionId, model.CandidatoSeleccionadoId);
                return View(vm ?? model);
            }

            selecciones[model.PuestoElectivoId] = model.CandidatoSeleccionadoId.Value <= 0 ? null : model.CandidatoSeleccionadoId;
            HttpContext.Session.SetObject("Selecciones", selecciones);

            TempData["Success"] = "Voto del puesto guardado correctamente.";
            return RedirectToAction(nameof(PuestosElectivos));
        }

        public IActionResult FinalizarVotacion()
        {
            if (!EsFlujoValido()) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinalizarVotacionConfirmar()
        {
            var errorFlujo = await ObtenerErrorFlujoActualAsync();
            if (errorFlujo != null)
            {
                TempData["Error"] = errorFlujo;
                return RedirectToAction("Index", "Home");
            }

            var ciudadanoId = HttpContext.Session.GetInt32("CiudadanoId")!.Value;
            var eleccionId = HttpContext.Session.GetInt32("EleccionId")!.Value;
            var selecciones = HttpContext.Session.GetObject<Dictionary<int, int?>>("Selecciones") ?? new Dictionary<int, int?>();

            var puestos = await _eleccionService.GetPuestosVotacionAsync(eleccionId, selecciones);
            var faltantes = puestos.Where(p => !selecciones.ContainsKey(p.Id)).Select(p => p.Nombre).ToList();
            if (faltantes.Count > 0)
            {
                TempData["Error"] = $"Debe completar su selección para los siguientes puestos electivos: {string.Join(", ", faltantes)}.";
                return RedirectToAction(nameof(PuestosElectivos));
            }

            var validationError = await _votoService.ValidateSelectionsAsync(ciudadanoId, eleccionId, selecciones);
            if (validationError != null)
            {
                TempData["Error"] = validationError;
                return RedirectToAction(nameof(PuestosElectivos));
            }

            var registrado = await _votoService.RegistrarVotoAsync(ciudadanoId, eleccionId, selecciones);
            if (!registrado)
            {
                TempData["Error"] = await _votoService.ValidateSelectionsAsync(ciudadanoId, eleccionId, selecciones)
                    ?? "No fue posible registrar su voto. Intente nuevamente.";
                return RedirectToAction(nameof(PuestosElectivos));
            }

            var ciudadano = await _ciudadanoService.GetByIdAsync(ciudadanoId);
            var eleccion = await _eleccionService.GetByIdAsync(eleccionId);
            var resumen = await _votoService.BuildResumenAsync(eleccionId, selecciones);

            if (ciudadano != null && eleccion != null && !string.IsNullOrWhiteSpace(ciudadano.CorreoElectronico))
            {
                try
                {
                    await _emailService.SendVotingSummaryAsync(
                        ciudadano.CorreoElectronico,
                        ciudadano.Nombre,
                        eleccion.Nombre,
                        eleccion.Fecha,
                        resumen.Select(r => (r.PuestoNombre, r.Seleccion, r.Partido)));
                }
                catch { /* no bloquear finalización */ }
            }

            HttpContext.Session.Clear();
            TempData["Success"] = "Su proceso de votación ha sido completado correctamente. Gracias por ejercer su derecho al voto.";
            return RedirectToAction("Index", "Home");
        }

        private bool EsFlujoValido()
        {
            return HttpContext.Session.GetInt32("CiudadanoId").HasValue
                && HttpContext.Session.GetInt32("EleccionId").HasValue
                && HttpContext.Session.GetString("CodigoValidado") == "true";
        }

        private async Task<string?> ObtenerErrorFlujoActualAsync()
        {
            if (!EsFlujoValido())
                return "Debe completar el proceso de validación de identidad antes de continuar.";

            var ciudadanoId = HttpContext.Session.GetInt32("CiudadanoId")!.Value;
            var eleccionId = HttpContext.Session.GetInt32("EleccionId")!.Value;

            var eleccionActiva = await _eleccionService.GetActiveElectionAsync();
            if (eleccionActiva == null || eleccionActiva.Id != eleccionId)
                return "No hay ningún proceso electoral en estos momentos.";

            var ciudadano = await _ciudadanoService.GetByIdAsync(ciudadanoId);
            if (ciudadano == null)
                return "No existe un ciudadano registrado con este número de documento.";

            if (!ciudadano.IsActive)
                return "Este ciudadano se encuentra inactivo y no puede participar en el proceso de votación.";

            if (await _ciudadanoService.HasVotedInElectionAsync(ciudadanoId, eleccionId))
                return "Ya ha ejercido su derecho al voto.";

            return null;
        }

        private async Task<VotarPuestoViewModel?> BuildVotarPuestoViewModelAsync(int puestoId, int eleccionId, int? candidatoSeleccionadoId = null)
        {
            var puesto = await _eleccionService.GetPuestoByIdAsync(puestoId);
            if (puesto == null)
                return null;

            var candidatos = await _eleccionService.GetCandidatosByPuestoAndEleccionAsync(puestoId, eleccionId);
            var vm = new VotarPuestoViewModel
            {
                PuestoElectivoId = puesto.Id,
                PuestoNombre = puesto.Nombre,
                Candidatos = _mapper.Map<List<CandidatoVotacionViewModel>>(candidatos),
                CandidatoSeleccionadoId = candidatoSeleccionadoId
            };

            vm.Candidatos.Add(new CandidatoVotacionViewModel
            {
                Id = -1,
                Nombre = "Ninguno",
                EsNinguno = true
            });

            return vm;
        }
    }
}

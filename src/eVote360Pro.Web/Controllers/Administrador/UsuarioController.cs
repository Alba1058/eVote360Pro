using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Usuarios;
using eVote360Pro.Core.Application.Interfaces.Elecciones;
using eVote360Pro.Core.Application.Interfaces.Partidos;
using eVote360Pro.Core.Application.Interfaces.Usuarios;
using eVote360Pro.Core.Application.ViewModels.Administrador;
using eVote360Pro.Core.Application.ViewModels.Usuarios;
using eVote360Pro.Core.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.Web.Controllers.Administrador
{
    [Authorize(Policy = "Administrador")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IPartidoPoliticoService _partidoPoliticoService;
        private readonly IEleccionService _eleccionService;
        private readonly IMapper _mapper;

        public UsuarioController(
            IUsuarioService usuarioService,
            IPartidoPoliticoService partidoPoliticoService,
            IEleccionService eleccionService,
            IMapper mapper)
        {
            _usuarioService = usuarioService;
            _partidoPoliticoService = partidoPoliticoService;
            _eleccionService = eleccionService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.HayEleccionActiva = await _eleccionService.HasActiveElectionAsync();
            return View(_mapper.Map<List<UsuarioViewModel>>(await _usuarioService.GetAllAsync()));
        }

        public async Task<IActionResult> Create()
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede crear un usuario mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            return View(new SaveUsuarioViewModel { IsActive = true, Rol = RolUsuario.Administrador });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveUsuarioViewModel model)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede crear un usuario mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            if (string.IsNullOrWhiteSpace(model.Contrasena))
                ModelState.AddModelError(nameof(model.Contrasena), "La contraseña es requerida.");
            if (!ModelState.IsValid) return View(model);

            if (await _usuarioService.ExistsByNombreUsuarioAsync(model.NombreUsuario))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un usuario registrado con este nombre de usuario.");
                return View(model);
            }

            if (await _usuarioService.ExistsByCorreoElectronicoAsync(model.CorreoElectronico))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un usuario registrado con este correo electrónico.");
                return View(model);
            }

            await _usuarioService.AddAsync(_mapper.Map<SaveUsuarioDto>(model));
            TempData["Success"] = "Usuario creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede editar un usuario mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            var dto = await _usuarioService.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(_mapper.Map<SaveUsuarioViewModel>(dto));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SaveUsuarioViewModel model)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede editar un usuario mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var current = await _usuarioService.GetByIdAsync(model.Id);
            if (current == null) return NotFound();

            var currentUserId = int.TryParse(User.FindFirst("UsuarioId")?.Value, out var uid) ? uid : 0;
            if (currentUserId == model.Id && (model.Rol != current.Rol || !model.IsActive))
            {
                ModelState.AddModelError(string.Empty, "No puede cambiar su propio rol ni desactivar su propio usuario mientras está autenticado.");
                return View(model);
            }

            if (current.Rol == RolUsuario.DirigentePolitico && current.PartidoPoliticoId.HasValue && model.Rol == RolUsuario.Administrador)
            {
                ModelState.AddModelError(string.Empty, "No se puede cambiar el rol de este usuario porque tiene un partido político asignado como dirigente.");
                return View(model);
            }

            if (current.Rol == RolUsuario.Administrador && model.Rol != RolUsuario.Administrador)
            {
                var admins = await _usuarioService.CountActiveAdminsAsync();
                if (admins <= 1 && current.IsActive)
                {
                    ModelState.AddModelError(string.Empty, "No se puede modificar este usuario porque es el único administrador activo del sistema.");
                    return View(model);
                }
            }

            // Validar contraseña solo si se quiere cambiar
            if (model.CambiarContrasena)
            {
                if (string.IsNullOrWhiteSpace(model.Contrasena))
                    ModelState.AddModelError(nameof(model.Contrasena), "La contraseña es requerida cuando se desea cambiar.");
            }

            if (!ModelState.IsValid) return View(model);

            if (await _usuarioService.ExistsByNombreUsuarioAsync(model.NombreUsuario, model.Id))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un usuario registrado con este nombre de usuario.");
                return View(model);
            }

            var existingUser = await _usuarioService.GetByNombreUsuarioAsync(model.NombreUsuario);
            if (existingUser != null && existingUser.Id != model.Id && existingUser.CorreoElectronico == model.CorreoElectronico)
            {
                ModelState.AddModelError(string.Empty, "Ya existe un usuario registrado con este correo electrónico.");
                return View(model);
            }

            var dto = _mapper.Map<SaveUsuarioDto>(model);
            if (!model.CambiarContrasena || string.IsNullOrWhiteSpace(model.Contrasena))
                dto.Contrasena = null!;
            await _usuarioService.UpdateAsync(dto);
            TempData["Success"] = "Usuario actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Activate(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede activar un usuario mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null) return NotFound();
            if (usuario.IsActive)
            {
                TempData["Error"] = "Este usuario ya se encuentra activo.";
                return RedirectToAction(nameof(Index));
            }
            return View("Activate", usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateConfirmed(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede activar un usuario mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null) return NotFound();
            if (usuario.IsActive)
            {
                TempData["Error"] = "Este usuario ya se encuentra activo.";
                return RedirectToAction(nameof(Index));
            }
            await _usuarioService.ActivateAsync(id);
            TempData["Success"] = "Usuario activado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede desactivar un usuario mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _usuarioService.GetByIdAsync(id);
            if (user == null) return NotFound();
            if (!user.IsActive)
            {
                TempData["Error"] = "Este usuario ya se encuentra inactivo.";
                return RedirectToAction(nameof(Index));
            }

            var currentUserId = int.TryParse(User.FindFirst("UsuarioId")?.Value, out var uid) ? uid : 0;
            if (currentUserId == id)
            {
                TempData["Error"] = "No puede desactivar su propio usuario mientras está autenticado.";
                return RedirectToAction(nameof(Index));
            }

            if (user.Rol == RolUsuario.Administrador)
            {
                var admins = await _usuarioService.CountActiveAdminsAsync();
                if (admins <= 1)
                {
                    TempData["Error"] = "No se puede desactivar este usuario porque es el único administrador activo del sistema.";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View("Deactivate", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateConfirmed(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede desactivar un usuario mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _usuarioService.GetByIdAsync(id);
            if (user == null) return NotFound();
            if (!user.IsActive)
            {
                TempData["Error"] = "Este usuario ya se encuentra inactivo.";
                return RedirectToAction(nameof(Index));
            }

            var currentUserId = int.TryParse(User.FindFirst("UsuarioId")?.Value, out var uid) ? uid : 0;
            if (currentUserId == id)
            {
                TempData["Error"] = "No puede desactivar su propio usuario mientras está autenticado.";
                return RedirectToAction(nameof(Index));
            }

            if (user.Rol == RolUsuario.Administrador)
            {
                var admins = await _usuarioService.CountActiveAdminsAsync();
                if (admins <= 1)
                {
                    TempData["Error"] = "No se puede desactivar este usuario porque es el único administrador activo del sistema.";
                    return RedirectToAction(nameof(Index));
                }
            }

            await _usuarioService.DeactivateAsync(id);
            TempData["Success"] = "Usuario desactivado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AsignarDirigentes()
        {
            ViewBag.HayEleccionActiva = await _eleccionService.HasActiveElectionAsync();
            var dirigentes = await _usuarioService.GetDirigentesConPartidoAsync();
            var vm = dirigentes.Select(d => new UsuarioDirigenteViewModel
            {
                UsuarioId = d.Id,
                Nombre = d.Nombre,
                Apellido = d.Apellido,
                NombreUsuario = d.NombreUsuario,
                PartidoPoliticoId = d.PartidoPoliticoId,
                PartidoNombre = d.NombrePartido,
                PartidoSiglas = d.PartidoPoliticoSiglas,
                IsActive = d.IsActive,
                PartidoActivo = d.PartidoPoliticoActivo
            }).ToList();
            return View(vm);
        }

        public async Task<IActionResult> CreateAsignacion()
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede crear una asignación de dirigente político mientras exista una elección activa.";
                return RedirectToAction(nameof(AsignarDirigentes));
            }

            await LoadAsignacionSelectsAsync();
            return View(new AsignarDirigenteViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsignacion(AsignarDirigenteViewModel model)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede crear una asignación de dirigente político mientras exista una elección activa.";
                return RedirectToAction(nameof(AsignarDirigentes));
            }

            if (!ModelState.IsValid)
            {
                await LoadAsignacionSelectsAsync();
                return View(model);
            }

            var ok = await _usuarioService.AssignPartidoAsync(model.UsuarioId, model.PartidoPoliticoId);
            TempData[ok ? "Success" : "Error"] = ok
                ? "Asignación creada correctamente."
                : "No fue posible crear la asignación. Verifique que el dirigente y el partido estén disponibles.";
            return RedirectToAction(nameof(AsignarDirigentes));
        }

        public async Task<IActionResult> EliminarAsignacion(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede eliminar una asignación de dirigente político mientras exista una elección activa.";
                return RedirectToAction(nameof(AsignarDirigentes));
            }

            var ok = await _usuarioService.RemovePartidoAsync(id);
            TempData[ok ? "Success" : "Error"] = ok
                ? "Asignación eliminada correctamente."
                : "La asignación seleccionada no existe o ya fue eliminada.";
            return RedirectToAction(nameof(AsignarDirigentes));
        }

        private async Task LoadAsignacionSelectsAsync()
        {
            ViewBag.Dirigentes = await _usuarioService.GetUsuariosSinPartidoAsync();
            var partidos = await _partidoPoliticoService.GetAllActiveAsync();
            var asignados = await _usuarioService.GetDirigentesConPartidoAsync();
            var partidosOcupados = asignados.Select(a => a.PartidoPoliticoId).ToHashSet();
            ViewBag.Partidos = partidos.Where(p => !partidosOcupados.Contains(p.Id)).ToList();
        }
    }
}

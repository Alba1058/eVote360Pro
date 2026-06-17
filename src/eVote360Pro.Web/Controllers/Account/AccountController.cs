using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Usuarios;
using eVote360Pro.Core.Application.Interfaces.Usuarios;
using eVote360Pro.Core.Application.ViewModels.Usuarios;
using eVote360Pro.Core.Domain.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.Web.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public AccountController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectByRole(User.IsInRole(RolUsuario.Administrador.ToString()) ? RolUsuario.Administrador : RolUsuario.DirigentePolitico);

            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var usuarioDto = await _usuarioService.LoginAsync(_mapper.Map<LoginDto>(model));
            if (usuarioDto == null)
            {
                ModelState.AddModelError(string.Empty, "Los datos de acceso son inválidos.");
                return View(model);
            }

            if (!usuarioDto.IsActive)
            {
                ModelState.AddModelError(string.Empty, "El usuario está inactivo.");
                return View(model);
            }

            if (usuarioDto.Rol == RolUsuario.DirigentePolitico && !usuarioDto.PartidoPoliticoId.HasValue)
            {
                ModelState.AddModelError(string.Empty, "No tiene un partido político asignado, por lo tanto no puede iniciar sesión. Por favor, póngase en contacto con un administrador.");
                return View(model);
            }

            if (usuarioDto.Rol == RolUsuario.DirigentePolitico && !usuarioDto.PartidoPoliticoActivo)
            {
                ModelState.AddModelError(string.Empty, "El partido político asignado a este usuario se encuentra inactivo.");
                return View(model);
            }

            var claims = new List<System.Security.Claims.Claim>
            {
                new(System.Security.Claims.ClaimTypes.Name, usuarioDto.NombreUsuario),
                new(System.Security.Claims.ClaimTypes.Role, usuarioDto.Rol.ToString()),
                new("UsuarioId", usuarioDto.Id.ToString())
            };

            if (usuarioDto.PartidoPoliticoId.HasValue)
                claims.Add(new System.Security.Claims.Claim("PartidoPoliticoId", usuarioDto.PartidoPoliticoId.Value.ToString()));

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                });

            return RedirectByRole(usuarioDto.Rol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();

        private IActionResult RedirectByRole(RolUsuario rol) => rol switch
        {
            RolUsuario.Administrador => RedirectToAction("Index", "Admin"),
            RolUsuario.DirigentePolitico => RedirectToAction("Index", "Dirigente"),
            _ => RedirectToAction("Index", "Home")
        };
    }
}

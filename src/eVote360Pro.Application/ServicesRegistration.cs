using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.Interfaces.Alianzas;
using eVote360Pro.Core.Application.Interfaces.Ciudadania;
using eVote360Pro.Core.Application.Interfaces.Elecciones;
using eVote360Pro.Core.Application.Interfaces.Partidos;
using eVote360Pro.Core.Application.Interfaces.Puestos;
using eVote360Pro.Core.Application.Interfaces.Usuarios;
using eVote360Pro.Core.Application.Services;
using eVote360Pro.Core.Application.Services.Alianzas;
using eVote360Pro.Core.Application.Services.Ciudadania;
using eVote360Pro.Core.Application.Services.Elecciones;
using eVote360Pro.Core.Application.Services.Partidos;
using eVote360Pro.Core.Application.Services.Puestos;
using eVote360Pro.Core.Application.Services.Usuarios;
using Microsoft.Extensions.DependencyInjection;

namespace eVote360Pro.Core.Application
{
    public static class ServicesRegistration
    {
        public static void AddApplicationLayerIoc(this IServiceCollection services)
        {
            #region Configurations
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(ServicesRegistration).Assembly);
            });
            #endregion

            #region Services IOC - Ciudadania
            services.AddTransient<ICiudadanoService, CiudadanoService>();
            services.AddTransient<ICodigoVerificacionService, CodigoVerificacionService>();
            #endregion

            #region Services IOC - Usuarios
            services.AddTransient<IUsuarioService, UsuarioService>();
            #endregion

            #region Services IOC - Partidos
            services.AddTransient<IPartidoPoliticoService, PartidoPoliticoService>();
            services.AddTransient<ICandidatoService, CandidatoService>();
            services.AddTransient<IAsignacionCandidatoPuestoService, AsignacionCandidatoPuestoService>();
            #endregion

            #region Services IOC - Alianzas
            services.AddTransient<ISolicitudAlianzaService, SolicitudAlianzaService>();
            services.AddTransient<IAlianzaPoliticaService, AlianzaPoliticaService>();
            #endregion

            #region Services IOC - Elecciones
            services.AddTransient<IEleccionService, EleccionService>();
            services.AddTransient<IEleccionPuestoService, EleccionPuestoService>();
            services.AddTransient<IVotoService, VotoService>();
            #endregion

            #region Services IOC - Puestos
            services.AddTransient<IPuestoElectivoService, PuestoElectivoService>();
            #endregion
        }
    }
}

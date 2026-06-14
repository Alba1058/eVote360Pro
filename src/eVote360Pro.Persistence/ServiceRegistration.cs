using eVote360Pro.Core.Domain.Interfaces.Repositories;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Alianzas;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Ciudadania;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Elecciones;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Partidos;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Puestos;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Usuarios;
using eVote360Pro.Persistence.Repositories.Alianzas;
using eVote360Pro.Persistence.Repositories.Ciudadania;
using eVote360Pro.Persistence.Repositories.Common;
using eVote360Pro.Persistence.Repositories.Elecciones;
using eVote360Pro.Persistence.Repositories.Partidos;
using eVote360Pro.Persistence.Repositories.Puestos;
using eVote360Pro.Persistence.Repositories.Usuarios;
using Microsoft.Extensions.DependencyInjection;

namespace eVote360Pro.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceLayerIoc(this IServiceCollection services)
        {
            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IPuestoElectivoRepository, PuestoElectivoRepository>();
            services.AddTransient<ICiudadanoRepository, CiudadanoRepository>();
            services.AddTransient<ICodigoVerificacionRepository, CodigoVerificacionRepository>();
            services.AddTransient<IEleccionRepository, EleccionRepository>();
            services.AddTransient<IEleccionPuestoRepository, EleccionPuestoRepository>();
            services.AddTransient<IVotoRepository, VotoRepository>();
            services.AddTransient<IDetalleVotoRepository, DetalleVotoRepository>();
            services.AddTransient<IPartidoPoliticoRepository, PartidoPoliticoRepository>();
            services.AddTransient<ICandidatoRepository, CandidatoRepository>();
            services.AddTransient<IAsignacionCandidatoPuestoRepository, AsignacionCandidatoPuestoRepository>();
            services.AddTransient<IAlianzaPoliticaRepository, AlianzaPoliticaRepository>();
            services.AddTransient<ISolicitudAlianzaRepository, SolicitudAlianzaRepository>();
            #endregion
        }
    }
}

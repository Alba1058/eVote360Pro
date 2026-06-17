using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Elecciones;
using eVote360Pro.Core.Application.ViewModels.Elecciones;
using eVote360Pro.Core.Application.ViewModels.Administrador;

namespace eVote360Pro.Core.Application.Mappings.Elecciones
{
    public class EleccionDtoMappingProfile : Profile
    {
        public EleccionDtoMappingProfile()
        {
            CreateMap<EleccionDto, EleccionViewModel>()
                .ReverseMap();

            CreateMap<SaveEleccionDto, SaveEleccionViewModel>()
                .ReverseMap();

            CreateMap<EleccionDto, SaveEleccionViewModel>()
                .ReverseMap();

            CreateMap<ResumenElectoralDto, ResumenElectoralViewModel>();
            CreateMap<ResultadoPuestoDto, ResultadoPuestoViewModel>();
            CreateMap<ResultadoOpcionDto, ResultadoOpcionViewModel>();
        }
    }
}

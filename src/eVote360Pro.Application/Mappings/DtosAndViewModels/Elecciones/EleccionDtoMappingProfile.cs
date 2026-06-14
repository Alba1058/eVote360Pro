using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Elecciones;
using eVote360Pro.Core.Application.ViewModels.Elecciones;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels.Elecciones
{
    public class EleccionDtoMappingProfile : Profile
    {
        public EleccionDtoMappingProfile()
        {
            CreateMap<EleccionDto, EleccionViewModel>()
                .ReverseMap();

            CreateMap<SaveEleccionDto, SaveEleccionViewModel>()
                .ReverseMap();
        }
    }
}

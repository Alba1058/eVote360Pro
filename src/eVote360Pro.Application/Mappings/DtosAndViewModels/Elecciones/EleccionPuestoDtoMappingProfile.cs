using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Elecciones;
using eVote360Pro.Core.Application.ViewModels.Elecciones;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels.Elecciones
{
    public class EleccionPuestoDtoMappingProfile : Profile
    {
        public EleccionPuestoDtoMappingProfile()
        {
            CreateMap<EleccionPuestoDto, EleccionPuestoViewModel>()
                .ReverseMap();

            CreateMap<SaveEleccionPuestoDto, SaveEleccionPuestoViewModel>()
                .ReverseMap();
        }
    }
}

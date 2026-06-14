using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Ciudadania;
using eVote360Pro.Core.Application.ViewModels.Ciudadania;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels.Ciudadania
{
    public class CiudadanoDtoMappingProfile : Profile
    {
        public CiudadanoDtoMappingProfile()
        {
            CreateMap<CiudadanoDto, CiudadanoViewModel>()
                .ReverseMap();

            CreateMap<SaveCiudadanoDto, SaveCiudadanoViewModel>()
                .ReverseMap();
        }
    }
}

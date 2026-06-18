using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Partidos;
using eVote360Pro.Core.Application.ViewModels.Partidos;

namespace eVote360Pro.Core.Application.Mappings.Partidos
{
    public class PartidoPoliticoDtoMappingProfile : Profile
    {
        public PartidoPoliticoDtoMappingProfile()
        {
            CreateMap<PartidoPoliticoDto, PartidoPoliticoViewModel>()
                .ReverseMap();

            CreateMap<SavePartidoPoliticoDto, SavePartidoPoliticoViewModel>()
                .ReverseMap();

            CreateMap<PartidoPoliticoDto, SavePartidoPoliticoViewModel>()
                .ReverseMap();
        }
    }
}

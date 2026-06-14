using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Partidos;
using eVote360Pro.Core.Application.ViewModels.Partidos;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels.Partidos
{
    public class CandidatoDtoMappingProfile : Profile
    {
        public CandidatoDtoMappingProfile()
        {
            CreateMap<CandidatoDto, CandidatoViewModel>()
                .ReverseMap();

            CreateMap<SaveCandidatoDto, SaveCandidatoViewModel>()
                .ReverseMap();
        }
    }
}

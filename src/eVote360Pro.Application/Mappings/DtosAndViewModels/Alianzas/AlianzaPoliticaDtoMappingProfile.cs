using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Alianzas;
using eVote360Pro.Core.Application.ViewModels.Alianzas;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels.Alianzas
{
    public class AlianzaPoliticaDtoMappingProfile : Profile
    {
        public AlianzaPoliticaDtoMappingProfile()
        {
            CreateMap<AlianzaPoliticaDto, AlianzaPoliticaViewModel>()
                .ReverseMap();
        }
    }
}

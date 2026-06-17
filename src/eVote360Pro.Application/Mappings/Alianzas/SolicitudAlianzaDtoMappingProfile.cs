using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Alianzas;
using eVote360Pro.Core.Application.ViewModels.Alianzas;

namespace eVote360Pro.Core.Application.Mappings.Alianzas
{
    public class SolicitudAlianzaDtoMappingProfile : Profile
    {
        public SolicitudAlianzaDtoMappingProfile()
        {
            CreateMap<SolicitudAlianzaDto, SolicitudAlianzaViewModel>()
                .ReverseMap();

            CreateMap<SaveSolicitudAlianzaDto, SaveSolicitudAlianzaViewModel>()
                .ReverseMap();

            CreateMap<SolicitudAlianzaDto, SaveSolicitudAlianzaViewModel>()
                .ReverseMap();
        }
    }
}

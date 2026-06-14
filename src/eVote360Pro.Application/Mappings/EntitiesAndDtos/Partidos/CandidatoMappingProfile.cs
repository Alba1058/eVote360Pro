using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Partidos;
using eVote360Pro.Core.Domain.Entities.Partidos;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos.Partidos
{
    public class CandidatoMappingProfile : Profile
    {
        public CandidatoMappingProfile()
        {
            CreateMap<Candidato, CandidatoDto>()
                .ReverseMap()
                .ForMember(dest => dest.PartidoPolitico, opt => opt.Ignore())
                .ForMember(dest => dest.AsignacionesPuestos, opt => opt.Ignore());

            CreateMap<Candidato, SaveCandidatoDto>()
                .ReverseMap()
                .ForMember(dest => dest.PartidoPolitico, opt => opt.Ignore())
                .ForMember(dest => dest.AsignacionesPuestos, opt => opt.Ignore());
        }
    }
}

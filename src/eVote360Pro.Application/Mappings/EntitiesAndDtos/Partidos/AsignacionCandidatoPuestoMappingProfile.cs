using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Partidos;
using eVote360Pro.Core.Domain.Entities.Partidos;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos.Partidos
{
    public class AsignacionCandidatoPuestoMappingProfile : Profile
    {
        public AsignacionCandidatoPuestoMappingProfile()
        {
            CreateMap<AsignacionCandidatoPuesto, AsignacionCandidatoPuestoDto>()
                .ForMember(dest => dest.NombreCandidato, opt => opt.MapFrom(src => src.Candidato != null ? src.Candidato.Nombre : string.Empty))
                .ForMember(dest => dest.ApellidoCandidato, opt => opt.MapFrom(src => src.Candidato != null ? src.Candidato.Apellido : string.Empty))
                .ForMember(dest => dest.NombrePuesto, opt => opt.MapFrom(src => src.PuestoElectivo != null ? src.PuestoElectivo.Nombre : string.Empty))
                .ForMember(dest => dest.NombrePartido, opt => opt.MapFrom(src => src.PartidoPolitico != null ? src.PartidoPolitico.Nombre : string.Empty));

            CreateMap<SaveAsignacionCandidatoPuestoDto, AsignacionCandidatoPuesto>()
                .ForMember(dest => dest.Candidato, opt => opt.Ignore())
                .ForMember(dest => dest.PuestoElectivo, opt => opt.Ignore())
                .ForMember(dest => dest.PartidoPolitico, opt => opt.Ignore());
        }
    }
}

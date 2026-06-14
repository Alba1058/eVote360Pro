using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Puestos;
using eVote360Pro.Core.Domain.Entities.Puestos;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos.Puestos
{
    public class PuestoElectivoMappingProfile : Profile
    {
        public PuestoElectivoMappingProfile()
        {
            CreateMap<PuestoElectivo, PuestoElectivoDto>()
                .ReverseMap()
                .ForMember(dest => dest.EleccionesPuestos, opt => opt.Ignore())
                .ForMember(dest => dest.AsignacionesCandidatos, opt => opt.Ignore());

            CreateMap<PuestoElectivo, SavePuestoElectivoDto>()
                .ReverseMap()
                .ForMember(dest => dest.EleccionesPuestos, opt => opt.Ignore())
                .ForMember(dest => dest.AsignacionesCandidatos, opt => opt.Ignore());
        }
    }
}

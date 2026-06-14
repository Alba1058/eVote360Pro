using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Elecciones;
using eVote360Pro.Core.Domain.Entities.Elecciones;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos.Elecciones
{
    public class EleccionPuestoMappingProfile : Profile
    {
        public EleccionPuestoMappingProfile()
        {
            CreateMap<EleccionPuesto, EleccionPuestoDto>()
                .ReverseMap()
                .ForMember(dest => dest.Eleccion, opt => opt.Ignore())
                .ForMember(dest => dest.PuestoElectivo, opt => opt.Ignore());

            CreateMap<EleccionPuesto, SaveEleccionPuestoDto>()
                .ReverseMap()
                .ForMember(dest => dest.Eleccion, opt => opt.Ignore())
                .ForMember(dest => dest.PuestoElectivo, opt => opt.Ignore());
        }
    }
}

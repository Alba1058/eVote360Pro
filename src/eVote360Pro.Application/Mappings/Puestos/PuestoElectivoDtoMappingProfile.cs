using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Puestos;
using eVote360Pro.Core.Application.ViewModels.Puestos;

namespace eVote360Pro.Core.Application.Mappings.Puestos
{
    public class PuestoElectivoDtoMappingProfile : Profile
    {
        public PuestoElectivoDtoMappingProfile()
        {
            CreateMap<PuestoElectivoDto, PuestoElectivoViewModel>()
                .ReverseMap();

            CreateMap<SavePuestoElectivoDto, SavePuestoElectivoViewModel>()
                .ReverseMap();

            CreateMap<PuestoElectivoDto, SavePuestoElectivoViewModel>()
                .ForMember(dest => dest.BloquearNombre, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}

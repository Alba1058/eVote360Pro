using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Usuarios;
using eVote360Pro.Core.Domain.Entities.Usuarios;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos.Usuarios
{
    public class UsuarioMappingProfile : Profile
    {
        public UsuarioMappingProfile()
        {
            CreateMap<Usuario, UsuarioDto>()
                .ReverseMap()
                .ForMember(dest => dest.Contrasena, opt => opt.Ignore());

            CreateMap<Usuario, SaveUsuarioDto>()
                .ReverseMap();

            CreateMap<Usuario, LoginDto>()
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Rol, opt => opt.Ignore())
                .ForMember(dest => dest.PartidoPoliticoId, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());
        }
    }
}

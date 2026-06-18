using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Usuarios;
using eVote360Pro.Core.Application.ViewModels.Usuarios;

namespace eVote360Pro.Core.Application.Mappings.Usuarios
{
    public class UsuarioDtoMappingProfile : Profile
    {
        public UsuarioDtoMappingProfile()
        {
            CreateMap<UsuarioDto, UsuarioViewModel>()
                .ForMember(dest => dest.PartidoPoliticoNombre, opt => opt.MapFrom(src => src.NombrePartido))
                .ReverseMap();

            CreateMap<SaveUsuarioDto, SaveUsuarioViewModel>()
                .ReverseMap();

            CreateMap<UsuarioDto, SaveUsuarioViewModel>()
                .ReverseMap();

            CreateMap<LoginDto, LoginViewModel>()
                .ReverseMap();
        }
    }
}

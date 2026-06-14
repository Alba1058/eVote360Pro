using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Usuarios;
using eVote360Pro.Core.Application.ViewModels.Usuarios;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels.Usuarios
{
    public class UsuarioDtoMappingProfile : Profile
    {
        public UsuarioDtoMappingProfile()
        {
            CreateMap<UsuarioDto, UsuarioViewModel>()
                .ReverseMap();

            CreateMap<SaveUsuarioDto, SaveUsuarioViewModel>()
                .ReverseMap();

            CreateMap<LoginDto, LoginViewModel>()
                .ReverseMap();
        }
    }
}

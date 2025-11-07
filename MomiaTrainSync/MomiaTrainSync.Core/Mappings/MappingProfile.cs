using MomiaTrainSync.Core.DTOs;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using AutoMapper;
using System;

namespace MomiaTrainSync.Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapea Rol también
            CreateMap<RolEnt, RolDto>();

            CreateMap<UsuarioEnt, UsuarioDto>();

            // DTO → Entidad
            CreateMap<UsuarioDto, UsuarioEnt>()
                .ForMember(dest => dest.Rol, opt => opt.Ignore());
        }
    }
}

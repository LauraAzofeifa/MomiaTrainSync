using MomiaTrainSync.Core.DTOs;
using MomiaTrainSync.Domain.Entities;
using AutoMapper;
using System;

namespace MomiaTrainSync.Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UsuarioEnt, UsuarioDto>()
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Rol.Nombre));

            // Podés seguir agregando otros mapeos:
            // CreateMap<RolEnt, RolDto>();
        }
    }
}

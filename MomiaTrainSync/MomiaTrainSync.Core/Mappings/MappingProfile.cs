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
            // Entidad → DTO
            CreateMap<UsuarioEnt, UsuarioDto>()
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Rol.Nombre));

            // DTO → Entidad (necesario para Update)
            CreateMap<UsuarioDto, UsuarioEnt>()
                .ForMember(dest => dest.Rol, opt => opt.Ignore()); // Ignora navegación para evitar bucles
        }
    }
}

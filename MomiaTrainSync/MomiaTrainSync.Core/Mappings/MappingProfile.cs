using AutoMapper;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.DTOs.RutinasEntrenamientos;
using MomiaTrainSync.Core.DTOs.EntrenadorAtleta;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using MomiaTrainSync.Domain.Entities.EntrenadorAtleta;
using MomiaTrainSync.Domain.Entities.RutinasAsignaciones;
using System;
using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using MomiaTrainSync.Core.DTOs.EntrenamientoZonas;

namespace MomiaTrainSync.Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Roles y Permisos
            CreateMap<PermisoEnt, PermisoDto>().ReverseMap();
            CreateMap<RolPermisoEnt, RolPermisoDto>().ReverseMap();
            CreateMap<RolEnt, RolDto>().ReverseMap();

            // Usuarios
            CreateMap<UsuarioEnt, UsuarioDto>()
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Rol))
                .ReverseMap()
                .ForMember(dest => dest.Rol, opt => opt.Ignore());

            // Mapea Entrenador Atleta
            CreateMap<EntrenadorAtletaEnt, EntrenadorAtletaDto>().ReverseMap();

            // Rutina
            CreateMap<RutinaEnt, RutinaDto>().ReverseMap();
            CreateMap<EntrenamientoEnt, EntrenamientoDto>().ReverseMap();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Core.Interfaces.Repositories;
using MomiaTrainSync.Domain.Entities;
using MomiaTrainSync.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly MomiaTrainSyncDbContext _context;

        public UsuarioRepository(MomiaTrainSyncDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioEnt?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios
                .Include(u => u.Rol) // si se necesita info del rol
                .FirstOrDefaultAsync(u => u.Correo == email);
        }

        public async Task<UsuarioEnt?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UsuarioEnt?> AddAsync(UsuarioEnt usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task UpdateAsync(UsuarioEnt usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }
}

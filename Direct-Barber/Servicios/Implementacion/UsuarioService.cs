using Microsoft.EntityFrameworkCore;
using Direct_Barber.Models;
using Direct_Barber.Servicios.Contrato;

namespace Direct_Barber.Servicios.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly DirectBarber1Context _context;
        public UsuarioService(DirectBarber1Context context)
        {
            _context = context;
        }

        public async Task<Usuario> GetUsuario(string correo, string contrasena)
        {
            Usuario usuario_encontrado = await _context.Usuarios
                .Include(u => u.Rol)  // Cargar la entidad Rol relacionada
                .Where(u => u.Correo == correo && u.Contrasena == contrasena)
                .FirstOrDefaultAsync();

            return usuario_encontrado;
        }


        public async Task<Usuario> SaveUsuario(Usuario modelo)
        {
            _context.Usuarios.Add(modelo);
            await _context.SaveChangesAsync();
            return modelo;
        }

        public async Task<List<Rol>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

    }
}

using Microsoft.EntityFrameworkCore;
using Direct_Barber.Models;

namespace Direct_Barber.Servicios.Contrato
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuario(string correo, string contrasena); //Devolver un usuario.
        Task<Usuario> SaveUsuario(Usuario modelo); //Guardar un usuario.
        Task<List<Rol>> GetRoles(); // Método para obtener roles

    }
}

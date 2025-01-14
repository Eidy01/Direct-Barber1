﻿using System.Collections.Generic;

namespace Direct_Barber.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Relación inversa: lista de usuarios que tienen este rol
        public ICollection<Usuario> Usuarios { get; set; }
    }
}

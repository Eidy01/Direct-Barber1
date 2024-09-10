using System.Security.Cryptography;
using System.Text;

namespace Direct_Barber.Recursos
{
    public class Utilidades
    {
        public static string EncriptarContra(string contrasena) { 
            
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create()) { 
                Encoding enc = Encoding.UTF8;

                byte[] resultado = hash.ComputeHash(enc.GetBytes(contrasena));

                foreach (byte b in resultado)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}

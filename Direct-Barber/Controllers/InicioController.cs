 using Microsoft.AspNetCore.Mvc;

//Trabajar con los servicos creados.
using Direct_Barber.Models;
using Direct_Barber.Recursos;
using Direct_Barber.Servicios.Contrato;

//Trabajar con la autenticación por cookies.
using System.Security.Claims; 
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

//Imagen
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Direct_Barber.Servicios.Implementacion;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Direct_Barber.Controllers
{
    public class InicioController : Controller
    {
        //Utilizar el servicio.
        private readonly IUsuarioService _usuarioServicio;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public InicioController(IUsuarioService usuarioServicio, IWebHostEnvironment webHostEnvironment)
        {
            _usuarioServicio = usuarioServicio;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet]
        public async Task<IActionResult> Registrarse()
        {
            // Obtener lista de roles de forma asíncrona
            var roles = await _usuarioServicio.GetRoles();

            // Pasar los roles a la vista, utilizando los nombres correctos de las propiedades
            ViewBag.Roles = new SelectList(roles, "Id", "Nombre");

            ViewData["Mensaje"] = null; // Inicializar con null
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario modelo, IFormFile foto)
        {
            // Verificar si el archivo está llegando
            if (foto == null || foto.Length == 0)
            {
                // Mostrar un mensaje si no se seleccionó una imagen
                ViewData["Mensaje"] = "No se ha seleccionado una imagen o la imagen está vacía.";
                return View(modelo);
            }

            // Invocar las imágenes
            string uFileName = Utilidades.UploadedFile(foto, _webHostEnvironment);      
            modelo.Foto = uFileName;

            // Encriptar la contraseña
            modelo.Contrasena = Utilidades.EncriptarContra(modelo.Contrasena);

            // Guardar el usuario
            Usuario usuario_creado = await _usuarioServicio.SaveUsuario(modelo);

            if (usuario_creado.Id > 0)
                return RedirectToAction("IniciarSesion", "Inicio");

            ViewData["Mensaje"] = "No se pudo crear el usuario";
            return View();
        }




        public IActionResult IniciarSesion()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string Correo, string Contrasena)
        {
            // Al usuario encontrado se le encripta la contraseña.
            Usuario usuario_encontrado = await _usuarioServicio.GetUsuario(Correo, Utilidades.EncriptarContra(Contrasena));

            // Validación. Si no se encuentra el usuario, mostrar un mensaje.
            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            // Invocar la imagen
            HttpContext.Session.SetString("Foto", usuario_encontrado.Foto ?? "usuario.png");

            // Crear una lista de claims con información relevante del usuario
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario_encontrado.Nombre),        // Nombre del usuario
                new Claim(ClaimTypes.Role, usuario_encontrado.Rol.Nombre),    // Rol del usuario
                new Claim(ClaimTypes.Email, usuario_encontrado.Correo)        // Correo electrónico del usuario
            };

            // Crear una identidad de claims utilizando el esquema de autenticación de cookies
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Opciones de autenticación, como permitir la actualización de las cookies
            AuthenticationProperties properties = new AuthenticationProperties
            {
                AllowRefresh = true  // Permitir que la cookie se pueda refrescar
            };

            // Realizar el inicio de sesión en el contexto HTTP, utilizando la identidad y las propiedades definidas
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,           // Esquema de autenticación de cookies
                new ClaimsPrincipal(claimsIdentity),                         // Principal con la identidad de claims
                properties                                                   // Propiedades de autenticación
            );


            // Redireccionar según el rol del usuario
            if (usuario_encontrado.Rol.Nombre == "Barbero")
            {
                return RedirectToAction("Barbero", "Home");
            }
            else if (usuario_encontrado.Rol.Nombre == "Cliente")
            {
                return RedirectToAction("Cliente", "Home");
            }

            // En caso de que no tenga rol o no se encuentre una coincidencia, redirigir a Index
            return RedirectToAction("Index", "Home");
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Direct_Barber.Models;
using System.Security.Claims;

namespace Direct_Barber.Controllers
{
    public class SolicitudsController : Controller
    {
        private readonly DirectBarber1Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SolicitudsController(DirectBarber1Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Solicituds
        public async Task<IActionResult> Index()
        {
            var directBarber1Context = _context.Solicituds.Include(s => s.IdBarberoNavigation).Include(s => s.IdClienteNavigation).Include(s => s.IdMetodoPagoNavigation);
            return View(await directBarber1Context.ToListAsync());
        }

        // GET: Solicituds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitud = await _context.Solicituds
                .Include(s => s.IdBarberoNavigation)
                .Include(s => s.IdClienteNavigation)
                .Include(s => s.IdMetodoPagoNavigation)
                .FirstOrDefaultAsync(m => m.IdSolicitud == id);
            if (solicitud == null)
            {
                return NotFound();
            }

            return View(solicitud);
        }

        // GET: Solicituds/Create
        public IActionResult Create()
        {
            // Cargar listas desplegables para los campos relacionados
            ViewData["IdBarbero"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            ViewData["IdBarbero"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            ViewData["IdMetodoPago"] = new SelectList(_context.MetodoPagos, "Id", "Metodo"); // Ajusta el campo mostrado si es necesario

            return View();
        }

        // POST: Solicituds/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSolicitud,IdCliente,IdBarbero,Direccion,Fecha,Descripcion,Precio,IdMetodoPago")] Solicitud solicitud)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    // Esto es para ver los mensajes de error
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            ViewData["IdBarbero"] = new SelectList(_context.Usuarios, "Id", "Nombre", solicitud.IdBarbero);
            ViewData["IdMetodoPago"] = new SelectList(_context.MetodoPagos, "Id", "Metodo", solicitud.IdMetodoPago);
            return View(solicitud);
        }



        // GET: Solicituds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitud = await _context.Solicituds.FindAsync(id);
            if (solicitud == null)
            {
                return NotFound();
            }
            ViewData["IdBarbero"] = new SelectList(_context.Usuarios, "Id", "Contrasena", solicitud.IdBarbero);
            ViewData["IdCliente"] = new SelectList(_context.Usuarios, "Id", "Contrasena", solicitud.IdCliente);
            ViewData["IdMetodoPago"] = new SelectList(_context.MetodoPagos, "Id", "Id", solicitud.IdMetodoPago);
            return View(solicitud);
        }

        // POST: Solicituds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSolicitud,IdCliente,IdBarbero,Dirección,Fecha,Descripcion,Precio,IdMetodoPago")] Solicitud solicitud)
        {
            if (id != solicitud.IdSolicitud)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(solicitud);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolicitudExists(solicitud.IdSolicitud))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdBarbero"] = new SelectList(_context.Usuarios, "Id", "Contrasena", solicitud.IdBarbero);
            ViewData["IdCliente"] = new SelectList(_context.Usuarios, "Id", "Contrasena", solicitud.IdCliente);
            ViewData["IdMetodoPago"] = new SelectList(_context.MetodoPagos, "Id", "Id", solicitud.IdMetodoPago);
            return View(solicitud);
        }

        // GET: Solicituds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitud = await _context.Solicituds
                .Include(s => s.IdBarberoNavigation)
                .Include(s => s.IdClienteNavigation)
                .Include(s => s.IdMetodoPagoNavigation)
                .FirstOrDefaultAsync(m => m.IdSolicitud == id);
            if (solicitud == null)
            {
                return NotFound();
            }

            return View(solicitud);
        }

        // POST: Solicituds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var solicitud = await _context.Solicituds.FindAsync(id);
            if (solicitud != null)
            {
                _context.Solicituds.Remove(solicitud);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SolicitudExists(int id)
        {
            return _context.Solicituds.Any(e => e.IdSolicitud == id);
        }
    }
}

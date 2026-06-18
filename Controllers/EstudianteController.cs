using Microsoft.AspNetCore.Mvc;
using ControlAcademicoMvc.Models;

namespace ControlAcademicoMvc.Controllers
{
    public class EstudianteController : Controller
    {
        // Almacenamiento simulado en memoria centralizada (Simulando Tier 3)
        private static readonly List<Estudiante> _baseDatosMemoria = new()
        {
            new Estudiante { Carne = 2026012, Nombre = "Fernando Velasquez", Promedio = 91.5 },
            new Estudiante { Carne = 2026045, Nombre = "Maria Mercedes", Promedio = 84.0 }
        };

        // GET: /Estudiante/Listar
        public IActionResult Listar()
        {
            // El controlador extrae los datos limpios del modelo y los inyecta a la vista
            return View(_baseDatosMemoria);
        }

        // GET: /Estudiante/Detalle/{id}
        public IActionResult Detalle(int id)
        {
            var estudiante = _baseDatosMemoria.FirstOrDefault(e => e.Carne == id);
            if (estudiante == null)
            {
                return NotFound();
            }
            return View(estudiante);
        }

        // GET: /Estudiante/Crear (muestra formulario)
        public IActionResult Crear()
        {
            return View();
        }

        // POST: /Estudiante/Registrar
        [HttpPost]
        public IActionResult Registrar([FromForm] Estudiante nuevoEstudiante)
        {
            // Validación perimetral rápida (Skinny Controller)
            if (nuevoEstudiante.Carne <= 0 || string.IsNullOrEmpty(nuevoEstudiante.Nombre))
            {
                return BadRequest(new { mensaje = "Datos del estudiante inválidos." });
            }
            
            _baseDatosMemoria.Add(nuevoEstudiante);
            return RedirectToAction(nameof(Listar));
        }

        // POST: /Estudiante/Eliminar/{id}
        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            var estudiante = _baseDatosMemoria.FirstOrDefault(e => e.Carne == id);
            if (estudiante == null)
            {
                return NotFound();
            }
            
            _baseDatosMemoria.Remove(estudiante);
            return Ok(new { mensaje = "Estudiante eliminado correctamente" });
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Actividad26
{
    [ApiController]
    [Route("api/[controller]")]
    public class Desafio2Controller : ControllerBase
    {
        private readonly MiDbContext _context;

        public Desafio2Controller(MiDbContext context)
        {
            _context = context;
        }

        [HttpPost("cargar-csv")]
        public async Task<IActionResult> CargarCSV([FromForm] IFormFile archivo)
        {
            // Validar que se haya enviado un archivo
            if (archivo == null || archivo.Length == 0)
            {
                return BadRequest("Debe enviar un archivo CSV válido.");
            }
            
            // Validar extensión del archivo
            if (!archivo.FileName.EndsWith(".csv", System.StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("El archivo debe tener extensión .csv");
            }
            
            var listaEstudiantes = new List<Estudiante>();
            int totalLineas = 0;
            int lineasValidas = 0;
            
            // 1. Procesar el archivo línea por línea con StreamReader (asíncrono)
            using var stream = archivo.OpenReadStream();
            using var reader = new StreamReader(stream);
            
            // Leer y descartar la cabecera (primera línea)
            string cabecera = await reader.ReadLineAsync();
            
            while (true)
            {
                string linea = await reader.ReadLineAsync();
                if (linea == null)
                    break;

                totalLineas++;
                
                // Saltar líneas vacías
                if (string.IsNullOrWhiteSpace(linea))
                    continue;
                
                // 2. Parsear línea CSV (usando ';' como separador)
                var columnas = linea.Split(';');
                
                // Validar que la línea tenga el número correcto de columnas
                if (columnas.Length < 2)
                    continue;
                
                try
                {
                    // 3. Mapear a objeto Estudiante
                    var estudiante = new Estudiante
                    {
                        Carne = columnas[0].Trim(),
                        Nombre = columnas[1].Trim()
                    };
                    
                    listaEstudiantes.Add(estudiante);
                    lineasValidas++;
                }
                catch
                {
                    // Registrar error en log (opcional)
                    continue;
                }
            }
            
            // 4. Validar que haya datos para insertar
            if (listaEstudiantes.Count == 0)
            {
                return BadRequest("El archivo no contiene datos válidos para procesar.");
            }
            
            // 5. Inserción por lotes (Batching)
            _context.Estudiantes.AddRange(listaEstudiantes);
            await _context.SaveChangesAsync();
            
            // 6. Retornar respuesta con estadísticas
            return Ok(new
            {
                Mensaje = "Carga masiva completada exitosamente",
                TotalLineas = totalLineas,
                LineasValidas = lineasValidas,
                RegistrosInsertados = listaEstudiantes.Count
            });
        }
    }

    // Clase modelo (definición de referencia)
    public class Estudiante
    {
        public int Id { get; set; }
        public string Carne { get; set; }
        public string Nombre { get; set; }
    }
}
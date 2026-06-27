using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class AlumnoService
{
    public async Task<Alumno> ObtenerAlumnoAsync()
    {
        string url = "https://api.usac.edu/v1/alumnos";
        
        using var client = new HttpClient();
        
        try
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            string jsonResponse = await response.Content.ReadAsStringAsync();
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var alumno = JsonSerializer.Deserialize<Alumno>(jsonResponse, options);
            return alumno;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error HTTP: {ex.Message}");
            throw;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error de deserialización: {ex.Message}");
            throw;
        }
    }
}

// Clase modelo (definición de referencia)
public class Alumno
{
    public int Id { get; set; }
    public string Carne { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
}
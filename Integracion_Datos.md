# Actividad 26: Interoperabilidad y Carga Masiva de Datos
# CARNET: 202303332

## PARTE 1: Evaluación Conceptual y Buenas Prácticas

### Formatos de Intercambio
Tabla comparativa de formatos masivos según las ventajas y desventajas expuestas en la sesión:

| Formato | Ventajas | Desventajas |
| :--- | :--- | :--- |
| **CSV** | Extremadamente ligero, fácil de generar desde Excel o hojas de cálculo. | No soporta jerarquías complejas, solo datos planos (sin anidación). |
| **XML** | Estructurado, soporta tipos de datos y jerarquías complejas (anidación). | Verboso, archivos más pesados que JSON o CSV. |

### 1. Diferenciación de Procesos: Serialización vs Deserialización

| Proceso | Dirección | Descripción | Método en `System.Text.Json` |
| :--- | :--- | :--- | :--- |
| **Serialización** | Objeto C# → JSON | Convierte un objeto en memoria a una cadena de texto con formato JSON para transmitirlo o almacenarlo. | `JsonSerializer.Serialize(obj)` |
| **Deserialización** | JSON → Objeto C# | Convierte una cadena JSON (recibida desde una API o archivo) en una instancia de una clase C# tipada. | `JsonSerializer.Deserialize<T>(json)` |

### 2. El Antipatrón del Rendimiento "N+1"

**¿En qué consiste?**
Es un error común de rendimiento que ocurre cuando se procesa un archivo masivo (o una colección) y, por cada elemento procesado, se realiza una llamada separada a la base de datos o a una API externa. Si hay **N** registros, se ejecutan **N+1** consultas (la consulta inicial + 1 por cada registro), lo que destruye el rendimiento.


### Referencias
Facultad de Ingeniería, USAC. (2026). Sesión 20: Integración de Datos. Consumo de APIs
Externas y Carga Masiva (CSV/XML). Laboratorio del curso Introducción a la
Programación y Computación 2. Guatemala.

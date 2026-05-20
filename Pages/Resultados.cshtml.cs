using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Linq;
using ProyectoWebIDP.Models;
using Postgrest;
using System;
using System.Collections.Generic;

namespace ProyectoWebIDP.Pages
{
    public class ResultadosModel : PageModel
    {
        private readonly Supabase.Client _supabase;

        public ResultadosModel(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public Usuario? UsuarioInfo { get; set; }
        public ResultadoDisc? Resultado { get; set; }
        public ResultadoCompleto? InfoPatron { get; set; }
        public DefinicionDisc? DefinicionLetraPrincipal { get; set; }
        
        // Propiedades para la determinación de la letra principal basada en puntajes
        public string LetraPrincipalDeterminada { get; set; } = string.Empty;
        public int PuntajeMayor { get; set; }

        public async Task<IActionResult> OnGetAsync(string usuarioId)
        {
            if (string.IsNullOrEmpty(usuarioId)) return RedirectToPage("/Index");

            try
            {
                // Obtener los resultados del usuario
                var resResultado = await _supabase.From<ResultadoDisc>().Where(x => x.UsuarioId == usuarioId).Get();
                Resultado = resResultado.Models.FirstOrDefault();

                if (Resultado == null) return RedirectToPage("/Index");

                // Determinar la letra principal basada en los PUNTAJES ORIGINALES
                if (Resultado != null)
                {
                    // Crear un diccionario con los puntajes originales
                    var puntajes = new Dictionary<string, int>
                    {
                        { "D", Resultado.ZDif },
                        { "I", Resultado.CuadradoDif },
                        { "S", Resultado.TrianguloDif },
                        { "C", Resultado.EstrellaDif }
                    };
                    
                    // Encontrar el puntaje más alto
                    var letraPrincipal = puntajes.OrderByDescending(x => x.Value).First();
                    LetraPrincipalDeterminada = letraPrincipal.Key;
                    PuntajeMayor = letraPrincipal.Value;
                    
                    Console.WriteLine($"Letra principal determinada por PUNTAJES: {LetraPrincipalDeterminada} (Puntaje: {PuntajeMayor})");
                    Console.WriteLine($"Puntajes: D:{puntajes["D"]}, I:{puntajes["I"]}, S:{puntajes["S"]}, C:{puntajes["C"]}");
                    
                    // Obtener la definición de la letra principal
                    try
                    {
                        var resDefinicion = await _supabase
                            .From<DefinicionDisc>()
                            .Where(x => x.Letra == LetraPrincipalDeterminada)
                            .Get();
                        
                        DefinicionLetraPrincipal = resDefinicion.Models.FirstOrDefault();
                        
                        if (DefinicionLetraPrincipal != null)
                        {
                            Console.WriteLine($"Definición encontrada para letra: {DefinicionLetraPrincipal.Letra}");
                        }
                        else
                        {
                            Console.WriteLine("No se encontró definición para esta letra");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al obtener definición: {ex.Message}");
                    }
                }

                // Obtener la información del patrón
                if (Resultado != null && !string.IsNullOrEmpty(Resultado.CodigoSegmento))
                {
                    Console.WriteLine($"Buscando patrón con código: {Resultado.CodigoSegmento}");
                    
                    try
                    {
                        var resPatron = await _supabase
                            .From<ResultadoCompleto>()
                            .Where(x => x.CodigoSegmento == Resultado.CodigoSegmento)
                            .Get();
                        
                        InfoPatron = resPatron.Models.FirstOrDefault();
                        
                        if (InfoPatron != null)
                        {
                            Console.WriteLine($"Patrón encontrado: {InfoPatron.NombrePatron}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al obtener patrón: {ex.Message}");
                    }
                }

                // Obtener datos del usuario
                var resUsuario = await _supabase.From<Usuario>().Where(u => u.Id == usuarioId).Get();
                UsuarioInfo = resUsuario.Models.FirstOrDefault();

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Resultados: {ex.Message}");
                return RedirectToPage("/Index");
            }
        }
    }
}
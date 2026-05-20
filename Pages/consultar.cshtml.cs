using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoWebIDP.Pages
{
    public class ConsultarModel : PageModel
    {
        private readonly Supabase.Client _supabase;

        public ConsultarModel(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        [BindProperty]
        public string Correo { get; set; } = string.Empty;

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Correo))
            {
                ViewData["ErrorConsulta"] = "Por favor ingresa un correo válido.";
                return Page();
            }

            try
            {
                // Buscar el usuario por correo en Supabase
                var respuesta = await _supabase
                    .From<Usuario>()
                    .Where(x => x.Correo == Correo)
                    .Get();

                var usuarioEncontrado = respuesta.Models.FirstOrDefault();

                if (usuarioEncontrado != null)
                {
                    // Si existe, lo mandamos a la página de resultados con su ID
                    return RedirectToPage("/Resultados", new { usuarioId = usuarioEncontrado.Id });
                }
                else
                {
                    // Si no existe
                    ViewData["ErrorConsulta"] = "No se encontró ninguna evaluación registrada con este correo.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorConsulta"] = "Error de conexión: " + ex.Message;
                return Page();
            }
        }
    }
}
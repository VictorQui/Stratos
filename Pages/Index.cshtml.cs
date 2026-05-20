using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Supabase;
using System;
using System.Threading.Tasks;
using Postgrest;
using System.Linq; // Para First()

namespace ProyectoWebIDP.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Supabase.Client _supabase;

        public IndexModel(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string Correo { get; set; } = string.Empty;

        [BindProperty]
        public string Carrera { get; set; } = string.Empty;

        [BindProperty]
        public string Semestre { get; set; } = string.Empty;

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                var nuevoUsuario = new Usuario
                {
                    NombreCompleto = Nombre,
                    Correo = Correo,
                    Carrera = Carrera,
                    Semestre = Semestre,
                    FechaRegistro = DateTime.Now
                };

                var respuesta = await _supabase
                    .From<Usuario>()
                    .Insert(nuevoUsuario);

                if (respuesta.Models.Count > 0)
                {
                    var usuarioInsertado = respuesta.Models.First();
                    string idReal = usuarioInsertado.Id;
                    
                    Console.WriteLine($"--- ID GENERADO POR SUPABASE: {idReal} ---");
                    Console.WriteLine($"--- REGISTRO EXITOSO EN DB: {Nombre} ---");
                    
                    return RedirectToPage("/Evaluacion", new { usuarioId = idReal });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "La base de datos no confirmó el guardado del usuario.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--- ERROR REAL: {ex.Message} ---");
               if (ex.Message.Contains("23505") || ex.Message.Contains("usuarios_correo_key"))
                {
                    ViewData["ErrorCorreo"] = "Ya existe el correo, ocupa otro.";
                    return Page();
                }
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
                return Page();
            }
        }
    }
}
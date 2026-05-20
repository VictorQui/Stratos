using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Supabase;
using System;
using System.Threading.Tasks;

namespace ProyectoWebIDP.Pages
{
    public class EvaluacionModel : PageModel
    {
        private readonly Supabase.Client _supabase;

        public EvaluacionModel(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        [BindProperty(SupportsGet = true)]
        public string UsuarioId { get; set; } = string.Empty;

        [BindProperty]
        public string ResultadoFinal { get; set; } = string.Empty;

        [BindProperty] public int ZMas { get; set; }
        [BindProperty] public int EstrellaMas { get; set; }
        [BindProperty] public int CuadradoMas { get; set; }
        [BindProperty] public int TrianguloMas { get; set; }

        [BindProperty] public int ZMenos { get; set; }
        [BindProperty] public int EstrellaMenos { get; set; }
        [BindProperty] public int CuadradoMenos { get; set; }
        [BindProperty] public int TrianguloMenos { get; set; }

        public async Task<IActionResult> OnGet(string usuarioId)
        {
            UsuarioId = usuarioId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                int zDif = ZMas - ZMenos;
                int estrellaDif = EstrellaMas - EstrellaMenos;
                int cuadradoDif = CuadradoMas - CuadradoMenos;
                int trianguloDif = TrianguloMas - TrianguloMenos;

                // Calcular los segmentos correctamente
                int segD = ObtenerSegmento(zDif, "D");
                int segI = ObtenerSegmento(cuadradoDif, "I");  // I es Cuadrado
                int segS = ObtenerSegmento(trianguloDif, "S"); // S es Triángulo
                int segC = ObtenerSegmento(estrellaDif, "C");  // C es Estrella
                
                string codigoGenerado = $"{segD}{segI}{segS}{segC}";
                
                Console.WriteLine("\n==========================================");
                Console.WriteLine($"PROCESANDO TEST PARA: {UsuarioId}");
                Console.WriteLine("------------------------------------------");
                Console.WriteLine($"D (Z)        | MÁS: {ZMas,2} | MENOS: {ZMenos,2} | DIF: {zDif,2} | SEG: {segD}");
                Console.WriteLine($"I (Cuadrado) | MÁS: {CuadradoMas,2} | MENOS: {CuadradoMenos,2} | DIF: {cuadradoDif,2} | SEG: {segI}");
                Console.WriteLine($"S (Triángulo)| MÁS: {TrianguloMas,2} | MENOS: {TrianguloMenos,2} | DIF: {trianguloDif,2} | SEG: {segS}");
                Console.WriteLine($"C (Estrella) | MÁS: {EstrellaMas,2} | MENOS: {EstrellaMenos,2} | DIF: {estrellaDif,2} | SEG: {segC}");
                Console.WriteLine($"CÓDIGO COMPLETO: {codigoGenerado}");
                Console.WriteLine("==========================================\n");

                // Crear el objeto con TODOS los datos
                var resultadoFinalDisc = new ResultadoDisc
                {
                    UsuarioId = UsuarioId,
                    ZMas = ZMas,
                    EstrellaMas = EstrellaMas,
                    CuadradoMas = CuadradoMas,
                    TrianguloMas = TrianguloMas,
                    ZMenos = ZMenos,
                    EstrellaMenos = EstrellaMenos,
                    CuadradoMenos = CuadradoMenos,
                    TrianguloMenos = TrianguloMenos,
                    ZDif = zDif,
                    EstrellaDif = estrellaDif,
                    CuadradoDif = cuadradoDif,
                    TrianguloDif = trianguloDif,
                    CodigoSegmento = codigoGenerado,  // Guardar el código correcto
                    LetraPrincipal = DeterminarLetraPrincipal(zDif, estrellaDif, cuadradoDif, trianguloDif)
                };

                // Insertar en Base de Datos
                await _supabase.From<ResultadoDisc>().Insert(resultadoFinalDisc);

                return RedirectToPage("/Resultados", new { usuarioId = UsuarioId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--- ERROR CRÍTICO EN EVALUACION: {ex.Message} ---");
                ResultadoFinal = "Hubo un problema al guardar tus resultados. Intenta de nuevo.";
                return Page();
            }
        }

        private string DeterminarLetraPrincipal(int z, int e, int c, int t)
        {
            // D (Z) - Dominancia
            // I (Cuadrado) - Influencia
            // S (Triángulo) - Estabilidad
            // C (Estrella) - Cumplimiento
            
            if (z >= e && z >= c && z >= t) return "D";
            if (c >= z && c >= e && c >= t) return "I";
            if (t >= z && t >= e && t >= c) return "S";
            return "C";
        }

        private int ObtenerSegmento(int score, string factor)
        {
            // D (Z) - Rango corregido
            if (factor == "D")
            {
                if (score >= 11) return 7;      // Muy alto
                if (score >= 6) return 6;
                if (score >= 1) return 5;
                if (score >= -4) return 4;
                if (score >= -10) return 3;
                if (score >= -28) return 2;
                return 1;                        // Muy bajo
            }
            // I (Cuadrado)
            if (factor == "I")
            {
                if (score >= 8) return 7;
                if (score >= 5) return 6;
                if (score >= 2) return 5;
                if (score >= -2) return 4;
                if (score >= -6) return 3;
                if (score >= -28) return 2;
                return 1;
            }
            // S (Triángulo)
            if (factor == "S")
            {
                if (score >= 12) return 7;
                if (score >= 9) return 6;
                if (score >= 6) return 5;
                if (score >= 1) return 4;
                if (score >= -4) return 3;
                if (score >= -28) return 2;
                return 1;
            }
            // C (Estrella)
            if (factor == "C")
            {
                if (score >= 11) return 7;
                if (score >= 7) return 6;
                if (score >= 4) return 5;
                if (score >= 0) return 4;
                if (score >= -3) return 3;
                if (score >= -28) return 2;
                return 1;
            }
            return 4;
        }
    }
}
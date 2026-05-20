using Postgrest.Attributes;
using Postgrest.Models;

namespace ProyectoWebIDP.Models
{
    [Table("vista_resultado_completo")]
    public class ResultadoCompleto : BaseModel
    {
        [Column("codigo_segmento")]
        public string CodigoSegmento { get; set; } = string.Empty;

        [Column("nombre_patron")]
        public string NombrePatron { get; set; } = string.Empty;

        [Column("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Column("emociones")]
        public string Emociones { get; set; } = string.Empty;

        [Column("meta")]
        public string Meta { get; set; } = string.Empty;

        [Column("juzga_otros")]
        public string JuzgaOtros { get; set; } = string.Empty;

        [Column("influye_mediante")]
        public string InfluyeMediante { get; set; } = string.Empty;

        [Column("valor_orga")]
        public string ValorOrga { get; set; } = string.Empty;

        [Column("abusa")]
        public string Abusa { get; set; } = string.Empty;

        [Column("bajo_presion")]
        public string BajoPresion { get; set; } = string.Empty;

        [Column("teme")]
        public string Teme { get; set; } = string.Empty;

        [Column("mas_eficaz")]
        public string MasEficaz { get; set; } = string.Empty;
    }
}
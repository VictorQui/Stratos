using Postgrest.Attributes;
using Postgrest.Models;

namespace ProyectoWebIDP.Models
{
    [Table("interpretacion_letras")]
    public class DefinicionDisc : BaseModel
    {
        [PrimaryKey("letra")]
        public string Letra { get; set; } = string.Empty;

        [Column("tendencias")]
        public string Tendencias { get; set; } = string.Empty;

        [Column("ambiente_deseado")]
        public string AmbienteDeseado { get; set; } = string.Empty;

        [Column("necesita_otros")]
        public string NecesitaOtros { get; set; } = string.Empty;

        [Column("necesita")]
        public string Necesita { get; set; } = string.Empty;
    }
}
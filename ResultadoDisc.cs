using Postgrest.Attributes;
using Postgrest.Models;

[Table("resultados_disc")]
public class ResultadoDisc : BaseModel
{
    [Column("usuario_id")] 
    public string UsuarioId { get; set; } = string.Empty;

    [Column("z_mas")] public int ZMas { get; set; }
    [Column("estrella_mas")] public int EstrellaMas { get; set; }
    [Column("cuadrado_mas")] public int CuadradoMas { get; set; }
    [Column("triangulo_mas")] public int TrianguloMas { get; set; }
    
    [Column("z_menos")] public int ZMenos { get; set; }
    [Column("estrella_menos")] public int EstrellaMenos { get; set; }
    [Column("cuadrado_menos")] public int CuadradoMenos { get; set; }
    [Column("triangulo_menos")] public int TrianguloMenos { get; set; }

    [Column("z_dif")] public int ZDif { get; set; }
    [Column("estrella_dif")] public int EstrellaDif { get; set; }
    [Column("cuadrado_dif")] public int CuadradoDif { get; set; }
    [Column("triangulo_dif")] public int TrianguloDif { get; set; }

    [Column("codigo_segmento")] 
    public string? CodigoSegmento { get; set; }

    [Column("letra_principal")] 
    public string? LetraPrincipal { get; set; }
}
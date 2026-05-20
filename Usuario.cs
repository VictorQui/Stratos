using Postgrest.Attributes;
using Postgrest.Models;
using System;

[Table("usuarios")]
public class Usuario : BaseModel
{
    [PrimaryKey("id", shouldInsert: false)] // ← Importante: shouldInsert = false
    public string Id { get; set; } = string.Empty;

    [Column("nombre_completo")] 
    public string NombreCompleto { get; set; } = string.Empty;

    [Column("correo")]
    public string Correo { get; set; } = string.Empty;

    [Column("carrera")] 
    public string Carrera { get; set; } = string.Empty;

    [Column("semestre")]
    public string Semestre { get; set; } = string.Empty;

    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
}
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace RangoAgilApi.Entitites;

public class Ingrediente
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Nome { get; set; }

    public ICollection<Rango> Rangos { get; set; } = [];

    public Ingrediente()
    {
            
    }

    [SetsRequiredMembers]
    public Ingrediente(int id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}


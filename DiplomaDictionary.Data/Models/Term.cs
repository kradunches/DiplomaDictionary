using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomaDictionary.Data.Models;

public class Term
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] [MaxLength(200)] public string Name { get; set; } = string.Empty;

    [Required] [MaxLength(1000)] public string Definition { get; set; } = string.Empty;

    [Required] public int SubjectId { get; set; }

    [ForeignKey(nameof(SubjectId))] public Subject Subject { get; set; } = null!;
}
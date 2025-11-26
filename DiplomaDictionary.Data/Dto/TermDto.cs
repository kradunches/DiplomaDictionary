namespace DiplomaDictionary.Domain.Dto;

public record TermDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Definition { get; set; }
    public string SubjectName { get; set; }
}

public record CreateTermDto
{
    public string Name { get; set; }
    public string Definition { get; set; }
    public int SubjectId { get; set; }
}
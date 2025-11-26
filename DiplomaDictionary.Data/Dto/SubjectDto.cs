namespace DiplomaDictionary.Domain.Dto;

public record SubjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public record CreateSubjectDto
{
    public string Name { get; set; }
}
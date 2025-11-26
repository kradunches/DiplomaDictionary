using DiplomaDictionary.Data;
using DiplomaDictionary.Data.Models;
using DiplomaDictionary.Domain.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiplomaDictionary.Domain.Services;

/// <summary>
///     Сервис объединяет в себе работу как с Term так и с Subject таблицами, т.к. это одна смысловая область
/// </summary>
public interface IConceptService
{
    // term
    Task<List<TermDto>> GetAllTermsAsync();
    Task<List<TermDto>> GetTermsByWordAsync(string word);
    Task<int> CreateTermAsync(CreateTermDto newTerm);
    Task<int> UpdateTermAsync(TermDto newTerm);
    Task<int> DeleteTermAsync(int id);

    // subject
    Task<List<SubjectDto>> GetAllSubjectsAsync();
    Task<List<SubjectDto>> GetSubjectsByEntranceAsync(string str);
    Task<int> CreateSubjectAsync(CreateSubjectDto newSubject);
    Task<int> UpdateSubjectAsync(SubjectDto newSubject);
    Task<int> DeleteSubjectAsync(int id);
}

public class ConceptService : IConceptService
{
    private readonly ILogger<ConceptService> _logger;

    // private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    // public ConceptService(ILogger<ConceptService> logger, IUnitOfWork unitOfWork, IMapper mapper)
    public ConceptService(ILogger<ConceptService> logger, IUnitOfWork unitOfWork)

    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        // _mapper = mapper;
    }

    public async Task<List<TermDto>> GetAllTermsAsync()
    {
        var terms = await _unitOfWork.Set<Term>()
            .Include(t => t.Subject)
            .ToListAsync();
        // var termDtos = _mapper.Map<List<TermDto>>(terms);
        var termDtos = new List<TermDto>();
        foreach (var term in terms)
            termDtos.Add(new TermDto
            {
                Id = term.Id,
                Name = term.Name,
                Definition = term.Definition,
                SubjectName = term.Subject.Name
            });

        return termDtos;
    }

    public async Task<List<TermDto>> GetTermsByWordAsync(string word)
    {
        var terms = await _unitOfWork.Set<Term>()
            .Include(t => t.Subject)
            .Where(t => t.Definition.Contains(word))
            .ToListAsync();
        // var termDtos = _mapper.Map<List<TermDto>>(terms);
        var termDtos = new List<TermDto>();
        foreach (var term in terms)
            termDtos.Add(new TermDto
            {
                Id = term.Id,
                Name = term.Name,
                Definition = term.Definition,
                SubjectName = term.Subject.Name
            });

        return termDtos;
    }

    public async Task<int> CreateTermAsync(CreateTermDto newTerm)
    {
        // var term = _mapper.Map<Term>(newTerm);
        var term = new Term
        {
            Name = newTerm.Name,
            Definition = newTerm.Definition,
            SubjectId = newTerm.SubjectId
        };
        await _unitOfWork.Set<Term>().AddAsync(term);
        await _unitOfWork.SaveChangesAsync();

        return term.Id;
    }

    public async Task<int> DeleteTermAsync(int id)
    {
        var term = await _unitOfWork.Set<Term>().FindAsync(id);
        if (term is null)
            throw new KeyNotFoundException($"Term with id {id} not found");

        _unitOfWork.Set<Term>().Remove(term);
        await _unitOfWork.SaveChangesAsync();

        return id;
    }

    public async Task<List<SubjectDto>> GetAllSubjectsAsync()
    {
        var subjects = await _unitOfWork.Set<Subject>().ToListAsync();
        // var subjectDtos = _mapper.Map<List<SubjectDto>>(subjects);
        var subjectDtos = new List<SubjectDto>();
        foreach (var subject in subjects)
            subjectDtos.Add(new SubjectDto
            {
                Id = subject.Id,
                Name = subject.Name
            });
        return subjectDtos;
    }

    public async Task<List<SubjectDto>> GetSubjectsByEntranceAsync(string str)
    {
        var subjects = await _unitOfWork.Set<Subject>()
            .Where(s => s.Name.Contains(str))
            .ToListAsync();

        var subjectDtos = new List<SubjectDto>();
        foreach (var subject in subjects)
            subjectDtos.Add(new SubjectDto
            {
                Id = subject.Id,
                Name = subject.Name
            });

        return subjectDtos;
    }

    public async Task<int> CreateSubjectAsync(CreateSubjectDto newSubject)
    {
        // var subject = _mapper.Map<Subject>(newSubject);
        var subject = new Subject
        {
            Name = newSubject.Name
        };
        await _unitOfWork.Set<Subject>().AddAsync(subject);
        await _unitOfWork.SaveChangesAsync();

        return subject.Id;
    }

    public async Task<int> UpdateSubjectAsync(SubjectDto newSubject)
    {
        var subject = await _unitOfWork.Set<Subject>().FirstOrDefaultAsync(s => s.Id == newSubject.Id);
        if (subject is null)
            throw new KeyNotFoundException($"Subject with id {newSubject.Id} not found");

        subject.Name = newSubject.Name;

        await _unitOfWork.SaveChangesAsync();

        return subject.Id;
    }

    public async Task<int> DeleteSubjectAsync(int id)
    {
        var subject = await _unitOfWork.Set<Subject>().FindAsync(id);
        if (subject is null)
            throw new KeyNotFoundException($"Subject with id {id} not found");

        _unitOfWork.Set<Subject>().Remove(subject);
        await _unitOfWork.SaveChangesAsync();

        return id;
    }

    public async Task<int> UpdateTermAsync(TermDto newTerm)
    {
        var term = await _unitOfWork.Set<Term>().FirstOrDefaultAsync(t => t.Id == newTerm.Id);
        if (term is null)
            throw new KeyNotFoundException($"$Term with id {newTerm.Id} not found");

        term.Name = newTerm.Name;
        term.Definition = newTerm.Definition;

        await _unitOfWork.SaveChangesAsync();

        return term.Id;
    }
}
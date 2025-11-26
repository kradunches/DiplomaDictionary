using DiplomaDictionary.Domain.Dto;
using DiplomaDictionary.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaDictionary.Controllers;

// todo: протестировать все эндпоинты
[ApiController]
[Route("controller")]
public class ConceptController : ControllerBase
{
    private readonly IConceptService _conceptService;
    private readonly ILogger<ConceptController> _logger;

    public ConceptController(ILogger<ConceptController> logger, IConceptService conceptService)
    {
        _logger = logger;
        _conceptService = conceptService;
    }

    [HttpGet("GetAllTerms")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TermDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllTerms()
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning($"Bad request:\n${ModelState}");
            return BadRequest(ModelState);
        }

        try
        {
            var allTermDtos = await _conceptService.GetAllTermsAsync();
            return Ok(allTermDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpGet("GetTermsByWord")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TermDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTermsByWord([FromQuery] string word)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning($"Bad request:\n${ModelState}");
            return BadRequest(ModelState);
        }

        try
        {
            var foundTermDtos = await _conceptService.GetTermsByWordAsync(word);
            return Ok(foundTermDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpPost("CreateTerm")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TermDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTerm([FromBody] CreateTermDto newTerm)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning($"Bad request:\n${ModelState}");
            return BadRequest(ModelState);
        }

        try
        {
            var termId = await _conceptService.CreateTermAsync(newTerm);
            return Ok(termId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpPut("UpdateTerm")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateTerm([FromBody] TermDto newTerm)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning($"Bad request:\n${ModelState}");
            return BadRequest(ModelState);
        }

        try
        {
            await _conceptService.UpdateTermAsync(newTerm);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpDelete("DeleteTerm")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteTerm([FromQuery] int id)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning($"Bad request:\n${ModelState}");
            return BadRequest(ModelState);
        }

        try
        {
            await _conceptService.DeleteTermAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }


    [HttpGet("GetAllSubjects")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SubjectDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllSubjects()
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning($"Bad request:\n${ModelState}");
            return BadRequest(ModelState);
        }

        try
        {
            var allSubjectDtos = await _conceptService.GetAllSubjectsAsync();
            return Ok(allSubjectDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpGet("GetSubjectsByEntrance")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubjectDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSubjectsByEntrance([FromQuery] string word)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning($"Bad request:\n${ModelState}");
            return BadRequest(ModelState);
        }

        try
        {
            var allSubjectDtos = await _conceptService.GetSubjectsByEntranceAsync(word);
            return Ok(allSubjectDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpPost("CreateSubject")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SubjectDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectDto newSubject)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning($"Bad request:\n${ModelState}");
            return BadRequest(ModelState);
        }

        try
        {
            var subjectId = await _conceptService.CreateSubjectAsync(newSubject);
            return Ok(subjectId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpPut("UpdateSubject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateSubject([FromBody] SubjectDto newSubject)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning($"Bad request:\n${ModelState}");
            return BadRequest(ModelState);
        }

        try
        {
            await _conceptService.UpdateSubjectAsync(newSubject);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpDelete("DeleteSubject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteSubject([FromQuery] int id)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning($"Bad request:\n${ModelState}");
            return BadRequest(ModelState);
        }

        try
        {
            await _conceptService.DeleteSubjectAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
}
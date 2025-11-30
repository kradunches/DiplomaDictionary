using System.Reflection;
using DiplomaDictionary.Domain.Dto;
using DiplomaDictionary.Domain.Services;
using Grpc.Core;

namespace GrpcService.Services;

public class ConceptGrpcService : SubjectService.SubjectServiceBase
{
    private readonly IConceptService _conceptService;
    private readonly ILogger<ConceptGrpcService> _logger;

    public ConceptGrpcService(IConceptService service, ILogger<ConceptGrpcService> logger)
    {
        _conceptService = service;
        _logger = logger;
    }

    public override async Task<GetAllTermsResponse> GetAllTerms(GetAllTermsRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"{MethodBase.GetCurrentMethod()?.Name} called");
        try
        {
            var terms = await _conceptService.GetAllTermsAsync();

            var response = new GetAllTermsResponse();

            foreach (var term in terms)
                response.Terms.Add(new Term
                {
                    Id = term.Id,
                    Name = term.Name,
                    Definition = term.Definition,
                    SubjectName = term.SubjectName
                });

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all subjects");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<GetTermsByWordResponse> GetTermsByWord(GetTermsByWordRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation(
            $"{MethodBase.GetCurrentMethod().Name} called with search string: {request.SearchString}");

        if (string.IsNullOrWhiteSpace(request.SearchString))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Search string cannot be empty"));

        try
        {
            var terms = await _conceptService.GetTermsByWordAsync(request.SearchString);

            var response = new GetTermsByWordResponse();

            foreach (var term in terms)
                response.Terms.Add(new Term
                {
                    Id = term.Id,
                    Name = term.Name,
                    Definition = term.Definition,
                    SubjectName = term.SubjectName
                });

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all subjects");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<CreateTermResponse> CreateTerm(CreateTermRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"{MethodBase.GetCurrentMethod().Name} called with name: {request.Name}");

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Term name cannot be empty"));

        if (request.Name.Length > 200)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Term name cannot exceed 200 characters"));

        try
        {
            var createDto = new CreateTermDto
            {
                Name = request.Name,
                Definition = request.Definition,
                SubjectId = request.SubjectId
            };

            var termId = await _conceptService.CreateTermAsync(createDto);

            return new CreateTermResponse
            {
                Id = termId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all subjects");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<UpdateTermResponse> UpdateTerm(UpdateTermRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"{MethodBase.GetCurrentMethod().Name} called with name: {request.Term.Name}");

        if (request.Term.Id <= 0)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Id must be positive"));

        if (string.IsNullOrWhiteSpace(request.Term.Name))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Term name cannot be empty"));

        if (request.Term.Name.Length > 200)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Term name cannot exceed 200 characters"));

        try
        {
            var updateDto = new TermDto
            {
                Id = request.Term.Id,
                Name = request.Term.Name,
                Definition = request.Term.Definition
            };

            var updatedId = await _conceptService.UpdateTermAsync(updateDto);

            return new UpdateTermResponse
            {
                Id = updatedId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all subjects");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<DeleteTermResponse> DeleteTerm(DeleteTermRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"{MethodBase.GetCurrentMethod()?.Name} called with id: {request.Id}");

        if (request.Id <= 0)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Id must be positive"));

        try
        {
            var deletedId = await _conceptService.DeleteTermAsync(request.Id);

            return new DeleteTermResponse
            {
                Id = deletedId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all subjects");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task SubscribeTermsStream(SubscribeTermsRequest request,
        IServerStreamWriter<Term> responseStream, ServerCallContext context)
    {
        _logger.LogInformation($"{MethodBase.GetCurrentMethod()?.Name} called with subjectId: {request.SubjectId}");

        var cancellationToken = context.CancellationToken;

        var terms = await _conceptService.GetAllTermsAsync();

        foreach (var term in terms)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var message = new Term
            {
                Id = term.Id,
                Name = term.Name,
                Definition = term.Definition,
                SubjectName = term.SubjectName
            };

            await responseStream.WriteAsync(message);

            await Task.Delay(500, cancellationToken);
        }
    }

    public override async Task<GetAllSubjectsResponse> GetAllSubjects(GetAllSubjectsRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation($"{MethodBase.GetCurrentMethod()?.Name} called");

        try
        {
            var subjects = await _conceptService.GetAllSubjectsAsync();

            var response = new GetAllSubjectsResponse();

            foreach (var subject in subjects)
                response.Subjects.Add(new Subject
                {
                    Id = subject.Id,
                    Name = subject.Name
                });

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all subjects");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<GetSubjectsByEntranceResponse> GetSubjectsByEntrance(
        GetSubjectsByEntranceRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation(
            $"{MethodBase.GetCurrentMethod()?.Name} called with search string: {request.SearchString}");

        if (string.IsNullOrWhiteSpace(request.SearchString))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Search string cannot be empty"));

        try
        {
            var subjects = await _conceptService.GetSubjectsByEntranceAsync(request.SearchString);

            var response = new GetSubjectsByEntranceResponse();

            foreach (var subject in subjects)
                response.Subjects.Add(new Subject
                {
                    Id = subject.Id,
                    Name = subject.Name
                });

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all subjects");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<CreateSubjectResponse> CreateSubject(CreateSubjectRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation($"{MethodBase.GetCurrentMethod()?.Name} called with name: {request.Name}");

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Subject name cannot be empty"));

        if (request.Name.Length > 100)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Subject name cannot exceed 100 characters"));

        try
        {
            var createDto = new CreateSubjectDto
            {
                Name = request.Name
            };

            var subjectId = await _conceptService.CreateSubjectAsync(createDto);

            return new CreateSubjectResponse
            {
                Id = subjectId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all subjects");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<UpdateSubjectResponse> UpdateSubject(UpdateSubjectRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation($"{MethodBase.GetCurrentMethod()?.Name} called with name: {request.Subject.Name}");

        if (request.Subject.Id <= 0)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Id must be positive"));

        if (string.IsNullOrWhiteSpace(request.Subject.Name))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Subject name cannot be empty"));

        if (request.Subject.Name.Length > 100)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Subject name cannot exceed 100 characters"));

        try
        {
            var updateDto = new SubjectDto
            {
                Id = request.Subject.Id,
                Name = request.Subject.Name
            };

            var updatedId = await _conceptService.UpdateSubjectAsync(updateDto);

            return new UpdateSubjectResponse
            {
                Id = updatedId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all subjects");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<DeleteSubjectResponse> DeleteSubject(DeleteSubjectRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation($"{MethodBase.GetCurrentMethod()?.Name} called with id: {request.Id}");

        if (request.Id <= 0)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Id must be positive"));

        try
        {
            var deletedId = await _conceptService.DeleteSubjectAsync(request.Id);

            return new DeleteSubjectResponse
            {
                Id = deletedId
            };
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Subject not found");
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
    }
}
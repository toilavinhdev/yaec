using MongoDB.Driver;
using Package.MongoDb;
using Package.Shared.Exceptions;
using Package.Shared.Extensions;
using Package.Shared.Mediator;
using Service.Identity.Domain.Aggregates.UserAggregate;

namespace Service.Identity.Application.UserModule.Commands;

public class SignInCommand : IRequest<SignInResponse>
{
    public string Key { get; set; } = null!;

    public string Password { get; set; } = null!;
}

public class SignInCommandHandler : IRequestHandler<SignInCommand, SignInResponse>
{
    private readonly IMongoDbService _mongoDbService;

    public SignInCommandHandler(IMongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    public async Task<SignInResponse> HandleAsync(SignInCommand request, CancellationToken cancellationToken)
    {
        var userAsyncCursor = await _mongoDbService.Collection<User>()
            .FindAsync(x => x.Email == request.Key || x.PhoneNumber == request.Key,
                cancellationToken: cancellationToken);
        var user = await userAsyncCursor.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (user is null) throw new BusinessExceptions("User not found");

        if (user.PasswordHash != request.Password.ToSha256())
            throw new BusinessExceptions("Password doesn't match");

        return new SignInResponse()
        {
            AccessToken = StringExtensions.RandomString(36),
            RefreshToken = StringExtensions.RandomString(36),
        };
    }
}

public class SignInResponse
{
    public string AccessToken { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;
}
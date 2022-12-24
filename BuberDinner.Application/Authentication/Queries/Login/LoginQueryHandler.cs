using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Errors;
using MediatR;
using ErrorOr;

namespace BuberDinner.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        // 1. Validate the user exists
        //if (_userRepository.GetUserByEmail(email) is not User user)
        if (_userRepository.GetUserByEmail(query.Email) is not { } user)
        {
            //throw new Exception("User with given email does not exists");
            //return Errors.User.DuplicateEmail;
            return Errors.Authentication.InvalidCredentials;
        }

        // 2. Validate the password is correct
        if (user.Password != query.Password)
        {
            //throw new Exception("Invalid password");
            //return Errors.Authentication.InvalidCredentials;
            return new[] { Errors.Authentication.InvalidCredentials };
        }

        // 3. Create JWT Token
        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(user, token);
    }
}

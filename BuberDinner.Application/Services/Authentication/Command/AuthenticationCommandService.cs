using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Services.Authentication.Common;
using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Errors;
using ErrorOr;

namespace BuberDinner.Application.Services.Authentication.Command;

public class AuthenticationCommandService : IAuthenticationCommandService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationCommandService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    //public AuthenticationResult Register(string firstName, string lastName, string email, string password)
    //public OneOf<AuthenticationResult, DuplicateEmailError> Register(string firstName, string lastName, string email,
    //public OneOf<AuthenticationResult, IError> Register(string firstName, string lastName, string email, string password)
    public ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
    {
        // 1. Validate the user doesn't exist
        if (_userRepository.GetUserByEmail(email) is not null)
        {
            //throw new Exception("User with given email already exists");
            //throw new DuplicateEmailExceptions();
            // return new DuplicateEmailError();
            //return Result.Fail<AuthenticationResult>(new[] { new DuplicateEmailError() });
            return Errors.User.DuplicateEmail;
        }

        // 2. Create user (generate unique ID) & Persist to DB

        var user = new User { FirstName = firstName, LastName = lastName, Email = email, Password = password };

        _userRepository.Add(user);

        // 3. Create JWT Token
        //var userId = Guid.NewGuid();
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}

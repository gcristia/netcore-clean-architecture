using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Errors;
//using FluentResults;
using ErrorOr;

namespace BuberDinner.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
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

    public ErrorOr<AuthenticationResult> Login(string email, string password)
    {
        // 1. Validate the user exists
        //if (_userRepository.GetUserByEmail(email) is not User user)
        if (_userRepository.GetUserByEmail(email) is not { } user)
        {
            //throw new Exception("User with given email does not exists");
            //return Errors.User.DuplicateEmail;
            return Errors.Authentication.InvalidCredentials;
        }

        // 2. Validate the password is correct
        if (user.Password != password)
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

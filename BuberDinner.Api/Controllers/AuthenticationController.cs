using BuberDinner.Api.Filters;
using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Services;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Errors;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[Route("auth")]
//[ErrorHandlingFilter]
public class AuthenticationController : ApiController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var registerResult =
            _authenticationService.Register(request.FirstName, request.LastName, request.Email, request.Password);

        /*if (!registerResult.IsT0)
            return Problem(statusCode: StatusCodes.Status409Conflict, title: "Email already exists.");

        var authResult = registerResult.AsT0;
        var response = MapAuthResult(authResult);
        return Ok(response);*/

        /*return registerResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            //_ => Problem(statusCode: StatusCodes.Status409Conflict, title: "Email already exists."));
            error => Problem(statusCode: (int)error.StatusCode, title: error.ErrorMessage));*/

        // FLUENT RESULT
        /*if (registerResult.IsSuccess)
        {
            return Ok(MapAuthResult(registerResult.Value));
        }

        var firstError = registerResult.Errors[0];

        if (firstError is DuplicateEmailError)
        {
            return Problem(statusCode: StatusCodes.Status409Conflict, title: "Email already exists.");
        }

        return Problem();*/

        // ErrorOr
        /*return registerResult.MatchFirst(
            authResult => Ok(MapAuthResult(authResult)),
            //  _ => Problem(statusCode: StatusCodes.Status409Conflict, title: "User already exists.")
            firstError => Problem(statusCode: StatusCodes.Status409Conflict, title: firstError.Description)
        );*/

        // With ApiController Errors
        return registerResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            //  _ => Problem(statusCode: StatusCodes.Status409Conflict, title: "User already exists.")
            Problem // ==>  errors => Problem(errors)
        );
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var loginResult = _authenticationService.Login(request.Email, request.Password);

        // ErrorOr
        /*return loginResult.MatchFirst(
            authResult => Ok(MapAuthResult(authResult)),
            //  _ => Problem(statusCode: StatusCodes.Status409Conflict, title: "User already exists.")
            firstError => Problem(statusCode: StatusCodes.Status409Conflict, title: firstError.Description)
        );*/

        if (loginResult.IsError && loginResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized, title: loginResult.FirstError.Description);
        }

        // With ApiController Errors
        return loginResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            //  _ => Problem(statusCode: StatusCodes.Status409Conflict, title: "User already exists.")
            Problem // ==>  errors => Problem(errors)
        );
    }


    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        var response = new AuthenticationResponse(
            authResult.User.Id,
            authResult.User.FirstName,
            authResult.User.LastName,
            authResult.User.Email,
            authResult.Token
        );
        return response;
    }
}

//using System.Net;
using FluentResults;

namespace BuberDinner.Application.Common.Errors;

public class DuplicateEmailError : IError
{
    /*public HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    public string ErrorMessage => "Email already exists";*/

    public List<IError> Reasons => throw new NotImplementedException();

    public string Message => throw new NotImplementedException();

    public Dictionary<string, object> Metadata => throw new NotImplementedException();
}

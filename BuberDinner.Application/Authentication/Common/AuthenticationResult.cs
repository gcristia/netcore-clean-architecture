using BuberDinner.Domain.Common.User;

namespace BuberDinner.Application.Authentication.Common;

public record AuthenticationResult(User User, string Token);

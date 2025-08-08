namespace Ambev.DeveloperEvaluation.Domain.Common.Security
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(IUser user);
    }

}

using Ambev.UsersDeveloperEvaluation.Domain.Entities;
using Ambev.UsersDeveloperEvaluation.Domain.Enums;

namespace Ambev.UsersDeveloperEvaluation.Domain.Specifications
{
    public class ActiveUserSpecification : ISpecification<User>
    {
        public bool IsSatisfiedBy(User user)
        {
            return user.Status == UserStatus.Active;
        }
    }
}

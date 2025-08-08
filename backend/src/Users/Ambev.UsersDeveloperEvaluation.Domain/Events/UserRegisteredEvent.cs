using Ambev.UsersDeveloperEvaluation.Domain.Entities;

namespace Ambev.UsersDeveloperEvaluation.Domain.Events
{
    public class UserRegisteredEvent
    {
        public User User { get; }

        public UserRegisteredEvent(User user)
        {
            User = user;
        }
    }
}

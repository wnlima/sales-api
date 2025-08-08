using Ambev.UsersDeveloperEvaluation.Domain.Entities;

using AutoMapper;

namespace Ambev.UsersDeveloperEvaluation.Application.Users.CreateUser
{
    /// <summary>
    /// Profile for mapping between User  and CreateUserResponse
    /// </summary>
    public class CreateUserProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for CreateUser operation
        /// </summary>
        public CreateUserProfile()
        {
            CreateMap<CreateUserCommand, User>();
            CreateMap<User, CreateUserResult>();
        }
    }
}

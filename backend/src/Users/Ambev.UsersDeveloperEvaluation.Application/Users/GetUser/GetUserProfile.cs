using Ambev.UsersDeveloperEvaluation.Domain.Entities;

using AutoMapper;

namespace Ambev.UsersDeveloperEvaluation.Application.Users.GetUser
{
    /// <summary>
    /// Profile for mapping between User  and GetUserResponse
    /// </summary>
    public class GetUserProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for GetUser operation
        /// </summary>
        public GetUserProfile()
        {
            CreateMap<User, GetUserResult>();
        }
    }
}

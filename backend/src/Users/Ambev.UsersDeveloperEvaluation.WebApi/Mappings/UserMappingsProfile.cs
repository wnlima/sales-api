using Ambev.UsersDeveloperEvaluation.Application.Users.CreateUser;
using Ambev.UsersDeveloperEvaluation.Application.Users.GetUser;
using Ambev.UsersDeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.UsersDeveloperEvaluation.WebApi.Features.Users.GetUser;

using AutoMapper;

namespace Ambev.UsersDeveloperEvaluation.WebApi.Mappings
{
    public class UserMappingsProfile : Profile
    {
        public UserMappingsProfile()
        {
            CreateMap<CreateUserRequest, CreateUserCommand>();
            CreateMap<GetUserResult, GetUserResponse>();
        }
    }
}
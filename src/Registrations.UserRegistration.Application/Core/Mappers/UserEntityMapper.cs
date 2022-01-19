using AutoMapper;
using Registrations.Common.Domain.Entities;
using Registrations.UserRegistration.Application.Core.Models;

namespace Registrations.UserRegistration.Application.Core.Mappers
{
    public class UserEntityMapper : Profile
    {
        public UserEntityMapper()
        {
            CreateMap<UserRequestModel, User>()
                .ConstructUsing(x => new User() { })
                .ForMember(x => x.Name,
                    opt => opt.MapFrom(x => x.username))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}

using AutoMapper;
using Registrations.Common.Domain.Entities;
using Registrations.UserRegistration.Application.Core.Models;

namespace Registrations.UserRegistration.Application.Core.Mappers
{
    public class UserModelMapper : Profile
    {
        public UserModelMapper()
        {
            CreateMap<User, UserModel>()
                .ConstructUsing(x => new UserModel() { })
                .ForMember(x => x.userid,
                    opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.username,
                    opt => opt.MapFrom(x => x.Name))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}

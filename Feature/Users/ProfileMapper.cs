using AutoMapper;

namespace hyouka_api.Feature.Users
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<Domain.Person, User>(MemberList.None);
        }
    }
}
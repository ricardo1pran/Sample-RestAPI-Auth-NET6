using AutoMapper;

namespace RicardoDevAPI.Helpers
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile() { 
            CreateMap<SignUp, UserDTO>();
            CreateMap<UpdateUser, UserDTO>().ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                )); ; 
        }
    }
}

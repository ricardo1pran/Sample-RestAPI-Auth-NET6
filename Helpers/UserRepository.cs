using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using RicardoDevAPI.Context;

namespace RicardoDevAPI.Helpers
{
    public interface IUserService
    {
        
       UserDTO GetUser(string username);
        void SignUpModel(SignUp model);
        void UpdateModel(string username, UpdateUser model);
        void Delete(string username);
    }

    public class UserService : IUserService
    {
        private ApiContext _context;
        private readonly IMapper _mapper;

        public UserService(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void Delete(string username)
        {
            var user = GetUser(username);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public UserDTO GetUser(string username)
        {
            return GetTheUser(username);
        }

        private UserDTO GetTheUser(string username)
        {
            var user = _context.Users.Find(username);
            if (user == null)
            {
                throw new KeyNotFoundException("No User found");

            }
            return user;
        }

        public void SignUpModel(SignUp model)
        {
            if(_context.Users.Any(x => x.User_id == model.User_id))
            {
                throw new ApplicationException("Already same '" + model.User_id + "' is used");
            }

            var user = _mapper.Map<UserDTO>(model);
            if (model.Nickname.Equals(""))
            {
                user.Nickname = model.User_id;
            }

            _context.Users.Add(user);
            _context.SaveChanges();
            
        }

        public void UpdateModel(string username, UpdateUser model)
        {
            var user = GetUser(username);
            if(username != user.User_id)
            {
                throw new ApplicationException("No user found");
            }

            _mapper.Map(model,user);
            _context.Users.Update(user);
            _context.SaveChanges() ;
        }

        
    }
}

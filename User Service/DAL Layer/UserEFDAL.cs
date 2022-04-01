using System;
using Microsoft.EntityFrameworkCore;

using Abstraction_Layer;
using DTO_Layer;

using DAL_Layer.Model;
namespace DAL_Layer
{    
    public class UserEFDAL: IUserCollection, IUserRegistration, IIdentifierRecursionChecker
    {
        private readonly UserContext _context;

        public UserEFDAL(DbContext context)
        {
            _context = context as UserContext ?? throw new ArgumentNullException(nameof(context));
        }

        public int AddUser(UserDTO userDTO)
        {   
            userDTO.RegistrationDate = DateTime.Now;

            _context.Users.Add(new User(userDTO));
            _context.SaveChanges();

            return userDTO.Id;
        }

        public UserDTO? GetUser(int Id)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Id == Id);
            if (user == null)
                return null;

            return user.ToDTO();
        }

        public UserDTO? GetUserByEmail(string email)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
                return null;

            return user.ToDTO();
        }

        public bool IsEmailUnique(string email)
        {
            return GetUserByEmail(email) == null;
        }

        public bool IsUsernameUnique(string username)
        {
            return _context.Users.FirstOrDefault(x => x.Username == username) == null;
        }
    }
}

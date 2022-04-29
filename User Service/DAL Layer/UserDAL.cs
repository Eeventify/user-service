using System;
using Microsoft.EntityFrameworkCore;

using Abstraction_Layer;
using DTO_Layer;

using DAL_Layer.Model;
namespace DAL_Layer
{    
    public class UserDAL: IUserCollection, IUserRegistration, IIdentifierRecursionChecker
    {
        private readonly UserContext _context;

        public UserDAL(DbContext context)
        {
            _context = context as UserContext ?? throw new ArgumentNullException(nameof(context));
        }

        public int AddUser(UserDTO userDTO)
        {   
            userDTO.RegistrationDate = DateTime.Now;

            User user = new User(userDTO);

            _context.Users.Add(user);
            _context.SaveChanges();

            return user.Id;
        }

        public UserDTO? GetUser(int Id)
        {
            User? user = _context.Users
                .Include(x => x.Events)
                .Include(x => x.Interests)
                .FirstOrDefault(x => x.Id == Id);

            if (user == null)
                return null;

            return user.ToDTO();
        }

        public UserDTO? GetUserByEmail(string email)
        {
            User? user = _context.Users
                .Include(x => x.Events)
                .Include(x => x.Interests)
                .FirstOrDefault(x => x.Email == email);

            if (user == null)
                return null;

            return user.ToDTO();
        }

        public bool UpdateUser(UserDTO user)
        {
            User? _user = _context.Users.FirstOrDefault(x => x.Id == user.Id);

            if (_user == null)
                return false;

            _user.Email = user.Email ?? _user.Email;
            _user.ProfileImg = user.ProfileImg ?? _user.ProfileImg;
            _user.Username = user.Username ?? _user.Username;

            _context.Update(_user);
            return _context.SaveChanges() > 0;
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

#pragma warning disable S3887

using System.Collections.Generic;

using Abstraction_Layer;
using DTO_Layer;


namespace User_Service.Mocks
{
    public class MockUserDAL : IUserCollection
    {
        public readonly List<UserDTO> users;

        public MockUserDAL()
        {
            users = new List<UserDTO>
            {
                new UserDTO { Id = 1, Username = "Robin", Email = "robin.vanhoof@student.fontys.nl", PasswordHash = "6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b" },
                new UserDTO { Id = 2, Username = "Jeffrey", Email = "jeffrey.derksen@student.fontys.nl", PasswordHash = "d4735e3a265e16eee03f59718b9b5d03019c07d8b6c51f90da3a666eec13ab35" },
                new UserDTO { Id = 3, Username = "Rens", Email = "rens.vlooswijk@student.fontys.nl", PasswordHash = "4e07408562bedb8b60ce05c1decfe3ad16b72230967de01f640b7e4729b49fce" },
                new UserDTO { Id = 4, Username = "Rik", Email = "rik.something@student.fontys.nl", PasswordHash = "4b227777d4dd1fc61c6f884f48641d02b4d121d3fd328cb08b5531fcacdabf8a" },
                new UserDTO { Id = 5, Username = "Tjerk", Email = "tjerk.something@student.fontys.nl", PasswordHash = "ef2d127de37b942baad06145e54b0c619a1f22327b2ebbcfbec78f5564afe39d" },
                new UserDTO { Id = 6, Username = "Test", Email = "test@student.fontys.nl", PasswordHash = "e7f6c011776e8db7cd330b54174fd76f7d0216b612387a5ffcfb81e6f0919683" }
            };
        }

        public UserDTO? GetUser(int Id)
        {
            return users.Find(x => x.Id == Id);
        }

        public UserDTO? GetUserByUsername(string username)
        {
            return users.Find(x => x.Username == username);
        }

        public UserDTO? GetUserByEmail(string email)
        {
            return users.Find(x => x.Email == email);
        }
    }
}

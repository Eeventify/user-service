using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Abstraction_Layer;
using DTO_Layer;

namespace DAL_Layer
{
    public class UserDAL : BaseDAL, IUserCollection
    {
        List<UserDTO> users;

        public UserDAL()
        {
            users = new List<UserDTO>();

            users.Add(new UserDTO { Id = 1, Name = "Robin", Email = "428780@student.fontys.nl", PasswordHash = "6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b" });
            users.Add(new UserDTO { Id = 2, Name = "Luna", Email = "Evalynnlunak@gmail.com", PasswordHash = "d4735e3a265e16eee03f59718b9b5d03019c07d8b6c51f90da3a666eec13ab35" });
            users.Add(new UserDTO { Id = 3, Name = "SaMu", Email = "robin.van.hoof@salvemundi.nl", PasswordHash = "4e07408562bedb8b60ce05c1decfe3ad16b72230967de01f640b7e4729b49fce" });
            users.Add(new UserDTO { Id = 4, Name = "Rens", Email = "rens.vlooswijk@lid.salvemundi.nl", PasswordHash = "4b227777d4dd1fc61c6f884f48641d02b4d121d3fd328cb08b5531fcacdabf8a" });
            users.Add(new UserDTO { Id = 5, Name = "Robin Personal", Email = "robin@vanhoof-erp.nl", PasswordHash = "ef2d127de37b942baad06145e54b0c619a1f22327b2ebbcfbec78f5564afe39d" });
        }

        public UserDTO? GetUser(int Id)
        {
            return users.Find(x => x.Id == Id);
        }

        public UserDTO? GetUserByUsername(string username)
        {
            return users.Find(x => x.Name == username);
        }
    }
}

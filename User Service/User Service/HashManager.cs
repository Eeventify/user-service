using System.Security.Cryptography;
using System.Text;

namespace User_Service
{
    public static class HashManager
    {
        public static string GetHash(string input)
        {
            using (SHA256 hasher = SHA256.Create())
            {
                byte[] hashBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder sBuilder = new StringBuilder();
                foreach (byte hashByte in hashBytes) 
                {
                    sBuilder.Append(hashByte.ToString("x2"));
                }
               
                return sBuilder.ToString();
            }
        }

        public static bool CompareStringToHash(string input, string hash)
        {            
            return (GetHash(input) == hash);
        }
    }
}

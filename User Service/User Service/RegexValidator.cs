using System.Text.RegularExpressions;

using Abstraction_Layer;

namespace User_Service
{
    public class RegexValidator : IIdentifierValidator
    {
        public bool Username(string username)
        {
            Regex regex = new("^(?=.{4,50}$)[a-zA-Z0-9_\\-]+$");
            return regex.IsMatch(username);
        }

        public bool Password(string password)
        {
            Regex regex = new("^(?=.{4,50}$)[a-zA-Z0-9!?@#$_&*\\-]+$");
            return regex.IsMatch(password);
        }

        public bool Email(string email)
        {
            Regex regex = new("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$");
            return regex.IsMatch(email);
        }
    }
}

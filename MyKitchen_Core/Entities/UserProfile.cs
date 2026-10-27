using MyKitchen_Core.Interfaces;

namespace MyKitchen_Core.Entities
{
    public class UserProfile :IEntity
    {
        private Guid _id;
        private string _name = string.Empty;
        private string _email = string.Empty;
        private string _passwordHash = string.Empty;
        private DateTime _registeredAt;

        public Guid ID
        {
            get { return _id; }
            init { _id = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string PasswordHash
        {
            get { return _passwordHash; }
            set { _passwordHash = value; }
        }
        public DateTime RegisteredAt
        {
            get { return _registeredAt; }
            init { _registeredAt = value; }
        }
    }
}
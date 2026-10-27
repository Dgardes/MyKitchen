using MyKitchen_Core.Interfaces;

namespace MyKitchen_Core.Entities
{
    public class Step :IEntity
    {
        private Guid _id;
        private int _order;
        private string _description = string.Empty;
        private string _photoPath = string.Empty;

        public Guid ID
        {
            get { return _id; }
            init { _id = value; }
        }
        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public string PhotoPath
        {
            get { return _photoPath; }
            set { _photoPath = value; }
        }
    }
}
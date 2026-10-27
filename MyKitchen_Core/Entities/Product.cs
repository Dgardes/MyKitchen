
using MyKitchen_Core.Interfaces;

namespace MyKitchen_Core.Entities
{
    public class Product :IEntity
    {
        private Guid _id;
        private string _name = string.Empty;
        private double _calories;
        private double _proteins;
        private double _fats;
        private double _carbohydrates;
        private bool _isCustom;

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
        public double Calories
        {
            get { return _calories; }
            set { _calories = value; }
        }
        public double Proteins
        {
            get { return _proteins; }
            set { _proteins = value; }
        }
        public double Fats
        {
            get { return _fats; }
            set { _fats = value; }
        }
        public double Carbohydrates
        {
            get { return _carbohydrates; }
            set { _carbohydrates = value; }
        }
        public bool IsCustom
        {
            get { return _isCustom; }
            set { _isCustom = value; }
        }
    }
}
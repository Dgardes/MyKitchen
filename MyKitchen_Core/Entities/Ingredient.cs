using MyKitchen_Core.Interfaces;

namespace MyKitchen_Core.Entities
{
    public class Ingredient :IEntity
    {
        private Guid _id;
        private string _rawName = string.Empty;
        private double _weight;
        private Guid? _referencedProduct;

        public Guid ID
        {
            get { return _id; }
            init { _id = value; }
        }
        public string RawName
        {
            get { return _rawName; }
            set { _rawName = value; }
        }
        public double Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
        public Guid? ReferencedProduct
        {
            get { return _referencedProduct; }
            set { _referencedProduct = value; }
        }
    }
}
using MyKitchen_Core.Interfaces;

namespace MyKitchen_Core.Entities
{
    public class ProductAlias :IEntity
    {

        private Guid _id;
        private string _aliasName = string.Empty;
        private Guid _referencedProduct;

        public Guid ID
        {
            get { return _id; }
            init { _id = value; }
        }
        public string AliasName
        {
            get { return _aliasName; }
            set { _aliasName = value; }
        }
        public Guid ReferencedProduct
        {
            get { return _referencedProduct; }
            set { _referencedProduct = value; }
        }
    }
}
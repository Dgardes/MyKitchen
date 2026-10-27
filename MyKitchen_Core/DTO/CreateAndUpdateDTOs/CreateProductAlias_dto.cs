
namespace MyKitchen_Core.DTO.CreateAndUpdateDTOs
{
    public class CreateProductAlias_dto
    {
        private string _aliasName = string.Empty;
        private Guid _referencedProduct;
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

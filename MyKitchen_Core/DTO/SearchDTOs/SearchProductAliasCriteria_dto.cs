using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKitchen_Core.DTO.SearchDTOs
{
    public class SearchProductAliasCriteria_dto
    {
        private Guid? _id; 
        private string? _aliasName;
        private Guid? _referencedProduct;
        public Guid? ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string? AliasName
        {
            get { return _aliasName; }
            set { _aliasName = value; }
        }
        public Guid? ReferencedProduct
        {
            get { return _referencedProduct; }
            set { _referencedProduct = value; }
        }
    }
}

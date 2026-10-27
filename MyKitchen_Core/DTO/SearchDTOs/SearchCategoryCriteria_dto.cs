using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKitchen_Core.DTO.SearchDTOs
{
    public class SearchCategoryCriteria_dto
    {
        private Guid? _id;
        private string? _name;
        private bool? _isCustom;
        public Guid? ID
            {
            get { return _id; }
            set { _id = value; }
        }
        public string? Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public bool? IsCustom
        {
            get { return _isCustom; }
            set { _isCustom = value; }
        }
    }
}

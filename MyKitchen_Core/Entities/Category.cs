using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyKitchen_Core.Interfaces;
namespace MyKitchen_Core.Entities
{
    public class Category :IEntity
    {
        private Guid _id;
        private string _name = string.Empty;
        private string? _description;
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
        public string? Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public bool IsCustom
        {
            get { return _isCustom; }
            set { _isCustom = value; }
        }
    }
}

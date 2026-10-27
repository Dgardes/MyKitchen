
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyKitchen_Core.Tools;

namespace MyKitchen_Core.DTO.SearchDTOs
{
    public class SearchCookBookCriteria_dto
    {
        private Guid? _id;
        private string? _authorName;
        private string? _name;

        private string? _recipeName;

        private ValueRange<DateTime>? _creationDate = new ValueRange<DateTime>();
        private ValueRange<DateTime>? _lastEdited = new ValueRange<DateTime>();
        private bool? _isPublic;
        public Guid? ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string? AuthorName
        {
            get { return _authorName; }
            set { _authorName = value; }
        }
        public string? Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public ValueRange<DateTime>? CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }
        public ValueRange<DateTime>? LastEdited
        {
            get { return _lastEdited; }
            set { _lastEdited = value; }
        }
        public bool? IsPublic
        {
            get { return _isPublic; }
            set { _isPublic = value; }
        }

        public string? RecipeName
        {
            get { return _recipeName; }
            set { _recipeName = value; }
        }

    }
}

using Microsoft.VisualBasic;
using MyKitchen_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyKitchen_Core.Tools;

namespace MyKitchen_Core.DTO.SearchDTOs
{
    public class SearchRecipeCriteria_dto
    {
        private Guid? _id;
        private string? _authorName;
        private string? _name;
        private ValueRange<TimeSpan> _fullPreparationTime = new ValueRange<TimeSpan>();
        private ValueRange<TimeSpan> _activePreparationTime = new ValueRange<TimeSpan>();
        private ValueRange<int> _servings = new ValueRange<int>();
        private List<Guid> _referencedCategories = new List<Guid>();

        private List<String> _productsAliases = new List<String>();
        
        private ValueRange<int> _dishCalories = new ValueRange<int>();
        private ValueRange<int> _dishProteins = new ValueRange<int>();
        private ValueRange<int> _dishFats = new ValueRange<int>();
        private ValueRange<int> _dishCarbohydrates = new ValueRange<int>();

        private ValueRange<int> _servingCalories = new ValueRange<int>();
        private ValueRange<int> _servingProteins = new ValueRange<int>();
        private ValueRange<int> _servingFats = new ValueRange<int>();
        private ValueRange<int> _servingCarbohydrates = new ValueRange<int>();

        private ValueRange<DateTime> _creationDate = new ValueRange<DateTime>();
        private ValueRange<DateTime> _lastEdited = new ValueRange<DateTime>();
        private bool? _isPublic;
        private ValueRange<double> _rating = new ValueRange<double>();

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
        public ValueRange<TimeSpan> FullPreparationTime
        {
            get { return _fullPreparationTime; }
            set { _fullPreparationTime = value; }
        }
        public ValueRange<TimeSpan> ActivePreparationTime
        {
            get { return _activePreparationTime; }
            set { _activePreparationTime = value; }
        }
        public ValueRange<int> Servings
        {
            get { return _servings; }
            set { _servings = value; }
        }
        public List<Guid> ReferencedCategories
        {
            get { return _referencedCategories; }
            set { _referencedCategories = value; }
        }
        public List<String> ProductsAliases
        {
            get { return _productsAliases; }
            set { _productsAliases = value; }
        }
        public ValueRange<DateTime> CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }
        public ValueRange<DateTime> LastEdited
        {
            get { return _lastEdited; }
            set { _lastEdited = value; }
        }
        public bool? IsPublic
        {
            get { return _isPublic; }
            set { _isPublic = value; }
        }
        public ValueRange<double> Rating
        {
            get { return _rating; }
            set { _rating = value; }
        }

        public ValueRange<int> DishCalories
        {
            get { return _dishCalories; }
            set { _dishCalories = value; }
        }

        public ValueRange<int> DishProteins
        {
            get { return _dishProteins; }
            set { _dishProteins = value; }
        }

        public ValueRange<int> DishFats
        {
            get { return _dishFats; }
            set { _dishFats = value; }
        }

        public ValueRange<int> DishCarbohydrates
        {
            get { return _dishCarbohydrates; }
            set { _dishCarbohydrates = value; }
        }

        public ValueRange<int> ServingCalories
        {
            get { return _servingCalories; }
            set { _servingCalories = value; }
        }

        public ValueRange<int> ServingProteins
        {
            get { return _servingProteins; }
            set { _servingProteins = value; }
        }

        public ValueRange<int> ServingFats
        {
            get { return _servingFats; }
            set { _servingFats = value; }
        }

        public ValueRange<int> ServingCarbohydrates
        {
            get { return _servingCarbohydrates; }
            set { _servingCarbohydrates = value; }
        }



    }
}

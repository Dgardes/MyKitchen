using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyKitchen_Core.Tools;

namespace MyKitchen_Core.DTO.SearchDTOs
{
    public class SearchProductCriteria_dto
    {
        private Guid? _id;
        private string? _name;
        private ValueRange<double>? _calories = new ValueRange<double>();
        private ValueRange<double>? _proteins = new ValueRange<double>();
        private ValueRange<double>? _fats = new ValueRange<double>();
        private ValueRange<double>? _carbohydrates = new ValueRange<double>();

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
        public ValueRange<double>? Calories
        {
            get { return _calories; }
            set { _calories = value; }
        }
        public ValueRange<double>? Proteins
        {
            get { return _proteins; }
            set { _proteins = value; }
        }
        public ValueRange<double>? Fats
        {
            get { return _fats; }
            set { _fats = value; }
        }
        public ValueRange<double>? Carbohydrates
        {
            get { return _carbohydrates; }
            set { _carbohydrates = value; }
        }
    }
}

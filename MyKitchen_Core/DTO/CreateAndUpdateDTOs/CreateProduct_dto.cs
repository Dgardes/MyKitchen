

namespace MyKitchen_Core.DTO.CreateAndUpdateDTOs
{
    public class CreateProduct_dto
    {
        private string _name = string.Empty;
        private double? _calories;
        private double? _proteins;
        private double? _fats;
        private double? _carbohydrates;
        public string Name 
        { 
            get { return _name; }
            set { _name = value; }
        }
        public double? Calories
        {
            get { return _calories; }
            set { _calories = value; }
        }
        public double? Proteins
        {
            get { return _proteins; }
            set { _proteins = value; }
        }
        public double? Fats
        {
            get { return _fats; }
            set { _fats = value; }
        }
        public double? Carbohydrates
        {
            get { return _carbohydrates; }
            set { _carbohydrates = value; }
        }
    }
}

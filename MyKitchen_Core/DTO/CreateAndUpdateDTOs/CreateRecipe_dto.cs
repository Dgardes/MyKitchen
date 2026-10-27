using MyKitchen_Core.Entities;

namespace MyKitchen_Core.DTO.CreateAndUpdateDTOs
{
    public class CreateRecipe_dto
    {
        private string _name = string.Empty;
        private string _description = string.Empty;
        private string _sourceLink = string.Empty;
        private TimeSpan _fullPreparationTime;
        private TimeSpan _activePreparationTime;
        private int _servings;
        private string _mainPhotoPath = string.Empty;
        private List<Guid> _referencedCategories = new List<Guid>();
        private List<Ingredient> _ingredients = new List<Ingredient>();
        private List<Step> _preparationSteps = new List<Step>();
        private bool _isPublic;
        private List<Guid> _cookBooksReferencedIn = new List<Guid>();

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public string SourceLink
        {
            get { return _sourceLink; }
            set { _sourceLink = value; }
        }
        public TimeSpan FullPreparationTime
        {
            get { return _fullPreparationTime; }
            set { _fullPreparationTime = value; }
        }
        public TimeSpan ActivePreparationTime
        {
            get { return _activePreparationTime; }
            set { _activePreparationTime = value; }
        }
        public int Servings
        {
            get { return _servings; }
            set { _servings = value; }
        }
        public string MainPhotoPath
        {
            get { return _mainPhotoPath; }
            set { _mainPhotoPath = value; }
        }
        public List<Guid> ReferencedCategories
        {
            get { return _referencedCategories; }
            set { _referencedCategories = value; }
        }
        public List<Ingredient> Ingredients
        {
            get { return _ingredients; }
            set { _ingredients = value; }
        }
        public List<Step> PreparationSteps
        {
            get { return _preparationSteps; }
            set { _preparationSteps = value; }
        }
        public bool IsPublic
        {
            get { return _isPublic; }
            set { _isPublic = value; }
        }
        public List<Guid> cookBooksReferencedIn
        {
            get { return _cookBooksReferencedIn; }
            set { _cookBooksReferencedIn = value; }
        }
    }
}

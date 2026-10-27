using MyKitchen_Core.Interfaces;

namespace MyKitchen_Core.Entities
{
    public class Recipe :IEntity
    {
        private Guid _id;
        private Guid _authorID;
        private string _name = string.Empty;
        private string _description = string.Empty;
        private string _sourceLink = string.Empty;
        private Guid _inspiredBy;
        private TimeSpan _fullPreparationTime;
        private TimeSpan _activePreparationTime;
        private int _servings;
        private string _mainPhotoPath = string.Empty;
        private List<Guid> _referencedCategories = new List<Guid>();
        private List<Ingredient> _ingredients = new List<Ingredient>();
        private List<Step> _preparationSteps = new List<Step>();
        private DateTime _creationDate;
        private DateTime _lastEdited;
        private bool _isPublic;
        private double _rating;

        public Guid ID
        {
            get { return _id; }
            init { _id = value; }
        }
        public Guid AuthorID
        {
            get { return _authorID; }
            init { _authorID = value; }
        }
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
        public Guid InspiredBy
        {
            get { return _inspiredBy; }
            init { _inspiredBy = value; }
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
        public DateTime CreationDate
        {
            get { return _creationDate; }
            init { _creationDate = value; }
        }
        public DateTime LastEdited
        {
            get { return _lastEdited; }
            set { _lastEdited = value; }
        }
        public bool IsPublic
        {
            get { return _isPublic; }
            set { _isPublic = value; }
        }
        public double Rating
        {
            get { return _rating; }
            set { _rating = value; }
        }
    }
}
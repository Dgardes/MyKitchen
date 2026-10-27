using MyKitchen_Core.Interfaces;

namespace MyKitchen_Core.Entities
{
    public class CookBook :IEntity
    {
        private Guid _id;
        private Guid _authorID;
        private string _name = string.Empty;
        private DateTime _creationDate;
        private DateTime _lastEdited;
        private string? _description;
        private bool _isPublic;
        private List<Guid> _referencedRecipes = new List<Guid>();

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
        public string? Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public bool IsPublic
        {
            get { return _isPublic; }
            set { _isPublic = value; }
        }
        public List<Guid> ReferencedRecipes
        {
            get { return _referencedRecipes; }
            set { _referencedRecipes = value; }
        }
    }
}
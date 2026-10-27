
namespace MyKitchen_Core.DTO.CreateAndUpdateDTOs
{
    public class CreateCookBook_dto
    {
        private string _name = string.Empty;
        private string _description = string.Empty;
        private bool _isPublic;
        private List<Guid> _referencedRecipes = new List<Guid>();
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

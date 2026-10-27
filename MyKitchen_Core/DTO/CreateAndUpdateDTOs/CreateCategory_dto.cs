
namespace MyKitchen_Core.DTO.CreateAndUpdateDTOs
{
    public class CreateCategory_dto
    {
        private string _name = string.Empty;
        private string? _description;

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
    }
}

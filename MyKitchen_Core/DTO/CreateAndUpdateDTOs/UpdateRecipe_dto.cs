
namespace MyKitchen_Core.DTO.CreateAndUpdateDTOs
{
    public class UpdateRecipe_dto : CreateRecipe_dto
    {
        private Guid _id;
        public Guid ID
        {
            get { return _id; }
            set { _id = value; }
        }
    }
}

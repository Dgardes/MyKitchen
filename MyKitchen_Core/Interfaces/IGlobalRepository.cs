using MyKitchen_Core.Entities;
using MyKitchen_Core.Tools;

namespace MyKitchen_Core.Interfaces
{
    public interface IGlobalRepository : ILocalRepository
    {
        public Response<List<UserProfile>> GetUsers();
        public Response<UserProfile> GetUserById(Guid ID);
        public Response<UserProfile> GetUserByEmail(string email);
        public Response<bool> AddUser(UserProfile user);
    }
}

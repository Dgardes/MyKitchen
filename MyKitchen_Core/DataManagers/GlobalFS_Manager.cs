using MyKitchen_Core.Entities;
using MyKitchen_Core.Tools;
using MyKitchen_Core.Interfaces;
using Newtonsoft.Json;
using MyKitchen_Core.Services;
using MyKitchen_Core.DTO.SearchDTOs;

namespace MyKitchen_Core.DataManagers
{
    public class GlobalFS_Manager : LocalFS_Manager, IGlobalRepository
    {
        private readonly string usersFilePath;

        public GlobalFS_Manager(string globalDataPath) : base(globalDataPath)
        {
            usersFilePath = Path.Combine(globalDataPath, "Users.json");
        }

        public Response<List<UserProfile>> GetUsers()
        {
            if (!File.Exists(usersFilePath))
            {
                return Response<List<UserProfile>>.createResponse(null, "Файл користувачів порожній", 
                    Response<List<UserProfile>>.responseStatus.error);
            }
            try
            {
                List<UserProfile> users = JsonConvert.DeserializeObject<List<UserProfile>>(File.ReadAllText(usersFilePath)) ?? new List<UserProfile>();
                return Response<List<UserProfile>>.createResponse(users, "Користувачів успішно завантажено", 
                    Response<List<UserProfile>>.responseStatus.success);
            }
            catch (Exception ex)
            {
                return Response<List<UserProfile>>.createResponse(null, "Помилка читання бази користувачів: " + ex.Message, 
                    Response<List<UserProfile>>.responseStatus.error);
            }
        }

        public Response<UserProfile> GetUserByEmail(string email)
        {
            Response<List<UserProfile>> usersResponse = GetUsers();
            if (usersResponse.status == Response<List<UserProfile>>.responseStatus.error)
            {
                return Response<UserProfile>.createResponse(null, usersResponse.message, 
                    Response<UserProfile>.responseStatus.error);
            }

            UserProfile user = usersResponse.data.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Response<UserProfile>.createResponse(user, "Пошук завершено", 
                Response<UserProfile>.responseStatus.success);
        }

        public Response<UserProfile> GetUserById(Guid ID)
        {
            Response<List<UserProfile>> usersResponse = GetUsers();
            if (usersResponse.status == Response<List<UserProfile>>.responseStatus.error)
            {
                return Response<UserProfile>.createResponse(null, usersResponse.message, 
                    Response<UserProfile>.responseStatus.error);
            }
            UserProfile user = usersResponse.data.FirstOrDefault(u => u.ID == ID);
            return Response<UserProfile>.createResponse(user, "Пошук завершено", 
                Response<UserProfile>.responseStatus.success);
        }

        public Response<bool> AddUser(UserProfile user)
        {
            Response<List<UserProfile>> usersResponse = GetUsers();
            if (usersResponse.status == Response<List<UserProfile>>.responseStatus.error)
            {
                return Response<bool>.createResponse(false, usersResponse.message, 
                    Response<bool>.responseStatus.error);
            }

            List<UserProfile> users = usersResponse.data;
            users.Add(user);

            try
            {
                File.WriteAllText(usersFilePath, JsonConvert.SerializeObject(users, Formatting.Indented));
                return Response<bool>.createResponse(true, "Користувача успішно додано", 
                    Response<bool>.responseStatus.success);
            }
            catch (Exception ex)
            {
                return Response<bool>.createResponse(false, "Помилка запису користувача: " + ex.Message, 
                    Response<bool>.responseStatus.error);
            }
        }

        public override Response<List<Recipe>> SearchRecipes(SearchRecipeCriteria_dto criteria)
        {
            Response<List<Recipe>> allRecipesResponse = GetRecipes();
            if (allRecipesResponse.status == Response<List<Recipe>>.responseStatus.error)
            {
                return Response<List<Recipe>>.createResponse(null, "Помилка при завантаженні рецептів: " + allRecipesResponse.message, 
                    Response<List<Recipe>>.responseStatus.error);
            }
            Response<List<UserProfile>> usersResponse = GetUsers();
            if (usersResponse.status == Response<List<UserProfile>>.responseStatus.error)
            {
                return Response<List<Recipe>>.createResponse(null, "Помилка при завантаженні користувачів: " + usersResponse.message,
                    Response<List<Recipe>>.responseStatus.error);
            }
            Response<List<Product>> productsResponse = GetProducts();
            if (productsResponse.status == Response<List<Product>>.responseStatus.error)
            {
                return Response<List<Recipe>>.createResponse(null, "Помилка при завантаженні продуктів: " + productsResponse.message,
                    Response<List<Recipe>>.responseStatus.error);
            }
            Response<List<ProductAlias>> productAliasesResponse = GetProductAliases();
            if (productAliasesResponse.status == Response<List<ProductAlias>>.responseStatus.error)
            {
                return Response<List<Recipe>>.createResponse(null, "Помилка при завантаженні псевдонімів продуктів: " + productAliasesResponse.message,
                    Response<List<Recipe>>.responseStatus.error);
            }

            return Filter.Instance.FilterGlobalRecipes(criteria, allRecipesResponse.data, productsResponse.data, productAliasesResponse.data, usersResponse.data);
        }

        public override Response<List<CookBook>> SearchCookBooks(SearchCookBookCriteria_dto criteria)
        {
            Response<List<CookBook>> allCookBooksResponse = GetCookBooks();
            if (allCookBooksResponse.status == Response<List<CookBook>>.responseStatus.error)
            {
                return Response<List<CookBook>>.createResponse(null, "Помилка при завантаженні кулінарних книг: " + allCookBooksResponse.message, 
                    Response<List<CookBook>>.responseStatus.error);
            }
            Response<List<UserProfile>> usersResponse = GetUsers();
            if (usersResponse.status == Response<List<UserProfile>>.responseStatus.error)
            {
                return Response<List<CookBook>>.createResponse(null, "Помилка при завантаженні користувачів: " + usersResponse.message,
                    Response<List<CookBook>>.responseStatus.error);
            }
            
            Response<List<Recipe>> recipesResponse = GetRecipes();
            if (recipesResponse.status == Response<List<Recipe>>.responseStatus.error)
            {
                return Response<List<CookBook>>.createResponse(null, "Помилка при завантаженні рецептів: " + recipesResponse.message,
                    Response<List<CookBook>>.responseStatus.error);
            }
            List<Recipe> globalRecipes = recipesResponse.data ?? new List<Recipe>();

            return Filter.Instance.FilterGlobalCookBooks(criteria, allCookBooksResponse.data, usersResponse.data, globalRecipes);
        }
    }
}
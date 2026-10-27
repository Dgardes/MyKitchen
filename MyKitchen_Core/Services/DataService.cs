using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyKitchen_Core.Entities;
using MyKitchen_Core.Interfaces;
using MyKitchen_Core.DataManagers;
using MyKitchen_Core.DTO.CreateAndUpdateDTOs;
using MyKitchen_Core.Tools;

using MyKitchen_Core.DTO.SearchDTOs;

namespace MyKitchen_Core.Services
{
    public class DataService
    {
        private ILocalRepository localRepo;
        private IGlobalRepository globalRepo;

        public enum DataScope
        {
            Local,
            Global
        }

        private static DataService _instance;
        private DataService() { }
        public static DataService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataService();
                }
                return _instance;
            }
        }

        #region Get Methods
        public Response<List<Recipe>> GetRecipes(DataScope scope)
        {
            Response<List<Recipe>> recipesResponse;
            switch(scope)
            {
                case DataScope.Local:
                    recipesResponse = localRepo.GetRecipes();
                    break;
                case DataScope.Global:
                    recipesResponse = globalRepo.GetRecipes();
                    break;
                default:
                    return Response<List<Recipe>>.createResponse(null, "Невідомий обсяг даних", 
                        Response<List<Recipe>>.responseStatus.error);
            }
             
            if(recipesResponse.status == Response<List<Recipe>>.responseStatus.error)
            {
                return Response<List<Recipe>>.createResponse(null, recipesResponse.message,
                    Response<List<Recipe>>.responseStatus.error);
            }
            List<Recipe> recipes = recipesResponse.data;
            List<Recipe> validRecipes = new List<Recipe>();
        
            string warningMessage = "";
            foreach(Recipe recipe in recipes)
            {
                Response<Recipe> validationResponse = Validator.Instance.validateRecipe(recipe);
                if (validationResponse.status == Response<Recipe>.responseStatus.error)
                {
                    warningMessage += "Рецепт " + recipe.ID + " має помилки в даних: " + validationResponse.message;
                    continue;
                }
                else if (validationResponse.status == Response<Recipe>.responseStatus.warning)
                {
                    warningMessage += "Рецепт " + recipe.ID + " має потенційні проблеми в даних: "  + validationResponse.message;
                }
                validRecipes.Add(recipe);
            }
            if(!string.IsNullOrEmpty(warningMessage))
            {
                return Response<List<Recipe>>.createResponse(validRecipes, "Знайдено помилки: " + warningMessage, 
                    Response<List<Recipe>>.responseStatus.warning);
            }
            else
            { 
                return Response<List<Recipe>>.createResponse(validRecipes, "Рецепти валідовано успішно", 
                    Response<List<Recipe>>.responseStatus.success);
            }
        }

        public Response<List<CookBook>> GetCookBooks(DataScope scope)
        {
            Response<List<CookBook>> cookBooksResponse;
            switch (scope)
            {
                case DataScope.Local:
                    cookBooksResponse = localRepo.GetCookBooks();
                    break;
                case DataScope.Global:
                    cookBooksResponse = globalRepo.GetCookBooks();
                    break;
                default:
                    return Response<List<CookBook>>.createResponse(null, "Невідомий обсяг даних", 
                        Response<List<CookBook>>.responseStatus.error);
            }

            if(cookBooksResponse.status == Response<List<CookBook>>.responseStatus.error)
            {
                return Response<List<CookBook>>.createResponse(null, cookBooksResponse.message,
                    Response<List<CookBook>>.responseStatus.error);
            }
            List<CookBook> cookBooks = cookBooksResponse.data;
            List<CookBook> validCookBooks = new List<CookBook>();
        
            string warningMessage = "";
            foreach(CookBook cookBook in cookBooks)
            {
                Response<CookBook> validationResponse = Validator.Instance.validateCookBook(cookBook);
                if (validationResponse.status == Response<CookBook>.responseStatus.error)
                {
                    warningMessage += "Збірка " + cookBook.ID + " має помилки в даних: " + validationResponse.message;
                    continue;
                }
                else if (validationResponse.status == Response<CookBook>.responseStatus.warning)
                {
                    warningMessage += "Збірка " + cookBook.ID + " має потенційні проблеми в даних: "  + validationResponse.message;
                }
                validCookBooks.Add(cookBook);
            }
            if(!string.IsNullOrEmpty(warningMessage))
            {
                return Response<List<CookBook>>.createResponse(validCookBooks, "Знайдено помилки: " + warningMessage, 
                    Response<List<CookBook>>.responseStatus.warning);
            }
            else
            { 
                return Response<List<CookBook>>.createResponse(validCookBooks, "Збірки валідовано успішно", 
                    Response<List<CookBook>>.responseStatus.success);
            }
        }

        public Response<List<Product>> GetProducts(DataScope scope)
        {
            Response<List<Product>> productsResponse;
            switch (scope)
            {
                case DataScope.Local:
                    productsResponse = localRepo.GetProducts();
                    break;
                case DataScope.Global:
                    productsResponse = globalRepo.GetProducts();
                    break;
                default:
                    return Response<List<Product>>.createResponse(null, "Невідомий обсяг даних", 
                        Response<List<Product>>.responseStatus.error);
            }

            if(productsResponse.status == Response<List<Product>>.responseStatus.error)
            {
                return Response<List<Product>>.createResponse(null, productsResponse.message,
                    Response<List<Product>>.responseStatus.error);
            }
            List<Product> products = productsResponse.data;
            List<Product> validProducts = new List<Product>();
        
            string warningMessage = "";
            foreach(Product product in products)
            {
                Response<Product> validationResponse = Validator.Instance.validateProduct(product);
                if (validationResponse.status == Response<Product>.responseStatus.error)
                {
                    warningMessage += "Продукт " + product.ID + " має помилки в даних: " + validationResponse.message;
                    continue;
                }
                else if (validationResponse.status == Response<Product>.responseStatus.warning)
                {
                    warningMessage += "Продукт " + product.ID + " має потенційні проблеми в даних: "  + validationResponse.message;
                }
                validProducts.Add(product);
            }
            if(!string.IsNullOrEmpty(warningMessage))
            {
                return Response<List<Product>>.createResponse(validProducts, "Знайдено помилки: " + warningMessage, 
                    Response<List<Product>>.responseStatus.warning);
            }
            else
            { 
                return Response<List<Product>>.createResponse(validProducts, "Продукти валідовано успішно", 
                    Response<List<Product>>.responseStatus.success);
            }
        }
        
        public Response<List<ProductAlias>> GetProductAliases(DataScope scope)
        {
            Response<List<ProductAlias>> productAliasesResponse;
            switch (scope)
            {
                case DataScope.Local:
                    productAliasesResponse = localRepo.GetProductAliases();
                    break;
                case DataScope.Global:
                    productAliasesResponse = globalRepo.GetProductAliases();
                    break;
                default:
                    return Response<List<ProductAlias>>.createResponse(null, "Невідомий обсяг даних",
                        Response<List<ProductAlias>>.responseStatus.error);
            }
            
            if(productAliasesResponse.status == Response<List<ProductAlias>>.responseStatus.error)
            {
                return Response<List<ProductAlias>>.createResponse(null, productAliasesResponse.message,
                    Response<List<ProductAlias>>.responseStatus.error);
            }
            List<ProductAlias> productAliases = productAliasesResponse.data;
            List<ProductAlias> validProductAliases = new List<ProductAlias>();
        
            string warningMessage = "";
            foreach(ProductAlias productAlias in productAliases)
            {
                Response<ProductAlias> validationResponse = Validator.Instance.validateProductAlias(productAlias);
                if (validationResponse.status == Response<ProductAlias>.responseStatus.error)
                {
                    warningMessage += "Синонім продукту " + productAlias.ID + " має помилки в даних: " + validationResponse.message;
                    continue;
                }
                else if (validationResponse.status == Response<ProductAlias>.responseStatus.warning)
                {
                    warningMessage += "Синонім продукту " + productAlias.ID + " має потенційні проблеми в даних: "  + validationResponse.message;
                }
                validProductAliases.Add(productAlias);
            }
            if(!string.IsNullOrEmpty(warningMessage))
            {
                return Response<List<ProductAlias>>.createResponse(validProductAliases, "Знайдено помилки: " + warningMessage, 
                    Response<List<ProductAlias>>.responseStatus.warning);
            }
            else
            { 
                return Response<List<ProductAlias>>.createResponse(validProductAliases, "Синоніми продуктів валідовано успішно", 
                    Response<List<ProductAlias>>.responseStatus.success);
            }
        }

        public Response<List<Category>> GetCategories(DataScope scope)
        {
            Response<List<Category>> categoriesResponse;
            switch (scope)
            {
                case DataScope.Local:
                    categoriesResponse = localRepo.GetCategories();
                    break;
                case DataScope.Global:
                    categoriesResponse = globalRepo.GetCategories();
                    break;
                default:
                    return Response<List<Category>>.createResponse(null, "Невідомий обсяг даних",
                        Response<List<Category>>.responseStatus.error);
            }    

            if(categoriesResponse.status == Response<List<Category>>.responseStatus.error)
            {
                return Response<List<Category>>.createResponse(null, categoriesResponse.message,
                    Response<List<Category>>.responseStatus.error);
            }
            List<Category> categories = categoriesResponse.data;
            List<Category> validCategories = new List<Category>();

            string warningMessage = "";
            foreach(Category category in categories)
            {
                Response<Category> validationResponse = Validator.Instance.validateCategory(category);
                if (validationResponse.status == Response<Category>.responseStatus.error)
                {
                    warningMessage += "Категорія " + category.ID + " має помилки в даних: " + validationResponse.message;
                    continue;
                }
                else if (validationResponse.status == Response<Category>.responseStatus.warning)
                {
                    warningMessage += "Категорія " + category.ID + " має потенційні проблеми в даних: "  + validationResponse.message;
                }
                validCategories.Add(category);
            }
            if(!string.IsNullOrEmpty(warningMessage))
            {
                return Response<List<Category>>.createResponse(validCategories, "Знайдено помилки: " + warningMessage, 
                    Response<List<Category>>.responseStatus.warning);
            }
            else
            { 
                return Response<List<Category>>.createResponse(validCategories, "Категорії валідовано успішно", 
                    Response<List<Category>>.responseStatus.success);
            }
        }

        #endregion

        #region Get by ID Methods
        public Response<Recipe> GetRecipe(Guid ID, DataScope scope)
        {
            Response<Recipe> recipeResponse;
            switch (scope)
            {
                case DataScope.Local:
                    recipeResponse = localRepo.GetRecipeByID(ID);
                    break;
                case DataScope.Global:
                    recipeResponse = globalRepo.GetRecipeByID(ID);
                    break;
                default:
                    return Response<Recipe>.createResponse(null, "Невідомий обсяг даних", 
                        Response<Recipe>.responseStatus.error);
            }

            if(recipeResponse.status == Response<Recipe>.responseStatus.error)
            {
                return Response<Recipe>.createResponse(null, recipeResponse.message,
                    Response<Recipe>.responseStatus.error);
            }
            Response<Recipe> validationResponse = Validator.Instance.validateRecipe(recipeResponse.data);
            if (validationResponse.status == Response<Recipe>.responseStatus.error)
            {
                return Response<Recipe>.createResponse(null, validationResponse.message, 
                    Response<Recipe>.responseStatus.error);
            }
            else if (validationResponse.status == Response<Recipe>.responseStatus.warning)
            {
                return Response<Recipe>.createResponse(validationResponse.data, "Рецепт отримано, але були виправлені помилки: " + validationResponse.message, 
                    Response<Recipe>.responseStatus.warning);
            }

            return Response<Recipe>.createResponse(validationResponse.data, "Рецепт успішно отримано", 
                Response<Recipe>.responseStatus.success);
        }

        public Response<CookBook> GetCookBook(Guid ID, DataScope scope)
        {
            Response<CookBook> cookBookResponse;
            switch (scope)
            {
                case DataScope.Local:
                    cookBookResponse = localRepo.GetCookBookByID(ID);
                    break;
                case DataScope.Global:
                    cookBookResponse = globalRepo.GetCookBookByID(ID);
                    break;
                default:
                    return Response<CookBook>.createResponse(null, "Невідомий обсяг даних",
                        Response<CookBook>.responseStatus.error);
            }

            if(cookBookResponse.status == Response<CookBook>.responseStatus.error)
            {
                return Response<CookBook>.createResponse(null, cookBookResponse.message,
                    Response<CookBook>.responseStatus.error);
            }
            Response<CookBook> validationResponse = Validator.Instance.validateCookBook(cookBookResponse.data);
            if (validationResponse.status == Response<CookBook>.responseStatus.error)
            {
                return Response<CookBook>.createResponse(null, validationResponse.message,
                    Response<CookBook>.responseStatus.error);
            }
            return Response<CookBook>.createResponse(validationResponse.data, "Кулінарна книга успішно отримана", 
                Response<CookBook>.responseStatus.success);
        }

        public Response<Product> GetProduct(Guid ID, DataScope scope)
        {
            Response<Product> productResponse;
            switch (scope)
            {
                case DataScope.Local:
                    productResponse = localRepo.GetProductByID(ID);
                    break;
                case DataScope.Global:
                    productResponse = globalRepo.GetProductByID(ID);
                    break;
                default:
                    return Response<Product>.createResponse(null, "Невідомий обсяг даних",
                        Response<Product>.responseStatus.error);
            }

            if(productResponse.status == Response<Product>.responseStatus.error)
            {
                return Response<Product>.createResponse(null, productResponse.message,
                    Response<Product>.responseStatus.error);
            }
            Response<Product> validationResponse = Validator.Instance.validateProduct(productResponse.data);
            if (validationResponse.status == Response<Product>.responseStatus.error)
            {
                return Response<Product>.createResponse(null, validationResponse.message,
                    Response<Product>.responseStatus.error);
            }
            return Response<Product>.createResponse(validationResponse.data, "Продукт успішно отримано", 
                Response<Product>.responseStatus.success);
        }

        public Response<ProductAlias> GetProductAlias(Guid ID, DataScope scope)
        {
            Response<ProductAlias> productAliasResponse;
            switch (scope)
            {
                case DataScope.Local:
                    productAliasResponse = localRepo.GetProductAliasByID(ID);
                    break;
                case DataScope.Global:
                    productAliasResponse = globalRepo.GetProductAliasByID(ID);
                    break;
                default:
                    return Response<ProductAlias>.createResponse(null, "Невідомий обсяг даних",
                        Response<ProductAlias>.responseStatus.error);
            }

            if(productAliasResponse.status == Response<ProductAlias>.responseStatus.error)
            {
                return Response<ProductAlias>.createResponse(null, productAliasResponse.message,
                    Response<ProductAlias>.responseStatus.error);
            }
            Response<ProductAlias> validationResponse = Validator.Instance.validateProductAlias(productAliasResponse.data);
            if (validationResponse.status == Response<ProductAlias>.responseStatus.error)
            {
                return Response<ProductAlias>.createResponse(null, validationResponse.message,
                    Response<ProductAlias>.responseStatus.error);
            }
            return Response<ProductAlias>.createResponse(validationResponse.data, "Аліас продукту успішно отримано",
                Response<ProductAlias>.responseStatus.success);
        }

        public Response<Category> GetCategory(Guid ID, DataScope scope)
        {
            Response<Category> categoryResponse;
            switch (scope)
            {
                case DataScope.Local:
                    categoryResponse = localRepo.GetCategoryByID(ID);
                    break;
                case DataScope.Global:
                    categoryResponse = globalRepo.GetCategoryByID(ID);
                    break;
                default:
                    return Response<Category>.createResponse(null, "Невідомий обсяг даних",
                        Response<Category>.responseStatus.error);
            }

            if(categoryResponse.status == Response<Category>.responseStatus.error)
            {
                return Response<Category>.createResponse(null, categoryResponse.message,
                    Response<Category>.responseStatus.error);
            }
            Response<Category> validationResponse = Validator.Instance.validateCategory(categoryResponse.data);
            if (validationResponse.status == Response<Category>.responseStatus.error)
            {
                return Response<Category>.createResponse(null, validationResponse.message,
                    Response<Category>.responseStatus.error);
            }
            return Response<Category>.createResponse(validationResponse.data, "Категорія успішно отримана",
                Response<Category>.responseStatus.success);
        }
        #endregion

        #region Search Methods

        public Response<List<Recipe>> SearchRecipes(SearchRecipeCriteria_dto criteria, DataScope scope)
        {
            Response<List<Recipe>> recipesResponse;
            switch (scope)
            {
                case DataScope.Local:
                    recipesResponse = localRepo.SearchRecipes(criteria);
                    break;
                case DataScope.Global:
                    recipesResponse = globalRepo.SearchRecipes(criteria);
                    break;
                default:
                    return Response<List<Recipe>>.createResponse(null, "Невідомий обсяг даних",
                        Response<List<Recipe>>.responseStatus.error);
            }

            if (recipesResponse.status == Response<List<Recipe>>.responseStatus.error)
            {
                return recipesResponse;
            }

            List<Recipe> recipes = recipesResponse.data ?? new List<Recipe>();
            List<Recipe> validRecipes = new List<Recipe>();
        
            string warningMessage = "";

            foreach (Recipe recipe in recipes)
            {
                if (recipe == null) continue;

                Response<Recipe> validationResponse = Validator.Instance.validateRecipe(recipe);
                
                if (validationResponse.status == Response<Recipe>.responseStatus.error)
                {
                    warningMessage += "Рецепт " + recipe.ID + " відсіяно через помилки: " + validationResponse.message + "\n";
                    continue;
                }
                else if (validationResponse.status == Response<Recipe>.responseStatus.warning)
                {
                    warningMessage += "Рецепт " + recipe.ID + " має зауваження: " + validationResponse.message + "\n";
                }

                validRecipes.Add(recipe);
            }
            if (!string.IsNullOrEmpty(warningMessage))
            {
                return Response<List<Recipe>>.createResponse(validRecipes, "Знайдено помилки при валідації сутностей:\n" + warningMessage, 
                    Response<List<Recipe>>.responseStatus.warning);
            }
            else
            { 
                return Response<List<Recipe>>.createResponse(validRecipes, "Рецепти знайдено та санітаровано успішно.", 
                    Response<List<Recipe>>.responseStatus.success);
            }
        }

        public Response<List<CookBook>> SearchCookBooks(SearchCookBookCriteria_dto criteria, DataScope scope)
        {
            Response<List<CookBook>> cookBooksResponse;
            switch (scope)
            {
                case DataScope.Local:
                    cookBooksResponse = localRepo.SearchCookBooks(criteria);
                    break;
                case DataScope.Global:
                    cookBooksResponse = globalRepo.SearchCookBooks(criteria);
                    break;
                default:
                    return Response<List<CookBook>>.createResponse(null, "Невідомий обсяг даних",
                        Response<List<CookBook>>.responseStatus.error);
            }
            if (cookBooksResponse.status == Response<List<CookBook>>.responseStatus.error)
            {
                return cookBooksResponse;
            }

            List<CookBook> cookBooks = cookBooksResponse.data ?? new List<CookBook>();
            List<CookBook> validCookBooks = new List<CookBook>();
        
            string warningMessage = "";

            foreach (CookBook cookBook in cookBooks)
            {
                if (cookBook == null) continue;

                Response<CookBook> validationResponse = Validator.Instance.validateCookBook(cookBook);

                if (validationResponse.status == Response<CookBook>.responseStatus.error)
                {
                    warningMessage += "Книгу рецептів " + cookBook.ID + " відсіяно через помилки: " + validationResponse.message + "\n";
                    continue;
                }
                else if (validationResponse.status == Response<CookBook>.responseStatus.warning)
                {
                    warningMessage += "Книгу рецептів " + cookBook.ID + " має зауваження: " + validationResponse.message + "\n";
                }

                validCookBooks.Add(cookBook);
            }
            if (!string.IsNullOrEmpty(warningMessage))
            {
                return Response<List<CookBook>>.createResponse(validCookBooks, "Знайдено помилки при валідації сутностей:\n" + warningMessage,
                    Response<List<CookBook>>.responseStatus.warning);
            }
            else
            {
                return Response<List<CookBook>>.createResponse(validCookBooks, "Книги рецептів знайдено та санітаровано успішно.",
                    Response<List<CookBook>>.responseStatus.success);
            }
        }

        public Response<List<Product>> SearchProducts(SearchProductCriteria_dto criteria, DataScope scope)
        {
            Response<List<Product>> productsResponse;
            switch (scope)
            {
                case DataScope.Local:
                    productsResponse = localRepo.SearchProducts(criteria);
                    break;
                case DataScope.Global:
                    productsResponse = globalRepo.SearchProducts(criteria);
                    break;
                default:
                    return Response<List<Product>>.createResponse(null, "Невідомий обсяг даних",
                        Response<List<Product>>.responseStatus.error);
            }
            if (productsResponse.status == Response<List<Product>>.responseStatus.error)
            {
                return productsResponse;
            }

            List<Product> products = productsResponse.data ?? new List<Product>();
            List<Product> validProducts = new List<Product>();
        
            string warningMessage = "";

            foreach (Product product in products)
            {
                if (product == null) continue;

                Response<Product> validationResponse = Validator.Instance.validateProduct(product);

                if (validationResponse.status == Response<Product>.responseStatus.error)
                {
                    warningMessage += "Продукт " + product.ID + " відсіяно через помилки: " + validationResponse.message + "\n";
                    continue;
                }
                else if (validationResponse.status == Response<Product>.responseStatus.warning)
                {
                    warningMessage += "Продукт " + product.ID + " має зауваження: " + validationResponse.message + "\n";
                }

                validProducts.Add(product);
            }
            if (!string.IsNullOrEmpty(warningMessage))
            {
                return Response<List<Product>>.createResponse(validProducts, "Знайдено помилки при валідації сутностей:\n" + warningMessage,
                    Response<List<Product>>.responseStatus.warning);
            }
            else
            {
                return Response<List<Product>>.createResponse(validProducts, "Продукти знайдено та санітаровано успішно.",
                    Response<List<Product>>.responseStatus.success);
            }
        }

        public Response<List<ProductAlias>> SearchProductAliases(SearchProductAliasCriteria_dto criteria, DataScope scope)
        {
            Response<List<ProductAlias>> aliasesResponse;
            switch (scope)
            {
                case DataScope.Local:
                    aliasesResponse = localRepo.SearchProductAliases(criteria);
                    break;
                case DataScope.Global:
                    aliasesResponse = globalRepo.SearchProductAliases(criteria);
                    break;
                default:
                    return Response<List<ProductAlias>>.createResponse(null, "Невідомий обсяг даних",
                        Response<List<ProductAlias>>.responseStatus.error);
            }
            if (aliasesResponse.status == Response<List<ProductAlias>>.responseStatus.error)
            {
                return aliasesResponse;
            }

            List<ProductAlias> aliases = aliasesResponse.data ?? new List<ProductAlias>();
            List<ProductAlias> validAliases = new List<ProductAlias>();

            string warningMessage = "";

            foreach (ProductAlias alias in aliases)
            {
                if (alias == null) continue;

                Response<ProductAlias> validationResponse = Validator.Instance.validateProductAlias(alias);

                if (validationResponse.status == Response<ProductAlias>.responseStatus.error)
                {
                    warningMessage += "Псевдонім " + alias.ID + " відсіяно через помилки: " + validationResponse.message + "\n";
                    continue;
                }
                else if (validationResponse.status == Response<ProductAlias>.responseStatus.warning)
                {
                    warningMessage += "Псевдонім " + alias.ID + " має зауваження: " + validationResponse.message + "\n";
                }

                validAliases.Add(alias);
            }
            if (!string.IsNullOrEmpty(warningMessage))
            {
                return Response<List<ProductAlias>>.createResponse(validAliases, "Знайдено помилки при валідації сутностей:\n" + warningMessage,
                    Response<List<ProductAlias>>.responseStatus.warning);
            }
            else
            {
                return Response<List<ProductAlias>>.createResponse(validAliases, "Псевдоніми знайдено та санітаровано успішно.",
                    Response<List<ProductAlias>>.responseStatus.success);
            }
        }

        public Response<List<Category>> SearchCategories(SearchCategoryCriteria_dto criteria, DataScope scope)
        {
            Response<List<Category>> categoriesResponse;
            switch (scope)
            {
                case DataScope.Local:
                    categoriesResponse = localRepo.SearchCategories(criteria);
                    break;
                case DataScope.Global:
                    categoriesResponse = globalRepo.SearchCategories(criteria);
                    break;
                default:
                    return Response<List<Category>>.createResponse(null, "Невідомий обсяг даних",
                        Response<List<Category>>.responseStatus.error);
            }
            if (categoriesResponse.status == Response<List<Category>>.responseStatus.error)
            {
                return categoriesResponse;
            }

            List<Category> categories = categoriesResponse.data ?? new List<Category>();
            List<Category> validCategories = new List<Category>();

            string warningMessage = "";

            foreach (Category category in categories)
            {
                if (category == null) continue;

                Response<Category> validationResponse = Validator.Instance.validateCategory(category);

                if (validationResponse.status == Response<Category>.responseStatus.error)
                {
                    warningMessage += "Категорія " + category.ID + " відсіяно через помилки: " + validationResponse.message + "\n";
                    continue;
                }
                else if (validationResponse.status == Response<Category>.responseStatus.warning)
                {
                    warningMessage += "Категорія " + category.ID + " має зауваження: " + validationResponse.message + "\n";
                }

                validCategories.Add(category);
            }
            if (!string.IsNullOrEmpty(warningMessage))
            {
                return Response<List<Category>>.createResponse(validCategories, "Знайдено помилки при валідації сутностей:\n" + warningMessage,
                    Response<List<Category>>.responseStatus.warning);
            }
            else
            {
                return Response<List<Category>>.createResponse(validCategories, "Категорії знайдено та санітаровано успішно.",
                    Response<List<Category>>.responseStatus.success);
            }
        }

        #endregion

        #region Add Methods
        public Response<Recipe> AddRecipe(CreateRecipe_dto dto)
        {
            string warningMessage = "";

            Response<CreateRecipe_dto> validationResponse = Validator.Instance.validateCreateRecipeDTO(dto);
            if (validationResponse.status == Response<CreateRecipe_dto>.responseStatus.error)
            {
                return Response<Recipe>.createResponse(null, validationResponse.message, 
                    Response<Recipe>.responseStatus.error);
            }
            if(AppSession.Instance.CurrentUser == null)
            {
                return Response<Recipe>.createResponse(null, "Користувач не авторизований. Не можна додати рецепт.",
                    Response<Recipe>.responseStatus.error);
            }

            Recipe newRecipe = new Recipe
            {
                ID = Guid.NewGuid(),
                AuthorID = AppSession.Instance.CurrentUser.ID,
                Name = dto.Name,
                Description = dto.Description,
                SourceLink = dto.SourceLink,
                FullPreparationTime = dto.FullPreparationTime,
                ActivePreparationTime = dto.ActivePreparationTime,
                Servings = dto.Servings,
                MainPhotoPath = dto.MainPhotoPath,
                ReferencedCategories = dto.ReferencedCategories,
                Ingredients = dto.Ingredients,
                PreparationSteps = dto.PreparationSteps,
                CreationDate = DateTime.Now,
                LastEdited = DateTime.Now,
                IsPublic = dto.IsPublic,
                Rating = 0,
                InspiredBy = Guid.Empty
            };

            Response<Recipe> addResponse = localRepo.AddRecipe(newRecipe);
            if(addResponse.status == Response<Recipe>.responseStatus.error)
            {
                return addResponse;
            }
            
            if(newRecipe.IsPublic)
            {
                Recipe globalClone = CreateGlobalRecipeClone(newRecipe);

                Response<Recipe> addToGlobalResponse = globalRepo.AddRecipe(globalClone);
                if(addToGlobalResponse.status == Response<Recipe>.responseStatus.error)
                {
                    warningMessage += "Рецепт було додано до локальної бази, але не вдалося опублікувати: " + addToGlobalResponse.message + "\n";
                }
            }

            if( dto.cookBooksReferencedIn != null && dto.cookBooksReferencedIn.Count > 0)
            {
                Response<List<CookBook>> cookBooksResponse = this.GetCookBooks(DataScope.Local);
                if (cookBooksResponse.status == Response<List<CookBook>>.responseStatus.error) 
                {
                    return Response<Recipe>.createResponse(null, "Помилка при перевірці збірок: " + cookBooksResponse.message, 
                        Response<Recipe>.responseStatus.error);
                }

                List<CookBook> cookBooksToUpdate = cookBooksResponse.data
                .Where(cb => dto.cookBooksReferencedIn.Contains(cb.ID)).ToList();
                
                foreach(CookBook cb in cookBooksToUpdate)
                { 

                    if (cb.IsPublic && !newRecipe.IsPublic)
                    {
                        warningMessage += $"Рецепт є приватним, тому його не було додано до вашої публічної збірки '{cb.Name}'.\n";
                        continue;
                    }

                    UpdateCookBook_dto updateCookBook_Dto = new UpdateCookBook_dto
                    {
                        ID = cb.ID,
                        Name = cb.Name,
                        Description = cb.Description,
                        IsPublic = cb.IsPublic,
                        ReferencedRecipes = cb.ReferencedRecipes.Append(newRecipe.ID).ToList()
                    };
                    
                    Response<CookBook> updateResponse = this.UpdateCookBook(updateCookBook_Dto);
                    if (updateResponse.status == Response<CookBook>.responseStatus.error)
                    {
                        warningMessage += "Рецепт не було додано до збірки " + cb.Name 
                                         + " через помилку: " + updateResponse.message + "\n";
                    }
                    if (updateResponse.status == Response<CookBook>.responseStatus.warning)
                    {
                        warningMessage += "Рецепт було додано до збірки " + cb.Name 
                                         + " але виникла наступна проблема: " + updateResponse.message + "\n";
                    }    
                }
            }

            if (validationResponse.status == Response<CreateRecipe_dto>.responseStatus.warning)
            {
                return Response<Recipe>.createResponse(newRecipe, validationResponse.message + "\n" + warningMessage, 
                    Response<Recipe>.responseStatus.warning);  
            }
            if (!string.IsNullOrEmpty(warningMessage))
            {
                return Response<Recipe>.createResponse(newRecipe, "Рецепт створено успішно, але виникли обмеження зв'язків: \n" + warningMessage, 
                    Response<Recipe>.responseStatus.warning);
            }
            return Response<Recipe>.createResponse(newRecipe, "Рецепт створено успішно.", 
                Response<Recipe>.responseStatus.success);
        }

        public Response<CookBook> AddCookBook(CreateCookBook_dto dto)
        {
            string warningMessage = "";
            Response<CreateCookBook_dto> validationResponse = Validator.Instance.validateCreateCookBookDTO(dto);
    
            if (validationResponse.status == Response<CreateCookBook_dto>.responseStatus.error)
            {
                return Response<CookBook>.createResponse(null, validationResponse.message, 
                    Response<CookBook>.responseStatus.error);
            }
            if (AppSession.Instance.CurrentUser == null)
            {
                return Response<CookBook>.createResponse(null, "Користувач не авторизований. Не можна додати збірку.",
                    Response<CookBook>.responseStatus.error);
            }

            List<Guid> recipeIDs = dto.ReferencedRecipes ?? new List<Guid>();

            if (dto.IsPublic)
            {
                foreach (Guid recipeId in recipeIDs)
                {
                    Response<Recipe> recipeRes = this.GetRecipe(recipeId, DataScope.Local);
                    if (recipeRes.status != Response<Recipe>.responseStatus.error && recipeRes.data != null)
                    {
                        Recipe recipe = recipeRes.data;

                        if (!recipe.IsPublic)
                        {
                            UpdateRecipe_dto updateRecipeDto = new UpdateRecipe_dto
                            {
                                ID = recipe.ID,
                                Name = recipe.Name,
                                Description = recipe.Description,
                                SourceLink = recipe.SourceLink,
                                FullPreparationTime = recipe.FullPreparationTime,
                                ActivePreparationTime = recipe.ActivePreparationTime,
                                Servings = recipe.Servings,
                                MainPhotoPath = recipe.MainPhotoPath,
                                ReferencedCategories = recipe.ReferencedCategories,
                                Ingredients = recipe.Ingredients,
                                PreparationSteps = recipe.PreparationSteps,
                                IsPublic = true
                            };

                            Response<Recipe> recipeUpdateRes = this.UpdateRecipe(updateRecipeDto);

                            if (recipeUpdateRes.status == Response<Recipe>.responseStatus.success || 
                                recipeUpdateRes.status == Response<Recipe>.responseStatus.warning)
                            {
                                warningMessage += $"Рецепт '{recipe.Name}' автоматично переведено в статус публічного через сервіс оновлення.\n";
                            }
                            else
                            {
                                warningMessage += $"Не вдалося автоматично опублікувати рецепт '{recipe.Name}': {recipeUpdateRes.message}\n";
                            }
                        }
                    }
                }
            }

            CookBook newCookBook = new CookBook
            {
                ID = Guid.NewGuid(),
                AuthorID = AppSession.Instance.CurrentUser.ID,
                Name = dto.Name,
                Description = dto.Description,
                CreationDate = DateTime.Now,
                LastEdited = DateTime.Now,
                IsPublic = dto.IsPublic,
                ReferencedRecipes = recipeIDs
            };

            Response<CookBook> addResponse = localRepo.AddCookBook(newCookBook);
            if (addResponse.status == Response<CookBook>.responseStatus.error)
            {
                return addResponse; 
            }

            if (newCookBook.IsPublic)
            {
                CookBook globalClone = CreateGlobalCookBookClone(newCookBook);
                Response<CookBook> addToGlobalResponse = globalRepo.AddCookBook(globalClone);
                if (addToGlobalResponse.status == Response<CookBook>.responseStatus.error)
                {
                    warningMessage += "Збірку було додано до локальної бази, але не вдалося опублікувати: " + addToGlobalResponse.message + "\n";
                }
            }

            if (validationResponse.status == Response<CreateCookBook_dto>.responseStatus.warning)
            {
                warningMessage += validationResponse.message + "\n";
            }

            if (!string.IsNullOrEmpty(warningMessage))
            {
                return Response<CookBook>.createResponse(addResponse.data, "Збірку створено успішно з каскадними змінами: \n" + warningMessage, 
                    Response<CookBook>.responseStatus.warning);
            }

            return Response<CookBook>.createResponse(addResponse.data, "Збірку створено успішно.", 
                Response<CookBook>.responseStatus.success);
        }

        public Response<Product> AddProduct(CreateProduct_dto dto)
        {
            string warningMessage = "";
            Response<CreateProduct_dto> validationResponse = Validator.Instance.validateCreateProductDTO(dto);
            if (validationResponse.status == Response<CreateProduct_dto>.responseStatus.error)
            {
                return Response<Product>.createResponse(null, validationResponse.message, 
                    Response<Product>.responseStatus.error);
            }

            Product newProduct = new Product
            {
                ID = Guid.NewGuid(),
                Name = dto.Name,
                Calories = dto.Calories ?? 0,
                Proteins = dto.Proteins ?? 0,
                Fats = dto.Fats ?? 0,
                Carbohydrates = dto.Carbohydrates ?? 0,
                IsCustom = true
            };
            Response<Product> addResponse = localRepo.AddProduct(newProduct);
            if (addResponse.status == Response<Product>.responseStatus.error)
            {
                return addResponse;
            }
            if (validationResponse.status == Response<CreateProduct_dto>.responseStatus.warning)
            {
                warningMessage += validationResponse.message + "\n";
            }
            if (!string.IsNullOrEmpty(warningMessage))
            {
                return Response<Product>.createResponse(addResponse.data, "Продукт створено успішно, але виникли наступні проблеми: \n" + warningMessage, 
                    Response<Product>.responseStatus.warning);
            }
            return Response<Product>.createResponse(addResponse.data, "Продукт створено успішно.", 
                Response<Product>.responseStatus.success);
        }

        public Response<ProductAlias> AddProductAlias(CreateProductAlias_dto dto)
        {
            string warningMessage = "";
            Response<CreateProductAlias_dto> validationResponse = Validator.Instance.validateCreateProductAliasDTO(dto);
            if (validationResponse.status == Response<CreateProductAlias_dto>.responseStatus.error)
            {
                return Response<ProductAlias>.createResponse(null, validationResponse.message,
                    Response<ProductAlias>.responseStatus.error);
            }

            ProductAlias newProductAlias = new ProductAlias
            {
                ID = Guid.NewGuid(),
                AliasName = dto.AliasName,
                ReferencedProduct = dto.ReferencedProduct
            };
            Response<ProductAlias> addResponse = localRepo.AddProductAlias(newProductAlias);
            if (addResponse.status == Response<ProductAlias>.responseStatus.error)
            {
                return addResponse;
            }

            if (validationResponse.status == Response<CreateProductAlias_dto>.responseStatus.warning)
            {
                return Response<ProductAlias>.createResponse(addResponse.data, validationResponse.message,
                    Response<ProductAlias>.responseStatus.warning);
            }
            return Response<ProductAlias>.createResponse(addResponse.data, "Аліас продукту створено успішно.",
                Response<ProductAlias>.responseStatus.success);
        }

        public Response<Category> AddCategory(CreateCategory_dto dto)
        {
            Response<CreateCategory_dto> validationResponse = Validator.Instance.validateCreateCategoryDTO(dto);
            if (validationResponse.status == Response<CreateCategory_dto>.responseStatus.error)
            {
                return Response<Category>.createResponse(null, validationResponse.message,
                    Response<Category>.responseStatus.error);
            }

            Category newCategory = new Category
            {
                ID = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsCustom = true
            };
            Response<Category> addResponse = localRepo.AddCategory(newCategory);
            if (addResponse.status == Response<Category>.responseStatus.error)
            {
                return addResponse;
            }
            if (validationResponse.status == Response<CreateCategory_dto>.responseStatus.warning)
            {
                return Response<Category>.createResponse(addResponse.data, validationResponse.message,
                    Response<Category>.responseStatus.warning);
            }
            return Response<Category>.createResponse(addResponse.data, "Категорію створено успішно.",
                Response<Category>.responseStatus.success);
        }

        #endregion

        #region Import from Global Methods
        public Response<Recipe> ImportGlobalRecipe(Guid globalRecipeID)
        {
            if (AppSession.Instance.CurrentUser == null)
            {
                return Response<Recipe>.createResponse(null, "Користувач не авторизований. Неможливо імпортувати рецепт.",
                    Response<Recipe>.responseStatus.error);
            }

            Response<Recipe> globalRecipeResponse = this.GetRecipe(globalRecipeID, DataScope.Global);
            if (globalRecipeResponse.status == Response<Recipe>.responseStatus.error || globalRecipeResponse.data == null)
            {
                return Response<Recipe>.createResponse(null, "Не вдалося знайти глобальний рецепт для імпорту: " + globalRecipeResponse.message,
                    Response<Recipe>.responseStatus.error);
            }

            Recipe originalRecipe = globalRecipeResponse.data;

            if (originalRecipe.AuthorID == AppSession.Instance.CurrentUser.ID)
            {
                return Response<Recipe>.createResponse(null, "Ви вже є автором цього рецепту у глобальній базі.",
                    Response<Recipe>.responseStatus.warning);
            }

            Recipe clonedRecipe = new Recipe
            {
                ID = Guid.NewGuid(), 
                AuthorID = AppSession.Instance.CurrentUser.ID,
                Name = originalRecipe.Name, 
                Description = originalRecipe.Description,
                SourceLink = originalRecipe.SourceLink,
                FullPreparationTime = originalRecipe.FullPreparationTime,
                ActivePreparationTime = originalRecipe.ActivePreparationTime,
                Servings = originalRecipe.Servings,
                MainPhotoPath = originalRecipe.MainPhotoPath,
                
                ReferencedCategories = originalRecipe.ReferencedCategories?.ToList() ?? new List<Guid>(),
                PreparationSteps = originalRecipe.PreparationSteps?.ToList() ?? new List<Step>(),
                Ingredients = originalRecipe.Ingredients?.ToList() ?? new List<Ingredient>(),
                
                CreationDate = DateTime.Now,
                LastEdited = DateTime.Now,
                IsPublic = false,
                Rating = 0,

                InspiredBy = originalRecipe.ID 
            };

            Response<Recipe> addResponse = localRepo.AddRecipe(clonedRecipe);
            if (addResponse.status == Response<Recipe>.responseStatus.error)
            {
                return addResponse;
            }

            return Response<Recipe>.createResponse(clonedRecipe, "Рецепт  " + originalRecipe.Name + " успішно запозичено до вашої книги.",
                Response<Recipe>.responseStatus.success);
        }

        public Response<CookBook> ImportGlobalCookBook(Guid globalCookBookID)
        {
            if (AppSession.Instance.CurrentUser == null)
            {
                return Response<CookBook>.createResponse(null, "Користувач не авторизований. Неможливо імпортувати збірку.",
                    Response<CookBook>.responseStatus.error);
            }

            Response<CookBook> globalBookResponse = this.GetCookBook(globalCookBookID, DataScope.Global);
            if (globalBookResponse.status == Response<CookBook>.responseStatus.error || globalBookResponse.data == null)
            {
                return Response<CookBook>.createResponse(null, "Не вдалося знайти глобальну збірку: " + globalBookResponse.message,
                    Response<CookBook>.responseStatus.error);
            }

            CookBook originalBook = globalBookResponse.data;

            if (originalBook.AuthorID == AppSession.Instance.CurrentUser.ID)
            {
                return Response<CookBook>.createResponse(null, "Ви вже є автором цієї збірки у глобальній базі.",
                    Response<CookBook>.responseStatus.warning);
            }

            CookBook clonedBook = new CookBook
            {
                ID = Guid.NewGuid(),
                AuthorID = AppSession.Instance.CurrentUser.ID,
                Name = originalBook.Name,
                Description = originalBook.Description,
                CreationDate = DateTime.Now,
                LastEdited = DateTime.Now,
                IsPublic = false, 
                ReferencedRecipes = new List<Guid>()
            };

            string importLogs = "";
            if (originalBook.ReferencedRecipes != null && originalBook.ReferencedRecipes.Count > 0)
            {
                foreach (Guid globalRecipeID in originalBook.ReferencedRecipes)
                {
                    Response<Recipe> recipeImportRes = this.ImportGlobalRecipe(globalRecipeID);
                    if (recipeImportRes.status != Response<Recipe>.responseStatus.error && recipeImportRes.data != null)
                    {
                        clonedBook.ReferencedRecipes.Add(recipeImportRes.data.ID);
                    }
                    else
                    {
                        importLogs += "Рецепт " + globalRecipeID + " пропущено: " + recipeImportRes.message + "\n";
                    }
                }
            }

            Response<CookBook> addResponse = localRepo.AddCookBook(clonedBook);
            if (addResponse.status == Response<CookBook>.responseStatus.error)
            {
                return addResponse;
            }

            if (!string.IsNullOrEmpty(importLogs))
            {
                return Response<CookBook>.createResponse(clonedBook, "Збірку " + originalBook.Name + " імпортовано частково:\n" + importLogs,
                    Response<CookBook>.responseStatus.warning);
            }

            return Response<CookBook>.createResponse(clonedBook, "Збірку " + originalBook.Name + " та всі її рецепти успішно клоновано локально.",
                Response<CookBook>.responseStatus.success);
        }

        #endregion

        #region Update Methods
        public Response<Recipe> UpdateRecipe(UpdateRecipe_dto dto)
        {
            string warningMessage = "";

            Response<UpdateRecipe_dto> validationResponse = Validator.Instance.validateUpdateRecipeDTO(dto);
            if (validationResponse.status == Response<UpdateRecipe_dto>.responseStatus.error)
            {
                return Response<Recipe>.createResponse(null, validationResponse.message, 
                    Response<Recipe>.responseStatus.error);
            }
            if (AppSession.Instance.CurrentUser == null)
            {
                return Response<Recipe>.createResponse(null, "Користувач не авторизований. Не можна оновити рецепт.",
                    Response<Recipe>.responseStatus.error);
            }
            Response<Recipe> existingRecipeResponse = localRepo.GetRecipeByID(dto.ID);
            if (existingRecipeResponse.status == Response<Recipe>.responseStatus.error)
            {
                return existingRecipeResponse;
            }
            if (existingRecipeResponse.data.AuthorID != AppSession.Instance.CurrentUser.ID)
            {
                return Response<Recipe>.createResponse(null, "Користувач не є автором рецепту. Не можна оновити рецепт.",
                    Response<Recipe>.responseStatus.error);
            }

            if (existingRecipeResponse.data.IsPublic && !dto.IsPublic)
            {
                Response<List<CookBook>> localBooksRes = this.GetCookBooks(DataScope.Local);
                if (localBooksRes.status != Response<List<CookBook>>.responseStatus.error && localBooksRes.data != null)
                {
                    foreach (CookBook cb in localBooksRes.data)
                    {
                        if (cb.IsPublic && cb.ReferencedRecipes.Contains(dto.ID))
                        {
                            return Response<Recipe>.createResponse(null, 
                                $"Неможливо зробити рецепт приватним, оскільки він опублікований у вашій публічній збірці '{cb.Name}'. " +
                                "Спочатку вилучіть цей рецепт із зазначеної збірки.", 
                                Response<Recipe>.responseStatus.error);
                        }
                    }
                }
            }

            Recipe updatedRecipe = new Recipe
            {
                ID = dto.ID,
                AuthorID = existingRecipeResponse.data.AuthorID,
                Name = dto.Name,
                Description = dto.Description,
                SourceLink = dto.SourceLink,
                FullPreparationTime = dto.FullPreparationTime,
                ActivePreparationTime = dto.ActivePreparationTime,
                Servings = dto.Servings,
                MainPhotoPath = dto.MainPhotoPath,
                ReferencedCategories = dto.ReferencedCategories,
                Ingredients = dto.Ingredients,
                PreparationSteps = dto.PreparationSteps,
                CreationDate = existingRecipeResponse.data.CreationDate,
                LastEdited = DateTime.Now,
                InspiredBy = existingRecipeResponse.data.InspiredBy,
                IsPublic = dto.IsPublic,
                Rating = existingRecipeResponse.data.Rating
            };

            Response<Recipe> updateResponse = localRepo.UpdateRecipe(updatedRecipe);
            if (updateResponse.status == Response<Recipe>.responseStatus.error)
            {
                return updateResponse;
            }

            // ====================================================================
            // СИНХРОНІЗАЦІЯ З КУЛІНАРНИМИ КНИГАМИ (КАСКАДНЕ ОНОВЛЕННЯ)
            // ====================================================================
            if (dto.cookBooksReferencedIn != null)
            {
                Response<List<CookBook>> cookBooksResponse = this.GetCookBooks(DataScope.Local);
                if (cookBooksResponse.status != Response<List<CookBook>>.responseStatus.error && cookBooksResponse.data != null)
                {
                    foreach (CookBook cb in cookBooksResponse.data)
                    {
                        bool isSelectedInUI = dto.cookBooksReferencedIn.Contains(cb.ID);
                        if (cb.ReferencedRecipes == null) cb.ReferencedRecipes = new List<Guid>();
                
                        bool isRecipeAlreadyInBook = cb.ReferencedRecipes.Contains(updatedRecipe.ID);

                        // Ситуація 1: Користувач поставив галочку, але рецепта в книзі ще немає -> ДОДАЄМО
                        if (isSelectedInUI && !isRecipeAlreadyInBook)
                        {
                            // Валідація бізнес-правила: не можна кидати приватний рецепт у публічну книгу
                            if (cb.IsPublic && !updatedRecipe.IsPublic)
                            {
                                warningMessage += $"Рецепт є приватним, тому його не додано до вашої публічної збірки '{cb.Name}'.\n";
                                continue;
                            }

                            UpdateCookBook_dto updateCookBook_Dto = new UpdateCookBook_dto
                            {
                                ID = cb.ID,
                                Name = cb.Name,
                                Description = cb.Description,
                                IsPublic = cb.IsPublic,
                                ReferencedRecipes = cb.ReferencedRecipes.Append(updatedRecipe.ID).ToList()
                            };
                    
                            Response<CookBook> bookUpdateRes = this.UpdateCookBook(updateCookBook_Dto);
                            if (bookUpdateRes.status == Response<CookBook>.responseStatus.error)
                            {
                                warningMessage += $"Не вдалося додати рецепт до збірки '{cb.Name}': {bookUpdateRes.message}\n";
                            }
                        }
                        // Ситуація 2: Користувач ЗНЯВ галочку на формі, але в книзі рецепт був -> ВИЛУЧАЄМО
                        else if (!isSelectedInUI && isRecipeAlreadyInBook)
                        {
                            UpdateCookBook_dto updateCookBook_Dto = new UpdateCookBook_dto
                            {
                                ID = cb.ID,
                                Name = cb.Name,
                                Description = cb.Description,
                                IsPublic = cb.IsPublic,
                                // Залишаємо всі рецепти, крім поточного
                                ReferencedRecipes = cb.ReferencedRecipes.Where(id => id != updatedRecipe.ID).ToList() 
                            };

                            Response<CookBook> bookUpdateRes = this.UpdateCookBook(updateCookBook_Dto);
                            if (bookUpdateRes.status == Response<CookBook>.responseStatus.error)
                            {
                                warningMessage += $"Не вдалося вилучити рецепт зі збірки '{cb.Name}': {bookUpdateRes.message}\n";
                            }
                        }
                    }
                }
            }

            // ====================================================================
            // СИНХРОНІЗАЦІЯ З ГЛОБАЛЬНОЮ БАЗОЮ (ХМАРОЮ)
            // ====================================================================
            if(existingRecipeResponse.data.IsPublic != dto.IsPublic && !dto.IsPublic)
            { 
                Response<bool> removeFromGlobalResponse = RemoveObject(ILocalRepository.dataType.recipe, updatedRecipe.ID, DataScope.Global);
                if (removeFromGlobalResponse.status == Response<bool>.responseStatus.error)
                {
                    warningMessage += "Рецепт не вдалося видалити глобально: " + removeFromGlobalResponse.message + "\n";
                }
            }
            else if (existingRecipeResponse.data.IsPublic != dto.IsPublic && dto.IsPublic)
            {
                Recipe globalClone = CreateGlobalRecipeClone(updatedRecipe); 
                Response<Recipe> addToGlobalResponse = globalRepo.AddRecipe(globalClone);
                if (addToGlobalResponse.status == Response<Recipe>.responseStatus.error)
                {
                    warningMessage += "Рецепт не вдалося додати в global-базу: " + addToGlobalResponse.message + "\n";
                }
            }
            else if (existingRecipeResponse.data.IsPublic && dto.IsPublic)
            {
                Recipe globalClone = CreateGlobalRecipeClone(updatedRecipe);
                Response<Recipe> updateGloballyResponse = globalRepo.UpdateRecipe(globalClone);
                if (updateGloballyResponse.status == Response<Recipe>.responseStatus.error)
                {
                    warningMessage += "Не вдалося оновити рецепт глобально: " + updateGloballyResponse.message + "\n";
                }
            }

            // Повернення результату з урахуванням накопичених попереджень (warnings)
            if (validationResponse.status == Response<UpdateRecipe_dto>.responseStatus.warning)
            {
                return Response<Recipe>.createResponse(updatedRecipe, validationResponse.message + "\n" + warningMessage, 
                    Response<Recipe>.responseStatus.warning);  
            }
            if (!string.IsNullOrEmpty(warningMessage))
            {
                return Response<Recipe>.createResponse(updatedRecipe, "Рецепт оновлено, але виникли зауваження: \n" + warningMessage, 
                    Response<Recipe>.responseStatus.warning);
            }

            return Response<Recipe>.createResponse(updateResponse.data, "Рецепт оновлено успішно.",
                Response<Recipe>.responseStatus.success);   
        }

        public Response<CookBook> UpdateCookBook(UpdateCookBook_dto dto)
        {
            string warningMessage = "";
            Response<UpdateCookBook_dto> validationResponse = Validator.Instance.validateUpdateCookBookDTO(dto);
            if (validationResponse.status == Response<UpdateCookBook_dto>.responseStatus.error)
            {
                return Response<CookBook>.createResponse(null, validationResponse.message, 
                    Response<CookBook>.responseStatus.error);
            }
            if (AppSession.Instance.CurrentUser == null)
            {
                return Response<CookBook>.createResponse(null, "Користувач не авторизований. Не можна оновити збірку.",
                    Response<CookBook>.responseStatus.error);
            }
            Response<CookBook> existingCookBookResponse = localRepo.GetCookBookByID(dto.ID);
            if (existingCookBookResponse.status == Response<CookBook>.responseStatus.error)
            {
                return existingCookBookResponse;
            }
            if (existingCookBookResponse.data.AuthorID != AppSession.Instance.CurrentUser.ID)
            {
                return Response<CookBook>.createResponse(null, "Користувач не є автором збірки. Ne можна оновити збірку.",
                    Response<CookBook>.responseStatus.error);
            }

            // ====================================================================
            // КАСКАДНА ПУБЛІКАЦІЯ: Робимо приватні рецепти ПУБЛІЧНИМИ, якщо книга йде в паблік
            // ====================================================================
            List<Guid> recipeIDs = dto.ReferencedRecipes ?? new List<Guid>();

            if (dto.IsPublic)
            {
                foreach (Guid recipeId in recipeIDs)
                {
                    Response<Recipe> recipeRes = this.GetRecipe(recipeId, DataScope.Local);
                    if (recipeRes.status != Response<Recipe>.responseStatus.error && recipeRes.data != null)
                    {
                        Recipe recipe = recipeRes.data;

                        // Якщо рецепт досі приватний — примусово публікуємо його!
                        if (!recipe.IsPublic)
                        {
                            recipe.IsPublic = true;
                            recipe.LastEdited = DateTime.Now;

                            // 1. Оновлюємо статус рецепта в локальній базі
                            localRepo.UpdateRecipe(recipe);

                            // 2. Клонуємо та заливаємо рецепт у глобальну хмару
                            Recipe globalRecipeClone = CreateGlobalRecipeClone(recipe);
                            Response<Recipe> globalAddRes = globalRepo.AddRecipe(globalRecipeClone);

                            if (globalAddRes.status != Response<Recipe>.responseStatus.error)
                            {
                                warningMessage += $"Рецепт '{recipe.Name}' автоматично опубліковано в хмарі, оскільки він входить до публічної збірки.\n";
                            }
                            else
                            {
                                warningMessage += $"Помилка автоматичної публікації рецепту '{recipe.Name}': {globalAddRes.message}\n";
                            }
                        }
                    }
                }
            }

            CookBook updatedCookBook = new CookBook
            {
                ID = dto.ID,
                AuthorID = existingCookBookResponse.data.AuthorID,
                Name = dto.Name,
                Description = dto.Description,
                CreationDate = existingCookBookResponse.data.CreationDate,
                LastEdited = DateTime.Now,
                IsPublic = dto.IsPublic,
                ReferencedRecipes = recipeIDs
            };

            Response<CookBook> updateResponse = localRepo.UpdateCookBook(updatedCookBook);
            if (updateResponse.status == Response<CookBook>.responseStatus.error)
            {
                return updateResponse;
            }

            if (existingCookBookResponse.data.IsPublic != dto.IsPublic && !dto.IsPublic)
            {
                Response<bool> removeFromGlobalResponse = RemoveObject(ILocalRepository.dataType.cookBook, updatedCookBook.ID, DataScope.Global);
                if (removeFromGlobalResponse.status == Response<bool>.responseStatus.error)
                {
                    return Response<CookBook>.createResponse(updateResponse.data, "Збірку оновлено локально, але не вдалося видалити глобально: " + removeFromGlobalResponse.message,
                        Response<CookBook>.responseStatus.warning);
                }
            }
            else if (existingCookBookResponse.data.IsPublic != dto.IsPublic && dto.IsPublic)
            {
                CookBook globalClone = CreateGlobalCookBookClone(updatedCookBook);
                Response<CookBook> addToGlobalResponse = globalRepo.AddCookBook(globalClone);
                if (addToGlobalResponse.status == Response<CookBook>.responseStatus.error)
                {
                    return Response<CookBook>.createResponse(updateResponse.data, "Збірку оновлено локально, але не вдалося додати глобально: " + addToGlobalResponse.message,
                        Response<CookBook>.responseStatus.warning);
                }
            }
            else if (existingCookBookResponse.data.IsPublic && dto.IsPublic)
            {
                CookBook globalClone = CreateGlobalCookBookClone(updatedCookBook); 
                Response<CookBook> updateGloballyResponse = globalRepo.UpdateCookBook(globalClone);
                if (updateGloballyResponse.status == Response<CookBook>.responseStatus.error)
                {
                    return Response<CookBook>.createResponse(updateResponse.data, "Збірку оновлено локально, але не вдалося оновити глобально: " + updateGloballyResponse.message,
                        Response<CookBook>.responseStatus.warning);
                }
            }

            if (!string.IsNullOrEmpty(warningMessage))
            {
                return Response<CookBook>.createResponse(updateResponse.data, "Збірку успішно опубліковано разом із залежними сутностями:\n" + warningMessage, 
                    Response<CookBook>.responseStatus.warning);
            }

            return Response<CookBook>.createResponse(updateResponse.data, "Збірку оновлено успішно.",
                Response<CookBook>.responseStatus.success);
        }

        public Response<Product> UpdateProduct(UpdateProduct_dto dto)
        {
            Response<UpdateProduct_dto> validationResponse = Validator.Instance.validateUpdateProductDTO(dto);
            if (validationResponse.status == Response<UpdateProduct_dto>.responseStatus.error)
            {
                return Response<Product>.createResponse(null, validationResponse.message,
                    Response<Product>.responseStatus.error);
            }
            Response<Product> existingProductResponse = localRepo.GetProductByID(dto.ID);
            if (existingProductResponse.status == Response<Product>.responseStatus.error)
            {
                return existingProductResponse;
            }
            if (existingProductResponse.data.IsCustom == false)
            {
                return Response<Product>.createResponse(null, "Неможливо оновити цей продукт, оскільки він є системним.",
                    Response<Product>.responseStatus.error);
            }

            double p = dto.Proteins ?? existingProductResponse.data.Proteins;
            double f = dto.Fats ?? existingProductResponse.data.Fats;
            double c = dto.Carbohydrates ?? existingProductResponse.data.Carbohydrates;
            double cal = dto.Calories ?? CaloriesCalculator.calculateCalories(p, f, c).data;
            
            Product updatedProduct = new Product
            {
                ID = dto.ID,
                Name = dto.Name,
                Calories = cal,
                Proteins = p,
                Fats = f,
                Carbohydrates = c,
                IsCustom = true
            };
            return localRepo.UpdateProduct(updatedProduct);

        }

        public Response<ProductAlias> UpdateProductAlias(UpdateProductAlias_dto dto)
        {
            Response<UpdateProductAlias_dto> validationResponse = Validator.Instance.validateUpdateProductAliasDTO(dto);
            if (validationResponse.status == Response<UpdateProductAlias_dto>.responseStatus.error)
            {
                return Response<ProductAlias>.createResponse(null, validationResponse.message,
                    Response<ProductAlias>.responseStatus.error);
            }

            Response<ProductAlias> existingProductAliasResponse = localRepo.GetProductAliasByID(dto.ID);
            if (existingProductAliasResponse.status == Response<ProductAlias>.responseStatus.error)
            {
                return existingProductAliasResponse;
            }
            ProductAlias updatedProductAlias = new ProductAlias
            {
                ID = dto.ID,
                AliasName = dto.AliasName,
                ReferencedProduct = existingProductAliasResponse.data.ReferencedProduct,
            };

            return localRepo.UpdateProductAlias(updatedProductAlias);
        }

        public Response<Category> UpdateCategory(UpdateCategory_dto dto)
        {
            Response<UpdateCategory_dto> validationResponse = Validator.Instance.validateUpdateCategoryDTO(dto);
            if (validationResponse.status == Response<UpdateCategory_dto>.responseStatus.error)
            {
                return Response<Category>.createResponse(null, validationResponse.message,
                    Response<Category>.responseStatus.error);
            }
            Response<Category> existingCategoryResponse = localRepo.GetCategoryByID(dto.ID);
            if (existingCategoryResponse.status == Response<Category>.responseStatus.error)
            {
                return existingCategoryResponse;
            }
            if (existingCategoryResponse.data.IsCustom == false)
            {
                return Response<Category>.createResponse(null, "Неможливо оновити цю категорію, оскільки вона є системною.",
                    Response<Category>.responseStatus.error);
            }

            Category updatedCategory = new Category
            {
                ID = dto.ID,
                Name = dto.Name,
                Description = dto.Description,
                IsCustom = true
            };

            return localRepo.UpdateCategory(updatedCategory);
        }
        #endregion

        #region Helper Methods for Sanitization
        private Recipe CreateGlobalRecipeClone(Recipe localRecipe)
        {
            Recipe globalClone = new Recipe
            {
                ID = localRecipe.ID,
                AuthorID = localRecipe.AuthorID,
                Name = localRecipe.Name,
                Description = localRecipe.Description,
                SourceLink = localRecipe.SourceLink,
                FullPreparationTime = localRecipe.FullPreparationTime,
                ActivePreparationTime = localRecipe.ActivePreparationTime,
                Servings = localRecipe.Servings,
                MainPhotoPath = localRecipe.MainPhotoPath,
                ReferencedCategories = localRecipe.ReferencedCategories?.ToList() ?? new List<Guid>(),
                CreationDate = localRecipe.CreationDate,
                LastEdited = localRecipe.LastEdited,
                IsPublic = true,
                Rating = localRecipe.Rating,
                InspiredBy = localRecipe.InspiredBy,
                PreparationSteps = localRecipe.PreparationSteps?.ToList() ?? new List<Step>(),
                Ingredients = new List<Ingredient>()
            };

            foreach (Ingredient localIngredient in localRecipe.Ingredients ?? new List<Ingredient>())
            {
                Guid finalProductGuid = Guid.Empty;

                if (localIngredient.ReferencedProduct.HasValue)
                {
                    Response<Product> productResponse = this.GetProduct(localIngredient.ReferencedProduct.Value, DataScope.Local);
                    if (productResponse.status == Response<Product>.responseStatus.success && !productResponse.data.IsCustom)
                    {
                        finalProductGuid = localIngredient.ReferencedProduct.Value;
                    }
                }

                globalClone.Ingredients.Add(new Ingredient
                {
                    ID = localIngredient.ID,
                    RawName = localIngredient.RawName,
                    Weight = localIngredient.Weight,
                    ReferencedProduct = finalProductGuid == Guid.Empty ? (Guid?)null : finalProductGuid
                });
            }

            foreach (Guid categoryID in (localRecipe.ReferencedCategories ?? new List<Guid>()).ToList())
            {
                Response<Category> categoryResponse = this.GetCategory(categoryID, DataScope.Local);
                if (categoryResponse.status == Response<Category>.responseStatus.error || categoryResponse.data.IsCustom)
                {
                    globalClone.ReferencedCategories.Remove(categoryID);
                }
            }

            return globalClone;
        }

        private CookBook CreateGlobalCookBookClone(CookBook localCookBook)
        {
            CookBook globalClone = new CookBook
            {
                ID = localCookBook.ID,
                AuthorID = localCookBook.AuthorID,
                Name = localCookBook.Name,
                Description = localCookBook.Description,
                CreationDate = localCookBook.CreationDate,
                LastEdited = localCookBook.LastEdited,
                IsPublic = true,
                ReferencedRecipes = new List<Guid>()
            };
            
            foreach (Guid recipeId in (localCookBook.ReferencedRecipes ?? new List<Guid>()).ToList())
            {
                Response<Recipe> recipeResponse = this.GetRecipe(recipeId, DataScope.Local);
                if (recipeResponse.status == Response<Recipe>.responseStatus.success && recipeResponse.data.IsPublic)
                {
                    globalClone.ReferencedRecipes.Add(recipeId);
                }
            }
            return globalClone;
        }
        #endregion
        public Response<bool> RemoveObject(ILocalRepository.dataType data, Guid ID, DataScope scope)
        {
            Guid authorID = Guid.Empty;
            bool isPublic = false; // Змінна для перевірки публічності

            switch (data)
            {
                case ILocalRepository.dataType.recipe:
                { 
                    Response<Recipe> recipeResponse = localRepo.GetRecipeByID(ID);
                    if (recipeResponse.status == Response<Recipe>.responseStatus.error)
                    {
                        return Response<bool>.createResponse(false, "Помилка при пошуку рецепта для видалення: " + recipeResponse.message,
                            Response<bool>.responseStatus.error);
                    }
                    authorID = recipeResponse.data.AuthorID;
                    isPublic = recipeResponse.data.IsPublic; // Запам'ятовуємо статус

                    // Каскадна чистка збірок
                    if (scope == DataScope.Local)
                    {
                        Response<List<CookBook>> booksResponse = localRepo.GetCookBooks();
                        if (booksResponse.status == Response<List<CookBook>>.responseStatus.success && booksResponse.data != null)
                        {
                            foreach (CookBook book in booksResponse.data)
                            {
                                if (book.ReferencedRecipes != null && book.ReferencedRecipes.Contains(ID))
                                {
                                    book.ReferencedRecipes.Remove(ID);
                                    localRepo.UpdateCookBook(book); 
                                }
                            }
                        }
                    }
                    break;
                }
                case ILocalRepository.dataType.cookBook:
                { 
                    Response<CookBook> cookBookResponse = localRepo.GetCookBookByID(ID);
                    if (cookBookResponse.status == Response<CookBook>.responseStatus.error)
                    {
                        return Response<bool>.createResponse(false, "Помилка при пошуку збірки для видалення: " + cookBookResponse.message,
                            Response<bool>.responseStatus.error);
                    }
                    authorID = cookBookResponse.data.AuthorID;
                    isPublic = cookBookResponse.data.IsPublic; // Запам'ятовуємо статус
                    break;
                }
            }

            if (AppSession.Instance.CurrentUser == null)
            {
                return Response<bool>.createResponse(false, "Користувач не авторизований. Не можна видалити об'єкт.",
                    Response<bool>.responseStatus.error);
            }
            if (authorID != Guid.Empty && authorID != AppSession.Instance.CurrentUser.ID)
            {
                return Response<bool>.createResponse(false, "Користувач не є автором цього об'єкта. Не можна видалити об'єкт.",
                    Response<bool>.responseStatus.error);
            }

            if (scope == DataScope.Global)
            {
                return globalRepo.RemoveObject(data, ID);
            }
            else if (scope == DataScope.Local)
            {
                if (data == ILocalRepository.dataType.recipe || data == ILocalRepository.dataType.cookBook)
                {
                    Response<bool> removeFromLocalResponse = localRepo.RemoveObject(data, ID);
                    if (removeFromLocalResponse.status == Response<bool>.responseStatus.error)
                    {
                        return Response<bool>.createResponse(false, "Помилка при видаленні об'єкта з локальної бази: " + removeFromLocalResponse.message,
                            Response<bool>.responseStatus.error);
                    }

                    // Йдемо в глобальну базу тільки якщо об'єкт публічний
                    if (isPublic) 
                    {
                        Response<bool> removeFromGlobalResponse = globalRepo.RemoveObject(data, ID);
                        if (removeFromGlobalResponse.status == Response<bool>.responseStatus.error)
                        {
                            return Response<bool>.createResponse(false, "Об'єкт видалено локально, але не вдалося видалити глобально: " + removeFromGlobalResponse.message,
                                Response<bool>.responseStatus.warning);
                        }
                    }

                    return Response<bool>.createResponse(true, "Об'єкт видалено успішно.", 
                        Response<bool>.responseStatus.success);
                }
                else if (data == ILocalRepository.dataType.product)
                {
                    Response<Product> productResponse = localRepo.GetProductByID(ID);
                    if (productResponse.status == Response<Product>.responseStatus.error)
                    {
                        return Response<bool>.createResponse(false, "Помилка при пошуку продукту для видалення: " + productResponse.message,
                            Response<bool>.responseStatus.error);
                    }
                    if (productResponse.data.IsCustom == false)
                    {
                        return Response<bool>.createResponse(false, "Неможливо видалити цей продукт, оскільки він є системним.",
                            Response<bool>.responseStatus.error);
                    }
                    return localRepo.RemoveObject(data, ID);
                }
                else if (data == ILocalRepository.dataType.category)
                {
                    Response<Category> categoryResponse = localRepo.GetCategoryByID(ID);
                    if (categoryResponse.status == Response<Category>.responseStatus.error)
                    {
                        return Response<bool>.createResponse(false, "Помилка при пошуку категорії для видалення: " + categoryResponse.message,
                            Response<bool>.responseStatus.error);
                    }
                    if (categoryResponse.data.IsCustom == false)
                    {
                        return Response<bool>.createResponse(false, "Неможливо видалити цю категорію, оскільки вона є системною.",
                            Response<bool>.responseStatus.error);
                    }
                    return localRepo.RemoveObject(data, ID);
                }
                else if (data == ILocalRepository.dataType.productAlias)
                {
                    return localRepo.RemoveObject(data, ID);
                }

            }
            return Response<bool>.createResponse(false, "Невідомий обсяг даних або тип даних не підтримує видалення в глобальному обсязі.",
                Response<bool>.responseStatus.error);
        }

        public Response<UserProfile> GetAuthorById(Guid authorID)
        {
            return globalRepo.GetUserById(authorID);
        }

        public void InitializeRepositories(ILocalRepository localRepo, IGlobalRepository globalRepo)
        {
            this.localRepo = localRepo;
            this.globalRepo = globalRepo;
        }
    }
}


using MyKitchen_Core.Entities;
using MyKitchen_Core.Interfaces;
using Newtonsoft.Json;
using MyKitchen_Core.DTO.SearchDTOs;
using MyKitchen_Core.Tools;
using MyKitchen_Core.Services;

namespace MyKitchen_Core.DataManagers
{
    public class LocalFS_Manager : ILocalRepository
    {
        protected readonly string rootDataDirectory;
        protected readonly string baseRecipesFolder;
        protected readonly string cookBooksFilePath;
        protected readonly string productsFilePath;
        protected readonly string productAliasesFilePath;
        protected readonly string categoriesFilePath;

        public LocalFS_Manager(string rootDataPath)
        {
            rootDataDirectory = rootDataPath;
            baseRecipesFolder = Path.Combine(rootDataPath, "Recipes");
            cookBooksFilePath = Path.Combine(rootDataPath, "CookBooks.json");
            productsFilePath = Path.Combine(rootDataPath, "Products.json");
            productAliasesFilePath = Path.Combine(rootDataPath, "ProductAliases.json");
            categoriesFilePath = Path.Combine(rootDataPath, "Categories.json");

            DatabaseSeeder.EnsureDirectoriesExist(rootDataDirectory);
        }

        #region Get Methods
public Response<List<Recipe>> GetRecipes()
{
    string warningMessage = "";
    if(!Directory.Exists(baseRecipesFolder))
    {
        DatabaseSeeder.EnsureDirectoriesExist(rootDataDirectory);
    }

    string[] recipeFiles = Directory.GetFiles(baseRecipesFolder, "recipe.json", SearchOption.AllDirectories);
    List<Recipe> recipes = new List<Recipe>();

    foreach (string file in recipeFiles) 
    {
        try 
        { 
            // 1. Десеріалізуємо об'єкт (отримуємо відносні шляхи з JSON)
            Recipe recipe = JsonConvert.DeserializeObject<Recipe>(File.ReadAllText(file));
            
            // 2.Динамічно відновлюємо повний шлях до головного фото
            if (!string.IsNullOrEmpty(recipe.MainPhotoPath) && !Path.IsPathRooted(recipe.MainPhotoPath))
            {
                recipe.MainPhotoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, recipe.MainPhotoPath);
            }
            
            // 3. Відновлюємо повні шляхи для кожного кроку приготування страви
            if (recipe.PreparationSteps != null)
            {
                foreach (var step in recipe.PreparationSteps)
                {
                    if (!string.IsNullOrEmpty(step.PhotoPath) && !Path.IsPathRooted(step.PhotoPath))
                    {
                        step.PhotoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, step.PhotoPath);
                    }
                }
            }

            recipes.Add(recipe);
        }
        catch (Exception ex)
        {
            warningMessage += "Помилка при завантаженні рецепту з файлу " + file + ": " + ex.Message + "\n";
        }
    }
    
    if(warningMessage != "")
    {
        return Response<List<Recipe>>.createResponse(recipes, warningMessage, Response<List<Recipe>>.responseStatus.warning);
    }
    return Response<List<Recipe>>.createResponse(recipes, warningMessage, Response<List<Recipe>>.responseStatus.success);
}

        public Response<List<CookBook>> GetCookBooks()
        {
            string cookBooksPath = cookBooksFilePath; 
            if (!File.Exists(cookBooksPath) || new FileInfo(cookBooksPath).Length == 0)
            {
                DatabaseSeeder.EnsureDirectoriesExist(rootDataDirectory);
            }

            List<CookBook> cookBooks = new List<CookBook>();
            try
            { 
                 cookBooks = JsonConvert.DeserializeObject<List<CookBook>>(File.ReadAllText(cookBooksPath));
            }
            catch (Exception ex)
            { 
                return Response<List<CookBook>>.createResponse(null, "Помилка при завантаженні книг рецептів: " + ex.Message, 
                    Response<List<CookBook>>.responseStatus.error);
            }
            return Response<List<CookBook>>.createResponse(cookBooks, "", Response<List<CookBook>>.responseStatus.success);
        }

        public Response<List<Product>> GetProducts()
        {
            string productsPath = productsFilePath;
            if(!File.Exists(productsPath) || new FileInfo(productsPath).Length == 0)
            {
                DatabaseSeeder.EnsureDirectoriesExist(rootDataDirectory);
            }

            List<Product> products = new List<Product>();
            try
            { 
                 products = JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(productsPath));
            }
            catch (Exception ex)
            { 
                return Response<List<Product>>.createResponse(null, "Помилка при завантаженні продуктів: " + ex.Message, 
                    Response<List<Product>>.responseStatus.error);
            }
            return Response<List<Product>>.createResponse(products, "", Response<List<Product>>.responseStatus.success);
        }

        public Response<List<ProductAlias>> GetProductAliases()
        {
            string productAliasesPath = productAliasesFilePath;
            if (!File.Exists(productAliasesPath) || new FileInfo(productAliasesPath).Length == 0)
            {
                DatabaseSeeder.EnsureDirectoriesExist(rootDataDirectory);
            }    

            List<ProductAlias> productAliases = new List<ProductAlias>();
            try
            { 
                 productAliases = JsonConvert.DeserializeObject<List<ProductAlias>>(File.ReadAllText(productAliasesPath));
            }
            catch (Exception ex)
            { 
                return Response<List<ProductAlias>>.createResponse(null, "Помилка при завантаженні псевдонімів продуктів: " + ex.Message, 
                    Response<List<ProductAlias>>.responseStatus.error);
            }
            return Response<List<ProductAlias>>.createResponse(productAliases, "", Response<List<ProductAlias>>.responseStatus.success);
        }

        public Response<List<Category>> GetCategories()
        {
            string categoriesPath = categoriesFilePath;
            if (!File.Exists(categoriesPath) || new FileInfo(categoriesPath).Length == 0)
            {
                DatabaseSeeder.EnsureDirectoriesExist(rootDataDirectory);
            }

            List<Category> categories = new List<Category>();
            try
            { 
                 categories = JsonConvert.DeserializeObject<List<Category>>(File.ReadAllText(categoriesPath));
            }
            catch (Exception ex)
            { 
                return Response<List<Category>>.createResponse(null, "Помилка при завантаженні категорій: " + ex.Message, 
                    Response<List<Category>>.responseStatus.error);
            }
            return Response<List<Category>>.createResponse(categories, "", Response<List<Category>>.responseStatus.success);
        }

        #endregion

        #region GetByID Methods

        public Response<Recipe> GetRecipeByID(Guid ID)
        {
            Response<List<Recipe>> recipe = SearchRecipes(new SearchRecipeCriteria_dto { ID = ID });
            if (recipe.status == Response<List<Recipe>>.responseStatus.error)
            {
                return Response<Recipe>.createResponse(null, "Помилка при завантаженні рецепту: " + recipe.message, 
                    Response<Recipe>.responseStatus.error);
            }
            if (recipe.data.Count == 0)
            {
                return Response<Recipe>.createResponse(null, "Рецепт за ID " + ID + " не знайдено", 
                    Response<Recipe>.responseStatus.error);
            }
            return Response<Recipe>.createResponse(recipe.data[0], "", 
                Response<Recipe>.responseStatus.success);
        }

        public Response<CookBook> GetCookBookByID(Guid ID)
        {             
            Response<List<CookBook>> cookBook = SearchCookBooks(new SearchCookBookCriteria_dto { ID = ID });
            if (cookBook.status == Response<List<CookBook>>.responseStatus.error)
            {
                return Response<CookBook>.createResponse(null, "Помилка при завантаженні збірки: " + cookBook.message, 
                    Response<CookBook>.responseStatus.error);
            }
            if (cookBook.data.Count == 0)
            {
                return Response<CookBook>.createResponse(null, "Збірку за ID " + ID + " не знайдено", 
                    Response<CookBook>.responseStatus.error);
            }
            return Response<CookBook>.createResponse(cookBook.data[0], "", 
                Response<CookBook>.responseStatus.success);
        }
        public Response<Product> GetProductByID(Guid ID)
        {
            Response<List<Product>> product = SearchProducts(new SearchProductCriteria_dto { ID = ID });
            if (product.status == Response<List<Product>>.responseStatus.error)
            {
                return Response<Product>.createResponse(null, "Помилка при завантаженні продукту: " + product.message, 
                    Response<Product>.responseStatus.error);
            }
            if (product.data.Count == 0)
            {
                return Response<Product>.createResponse(null, "Продукт за ID " + ID + " не знайдено", 
                    Response<Product>.responseStatus.error);
            }
            return Response<Product>.createResponse(product.data[0], "", 
                Response<Product>.responseStatus.success);
        }
        public Response<ProductAlias> GetProductAliasByID(Guid ID)
        {
            Response<List<ProductAlias>> productAlias = SearchProductAliases(new SearchProductAliasCriteria_dto { ID = ID });
            if (productAlias.status == Response<List<ProductAlias>>.responseStatus.error)
            {
                return Response<ProductAlias>.createResponse(null, "Помилка при завантаженні псевдоніму продукту: " + productAlias.message, 
                    Response<ProductAlias>.responseStatus.error);
            }
            if (productAlias.data.Count == 0)
            {
                return Response<ProductAlias>.createResponse(null, "Псевдонім продукту за ID " + ID + " не знайдено", 
                    Response<ProductAlias>.responseStatus.error);
            }
            return Response<ProductAlias>.createResponse(productAlias.data[0], "", 
                Response<ProductAlias>.responseStatus.success);
        }

        public Response<Category> GetCategoryByID(Guid ID)
        {
            Response<List<Category>> category = SearchCategories(new SearchCategoryCriteria_dto { ID = ID });
            if (category.status == Response<List<Category>>.responseStatus.error)
            {
                return Response<Category>.createResponse(null, "Помилка при завантаженні категорії: " + category.message, 
                    Response<Category>.responseStatus.error);
            }
            if (category.data.Count == 0)
            {
                return Response<Category>.createResponse(null, "Категорія за ID " + ID + " не знайдено", 
                    Response<Category>.responseStatus.error);
            }
            return Response<Category>.createResponse(category.data[0], "", 
                Response<Category>.responseStatus.success);
        }
        #endregion

        #region Search Methods
        public virtual Response<List<Recipe>> SearchRecipes(SearchRecipeCriteria_dto criteria)
        {
            Response<List<Recipe>> allRecipesResponse = GetRecipes();
            if (allRecipesResponse.status == Response<List<Recipe>>.responseStatus.error)
            {
                return Response<List<Recipe>>.createResponse(null, "Помилка при завантаженні рецептів: " + allRecipesResponse.message, 
                    Response<List<Recipe>>.responseStatus.error);
            }
            Response<List<Product>> productsResponse = GetProducts();
            if (productsResponse.status == Response<List<Product>>.responseStatus.error)
            {
                return Response<List<Recipe>>.createResponse(null, "Помилка при завантаженні продуктів для фільтрації рецептів: " + productsResponse.message, 
                    Response<List<Recipe>>.responseStatus.error);
            }
            Response<List<ProductAlias>> productAliasesResponse = GetProductAliases();
            if (productAliasesResponse.status == Response<List<ProductAlias>>.responseStatus.error)
            {
                return Response<List<Recipe>>.createResponse(null, "Помилка при завантаженні псевдонімів продуктів: " + productAliasesResponse.message,
                    Response<List<Recipe>>.responseStatus.error);
            }

            return Filter.Instance.FilterRecipes(criteria, allRecipesResponse.data, productsResponse.data, productAliasesResponse.data);
        }

        public virtual Response<List<CookBook>> SearchCookBooks(SearchCookBookCriteria_dto criteria)
        {
            Response<List<CookBook>> allCookBooksResponse = GetCookBooks();
            if (allCookBooksResponse.status == Response<List<CookBook>>.responseStatus.error)
            {
                return Response<List<CookBook>>.createResponse(null, "Помилка при завантаженні кулінарних книг: " + allCookBooksResponse.message, 
                    Response<List<CookBook>>.responseStatus.error);
            }
            
            Response<List<Recipe>> recipesResponse = GetRecipes();
            if (recipesResponse.status == Response<List<Recipe>>.responseStatus.error)
            {
                return Response<List<CookBook>>.createResponse(null, "Помилка при завантаженні рецептів: " + recipesResponse.message, 
                    Response<List<CookBook>>.responseStatus.error);
            }

            List<Recipe> localRecipes = recipesResponse.data ?? new List<Recipe>();
            
            return Filter.Instance.FilterCookBooks(criteria, allCookBooksResponse.data, localRecipes);
        }

        public Response<List<Product>> SearchProducts(SearchProductCriteria_dto criteria)
        {
            Response<List<Product>> allProductsResponse = GetProducts();
            if (allProductsResponse.status == Response<List<Product>>.responseStatus.error)
            {
                return Response<List<Product>>.createResponse(null, "Помилка при завантаженні продуктів: " + allProductsResponse.message, 
                    Response<List<Product>>.responseStatus.error);
            }
            
            return Filter.Instance.FilterProducts(criteria, allProductsResponse.data);
        }
        public Response<List<ProductAlias>> SearchProductAliases(SearchProductAliasCriteria_dto criteria)
        {
            Response<List<ProductAlias>> allProductAliasesResponse = GetProductAliases();
            if (allProductAliasesResponse.status == Response<List<ProductAlias>>.responseStatus.error)
            {
                return Response<List<ProductAlias>>.createResponse(null, "Помилка при завантаженні псевдонімів продуктів: " + allProductAliasesResponse.message, 
                    Response<List<ProductAlias>>.responseStatus.error);
            }
            
            return Filter.Instance.FilterProductAliases(criteria, allProductAliasesResponse.data);
        }

        public Response<List<Category>> SearchCategories(SearchCategoryCriteria_dto criteria)
        {
            Response<List<Category>> allCategoriesResponse = GetCategories();
            if (allCategoriesResponse.status == Response<List<Category>>.responseStatus.error)
            {
                return Response<List<Category>>.createResponse(null, "Помилка при завантаженні категорій: " + allCategoriesResponse.message, 
                    Response<List<Category>>.responseStatus.error);
            }
            
            return Filter.Instance.FilterCategories(criteria, allCategoriesResponse.data);
        }

        #endregion

        #region Add Methods

        public Response<Recipe> AddRecipe(Recipe recipe)
        {
            if(!Directory.Exists(baseRecipesFolder))
            {
                DatabaseSeeder.EnsureDirectoriesExist(rootDataDirectory);
            }

            string newRecipeFolder = Path.Combine(baseRecipesFolder, recipe.ID.ToString());
            string photosFolder = Path.Combine(newRecipeFolder, "Photos");    
            string jsonFilePath = Path.Combine(newRecipeFolder, "recipe.json");

            try
            { 
                Directory.CreateDirectory(newRecipeFolder);
                Directory.CreateDirectory(photosFolder);

                // --- 1. Обробка головного фото ---
                string sourceMainPhotoPath = recipe.MainPhotoPath;
                string destMainPhotoPath = Path.Combine(photosFolder, Path.GetFileName(sourceMainPhotoPath));
                Response<string> copyPhotoResponse = this.CopyPhoto(sourceMainPhotoPath, destMainPhotoPath);
                
                if (copyPhotoResponse.status == Response<string>.responseStatus.success)
                {
                    // 🔥 ПЕРЕВОДИМО ВО ВІДНОСНИЙ ШЛЯХ: Вирізаємо все, що йде до папки "Local" (або "Global")
                    recipe.MainPhotoPath = GetRelativePath(copyPhotoResponse.data);
                }
                else
                {
                    recipe.MainPhotoPath = "";
                }

                // --- 2. Обробка фотографій кроків ---
                foreach(Step step in recipe.PreparationSteps)
                { 
                    string sourceStepPhotoPath = step.PhotoPath;
                    string destStepPhotoPath = Path.Combine(photosFolder, Path.GetFileName(sourceStepPhotoPath));
                    Response<string> copyStepPhotoResponse = this.CopyPhoto(sourceStepPhotoPath, destStepPhotoPath);
                    
                    if (copyStepPhotoResponse.status == Response<string>.responseStatus.success)
                    {
                        // 🔥 ПЕРЕВОДИМО ВО ВІДНОСНИЙ ШЛЯХ
                        step.PhotoPath = GetRelativePath(copyStepPhotoResponse.data);
                    }
                    else
                    {
                        step.PhotoPath = "";
                    }
                }            
                
                string json = JsonConvert.SerializeObject(recipe, Formatting.Indented);
                File.WriteAllText(jsonFilePath, json);
            }
            catch (Exception ex)
            {
                return Response<Recipe>.createResponse(null, "Помилка при збереженні рецепту: " + ex.Message, Response<Recipe>.responseStatus.error);
            }
            return Response<Recipe>.createResponse(recipe, "Рецепт успішно збережено", Response<Recipe>.responseStatus.success);
        }


        public Response<CookBook> AddCookBook(CookBook cookBook)
        {
            Response <List<CookBook>> cookBooksResponse = GetCookBooks();
            if (cookBooksResponse.status == Response<List<CookBook>>.responseStatus.error)
            {
                return Response<CookBook>.createResponse(null, "Помилка при завантаженні книг рецептів: " + cookBooksResponse.message, 
                    Response<CookBook>.responseStatus.error);
            }

            List<CookBook> cookBooks = cookBooksResponse.data;
            cookBooks.Add(cookBook);

            string json = JsonConvert.SerializeObject(cookBooks, Formatting.Indented);
            string cookBooksPath = cookBooksFilePath;
            try
            { 
                File.WriteAllText(cookBooksPath, json);
            }
            catch (Exception ex)
            { 
                return Response<CookBook>.createResponse(null, "Помилка при збереженні книг рецептів: " + ex.Message, 
                    Response<CookBook>.responseStatus.error);
            }

            return Response<CookBook>.createResponse(cookBook, "Книга рецептів успішно збережена", 
                Response<CookBook>.responseStatus.success);
        }

        public Response<Product> AddProduct(Product product)
        {
            Response<List<Product>> productsResponse = GetProducts();
            if (productsResponse.status == Response<List<Product>>.responseStatus.error)
            {
                return Response<Product>.createResponse(null, "Помилка при завантаженні продуктів: " + productsResponse.message, 
                    Response<Product>.responseStatus.error);
            }
            List<Product> products = productsResponse.data;
            products.Add(product);

            string json = JsonConvert.SerializeObject(products, Formatting.Indented);
            string productsPath = productsFilePath;
            try
            { 
                File.WriteAllText(productsPath, json);
            }
            catch (Exception ex)
            { 
                return Response<Product>.createResponse(null, "Помилка при збереженні продуктів: " + ex.Message, 
                    Response<Product>.responseStatus.error);
            }

            return Response<Product>.createResponse(product, "Продукт успішно збережено", 
                Response<Product>.responseStatus.success);
        }
        public Response<ProductAlias> AddProductAlias(ProductAlias productAlias)
        {
            Response<List<ProductAlias>> productAliasesResponse = GetProductAliases();
            if (productAliasesResponse.status == Response<List<ProductAlias>>.responseStatus.error)
            {
                return Response<ProductAlias>.createResponse(null, "Помилка при завантаженні псевдонімів продуктів: " + productAliasesResponse.message, 
                    Response<ProductAlias>.responseStatus.error);
            }
            List<ProductAlias> productAliases = productAliasesResponse.data;
            productAliases.Add(productAlias);
            
            string json = JsonConvert.SerializeObject(productAliases, Formatting.Indented);
            string productAliasesPath = productAliasesFilePath;
            try
            { 
                File.WriteAllText(productAliasesPath, json);
            }
            catch (Exception ex)
            { 
                return Response<ProductAlias>.createResponse(null, "Помилка при збереженні псевдонімів продуктів: " + ex.Message, 
                    Response<ProductAlias>.responseStatus.error);
            }
        
            return Response<ProductAlias>.createResponse(productAlias, "Псевдонім продукту успішно збережено", 
                Response<ProductAlias>.responseStatus.success);
        }

        public Response<Category> AddCategory(Category category)
        {
            Response<List<Category>> categoriesResponse = GetCategories();
            if (categoriesResponse.status == Response<List<Category>>.responseStatus.error)
            {
                return Response<Category>.createResponse(null, "Помилка при завантаженні категорій: " + categoriesResponse.message, 
                    Response<Category>.responseStatus.error);
            }
            List<Category> categories = categoriesResponse.data;
            categories.Add(category);
            
            string json = JsonConvert.SerializeObject(categories, Formatting.Indented);
            string categoriesPath = categoriesFilePath;
            try
            { 
                File.WriteAllText(categoriesPath, json);
            }
            catch (Exception ex)
            { 
                return Response<Category>.createResponse(null, "Помилка при збереженні категорій: " + ex.Message, 
                    Response<Category>.responseStatus.error);
            }
        
            return Response<Category>.createResponse(category, "Категорія успішно збережено", 
                Response<Category>.responseStatus.success);
        }

        #endregion

        #region Update Methods

        public Response<Recipe> UpdateRecipe(Recipe recipe)
        {
            if (!Directory.Exists(baseRecipesFolder))
            {
                DatabaseSeeder.EnsureDirectoriesExist(rootDataDirectory);
            }

            string rescipeFolder = Path.Combine(baseRecipesFolder, recipe.ID.ToString());
            if(!Directory.Exists(rescipeFolder))
            { 
                return Response<Recipe>.createResponse(null, "не знайдено директорію для рецепту " + recipe.ID,
                    Response<Recipe>.responseStatus.error);
            }
            
            try
            {
                List<string> usedPhotos = new List<string>();
                string photosFolder = Path.Combine(rescipeFolder, "Photos");
                if (!Directory.Exists(photosFolder))
                {
                    Directory.CreateDirectory(photosFolder);
                }
                List<string> photoPaths = Directory.GetFiles(Path.Combine(rescipeFolder, "Photos")).ToList();
                
                // Якщо шлях уже відносний, відновлюємо його до абсолютного для перевірки копіювання
                string mainPhotoSource = GetAbsolutePath(recipe.MainPhotoPath);
                string destMainPhotoPath = Path.Combine(rescipeFolder, "Photos", Path.GetFileName(mainPhotoSource));

                if(string.IsNullOrEmpty(mainPhotoSource) || !File.Exists(mainPhotoSource)) 
                {
                    recipe.MainPhotoPath = "";
                }
                else if(mainPhotoSource != destMainPhotoPath)
                {
                    Response<string> copyMainPhotoResponse = this.CopyPhoto(mainPhotoSource, destMainPhotoPath);
                    if (copyMainPhotoResponse.status == Response<string>.responseStatus.success)
                    {
                        recipe.MainPhotoPath = GetRelativePath(copyMainPhotoResponse.data); // 🔥 Зберігаємо відносний
                        usedPhotos.Add(destMainPhotoPath);
                    }
                    else
                    {
                        recipe.MainPhotoPath = "";
                    }
                }
                else
                { 
                    recipe.MainPhotoPath = GetRelativePath(destMainPhotoPath); // 🔥 Зберігаємо відносний
                    usedPhotos.Add(destMainPhotoPath);
                }

                foreach(Step step in recipe.PreparationSteps)
                { 
                    string stepPhotoSource = GetAbsolutePath(step.PhotoPath);
                    string destStepPhotoPath = Path.Combine(rescipeFolder, "Photos", Path.GetFileName(stepPhotoSource));
                    
                    if(string.IsNullOrEmpty(stepPhotoSource) || !File.Exists(stepPhotoSource))
                    {
                        step.PhotoPath = "";
                    }
                    else if(stepPhotoSource != destStepPhotoPath)
                    {
                        Response<string> copyStepPhotoResponse = this.CopyPhoto(stepPhotoSource, destStepPhotoPath);
                        if (copyStepPhotoResponse.status == Response<string>.responseStatus.success)
                        {
                            step.PhotoPath = GetRelativePath(copyStepPhotoResponse.data); // 🔥 Зберігаємо відносний
                            usedPhotos.Add(destStepPhotoPath);
                        }
                        else
                        {
                            step.PhotoPath = "";
                        }
                    }
                    else
                    { 
                        step.PhotoPath = GetRelativePath(destStepPhotoPath); // 🔥 Зберігаємо відносний
                        usedPhotos.Add(destStepPhotoPath);
                    }
                }

                List<string> photosToDelete = photoPaths.Where(p => !usedPhotos.Contains(p)).ToList();
                foreach(string photo in photosToDelete)
                { 
                    File.Delete(photo);
                }

                string json = JsonConvert.SerializeObject(recipe, Formatting.Indented);
                File.WriteAllText(Path.Combine(rescipeFolder, "recipe.json"), json);
            }
            catch (Exception ex)
            { 
                return Response<Recipe>.createResponse(null, "Помилка при оновленні рецепту: " + ex.Message, 
                    Response<Recipe>.responseStatus.error);
            }

            return Response<Recipe>.createResponse(recipe, "Рецепт успішно оновлено", 
                Response<Recipe>.responseStatus.success);
        }

        public Response<CookBook> UpdateCookBook(CookBook cookBook)
        {
            Response<List<CookBook>> cookBooksResponse = GetCookBooks();
            if (cookBooksResponse.status == Response<List<CookBook>>.responseStatus.error)
            {
                return Response<CookBook>.createResponse(null, "Помилка при завантаженні книг рецептів: " + cookBooksResponse.message, 
                    Response<CookBook>.responseStatus.error);
            }
            List<CookBook> cookBooks = cookBooksResponse.data;
            foreach(CookBook cb in cookBooks)
            {
                if (cb.ID == cookBook.ID)
                {
                    cb.Name = cookBook.Name;
                    cb.Description = cookBook.Description;
                    cb.LastEdited = cookBook.LastEdited;
                    cb.IsPublic = cookBook.IsPublic;
                    cb.ReferencedRecipes = cookBook.ReferencedRecipes;
                    break;
                }
            }

            string json = JsonConvert.SerializeObject(cookBooks, Formatting.Indented);
            File.WriteAllText(cookBooksFilePath, json);

            return Response<CookBook>.createResponse(cookBook, "Книга рецептів успішно оновлена", 
                Response<CookBook>.responseStatus.success);
        }
        public Response<Product> UpdateProduct(Product product)
        {
            Response<List<Product>> productsResponse = GetProducts();
            if (productsResponse.status == Response<List<Product>>.responseStatus.error)
            {
                return Response<Product>.createResponse(null, "Помилка при завантаженні продуктів: " + productsResponse.message, 
                    Response<Product>.responseStatus.error);
            }
            List<Product> products = productsResponse.data;
            foreach(Product p in products)
            {
                if (p.ID == product.ID)
                {
                    p.Name = product.Name;
                    p.Calories = product.Calories;
                    p.Proteins = product.Proteins;
                    p.Fats = product.Fats;
                    p.Carbohydrates = product.Carbohydrates;
                    p.IsCustom = product.IsCustom;
                    break;
                }
            }
            string json = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText(productsFilePath, json);
            
            return Response<Product>.createResponse(product, "Продукт успішно оновлено", 
                Response<Product>.responseStatus.success);
        }
        public Response<ProductAlias> UpdateProductAlias(ProductAlias productAlias)
        {
            Response<List<ProductAlias>> productAliasesResponse = GetProductAliases();
            if (productAliasesResponse.status == Response<List<ProductAlias>>.responseStatus.error)
            {
                return Response<ProductAlias>.createResponse(null, "Помилка при завантаженні псевдонімів продуктів: " + productAliasesResponse.message, 
                    Response<ProductAlias>.responseStatus.error);
            }
            List<ProductAlias> productAliases = productAliasesResponse.data;
            foreach(ProductAlias pa in productAliases)
            {
                if (pa.ID == productAlias.ID)
                {
                    pa.AliasName = productAlias.AliasName;
                    pa.ReferencedProduct = productAlias.ReferencedProduct;
                    break;
                }
            }
            string json = JsonConvert.SerializeObject(productAliases, Formatting.Indented);
            File.WriteAllText(productAliasesFilePath, json);

            return Response<ProductAlias>.createResponse(productAlias, "Псевдонім продукту успішно оновлено", 
                Response<ProductAlias>.responseStatus.success);
        }

        public Response<Category> UpdateCategory(Category category)
        {
            Response<List<Category>> categoriesResponse = GetCategories();
            if (categoriesResponse.status == Response<List<Category>>.responseStatus.error)
            {
                return Response<Category>.createResponse(null, "Помилка при завантаженні категорій: " + categoriesResponse.message, 
                    Response<Category>.responseStatus.error);
            }
            List<Category> categories = categoriesResponse.data;
            foreach(Category c in categories)
            {
                if (c.ID == category.ID)
                {
                    c.Name = category.Name;
                    c.Description = category.Description;
                    break;
                }
            }
            string json = JsonConvert.SerializeObject(categories, Formatting.Indented);
            File.WriteAllText(categoriesFilePath, json);

            return Response<Category>.createResponse(category, "Категорія успішно оновлено", 
                Response<Category>.responseStatus.success);
        }

        #endregion

        public Response<bool> RemoveObject(ILocalRepository.dataType data, Guid ID)
        {
            switch(data)
            {
                case ILocalRepository.dataType.recipe:
                {
                    string recipeFolder = Path.Combine(baseRecipesFolder, ID.ToString());
                    if (!Directory.Exists(recipeFolder))
                    {
                        return Response<bool>.createResponse(false, "Рецепт за ID " + ID + " не знайдено", 
                            Response<bool>.responseStatus.error);
                    }
                    try
                    {
                        Directory.Delete(recipeFolder, true);
                        return Response<bool>.createResponse(true, "Рецепт успішно видалено", 
                            Response<bool>.responseStatus.success);
                    }
                    catch (Exception ex)
                    {
                        return Response<bool>.createResponse(false, "Помилка при видаленні рецепту: " + ex.Message, 
                            Response<bool>.responseStatus.error);
                    }
                }
                case ILocalRepository.dataType.cookBook:
                {
                    string filePath = cookBooksFilePath;
                    return RemoveFromJsonFile<CookBook>( ID, GetCookBooks, filePath);
                }
                    
                case ILocalRepository.dataType.product:
                { 
                    string filePath = productsFilePath;
                    return RemoveFromJsonFile<Product>( ID, GetProducts, filePath);
                }
                case ILocalRepository.dataType.productAlias:
                {
                    string filePath = productAliasesFilePath;
                    return RemoveFromJsonFile<ProductAlias>(ID, GetProductAliases, filePath);
                }
                case ILocalRepository.dataType.category:
                {
                    string filePath = categoriesFilePath;
                    return RemoveFromJsonFile<Category>(ID, GetCategories, filePath);
                }
                default:
                { 
                    return Response<bool>.createResponse(false, "Невідомий тип даних для видалення", 
                        Response<bool>.responseStatus.error);
                }
            }
        }


        private Response<bool> RemoveFromJsonFile<T>(Guid ID, Func<Response<List<T>>> getListMethod, string filePath) 
        where T : IEntity
        {
            Response<List<T>> listResponse = getListMethod();
            if (listResponse.status == Response<List<T>>.responseStatus.error)
            {
                return Response<bool>.createResponse(false, "Помилка при завантаженні даних: " + listResponse.message, 
                    Response<bool>.responseStatus.error);
            }

            List<T> list = listResponse.data;
            int removedCount = list.RemoveAll(item => item.ID == ID);
    
            if (removedCount == 0)
            {
                return Response<bool>.createResponse(false, "Об'єкт за ID " + ID + " не знайдено", 
                    Response<bool>.responseStatus.error);
            }

            try
            {
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                File.WriteAllText(filePath, json);
                return Response<bool>.createResponse(true, "Об'єкт успішно видалено", 
                    Response<bool>.responseStatus.success);
            }
            catch (Exception ex)
            {
                return Response<bool>.createResponse(false, "Помилка при збереженні даних: " + ex.Message, 
                    Response<bool>.responseStatus.error);
            }
        }

        private Response<string> CopyPhoto(string sourcePath, string destinationPath)
        {
            if (!string.IsNullOrEmpty(sourcePath) && File.Exists(sourcePath))
            {
                try
                {
                    File.Copy(sourcePath, destinationPath, true);
                    return Response<string>.createResponse(destinationPath, "Фотографія успішно скопійована", 
                        Response<string>.responseStatus.success);
                }
                catch (Exception ex)
                {
                    return Response<string>.createResponse(null, "Помилка при копіюванні фотографії: " + ex.Message, 
                        Response<string>.responseStatus.error);
                }
            }
            return Response<string>.createResponse(null, "Невірний шлях до фотографії", 
                Response<string>.responseStatus.error);
        }

        private string GetRelativePath(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath)) return "";
            
            // Шукаємо індекс початку папки проекту (наприклад, твоєї кореневої папки Local або Global)
            string appRoot = AppDomain.CurrentDomain.BaseDirectory;
            if (fullPath.StartsWith(appRoot, StringComparison.OrdinalIgnoreCase))
            {
                return fullPath.Substring(appRoot.Length);
            }
            return fullPath;
        }

        private string GetAbsolutePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return "";
            if (Path.IsPathRooted(relativePath)) return relativePath; // Якщо вже абсолютний

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        }

        
    }
}

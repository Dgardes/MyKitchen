using MyKitchen_Core.DTO.CreateAndUpdateDTOs;
using MyKitchen_Core.DTO.SearchDTOs;
using MyKitchen_Core.Entities;
using MyKitchen_Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKitchen_Core.Services
{
    public class Validator
    {
        private static Validator _instance;
        private Validator() { }
        public static Validator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Validator();
                }
                return _instance;
            }
        }
        #region create dto validation
        public Response<CreateRecipe_dto> validateCreateRecipeDTO(CreateRecipe_dto dto)
        { 
            if (string.IsNullOrEmpty(dto.Name))
            {
                return Response<CreateRecipe_dto>.createResponse(null, "Назва рецепту не може бути порожньою.", 
                    Response<CreateRecipe_dto>.responseStatus.error);
            }
            if (dto.Name.Length < 3)
            {
                return Response<CreateRecipe_dto>.createResponse(null, "Назва рецепту менше 3 символів.", 
                    Response<CreateRecipe_dto>.responseStatus.error);
            }
            if(dto.Name.Length > 45)
            {
                return Response<CreateRecipe_dto>.createResponse(null, "Назва рецепту не може бути довшою за 45 символів.",
                    Response<CreateRecipe_dto>.responseStatus.error);
            }
            if(dto.SourceLink.Length > 1000)
            {
                return Response<CreateRecipe_dto>.createResponse(null, "Посилання на джерело не може бути довшим за 1000 символів.",
                    Response<CreateRecipe_dto>.responseStatus.error);
            }
            if(dto.Description.Length > 2000)
            {
                return Response<CreateRecipe_dto>.createResponse(null, "Опис рецепту не може бути довшим за 2000 символів.",
                    Response<CreateRecipe_dto>.responseStatus.error);
            }
            if(dto.Servings != null && dto.Servings <= 0)
            {
                return Response<CreateRecipe_dto>.createResponse(null, "Кількість порцій повинна бути більше нуля.",
                    Response<CreateRecipe_dto>.responseStatus.error);
            }
            if(dto.cookBooksReferencedIn != null && dto.cookBooksReferencedIn.Count > 0)
            {
                //
                // технічний борг: винести метод пошуку в DataService, щоб не створювати залежність від нього в Validator.
                //

                Response<List<CookBook>> cookBooksResponse = DataService.Instance.GetCookBooks(DataService.DataScope.Local);
                if(cookBooksResponse.status == Response<List<CookBook>>.responseStatus.error)
                {
                    return Response<CreateRecipe_dto>.createResponse(null, "Помилка при отриманні збірок для валідації.",
                        Response<CreateRecipe_dto>.responseStatus.error);
                }

                List<Guid> cookBookIDs = cookBooksResponse.data.Select(cb => cb.ID).ToList();
                string message = "";

                List<Guid> nonExistentCookBooks = dto.cookBooksReferencedIn
                .Where(id => !cookBookIDs.Contains(id)).ToList();
        
                if(nonExistentCookBooks.Count > 0)
                {
                    foreach(Guid n in nonExistentCookBooks)
                    { 
                        message += "Збірка з ID " + n + " не знайдена.\n";
                    }
                }

                if(message != "")
                {
                    return Response<CreateRecipe_dto>.createResponse(null, message,
                        Response<CreateRecipe_dto>.responseStatus.warning);
                }
            }

            return Response<CreateRecipe_dto>.createResponse(dto, "Валідація успішна.", 
                Response<CreateRecipe_dto>.responseStatus.success);
        }

        public Response<CreateCookBook_dto> validateCreateCookBookDTO(CreateCookBook_dto dto)
        {
            if (string.IsNullOrEmpty(dto.Name))
            {
                return Response<CreateCookBook_dto>.createResponse(null, "Назва збірки не може бути порожньою.",
                    Response<CreateCookBook_dto>.responseStatus.error);
            }
            if (dto.Name.Length < 3)
            {
                return Response<CreateCookBook_dto>.createResponse(null, "Назва збірки менше 3 символів.",
                    Response<CreateCookBook_dto>.responseStatus.error);
            }
            if (dto.Name.Length > 45)
            {
                return Response<CreateCookBook_dto>.createResponse(null, "Назва збірки не може бути довшою за 45 символів.",
                    Response<CreateCookBook_dto>.responseStatus.error);
            }
            if (dto.Description.Length > 2000)
            {
                return Response<CreateCookBook_dto>.createResponse(null, "Опис збірки не може бути довшим за 2000 символів.",
                    Response<CreateCookBook_dto>.responseStatus.error);
            }
            if(dto.ReferencedRecipes != null && dto.ReferencedRecipes.Count > 0)
            { 
                //
                // технічний борг: винести метод пошуку в DataService, щоб не створювати залежність від нього в Validator.
                //

                Response<List<Recipe>> recipesResponse = DataService.Instance.GetRecipes(DataService.DataScope.Local);
                if(recipesResponse.status == Response<List<Recipe>>.responseStatus.error)
                {
                    return Response<CreateCookBook_dto>.createResponse(null, "Помилка при отриманні рецептів для валідації.",
                        Response<CreateCookBook_dto>.responseStatus.error);
                }
                
                List<Guid> recipesIDs = recipesResponse.data.Select(r => r.ID).ToList();
                string message = "";

                List<Guid> nonExistentRecipes = dto.ReferencedRecipes
                .Where(id => !recipesIDs.Contains(id)).ToList();

                if(nonExistentRecipes.Count > 0)
                {
                    foreach(Guid n in nonExistentRecipes)
                    { 
                        message += "Рецепт з ID " + n + " не знайдено.\n";
                    }
                }

                if(message != "")
                {
                    return Response<CreateCookBook_dto>.createResponse(null, message,
                        Response<CreateCookBook_dto>.responseStatus.warning);
                }
            }

            return Response<CreateCookBook_dto>.createResponse(dto, "Валідація успішна.",
                Response<CreateCookBook_dto>.responseStatus.success);
        }

        public Response<CreateProduct_dto> validateCreateProductDTO(CreateProduct_dto dto)
        {
            if (string.IsNullOrEmpty(dto.Name))
            {
                return Response<CreateProduct_dto>.createResponse(null, "Назва продукту не може бути порожньою.",
                    Response<CreateProduct_dto>.responseStatus.error);
            }
            if (dto.Name.Length < 3)
            {
                return Response<CreateProduct_dto>.createResponse(null, "Назва продукту менше 3 символів.",
                    Response<CreateProduct_dto>.responseStatus.error);
            }
            if (dto.Name.Length > 45)
            {
                return Response<CreateProduct_dto>.createResponse(null, "Назва продукту не може бути довшою за 45 символів.",
                    Response<CreateProduct_dto>.responseStatus.error);
            }
            if (dto.Proteins < 0)
            {
                return Response<CreateProduct_dto>.createResponse(null, "Кількість білків не може бути від'ємною.",
                    Response<CreateProduct_dto>.responseStatus.error);
            }
            if (dto.Fats < 0)
            {
                return Response<CreateProduct_dto>.createResponse(null, "Кількість жирів не може бути від'ємною.",
                    Response<CreateProduct_dto>.responseStatus.error);
            }
            if (dto.Carbohydrates < 0)
            {
                return Response<CreateProduct_dto>.createResponse(null, "Кількість вуглеводів не може бути від'ємною.",
                    Response<CreateProduct_dto>.responseStatus.error);
            }
            if (dto.Calories < 0)
            {
                return Response<CreateProduct_dto>.createResponse(null, "Кількість калорій не може бути від'ємною.",
                    Response<CreateProduct_dto>.responseStatus.error);
            }
            if (dto.Proteins + dto.Fats + dto.Carbohydrates > 100)
            {
                return Response<CreateProduct_dto>.createResponse(null, "Сума білків, жирів і вуглеводів не може перевищувати 100г.",
                    Response<CreateProduct_dto>.responseStatus.error);
            }
            Response<double> calcResponse = 
            CaloriesCalculator.calculateCalories(dto.Proteins ?? 0, dto.Carbohydrates ?? 0, dto.Fats ?? 0);
            double minCalories = calcResponse.data;
            
            if(dto.Calories < minCalories) 
            {
                return Response<CreateProduct_dto>.createResponse(null, "Калорійність продукта не може бути менше " + minCalories + " ккал на 100г",
                    Response<CreateProduct_dto>.responseStatus.error);
            }

            return Response<CreateProduct_dto>.createResponse(dto, "Валідація успішна.",
                Response<CreateProduct_dto>.responseStatus.success);
        }

        public Response<CreateProductAlias_dto> validateCreateProductAliasDTO(CreateProductAlias_dto dto)
        {
            if (string.IsNullOrEmpty(dto.AliasName))
            {
                return Response<CreateProductAlias_dto>.createResponse(null, "Назва псевдоніму продукту не може бути порожньою.",
                    Response<CreateProductAlias_dto>.responseStatus.error);
            }
            if (dto.AliasName.Length < 3)
            {
                return Response<CreateProductAlias_dto>.createResponse(null, "Назва псевдоніму продукту менше 3 символів.",
                    Response<CreateProductAlias_dto>.responseStatus.error);
            }
            if (dto.AliasName.Length > 45)
            {
                return Response<CreateProductAlias_dto>.createResponse(null, "Назва псевдоніму продукту не може бути довшою за 45 символів.",
                    Response<CreateProductAlias_dto>.responseStatus.error);
            }
            //
            //технічний борг: винести метод пошуку в DataService, щоб не створювати залежність від нього в Validator.
            //
            Response<Product> productResponse = DataService.Instance.GetProduct(dto.ReferencedProduct, DataService.DataScope.Local);
            if (productResponse.status == Response<Product>.responseStatus.error)
            {
                return Response<CreateProductAlias_dto>.createResponse(null, "Помилка при пошуку референтного продукту для синоніма. ID продукта: " + dto.ReferencedProduct,
                    Response<CreateProductAlias_dto>.responseStatus.error);
            }
            if (productResponse.data == null)
            {
                return Response<CreateProductAlias_dto>.createResponse(null, "Референтний продукт не знайдено. ID продукта: " + dto.ReferencedProduct,
                    Response<CreateProductAlias_dto>.responseStatus.error);
            }

            return Response<CreateProductAlias_dto>.createResponse(dto, "Валідація успішна.",
                Response<CreateProductAlias_dto>.responseStatus.success);
        }

        public Response<CreateCategory_dto> validateCreateCategoryDTO(CreateCategory_dto dto)
        {
            if (string.IsNullOrEmpty(dto.Name))
            {
                return Response<CreateCategory_dto>.createResponse(null, "Назва категорії не може бути порожньою.",
                    Response<CreateCategory_dto>.responseStatus.error);
            }
            if (dto.Name.Length < 3)
            {
                return Response<CreateCategory_dto>.createResponse(null, "Назва категорії менше 3 символів.",
                    Response<CreateCategory_dto>.responseStatus.error);
            }
            if (dto.Name.Length > 45)
            {
                return Response<CreateCategory_dto>.createResponse(null, "Назва категорії не може бути довшою за 45 символів.",
                    Response<CreateCategory_dto>.responseStatus.error);
            }
            if(dto.Description.Length > 2000)
            {
                return Response<CreateCategory_dto>.createResponse(null, "Опис категорії не може бути довшим за 2000 символів.",
                    Response<CreateCategory_dto>.responseStatus.error);
            }

            return Response<CreateCategory_dto>.createResponse(dto, "Валідація успішна.",
                Response<CreateCategory_dto>.responseStatus.success);
        }

        #endregion

        #region update dto validation

        public Response<UpdateRecipe_dto> validateUpdateRecipeDTO(UpdateRecipe_dto dto)
        {
            if(dto.ID == Guid.Empty)
            {
                return Response<UpdateRecipe_dto>.createResponse(null, "ID рецепта не може бути порожнім.",
                    Response<UpdateRecipe_dto>.responseStatus.error);
            }
            Response<CreateRecipe_dto> baseRes = validateCreateRecipeDTO(dto);

            Response<UpdateRecipe_dto>.responseStatus finalStatus = Response<UpdateRecipe_dto>.responseStatus.success;

            if(baseRes.status == Response<CreateRecipe_dto>.responseStatus.error)
                finalStatus = Response<UpdateRecipe_dto>.responseStatus.error;
            else if(baseRes.status == Response<CreateRecipe_dto>.responseStatus.warning)
                finalStatus = Response<UpdateRecipe_dto>.responseStatus.warning;

            UpdateRecipe_dto dataToReturn = (finalStatus == Response<UpdateRecipe_dto>.responseStatus.error) ? null : dto;

            return Response<UpdateRecipe_dto>.createResponse(dataToReturn, baseRes.message, finalStatus);
        }

        public Response<UpdateCookBook_dto> validateUpdateCookBookDTO(UpdateCookBook_dto dto)
        {
            if(dto.ID == Guid.Empty)
            {
                return Response<UpdateCookBook_dto>.createResponse(null, "ID збірки не може бути порожнім.",
                    Response<UpdateCookBook_dto>.responseStatus.error);
            }
            Response<CreateCookBook_dto> baseRes = validateCreateCookBookDTO(dto);

            Response<UpdateCookBook_dto>.responseStatus finalStatus = Response<UpdateCookBook_dto>.responseStatus.success;

            if(baseRes.status == Response<CreateCookBook_dto>.responseStatus.error)
                finalStatus = Response<UpdateCookBook_dto>.responseStatus.error;
            else if(baseRes.status == Response<CreateCookBook_dto>.responseStatus.warning)
                finalStatus = Response<UpdateCookBook_dto>.responseStatus.warning;

            UpdateCookBook_dto dataToReturn = (finalStatus == Response<UpdateCookBook_dto>.responseStatus.error) ? null : dto;

            return Response<UpdateCookBook_dto>.createResponse(dataToReturn, baseRes.message, finalStatus);
        }

        public Response<UpdateProduct_dto> validateUpdateProductDTO(UpdateProduct_dto dto)
        {
            if(dto.ID == Guid.Empty)
            {
                return Response<UpdateProduct_dto>.createResponse(null, "ID продукту не може бути порожнім.",
                    Response<UpdateProduct_dto>.responseStatus.error);
            }
            Response<CreateProduct_dto> baseRes = validateCreateProductDTO(dto);

            Response<UpdateProduct_dto>.responseStatus finalStatus = Response<UpdateProduct_dto>.responseStatus.success;

            if(baseRes.status == Response<CreateProduct_dto>.responseStatus.error)
                finalStatus = Response<UpdateProduct_dto>.responseStatus.error;
            else if(baseRes.status == Response<CreateProduct_dto>.responseStatus.warning)
                finalStatus = Response<UpdateProduct_dto>.responseStatus.warning;

            UpdateProduct_dto dataToReturn = (finalStatus == Response<UpdateProduct_dto>.responseStatus.error) ? null : dto;

            return Response<UpdateProduct_dto>.createResponse(dataToReturn, baseRes.message, finalStatus);
        }

        public Response<UpdateCategory_dto> validateUpdateCategoryDTO(UpdateCategory_dto dto)
        {
            if(dto.ID == Guid.Empty)
            {
                return Response<UpdateCategory_dto>.createResponse(null, "ID категорії не може бути порожнім.",
                    Response<UpdateCategory_dto>.responseStatus.error);
            }
            Response<CreateCategory_dto> baseRes = validateCreateCategoryDTO(dto);

            Response<UpdateCategory_dto>.responseStatus finalStatus = Response<UpdateCategory_dto>.responseStatus.success;

            if(baseRes.status == Response<CreateCategory_dto>.responseStatus.error)
                finalStatus = Response<UpdateCategory_dto>.responseStatus.error;
            else if(baseRes.status == Response<CreateCategory_dto>.responseStatus.warning)
                finalStatus = Response<UpdateCategory_dto>.responseStatus.warning;

            UpdateCategory_dto dataToReturn = (finalStatus == Response<UpdateCategory_dto>.responseStatus.error) ? null : dto;

            return Response<UpdateCategory_dto>.createResponse(dataToReturn, baseRes.message, finalStatus);
        }

        public Response<UpdateProductAlias_dto> validateUpdateProductAliasDTO(UpdateProductAlias_dto dto)
        {
            if(dto.ID == Guid.Empty)
            {
                return Response<UpdateProductAlias_dto>.createResponse(null, "ID продукту не може бути порожнім.",
                    Response<UpdateProductAlias_dto>.responseStatus.error);
            }
            Response<CreateProductAlias_dto> baseRes = validateCreateProductAliasDTO(dto);

            Response<UpdateProductAlias_dto>.responseStatus finalStatus = Response<UpdateProductAlias_dto>.responseStatus.success;

            if(baseRes.status == Response<CreateProductAlias_dto>.responseStatus.error)
                finalStatus = Response<UpdateProductAlias_dto>.responseStatus.error;
            else if(baseRes.status == Response<CreateProductAlias_dto>.responseStatus.warning)
                finalStatus = Response<UpdateProductAlias_dto>.responseStatus.warning;

            UpdateProductAlias_dto dataToReturn = (finalStatus == Response<UpdateProductAlias_dto>.responseStatus.error) ? null : dto;

            return Response<UpdateProductAlias_dto>.createResponse(dataToReturn, baseRes.message, finalStatus);
        }

        #endregion

        #region entities validation
        public Response<Recipe> validateRecipe(Recipe recipe)
        {
            if (recipe == null)
            { return Response<Recipe>.createResponse(null, "Рецепт не ініціалізовано.", Response<Recipe>.responseStatus.error); }

            string warningMessages = "";
        
            if (recipe.ID == Guid.Empty)
            { 
                return Response<Recipe>.createResponse(null, "ID рецепта відсутній або порожній.", 
                    Response<Recipe>.responseStatus.error); 
            }
            if (string.IsNullOrWhiteSpace(recipe.Name))
            {
                recipe.Name = "Untitled Recipe";
                warningMessages += "Назва рецепту була порожньою. Встановлено назву за замовчуванням.\n";
            }
            else if (recipe.Name.Length < 3)
            {
                warningMessages += "Попередження: Назва рецепту занадто коротка.\n";
            }
            else if (recipe.Name.Length > 45)
            {
                recipe.Name = recipe.Name.Substring(0, 45);
                warningMessages += "Назва рецепту перевищує 45 символів і була обрізана.\n";
            }

            if (recipe.Description?.Length > 2000)
            {
                recipe.Description = recipe.Description.Substring(0, 2000);
                warningMessages += "Опис занадто довгий і був обрізаний. Максимально 2000 символів.\n";
            }
            if (recipe.SourceLink?.Length > 1000)
            {
                recipe.SourceLink = recipe.SourceLink.Substring(0, 1000);
                warningMessages += "Посилання занадто довге і було обрізане. Максимально 1000 символів.\n";
            }
            if (recipe.Servings <= 0)
            {
                recipe.Servings = 1;
                warningMessages += "Кількість порцій не може бути <= 0. Встановлено 1.\n";
            }
            if (recipe.FullPreparationTime < TimeSpan.Zero)
            {
                recipe.FullPreparationTime = TimeSpan.Zero;
                warningMessages += "Загальний час приготування був від'ємним. Скинуто до 0.\n";
            }

            if (recipe.ActivePreparationTime > recipe.FullPreparationTime)
            {
                recipe.ActivePreparationTime = recipe.FullPreparationTime;
                warningMessages += "Активний час не може бути більшим за загальний. Прирівняно до загального.\n";
            }
            if (recipe.Rating < 0 || recipe.Rating > 5)
            {
                recipe.Rating = Math.Clamp(recipe.Rating, 0, 5);
                warningMessages += "Рейтинг має бути в межах 0-5. Значення скориговано.\n";
            }
            if(recipe.Ingredients != null && recipe.Ingredients.Count != 0)
            {
                foreach(Ingredient ingr in recipe.Ingredients)
                {
                    Response<Ingredient> res = validateIngredient(ingr);
                    if(res.status == Response<Ingredient>.responseStatus.error)
                    {
                        warningMessages += "Помилка валідації інгредієнта: " + ingr.RawName + ".\n";
                    }
                    else if(res.status == Response<Ingredient>.responseStatus.warning)
                    {
                        warningMessages += "Попередження в інгредієнті " + ingr.RawName + ": " + res.message + "\n";
                    }
                }
            }
            if (recipe.PreparationSteps != null && recipe.PreparationSteps.Count != 0)
            {
                foreach(Step step in recipe.PreparationSteps)
                {
                    Response<Step> res = validateStep(step);
                    if(res.status == Response<Step>.responseStatus.error)
                    {
                        warningMessages += "Помилка валідації кроку: " + step.Description + ".\n";
                    }
                    else if(res.status == Response<Step>.responseStatus.warning)
                    {
                        warningMessages += "Попередження у кроці " + step.Order + ": " + res.message + "\n";
                    }
                }
            }

            if (warningMessages.Length > 0)
            {
                return Response<Recipe>.createResponse(recipe, warningMessages, 
                    Response<Recipe>.responseStatus.warning);
            }

            return Response<Recipe>.createResponse(recipe, "Валідація успішна.", 
                Response<Recipe>.responseStatus.success);
        }

        public Response<CookBook> validateCookBook(CookBook cookBook)
        {
            if (cookBook == null)
            { return Response<CookBook>.createResponse(null, "Збірка не ініціалізована.", Response<CookBook>.responseStatus.error); }
            string warningMessages = "";
        
            if (cookBook.ID == Guid.Empty)
            { 
                return Response<CookBook>.createResponse(null, "ID збірки відсутній або порожній.", 
                    Response<CookBook>.responseStatus.error); 
            }
            if (string.IsNullOrWhiteSpace(cookBook.Name))
            {
                cookBook.Name = "Untitled CookBook";
                warningMessages += "Назва збірки була порожньою. Встановлено назву за замовчуванням.\n";
            }
            else if (cookBook.Name.Length < 3)
            {
                warningMessages += "Попередження: Назва збірки занадто коротка.\n";
            }
            else if (cookBook.Name.Length > 45)
            {
                cookBook.Name = cookBook.Name.Substring(0, 45);
                warningMessages += "Назва збірки перевищує 45 символів і була обрізана.\n";
            }
            if (cookBook.Description?.Length > 2000)
            {
                cookBook.Description = cookBook.Description.Substring(0, 2000);
                warningMessages += "Опис занадто довгий і був обрізаний. Максимально 2000 символів.\n";
            }
            if (warningMessages.Length > 0)
            {
                return Response<CookBook>.createResponse(cookBook, warningMessages, 
                    Response<CookBook>.responseStatus.warning);
            }
            return Response<CookBook>.createResponse(cookBook, "Валідація успішна.", 
                Response<CookBook>.responseStatus.success);
        }

        public Response<Product> validateProduct(Product product)
        {
            if (product == null)
            { return Response<Product>.createResponse(null, "Продукт не ініціалізовано.", Response<Product>.responseStatus.error); }
            string warningMessages = "";
        
            if (product.ID == Guid.Empty)
            { 
                return Response<Product>.createResponse(null, "ID продукту відсутній або порожній.", 
                    Response<Product>.responseStatus.error); 
            }
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                product.Name = "Unnamed Product";
                warningMessages += "Назва продукта була порожньою. Встановлено назву за замовчуванням.\n";
            }
            else if (product.Name.Length < 3)
            {
                warningMessages += "Попередження: Назва продукта занадто коротка.\n";
            }
            else if (product.Name.Length > 45)
            {
                product.Name = product.Name.Substring(0, 45);
                warningMessages += "Назва продукта перевищує 45 символів і була обрізана.\n";
            }
            if (warningMessages.Length > 0)
            {
                return Response<Product>.createResponse(product, warningMessages, 
                    Response<Product>.responseStatus.warning);
            }

            double totalGrams = product.Proteins + product.Fats + product.Carbohydrates;
            double normalizedProteins = 0;
            double normalizedFats = 0;
            double normalizedCarbohydrates = 0;

            if (totalGrams > 100)
            {
                normalizedProteins = (product.Proteins / totalGrams) * 100;
                normalizedFats = (product.Fats / totalGrams) * 100;
                normalizedCarbohydrates = (product.Carbohydrates / totalGrams) * 100;

                product.Proteins = normalizedProteins;
                product.Fats = normalizedFats;
                product.Carbohydrates = normalizedCarbohydrates;

                Response<double> calculateResponse = 
                CaloriesCalculator.calculateCalories(product.Proteins, product.Carbohydrates, product.Fats);
                product.Calories = calculateResponse.data;
                
                warningMessages += "Сума білків жирів і вуглеводів не може перевищувати 100г. Співвідношення було нормалізовано";
            }

            Response<double> calculateResponse_2 = 
            CaloriesCalculator.calculateCalories(product.Proteins, product.Carbohydrates, product.Fats);
            double minCalories = calculateResponse_2.data;

            if(product.Calories < minCalories)
            {
                product.Calories = minCalories;
                warningMessages += "Калорії продукту були нижчими за мінімально допустимі " + minCalories + " і були нормалізовані.\n";
            }
            return Response<Product>.createResponse(product, "Валідація успішна.", 
                Response<Product>.responseStatus.success);
        
        }

        public Response<ProductAlias> validateProductAlias(ProductAlias productAlias)
        {
            if (productAlias == null)
            { return Response<ProductAlias>.createResponse(null, "Синонім не ініціалізований.", Response<ProductAlias>.responseStatus.error); }
            string warningMessages = "";
            if (productAlias.ID == Guid.Empty)
            { 
                return Response<ProductAlias>.createResponse(null, "ID синоніма відсутній або порожній.", 
                    Response<ProductAlias>.responseStatus.error); 
            }
            if (productAlias.ReferencedProduct == Guid.Empty)
            {
                return Response<ProductAlias>.createResponse(null, "ID референтного продукту відсутній або порожній для синоніма " + productAlias.ID,
                    Response<ProductAlias>.responseStatus.error);
            }
            //
            // технічний борг: винести метод пошуку в DataService, щоб не створювати залежність від нього в Validator.
            //

            Response<Product> productResponse = DataService.Instance.GetProduct(productAlias.ReferencedProduct, DataService.DataScope.Local);
            if (productResponse.status == Response<Product>.responseStatus.error)
            {
                return Response<ProductAlias>.createResponse(null, "Помилка при пошуку референтного продукту для синоніма. ID продукта: " + productAlias.ReferencedProduct,
                    Response<ProductAlias>.responseStatus.error);
            }
            if(productResponse.data == null)
            {
                return Response<ProductAlias>.createResponse(null, "Референтний продукт для синоніма не знайдений. ID продукта: " + productAlias.ReferencedProduct,
                    Response<ProductAlias>.responseStatus.error);
            }
            if (string.IsNullOrWhiteSpace(productAlias.AliasName))
            {
                productAlias.AliasName = "Unnamed Alias";
                warningMessages += "Назва синоніма була пустою. Встановлено назву за замовчуваннм.\n";
            }
            else if (productAlias.AliasName.Length < 3)
            {
                warningMessages += "Попередження: Назва синоніма занадто коротка.\n";
            }
            else if (productAlias.AliasName.Length > 45)
            {
                productAlias.AliasName = productAlias.AliasName.Substring(0, 45);
                warningMessages += "Назва синоніма перевищує 45 символів і була обрізана.\n";
            }

            if (warningMessages.Length > 0)
            {
                return Response<ProductAlias>.createResponse(productAlias, warningMessages, 
                    Response<ProductAlias>.responseStatus.warning);
            }
            return Response<ProductAlias>.createResponse(productAlias, "Валідація успішна.", 
                Response<ProductAlias>.responseStatus.success);
        }
        
        public Response<Ingredient> validateIngredient(Ingredient ingredient)
        {
            if (ingredient == null)
            { 
                return Response<Ingredient>.createResponse(null, "Інгредієнт не ініціалізований.", 
                    Response<Ingredient>.responseStatus.error); 
            }
            string warningMessages = "";
            

            if(ingredient.ReferencedProduct != Guid.Empty)
            {
                List<ProductAlias> productAliases = new List<ProductAlias>();
                SearchProductAliasCriteria_dto criteria = new SearchProductAliasCriteria_dto
                {
                    ReferencedProduct = ingredient.ReferencedProduct
                };

                //
                // технічний борг: винести метод пошуку в DataService, щоб не створювати залежність від нього в Validator.
                //

                Response<List<ProductAlias>> productAliasesResponse = DataService.Instance.SearchProductAliases(criteria, DataService.DataScope.Local);
                if(productAliasesResponse.status == Response<List<ProductAlias>>.responseStatus.error)
                {
                    warningMessages += "Помилка при пошуку продукту для інгредієнта. ID продукта: " + ingredient.ReferencedProduct + ".\n";
                }
                else
                {
                    productAliases = productAliasesResponse.data;
                    if(!productAliases.Any(p => p.AliasName == ingredient.RawName))
                    {
                        warningMessages += "Попередження: Назва інгредієнта не знайдена в синонімах продукту.\n";
                        if (productAliases.Count > 0)
                        {
                            ingredient.RawName = productAliases[0].AliasName;
                        }
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(ingredient.RawName))
            {
                ingredient.RawName = "Unnamed Ingredient";
                warningMessages += "Назва інгредієнта була пустою. Установлено назву за замовчуванням.\n";
            }
            else if (ingredient.RawName.Length < 3)
            {
                warningMessages += "Попередження: Назва інгредієнта занадто коротка.\n";
            }
            else if (ingredient.RawName.Length > 45)
            {
                ingredient.RawName = ingredient.RawName.Substring(0, 45);
                warningMessages += "Назва інгредієнта перевищує 45 символів і була обрізана.\n";
            }
            if (warningMessages.Length > 0)
            {
                return Response<Ingredient>.createResponse(ingredient, warningMessages, 
                    Response<Ingredient>.responseStatus.warning);
            }
            return Response<Ingredient>.createResponse(ingredient, "Валідація успішна.", 
                Response<Ingredient>.responseStatus.success);
        }

        public Response<Category> validateCategory(Category category)
        {
            
            string warningMessages = "";
            if(category == null)
            { 
                return Response<Category>.createResponse(null, "Категорія не ініціалізована.", 
                    Response<Category>.responseStatus.error); 
            }
            if (category.ID == Guid.Empty)
            { 
                return Response<Category>.createResponse(null, "ID категорії відсутній або порожній.", 
                    Response<Category>.responseStatus.error); 
            }
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                category.Name = "Unnamed Category";
                warningMessages += "Назва категорії була порожньою. Встановлено назву за замовчуванням.\n";
            }
            else if (category.Name.Length < 3)
            {
                warningMessages += "Попередження: Назва категорії занадто коротка.\n";
            }
            else if (category.Name.Length > 45)
            {
                category.Name = category.Name.Substring(0, 45);
                warningMessages += "Назва категорії перевищує 45 символів і була обрізана.\n";
            }
            if (category.Description?.Length > 2000)
            {
                category.Description = category.Description.Substring(0, 2000);
                warningMessages += "Опис занадто довгий і був обрізаний. Максимально 2000 символів.\n";
            }
            
            if (warningMessages.Length > 0)
            {
                return Response<Category>.createResponse(category, warningMessages, 
                    Response<Category>.responseStatus.warning);
            }
            return Response<Category>.createResponse(category, "Валідація успішна.", 
                Response<Category>.responseStatus.success);
        }

        public Response<Step> validateStep(Step step)
        {
            if (step == null)
            { 
                return Response<Step>.createResponse(null, "Крок не ініціалізований.", 
                    Response<Step>.responseStatus.error); 
            }
            string warningMessages = "";
            if (step.ID == Guid.Empty)
            { 
                return Response<Step>.createResponse(null, "ID кроку відсутній або порожній.", 
                    Response<Step>.responseStatus.error); 
            }
            if (step.Description.Length > 2000)
            {
                step.Description = step.Description.Substring(0, 2000);
                warningMessages += "Опис кроку перевищує 2000 символів і був обрізаний.\n";
            }
            if (step.Order < 0)
            {
                step.Order = 0;
                warningMessages += "Порядковий номер кроку не може бути від'ємним. Встановлено 0.\n";
            }
            if (warningMessages.Length > 0)
            {
                return Response<Step>.createResponse(step, warningMessages, 
                    Response<Step>.responseStatus.warning);
            }
            return Response<Step>.createResponse(step, "Валідація успішна.", 
                Response<Step>.responseStatus.success);
        }

        #endregion

    }
}

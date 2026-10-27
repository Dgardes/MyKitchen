using MyKitchen_Core.DataManagers;
using MyKitchen_Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyKitchen_Core.Entities;
using MyKitchen_Core.DTO.SearchDTOs;
namespace MyKitchen_Core.Services
{
    public class Filter
    {
        private static Filter _instance;
        private Filter() { }

        public static Filter Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Filter();
                }
                return _instance;
            }
        }

        public Response<List<Recipe>> FilterRecipes(SearchRecipeCriteria_dto criteria, List<Recipe> recipes, 
            List<Product> products, List<ProductAlias> productAliases)
        {
            IQueryable<Recipe> searchResult = recipes.AsQueryable();

            if (criteria.ID != null)
            { searchResult = searchResult.Where(r => r.ID == criteria.ID); }
            if (criteria.Name != null) 
            { searchResult = searchResult.Where(r => r.Name.ToLower().Contains(criteria.Name.ToLower())); }
            
            if (criteria.FullPreparationTime.Min.HasValue || criteria.FullPreparationTime.Max.HasValue)
            { 
                if (criteria.FullPreparationTime.Min.HasValue)
                { searchResult = searchResult.Where(r => r.FullPreparationTime >= criteria.FullPreparationTime.Min.Value); }
                if (criteria.FullPreparationTime.Max.HasValue)
                { searchResult = searchResult.Where(r => r.FullPreparationTime <= criteria.FullPreparationTime.Max.Value); }
            }
            if (criteria.ActivePreparationTime.Min.HasValue || criteria.ActivePreparationTime.Max.HasValue)
            {
                if (criteria.ActivePreparationTime.Min.HasValue)
                { searchResult = searchResult.Where(r => r.ActivePreparationTime >= criteria.ActivePreparationTime.Min.Value); }
                if (criteria.ActivePreparationTime.Max.HasValue)
                { searchResult = searchResult.Where(r => r.ActivePreparationTime <= criteria.ActivePreparationTime.Max.Value); }
            }
            if (criteria.Servings.Min.HasValue || criteria.Servings.Max.HasValue)
            {
                if (criteria.Servings.Min.HasValue)
                { searchResult = searchResult.Where(r => r.Servings >= criteria.Servings.Min.Value); }
                if (criteria.Servings.Max.HasValue)
                { searchResult = searchResult.Where(r => r.Servings <= criteria.Servings.Max.Value); }
            }
            if (criteria.ReferencedCategories != null && criteria.ReferencedCategories.Count > 0)
            { searchResult = searchResult.Where(r => criteria.ReferencedCategories.All(c => r.ReferencedCategories.Contains(c)));}
            if (criteria.CreationDate.Min.HasValue || criteria.CreationDate.Max.HasValue)
            {
                if (criteria.CreationDate.Min.HasValue)
                { searchResult = searchResult.Where(r => r.CreationDate >= criteria.CreationDate.Min.Value); }
                if (criteria.CreationDate.Max.HasValue)
                { searchResult = searchResult.Where(r => r.CreationDate <= criteria.CreationDate.Max.Value); }
            }
            if (criteria.LastEdited.Min.HasValue || criteria.LastEdited.Max.HasValue)
            {
                if (criteria.LastEdited.Min.HasValue)
                { searchResult = searchResult.Where(r => r.LastEdited >= criteria.LastEdited.Min.Value); }
                if (criteria.LastEdited.Max.HasValue)
                { searchResult = searchResult.Where(r => r.LastEdited <= criteria.LastEdited.Max.Value); }
            }
            if (criteria.IsPublic != null)
            { searchResult = searchResult.Where(r => r.IsPublic == criteria.IsPublic); }
            if (criteria.Rating.Min.HasValue || criteria.Rating.Max.HasValue)
            {
                if (criteria.Rating.Min.HasValue)
                { searchResult = searchResult.Where(r => r.Rating >= criteria.Rating.Min.Value); }

                if (criteria.Rating.Max.HasValue)
                { searchResult = searchResult.Where(r => r.Rating <= criteria.Rating.Max.Value); }
            }
            if (criteria.ProductsAliases != null && criteria.ProductsAliases.Count > 0)
            {
                foreach (string term in criteria.ProductsAliases)
                {
                    string cleanTerm = term.Trim().ToLower();
                    if (string.IsNullOrWhiteSpace(cleanTerm)) continue;

                    List<Guid> matchedProductIds = products
                        .Where(p => p.Name.ToLower().Contains(cleanTerm))
                        .Select(p => p.ID)
                        .ToList();

                    List<Guid> matchedByAliasProductIds = productAliases
                        .Where(a => a.AliasName.ToLower().Contains(cleanTerm))
                        .Select(a => a.ReferencedProduct)
                        .ToList();

                    HashSet<Guid> validProductIdsForTerm = new HashSet<Guid>(matchedProductIds.Concat(matchedByAliasProductIds));

                    searchResult = searchResult.Where(r => r.Ingredients.Any(ing =>
                        (ing.ReferencedProduct.HasValue && validProductIdsForTerm.Contains(ing.ReferencedProduct.Value)) ||
                        (!string.IsNullOrEmpty(ing.RawName) && ing.RawName.ToLower().Contains(cleanTerm))
                    ));
                }
            }

            List<Recipe> finalFilteredList = new List<Recipe>();

            foreach (Recipe r in searchResult.ToList())
            {
                double totalCal = 0, totalProt = 0, totalFat = 0, totalCarb = 0;

                if (r.Ingredients != null)
                {
                    foreach (Ingredient ing in r.Ingredients)
                    {
                        if (!ing.ReferencedProduct.HasValue) continue;
                        Product? prod = products.FirstOrDefault(p => p.ID == ing.ReferencedProduct.Value);
                        if (prod != null)
                        {
                            double factor = ing.Weight / 100.0;
                            totalProt += prod.Proteins * factor;
                            totalFat += prod.Fats * factor;
                            totalCarb += prod.Carbohydrates * factor;

                            var calRes = CaloriesCalculator.calculateIngredientCalories(prod, ing.Weight);
                            if (calRes.status == Response<double>.responseStatus.success) 
                                totalCal += calRes.data;
                        }
                    }
                }

                double servings = r.Servings > 0 ? r.Servings : 1;

                if (criteria.DishCalories.Min.HasValue && totalCal < criteria.DishCalories.Min.Value) continue;
                if (criteria.DishCalories.Max.HasValue && totalCal > criteria.DishCalories.Max.Value) continue;
                if (criteria.DishProteins.Min.HasValue && totalProt < criteria.DishProteins.Min.Value) continue;
                if (criteria.DishProteins.Max.HasValue && totalProt > criteria.DishProteins.Max.Value) continue;
                if (criteria.DishFats.Min.HasValue && totalFat < criteria.DishFats.Min.Value) continue;
                if (criteria.DishFats.Max.HasValue && totalFat > criteria.DishFats.Max.Value) continue;
                if (criteria.DishCarbohydrates.Min.HasValue && totalCarb < criteria.DishCarbohydrates.Min.Value) continue;
                if (criteria.DishCarbohydrates.Max.HasValue && totalCarb > criteria.DishCarbohydrates.Max.Value) continue;

                if (criteria.ServingCalories.Min.HasValue && (totalCal / servings) < criteria.ServingCalories.Min.Value) continue;
                if (criteria.ServingCalories.Max.HasValue && (totalCal / servings) > criteria.ServingCalories.Max.Value) continue;
                if (criteria.ServingProteins.Min.HasValue && (totalProt / servings) < criteria.ServingProteins.Min.Value) continue;
                if (criteria.ServingProteins.Max.HasValue && (totalProt / servings) > criteria.ServingProteins.Max.Value) continue;
                if (criteria.ServingFats.Min.HasValue && (totalFat / servings) < criteria.ServingFats.Min.Value) continue;
                if (criteria.ServingFats.Max.HasValue && (totalFat / servings) > criteria.ServingFats.Max.Value) continue;
                if (criteria.ServingCarbohydrates.Min.HasValue && (totalCarb / servings) < criteria.ServingCarbohydrates.Min.Value) continue;
                if (criteria.ServingCarbohydrates.Max.HasValue && (totalCarb / servings) > criteria.ServingCarbohydrates.Max.Value) continue;

                finalFilteredList.Add(r);
            }

            return Response<List<Recipe>>.createResponse(finalFilteredList.ToList(), "знайдені рецепти", 
                Response<List<Recipe>>.responseStatus.success);
        }

        public Response<List<Recipe>> FilterGlobalRecipes(SearchRecipeCriteria_dto criteria, List<Recipe> recipes, 
            List<Product> products, List<ProductAlias> productAliases, List<UserProfile> users)
        {
            List<Recipe> recipesFilteredByAuthor = recipes;

            if (!string.IsNullOrWhiteSpace(criteria.AuthorName))
            {
                List<Guid> allowedAuthorIds = users
                    .Where(u => u.Name.ToLower()
                    .Contains(criteria.AuthorName.ToLower()))
                    .Select(u => u.ID)
                    .ToList();

                recipesFilteredByAuthor = recipes.Where(r => allowedAuthorIds.Contains(r.AuthorID)).ToList();
            
                if (recipesFilteredByAuthor.Count == 0)
                {
                    return Response<List<Recipe>>.createResponse(new List<Recipe>(), "Рецептів вказаного автора не знайдено", 
                        Response<List<Recipe>>.responseStatus.success);
                }
            }
            return this.FilterRecipes(criteria, recipesFilteredByAuthor, products, productAliases);
        }

        public Response<List<CookBook>> FilterCookBooks(SearchCookBookCriteria_dto criteria, List<CookBook> cookBooks, List<Recipe> allRecipes)
        {
            IQueryable<CookBook> searchResult = cookBooks.AsQueryable();
            
            if (criteria.ID != null)
            { searchResult = searchResult.Where(cb => cb.ID == criteria.ID); }
            
            if (criteria.Name != null) 
            { 
                string searchName = criteria.Name.ToLower();
                searchResult = searchResult.Where(cb => (cb.Name ?? "").ToLower().Contains(searchName)); 
            }

            if (criteria.CreationDate != null)
            {
                if (criteria.CreationDate.Min.HasValue)
                { 
                    long min = criteria.CreationDate.Min.Value.Ticks;
                    searchResult = searchResult.Where(cb => cb.CreationDate.Ticks >= min); 
                }
                if (criteria.CreationDate.Max.HasValue)
                { 
                    long max = criteria.CreationDate.Max.Value.Ticks;
                    searchResult = searchResult.Where(cb => cb.CreationDate.Ticks <= max); 
                }
            }

            if (criteria.LastEdited != null)
            {
                if (criteria.LastEdited.Min.HasValue)
                { 
                    long min = criteria.LastEdited.Min.Value.Ticks;
                    searchResult = searchResult.Where(cb => cb.LastEdited.Ticks >= min); 
                }
                if (criteria.LastEdited.Max.HasValue)
                { 
                    long max = criteria.LastEdited.Max.Value.Ticks;
                    searchResult = searchResult.Where(cb => cb.LastEdited.Ticks <= max); 
                }
            }

            if (criteria.IsPublic != null)
            { searchResult = searchResult.Where(cb => cb.IsPublic == criteria.IsPublic); }

            if (!string.IsNullOrWhiteSpace(criteria.RecipeName))
            {
                string searchRecipe = criteria.RecipeName.ToLower();
                
                List<Guid> matchingRecipeIds = allRecipes
                    .Where(r => (r.Name ?? "").ToLower().Contains(searchRecipe))
                    .Select(r => r.ID)
                    .ToList();

                searchResult = searchResult.Where(cb => cb.ReferencedRecipes != null && 
                                                        cb.ReferencedRecipes.Any(id => matchingRecipeIds.Contains(id)));
            }

            return Response<List<CookBook>>.createResponse(searchResult.ToList(), "знайдені кулінарні книги",
                Response<List<CookBook>>.responseStatus.success);
        }

        public Response<List<CookBook>> FilterGlobalCookBooks(SearchCookBookCriteria_dto criteria, List<CookBook> cookBooks, List<UserProfile> users, List<Recipe> allRecipes)
        {
            List<CookBook> cookBooksFilteredByAuthor = cookBooks;
            
            if (!string.IsNullOrWhiteSpace(criteria.AuthorName))
            {
                string searchAuthor = criteria.AuthorName.ToLower();
                List<Guid> allowedAuthorIds = users
                    .Where(u => (u.Name ?? "").ToLower().Contains(searchAuthor))
                    .Select(u => u.ID)
                    .ToList();

                cookBooksFilteredByAuthor = cookBooks
                    .Where(cb => allowedAuthorIds.Contains(cb.AuthorID))
                    .ToList();

                if (cookBooksFilteredByAuthor.Count == 0)
                {
                    return Response<List<CookBook>>.createResponse(new List<CookBook>(), "Кулінарних книг вказаного автора не знайдено", 
                        Response<List<CookBook>>.responseStatus.success);
                }
            }
            
            return this.FilterCookBooks(criteria, cookBooksFilteredByAuthor, allRecipes);
        }

        public Response<List<Product>> FilterProducts(SearchProductCriteria_dto criteria, List<Product> products)
        {
            IQueryable<Product> searchResult = products.AsQueryable();
            if (criteria.ID != null)
            { searchResult = searchResult.Where(p => p.ID == criteria.ID); }
            if (criteria.Name != null)
            { searchResult = searchResult.Where(p => p.Name.ToLower().Contains(criteria.Name.ToLower())); }
            if (criteria.Calories.Min.HasValue || criteria.Calories.Max.HasValue)
            {
                if (criteria.Calories.Min.HasValue)
                { searchResult = searchResult.Where(p => p.Calories >= criteria.Calories.Min.Value); }
                if (criteria.Calories.Max.HasValue)
                { searchResult = searchResult.Where(p => p.Calories <= criteria.Calories.Max.Value); }
            }
            if (criteria.Proteins.Min.HasValue || criteria.Proteins.Max.HasValue)
            {
                if (criteria.Proteins.Min.HasValue)
                { searchResult = searchResult.Where(p => p.Proteins >= criteria.Proteins.Min.Value); }
                if (criteria.Proteins.Max.HasValue)
                { searchResult = searchResult.Where(p => p.Proteins <= criteria.Proteins.Max.Value); }
            }
            if (criteria.Fats.Min.HasValue || criteria.Fats.Max.HasValue)
            {
                if (criteria.Fats.Min.HasValue)
                { searchResult = searchResult.Where(p => p.Fats >= criteria.Fats.Min.Value); }
                if (criteria.Fats.Max.HasValue)
                { searchResult = searchResult.Where(p => p.Fats <= criteria.Fats.Max.Value); }
            }
            if (criteria.Carbohydrates.Min.HasValue || criteria.Carbohydrates.Max.HasValue)
            {
                if (criteria.Carbohydrates.Min.HasValue)
                { searchResult = searchResult.Where(p => p.Carbohydrates >= criteria.Carbohydrates.Min.Value); }
                if (criteria.Carbohydrates.Max.HasValue)
                { searchResult = searchResult.Where(p => p.Carbohydrates <= criteria.Carbohydrates.Max.Value); }
            }

            return Response<List<Product>>.createResponse(searchResult.ToList(), "знайдені продукти",
                Response<List<Product>>.responseStatus.success);
        }

        public Response<List<ProductAlias>> FilterProductAliases(SearchProductAliasCriteria_dto criteria, List<ProductAlias> productAliases)
        {
            IQueryable<ProductAlias> searchResult = productAliases.AsQueryable();
            if (criteria.ID != null)
            { searchResult = searchResult.Where(pa => pa.ID == criteria.ID); }
            
            if (criteria.AliasName != null)
            { searchResult = searchResult.Where(pa => pa.AliasName.ToLower().Contains(criteria.AliasName.ToLower())); }

            if (criteria.ReferencedProduct != null)
            { searchResult = searchResult.Where(pa => pa.ReferencedProduct == criteria.ReferencedProduct); }

            return Response<List<ProductAlias>>.createResponse(searchResult.ToList(), "знайдені продукти",
                Response<List<ProductAlias>>.responseStatus.success);
        }

        public Response<List<Category>> FilterCategories(SearchCategoryCriteria_dto criteria, List<Category> categories)
        {
            IQueryable<Category> searchResult = categories.AsQueryable();
            if (criteria.ID != null) 
            { searchResult = searchResult.Where(c => c.ID == criteria.ID); }
            if (criteria.Name != null) 
            { searchResult = searchResult.Where(c => c.Name.ToLower().Contains(criteria.Name.ToLower())); }
            if (criteria.IsCustom != null) 
            { searchResult = searchResult.Where(c => c.IsCustom == criteria.IsCustom); }

            return Response<List<Category>>.createResponse(searchResult.ToList(), "знайдені категорії",
                Response<List<Category>>.responseStatus.success);
        }
    }
}

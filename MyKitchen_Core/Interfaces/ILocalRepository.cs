using MyKitchen_Core.Entities;
using MyKitchen_Core.DTO.SearchDTOs;
using MyKitchen_Core.Tools;

namespace MyKitchen_Core.Interfaces
{
    public interface ILocalRepository
    {
        public enum dataType         
        {
            recipe,
            cookBook,
            product,
            productAlias,
            category
        }

        public Response<List<Recipe>> GetRecipes();
        public Response<List<CookBook>> GetCookBooks();
        public Response<List<Product>> GetProducts();
        public Response<List<ProductAlias>> GetProductAliases();
        public Response<List<Category>> GetCategories();

        public Response<Recipe> GetRecipeByID(Guid ID);
        public Response<CookBook> GetCookBookByID(Guid ID);
        public Response<Product> GetProductByID(Guid ID);
        public Response<ProductAlias> GetProductAliasByID(Guid ID);
        public Response<Category> GetCategoryByID(Guid ID);

        public Response<List<Recipe>> SearchRecipes(SearchRecipeCriteria_dto criteria);
        public Response<List<CookBook>> SearchCookBooks(SearchCookBookCriteria_dto criteria);
        public Response<List<Product>> SearchProducts(SearchProductCriteria_dto criteria);
        public Response<List<ProductAlias>> SearchProductAliases(SearchProductAliasCriteria_dto criteria);
        public Response<List<Category>> SearchCategories(SearchCategoryCriteria_dto criteria);

        public Response<Recipe> AddRecipe(Recipe recipe);
        public Response<CookBook> AddCookBook(CookBook cookBook);
        public Response<Product> AddProduct(Product product);
        public Response<ProductAlias> AddProductAlias(ProductAlias productAlias);
        public Response<Category> AddCategory(Category category);
        
        public Response<Recipe> UpdateRecipe(Recipe recipe);
        public Response<CookBook> UpdateCookBook(CookBook cookBook);
        public Response<Product> UpdateProduct(Product product);
        public Response<ProductAlias> UpdateProductAlias(ProductAlias productAlias);
        public Response<Category> UpdateCategory(Category category);
        
        public Response<bool> RemoveObject(dataType data, Guid ID);
    }
}

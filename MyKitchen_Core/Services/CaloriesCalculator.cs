using MyKitchen_Core.Entities;
using MyKitchen_Core.Tools;

namespace MyKitchen_Core.Services
{
    public static class CaloriesCalculator
    {
        public static Response<double> calculateCalories(double proteins, double carbohydrates, double fats)
        {
            double calories = (proteins * 4) + (carbohydrates * 4) + (fats * 9);
            return Response<double>.createResponse(calories, "Калорії продукту успішно розраховані.",
                Response<double>.responseStatus.success);
        }

        public static Response<double> calculateIngredientCalories(Product product, double weignt)
        {
            if (product == null)
            {
                return Response<double>.createResponse(0, "Продукт не знайдено.",
                    Response<double>.responseStatus.warning);
            }
            else
            {
                Response<double> Res = calculateCalories(product.Proteins, product.Carbohydrates, product.Fats);
                double productCalories = Res.data;
                double ingredientCalories =  productCalories * (weignt / 100);
                return Response<double>.createResponse(ingredientCalories, "Калорії інгредієнта успішно розраховані.",
                    Response<double>.responseStatus.success);
            }
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MyKitchen_Core.Entities;
using MyKitchen_Core.Tools; // Для використання Response<T>

namespace MyKitchen_Core.Services
{
    public static class ReportBuilder
    {
        // Формує та зберігає звіт виключно по локальному сховищу.
        public static Response<bool> GenerateLocalReport(List<Recipe> localRecipes, List<CookBook> localBooks, string filePath)
        {
            try
            {
                string reportText = BuildLocalReportString(localRecipes, localBooks);
                File.WriteAllText(filePath, reportText, Encoding.UTF8);
                return Response<bool>.createResponse(true, "Локальний звіт успішно збережено", Response<bool>.responseStatus.success);
            }
            catch (Exception ex)
            {
                return Response<bool>.createResponse(false, $"Помилка збереження локального звіту: {ex.Message}", Response<bool>.responseStatus.error);
            }
        }

        
        // Формує та зберігає звіт виключно по глобальному сховищу (публічному).
        public static Response<bool> GenerateGlobalReport(List<Recipe> globalRecipes, List<CookBook> globalBooks, List<UserProfile> users, string filePath)
        {
            try
            {
                string reportText = BuildGlobalReportString(globalRecipes, globalBooks, users);
                File.WriteAllText(filePath, reportText, Encoding.UTF8);
                return Response<bool>.createResponse(true, "Глобальний звіт успішно збережено", Response<bool>.responseStatus.success);
            }
            catch (Exception ex)
            {
                return Response<bool>.createResponse(false, $"Помилка збереження глобального звіту: {ex.Message}", Response<bool>.responseStatus.error);
            }
        }

        // Формує та зберігає об'єднаний (повний) звіт по обох сховищах.
        public static Response<bool> GenerateCombinedReport(List<Recipe> localRecipes, List<CookBook> localBooks, 
                                                            List<Recipe> globalRecipes, List<CookBook> globalBooks, 
                                                            List<UserProfile> users, string filePath)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                
                sb.AppendLine("==================================================");
                sb.AppendLine("               ПОВНИЙ ЗВІТ СИСТЕМИ");
                sb.AppendLine("==================================================");
                sb.AppendLine($"Час генерації: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                sb.AppendLine();

                // Додаємо локальну частину
                sb.AppendLine(BuildLocalReportString(localRecipes, localBooks));
                sb.AppendLine();
                
                // Додаємо глобальну частину
                sb.AppendLine(BuildGlobalReportString(globalRecipes, globalBooks, users));

                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                return Response<bool>.createResponse(true, "Об'єднаний звіт успішно збережено", Response<bool>.responseStatus.success);
            }
            catch (Exception ex)
            {
                return Response<bool>.createResponse(false, $"Помилка збереження об'єднаного звіту: {ex.Message}", Response<bool>.responseStatus.error);
            }
        }

        private static string BuildLocalReportString(List<Recipe> recipes, List<CookBook> books)
        {
            StringBuilder sb = new StringBuilder();
            
            //Отримуємо ID поточного юзера із сесії програми
            Guid? currentUserId = AppSession.Instance.CurrentUser?.ID;

            // Фільтруємо списки, щоб рахувати тільки те, що належить поточному власнику акаунту
            var userRecipes = recipes.Where(r => r.AuthorID == currentUserId).ToList();
            var userBooks = books.Where(b => b.AuthorID == currentUserId).ToList();

            int totalRecipes = userRecipes.Count;
            int ownRecipes = userRecipes.Count(r => r.InspiredBy == Guid.Empty);
            int downloadedRecipes = userRecipes.Count(r => r.InspiredBy != Guid.Empty);
            int publicRecipes = userRecipes.Count(r => r.IsPublic);

            int totalBooks = userBooks.Count;
            int publicBooks = userBooks.Count(b => b.IsPublic);

            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine("               ЛОКАЛЬНЕ СХОВИЩЕ КОРИСТУВАЧА");
            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine("[РЕЦЕПТИ]");
            sb.AppendLine($"- Загальна кількість у вашій базі: {totalRecipes}");
            sb.AppendLine($"- Створено вами (власні): {ownRecipes}");
            sb.AppendLine($"- Запозичено з мережі (завантажені): {downloadedRecipes}");
            sb.AppendLine($"- Опубліковано вами в загальну мережу: {publicRecipes}");
            sb.AppendLine();
            sb.AppendLine("[КУЛІНАРНІ КНИГИ]");
            sb.AppendLine($"- Загальна кількість ваших книг: {totalBooks}");
            sb.AppendLine($"- Опубліковано в загальну мережу: {publicBooks}");
            sb.AppendLine("--------------------------------------------------");

            return sb.ToString();
        }

        private static string BuildGlobalReportString(List<Recipe> recipes, List<CookBook> books, List<UserProfile> users)
        {
            StringBuilder sb = new StringBuilder();
            
            // Якщо список юзерів порожній або null (бо кеш вимкнено ради безпеки),
            // ставимо 1 (поточний користувач), щоб уникнути ділення на нуль (DivideByZeroException)
            int totalUsers = (users != null && users.Count > 0) ? users.Count : 1;
            int totalRecipes = recipes.Count;
            int totalBooks = books.Count;

            double avgRecipesPerUser = Math.Round((double)totalRecipes / totalUsers, 1);

            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine("               ГЛОБАЛЬНА СТАТИСТИКА");
            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine("[КОРИСТУВАЧІ]");
            sb.AppendLine($"- Активних авторів у системі: {totalUsers}");
            sb.AppendLine();
            sb.AppendLine("[ЗАГАЛЬНИЙ КОНТЕНТ СИСТЕМИ]");
            sb.AppendLine($"- Всього публічних рецептів у хмарі: {totalRecipes}");
            sb.AppendLine($"- Всього публічних збірок у хмарі: {totalBooks}");
            sb.AppendLine($"- Середня активність (рецептів на автора): {avgRecipesPerUser}");
            sb.AppendLine("--------------------------------------------------");

            return sb.ToString();
        }
    }
}
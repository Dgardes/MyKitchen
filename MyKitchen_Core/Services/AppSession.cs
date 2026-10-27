using MyKitchen_Core.DataManagers;
using MyKitchen_Core.Entities;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MyKitchen_Core.Interfaces;
using MyKitchen_Core.Tools;

namespace MyKitchen_Core.Services
{
    public class AppSession
    {
        private static AppSession _instance;
        private UserProfile _currentUser;
        private AppSession() { }
        public static AppSession Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new AppSession();
                }
                return _instance;
            }
        }

        public UserProfile CurrentUser 
        { 
            get { return _currentUser; } 
            private set { _currentUser = value; } 
        }

        private ILocalRepository localRepository;
        private IGlobalRepository globalRepository;

        public void Initialize(ILocalRepository localRepo, IGlobalRepository globalRepo)
        {
            this.localRepository = localRepo;
            this.globalRepository = globalRepo;

            DataService.Instance.InitializeRepositories(this.localRepository, this.globalRepository);
        }

        public Response<bool> Register(string name, string email, string password)
        {
            Response<UserProfile> userCheck = globalRepository.GetUserByEmail(email);
            if (userCheck.status == Response<UserProfile>.responseStatus.error)
            {
                return Response<bool>.createResponse(false, "Помилка перевірки бази: " + userCheck.message, 
                    Response<bool>.responseStatus.error);
            }
            
            if (userCheck.data != null)
            {
                return Response<bool>.createResponse(false, "Користувач з такою поштою вже існує", 
                    Response<bool>.responseStatus.error);
            }

            UserProfile newUser = new UserProfile
            {
                ID = Guid.NewGuid(),
                Name = name,
                Email = email,
                PasswordHash = HashPassword(password),
                RegisteredAt = DateTime.Now
            };

            Response<bool> addResult = globalRepository.AddUser(newUser);
            if (addResult.status == Response<bool>.responseStatus.error)
            {
                return addResult;
            }

            return Response<bool>.createResponse(true, "Реєстрація успішна", 
                Response<bool>.responseStatus.success);
        }

        public Response<bool> Login(string email, string password)
        {
            Response<UserProfile> userCheck = globalRepository.GetUserByEmail(email);
            if (userCheck.status == Response<UserProfile>.responseStatus.error)
            {
                return Response<bool>.createResponse(false, "Помилка авторизації: " + userCheck.message, 
                    Response<bool>.responseStatus.error);
            }

            string hash = HashPassword(password);
            if (userCheck.data == null || userCheck.data.PasswordHash != hash)
            {
                return Response<bool>.createResponse(false, "Невірна пошта або пароль", 
                    Response<bool>.responseStatus.error);
            }

            CurrentUser = userCheck.data; 
            return Response<bool>.createResponse(true, "Вхід виконано успішно", 
                Response<bool>.responseStatus.success);
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
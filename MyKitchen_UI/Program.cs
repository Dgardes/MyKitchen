using MyKitchen_UI;
using MyKitchen_Core.DataManagers;
using MyKitchen_Core.Interfaces;
using MyKitchen_Core.Services;

namespace MyKitchen_2
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            string globalFS_path = Path.Combine(Application.StartupPath, "Global", "Data");
            string localFS_path = Path.Combine(Application.StartupPath, "Local", "Data");

            IGlobalRepository globalRepo = new GlobalFS_Manager(globalFS_path);
            ILocalRepository localRepo = new LocalFS_Manager(localFS_path);

            AppSession.Instance.Initialize(localRepo, globalRepo);
            Application.Run(new MainForm());
        }
    }
}
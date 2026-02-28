using System;
using System.IO;
using System.Windows.Forms;

namespace DepoHelper
{
    public static class DatabaseHelper
    {
        // Aceasta este singura variabilă care va conține string-ul de conexiune în tot proiectul
        private static string _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    SeteazaBazaDeDateFixa();
                }
                return _connectionString;
            }
        }

        private static void SeteazaBazaDeDateFixa()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folderFix = Path.Combine(appDataPath, "DepoHelper");

            if (!Directory.Exists(folderFix)) Directory.CreateDirectory(folderFix);

            string caleMdfFixa = Path.Combine(folderFix, "DepouFeroviar.mdf");

            // Dacă nu e în folderul fix, o copiem de unde a fost instalată (StartupPath)
            if (!File.Exists(caleMdfFixa))
            {
                string caleMdfOriginala = Path.Combine(Application.StartupPath, "DepouFeroviar.mdf");
                if (File.Exists(caleMdfOriginala)) File.Copy(caleMdfOriginala, caleMdfFixa);
            }

            _connectionString = $@"Server=(LocalDB)\MSSQLLocalDB;AttachDbFilename={caleMdfFixa};Integrated Security=True;";
        }
    }
}
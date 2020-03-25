using System;

namespace CSV.Models
{
    public class Constants
    {

        public readonly Student Student = new Student { StudentId = "200425898", FirstName = "Ankita", LastName = "Singh" };

        public class Locations
        {
            public readonly static string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            public readonly static string ExePath = Environment.CurrentDirectory;

            public readonly static string ContentFolder = $"{ExePath}//..//..//..//Content";
            public readonly static string DataFolder = $"{ContentFolder}//Data";
            public readonly static string ImagesFolder = $"{ContentFolder}//Images";

            public readonly static string CSVPath = $"{DataFolder}//students.csv";
            public readonly static string JSONPath = $"{DataFolder}//students.json";
            public readonly static string XMLPath = $"{DataFolder}//students.xml";

            public const string InfoFile = "info.csv";
            public const string ImageFile = "myimage.jpg";
        }

        public class FTP
        {
            public const string Username = @"bdat100119f\bdat1001";
            public const string Password = "bdat1001";

            public const string uploadcsvDest = BaseUrl + "/200425898 Ankita Singh/students.csv";
            public const string uploadjsonDest = BaseUrl + "/200425898 Ankita Singh/students.json";
            public const string uploadxmlDest = BaseUrl + "/200425898 Ankita Singh/students.xml";

            public const string BaseUrl = "ftp://waws-prod-dm1-127.ftp.azurewebsites.windows.net/bdat1001-20914";

            public const int OperationPauseTime = 10000;
        }
    }
}


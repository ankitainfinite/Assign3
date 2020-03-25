using CSV.Models;
using CSV.Models.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CSV
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> directories = FTP.GetDirectory(Constants.FTP.BaseUrl);
            List<Student> students = new List<Student>();

            foreach (var directory in directories)
            {

                Student student = new Student() { AbsoluteUrl = Constants.FTP.BaseUrl };
                student.FromDirectory(directory);
                string infoFilePath = student.FullPathUrl + "/" + Constants.Locations.InfoFile;

                bool fileExists = FTP.FileExists(infoFilePath);
                if (fileExists == true)
                {
                    Console.WriteLine("Found info file:");
                    //string[] csvData = await FTP.getCSVData(infoFilePath);
                    //student.DateOfBirth = csvData[1].Split(',')[3];
                    //student.ImageData = csvData[1].Split(',')[4];
                    byte[] infobytes = FTP.DownloadFileBytes(infoFilePath);
                    string csvString = Encoding.Default.GetString(infobytes);
                    string[] csvData = csvString.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    if (csvData.Length != 2)
                    {
                        Console.WriteLine("Info.csv has incorrect information");
                    }
                    else
                    {
                        student.FromCSV(csvData[1]);
                    }
                }
                else
                {
                    Console.WriteLine("Could not find info file:");
                }
                students.Add(student);
            }

            List<int> ages = new List<int>();
            Console.WriteLine("4b and 4c");
            foreach(Student student in students)
            {
                Console.WriteLine("To String:: " + student.ToString());
                Console.WriteLine("To CSV:: " + student.ToCSV());
                Console.WriteLine("====================");
                ages.Add(student.Age);
            }

            
            Console.WriteLine("Student Count:: " + StudentCount(students));

            //Starts With
            Console.WriteLine("Starts With string");
            string name = Console.ReadLine();
            Console.WriteLine("No of Students:: " + FindStudentCount1(name, students));

            //Contains
            Console.WriteLine("Contains With String");
            string containsText = Console.ReadLine();
            Console.WriteLine("No of Students:: " + FindStudentCount2(containsText, students));

            //Finding my record
            Console.WriteLine("Getting My Details");
            Student s = GetMyDetails(students);
            Console.WriteLine(s.ToString());

            Console.WriteLine("Average Age of Students");
            Console.WriteLine(AverageAge(students));

            Console.WriteLine("Maximum Age");
            int maxAge = ages.Max();
            Console.WriteLine(maxAge);

            Console.WriteLine("Minimum Age");
            int minAge = ages.Min();
            Console.WriteLine(minAge);

            //Making csv file
            string CSVPath = "/Users/ankitasingh/Downloads/Assignments/CSV/CSV/Content/Data/students.csv";
            using (StreamWriter fs1 = new StreamWriter(CSVPath))
            {
                fs1.WriteLine("StudentId,FirstName,LastName,Age,DateOfBirth,MyRecord,Image");
                foreach(Student st1 in students)
                {
                    fs1.WriteLine(st1.ToCSV());
                }
            }

            //Making json file
            string JSONPath = "/Users/ankitasingh/Downloads/Assignments/CSV/CSV/Content/Data/students.json";
            using (StreamWriter fs2 = new StreamWriter(JSONPath))
            {
                foreach (Student st2 in students)
                {
                    var student = Newtonsoft.Json.JsonConvert.SerializeObject(st2);
                    fs2.WriteLine(student.ToString());
                }
            }

            //making xml file
            string XMLPath = "/Users/ankitasingh/Downloads/Assignments/CSV/CSV/Content/Data/students.xml";
            using (StreamWriter fs3 = new StreamWriter(XMLPath))
            {
                  List<SerializableStudent> serializableStudents = new List<SerializableStudent>();
                  foreach(Student ss in students)
                {
                    SerializableStudent serialStudent = new SerializableStudent();
                    serialStudent.SerializeStudent(ss);
                    serializableStudents.Add(serialStudent);
                }
                   
                  XmlSerializer x = new XmlSerializer(serializableStudents.GetType());
                  x.Serialize(fs3, serializableStudents);
            }

            ///uploads files to ftp site
            FTP.UploadFile(CSVPath, Constants.FTP.uploadcsvDest);
            FTP.UploadFile(JSONPath, Constants.FTP.uploadjsonDest);
            FTP.UploadFile(XMLPath, Constants.FTP.uploadxmlDest);

            return;

            //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            //string exePath = Environment.CurrentDirectory;
            //string dataFolder = $"{exePath}\\..\\..\\..\\Content\\Data";
            //string imagesFolder = $"{exePath}\\..\\..\\..\\Content\\Images";

            //string filePath = $@"{Constants.Locations.DataFolder}\{Constants.Locations.InfoFile}";
            //string fileContents;

            //using (StreamReader stream = new StreamReader(filePath))
            //{
            //    fileContents = stream.ReadToEnd();
            //}

            //List<string> entries = new List<string>();

            //entries = fileContents.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();

            //Student student = new Student();
            //student.FromCSV(entries[1]);



            //string[] data = entries[1].Split(",", StringSplitOptions.None);

            //Student student = new Student();
            //student.StudentId = data[0];
            //student.FirstName = data[1];
            //student.LastName = data[2];
            //student.DateOfBirth = data[3];
            //student.ImageData = data[4];

            //Console.WriteLine(student.ToCSV());
            //Console.WriteLine(student.ToString());



            //string imagefilePath = $"{Constants.Locations.ImagesFolder}\\{Constants.Locations.ImageFile}";
            //Image image = Image.FromFile(imagefilePath);
            //string base64Image = Imaging.ImageToBase64(image, ImageFormat.Jpeg);
            //student.ImageData = base64Image;

            //string newfilePath = $"{Constants.Locations.DesktopPath}\\{student.ToString()}.jpg";
            //FileInfo newfileinfo = new FileInfo(newfilePath);
            //Image studentImage = Imaging.Base64ToImage(student.ImageData);
            //studentImage.Save(newfileinfo.FullName, ImageFormat.Jpeg);
        }

        //Question 5 Methods
        public static int StudentCount(List<Student> students)
        {
            return students.Count;
        }

        //Starts With Method
        public static int FindStudentCount1(string name, List<Student> students)
        {
            List<Student> filteredStudents = new List<Student>();
            foreach (Student student in students)
            {
                //Comparing alphabet with firstname and lastname of student.
                if (student.FirstName.StartsWith(name) || student.LastName.StartsWith(name))
                {
                    filteredStudents.Add(student);
                    Console.WriteLine(student.ToString());
                }
            }
            return filteredStudents.Count;
        }

        //Contains Method
        public static int FindStudentCount2(string name, List<Student> students)
        {
            List<Student> filteredStudents = new List<Student>();
            foreach (Student student in students)
            {
                //Comparing if firstName or secondName contains the string.
                if (student.FirstName.Contains(name) || student.LastName.Contains(name))
                {
                    filteredStudents.Add(student);
                    Console.WriteLine(student.ToString());
                }
            }
            return filteredStudents.Count;
        }

        //getting my details
        public static Student GetMyDetails(List<Student> students)
        {
            Student student = new Student();
            foreach(Student s in students)
            {
                if(s.StudentId == "200425898")
                {
                    s.MyRecord = true;
                    student = s;
                    break;
                }
            }
            return student;
        }


        public static int AverageAge(List<Student> students)
        {
            int total = 0;
            foreach (Student s in students)
            {
                total += s.Age;
            }
            return total / students.Count;

        }

    }
}


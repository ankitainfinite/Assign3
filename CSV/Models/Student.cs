using System;
using System.Collections.Generic;
using System.Text;

namespace CSV.Models
{
    public class Student
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        private string _DateOfBirth;
        public string DateOfBirth
        {
            get { return _DateOfBirth; }
            set
            {
                _DateOfBirth = value;

                //Convert DateOfBirth to DateTime
                DateTime dtOut;
                DateTime.TryParse(_DateOfBirth, out dtOut);
                DateOfBirthDT = dtOut;
            }
        }

        public DateTime DateOfBirthDT { get; internal set; }
        public string ImageData { get; set; }
        public Boolean MyRecord { get; set; }
        //readonly int Age;


        public string AbsoluteUrl { get; set; }
        public string Directory { get; set; }
        public string FullPathUrl
        {
            get
            {
                return AbsoluteUrl + "/" + Directory;
            }
        }


        public void FromCSV(string csvdata)
        {
            string[] data = csvdata.Split(new string[] { "," }, StringSplitOptions.None);

            StudentId = data[0];
            FirstName = data[1];
            LastName = data[2];
            DateOfBirth = data[3];
            ImageData = data[4];
            
        }

        public void FromDirectory(string directory)
        {
            Directory = directory;

            if (String.IsNullOrEmpty(directory.Trim()))
            {
                return;
            }

            string[] data = directory.Trim().Split(new string[] { " " }, StringSplitOptions.None);

            StudentId = data[0];
            FirstName = data[1];
            LastName = data[2];
        }

        public string ToCSV()
        {
            string result = $"{StudentId},{FirstName},{LastName},{DateOfBirthDT.ToShortDateString()},{ImageData}";
return result;
        }

        public override string ToString()
        {
            string result = $"{StudentId} {FirstName} {LastName} {DateOfBirth} {Age}";
            return result;
        }

        //public int calculateAge()
        //{
        //    int age = 0;
        //    DateTime dob = DateTime.Parse(DateOfBirth);
        //    age = DateTime.Now.Year - dob.Year;
        //    if (DateTime.Now.DayOfYear < dob.DayOfYear)
        //        age = age - 1;

        //    return age;

        //}
        public virtual int Age
        {
            get
            {
                DateTime Now = DateTime.Now;
                int Years = new DateTime(DateTime.Now.Subtract(DateOfBirthDT).Ticks).Year - 1;
                DateTime PastYearDate = DateOfBirthDT.AddYears(Years);
                int Months = 0;
                for (int i = 1; i <= 12; i++)
                {
                    if (PastYearDate.AddMonths(i) == Now)
                    {
                        Months = i;
                        break;
                    }
                    else if (PastYearDate.AddMonths(i) >= Now)
                    {
                        Months = i - 1;
                        break;
                    }
                }
                int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
                int Hours = Now.Subtract(PastYearDate).Hours;
                int Minutes = Now.Subtract(PastYearDate).Minutes;
                int Seconds = Now.Subtract(PastYearDate).Seconds;
                return Years;
            }
        }



    }

    public class SerializableStudent
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string MyRecord { get; set; }
        public string Image { get; set; }

        public void SerializeStudent(Student s)
        {
            StudentId = s.StudentId;
            FirstName = s.FirstName;
            LastName = s.LastName;
            DateOfBirth = s.DateOfBirthDT;
            Age = s.Age;
            MyRecord = ""+s.MyRecord;
            Image = s.ImageData;
        }

    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using StudentLibrary;
using System.Runtime.Serialization.Json;
namespace StudentGenerator
{
    public class Students
    {
        public List<Student> students { get; set; }
        public Students() { students = new List<Student>(); }
        public Students(List<Student> students)
        {
            this.students = new List<Student>(students);
        }

        public override string ToString()
        {
            return $"Students: {string.Join(Environment.NewLine, students)}";
        }
    }
    class Program
    {
        public static Random random = new Random();

        static void Main(string[] args)
        {
            try
            {
                List<Student> students = new List<Student>();
                for (int i = 0; i < 30; i++)
                {
                    double rnd = random.NextDouble() * (10 - 4) + 4;
                    Student student = new Student(BuildName(), rnd, GenerateFaculty());
                    students.Add(student);
                    Console.WriteLine(students[i].ToString());
                }
                Students studentsNew = new Students(students);
                Serialize<Students>(studentsNew);
                Students serioilizedStudents = Deserialize<Students>();
                //Console.WriteLine(Environment.NewLine + "After serialization:");
                //Console.WriteLine(serioilizedStudents);

                var selectStudentFromMiem = from student in students
                                            where (int)student.Faculty == 1
                                            select student;
                var sortByMarks = from student in students
                                  orderby student.Mark descending
                                  select student;
                List<Student> studentSortedByMarks = new List<Student>(sortByMarks);
                Console.WriteLine($"Количество студентов МИЭМА = {(new List<Student>(selectStudentFromMiem)).Count}");
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(studentSortedByMarks[i]);
                }
                Console.WriteLine();
                var orderByFaculty = students.GroupBy(student => (int)student.Faculty);
                List<Student> bigStudents = new List<Student>();
                foreach (IGrouping<int, Student> faculty in orderByFaculty)
                {
                    Student bigStudent = faculty.Aggregate((st1, st2) => st1 + st2);
                    bigStudents.Add(bigStudent);
                    Console.WriteLine(bigStudent);
                    Console.WriteLine();
                }
                //var sortBigStudents = from bigStudent in bigStudents
                //                      orderby bigStudent.Mark, bigStudent.Name descending
                //                      select bigStudent;
                var sortBigStudents = bigStudents.OrderByDescending(student => student.Mark).ThenByDescending(student => student.Name);
                List<Student> sortedBigStudents = new List<Student>(sortBigStudents);
                for (int i = 0; i < sortedBigStudents.Count; i++)
                {
                    Console.WriteLine(sortedBigStudents[i]);
                }
            }
            catch (Exception)
            {
            }
        }



        public static void Serialize<T>(T obj)
        {
            try
            {
                using (FileStream fs = new FileStream("../../../students.json", FileMode.Create))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                    serializer.WriteObject(fs, obj);
                }
            }
            catch (IOException)
            {

            }
        }
        public static T Deserialize<T>()
        {
            try
            {
                using (FileStream fs = new FileStream("../../../students.json", FileMode.Open))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                    return (T)serializer.ReadObject(fs);
                }
            }
            catch (IOException)
            {

            }
            return default(T);

        }
        public static string BuildName()
        {
            //диапазон проверить
            int length = random.Next(6, 11);
            StringBuilder name = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                if (random.NextDouble() < 0.5)
                    name.Append((char)random.Next('A', 'Z' + 1));
                else name.Append((char)random.Next('a', 'z' + 1));
            }
            return name.ToString();
        }
        public static Faculty GenerateFaculty()
        {
            int randomFaculty = random.Next(0, 3);
            Faculty fac1 = Faculty.CS;
            Faculty fac2 = Faculty.MIEM;
            Faculty fac3 = Faculty.Design;
            if (randomFaculty == (int)fac1) return fac1;
            else if (randomFaculty == (int)fac2) return fac2;
            else if (randomFaculty == (int)fac3) return fac3;
            // эта часть кода достигаться не будет, возвращаем любое значение
            return fac1;

        }
    }
}

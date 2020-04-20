using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using StudentLibrary;
using System.Runtime.Serialization.Json;
namespace StudentGenerator
{
    // класс, нужный для того, чтобы преобразовать список объектов типа Student 
    // в одну переменную типа класса (то есть типа Students)
    public class Students
    {
        public List<Student> students { get; set; }
        public Students() { students = new List<Student>(); }
        // конструктор, который присваивает свойству класса 
        // переданный в аргументе список студентов 
        public Students(List<Student> students)
        {
            this.students = new List<Student>(students);
        }
        /// <summary>
        /// Метод для вывода информации о классе (= о списке студентов)
        /// </summary>
        /// <returns>Строка с информацией</returns>
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
                // создаём список студентов - 30 объектов типа Student
                List<Student> students = new List<Student>();
                for (int i = 0; i < 30; i++)
                {
                    double rnd = random.NextDouble() * (10 - 4) + 4;
                    Student student = new Student(BuildName(), rnd, GenerateFaculty());
                    students.Add(student);
                    Console.WriteLine(students[i].ToString());
                }
                // преобразуем список студентов в объект типа Students
                Students studentsNew = new Students(students);
                // вызываем метод сериализации
                Serialize<Students>(studentsNew);
                // вызываем метод десериализации
                Students serioilizedStudents = Deserialize<Students>();
                //Console.WriteLine(Environment.NewLine + "After serialization:");
                //Console.WriteLine(serioilizedStudents);
                // создаем переменную, в которую с помощью LINQ кладем объекты из МИЭМа
                var selectStudentFromMiem = from student in students
                                            where (int)student.Faculty == 1
                                            select student;
                // выводим сообщение о количестве студентов из МИЭМа, преобразуя переменную в список List
                Console.WriteLine($"Количество студентов МИЭМа = {(new List<Student>(selectStudentFromMiem)).Count}");
                // создаем переменную, в которую с помощью LINQ кладем объекты по убыванию их оценок
                var sortByMarks = from student in students
                                  orderby student.Mark descending
                                  select student;
                List<Student> studentSortedByMarks = new List<Student>(sortByMarks);
                Console.WriteLine("Остортированный список студентов по убыванию оценок: ");
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(studentSortedByMarks[i]);
                }
                // с помощью GroupBy группируем элементы списка по факультетам
                var orderByFaculty = students.GroupBy(student => (int)student.Faculty);
                List<Student> bigStudents = new List<Student>();
                Console.WriteLine("Сложение студентов по факультетам: ");
                foreach (IGrouping<int, Student> faculty in orderByFaculty)
                {
                    // с помощью Aggregate складываем два элемента из одной группы
                    Student bigStudent = faculty.Aggregate((st1, st2) => st1 + st2);
                    bigStudents.Add(bigStudent);
                    Console.WriteLine(bigStudent);
                }
                //var sortBigStudents = from bigStudent in bigStudents
                //                      orderby bigStudent.Mark, bigStudent.Name descending
                //                      select bigStudent;
                // получившийся список из 3 объектов сортируем по убыванию оценок
                // в случае равенства оценок сортируем по имени
                var sortBigStudents = bigStudents.OrderByDescending(student => student.Mark).ThenByDescending(student => student.Name);
                List<Student> sortedBigStudents = new List<Student>(sortBigStudents);
                Console.WriteLine("Остортированный список трех студентов:");
                for (int i = 0; i < sortedBigStudents.Count; i++)
                {
                    Console.WriteLine(sortedBigStudents[i]);
                }
            }
            // ловим исключение
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Метод выполняет JSON-сериализацию списка в файл 
        /// </summary>
        /// <typeparam name="T">Тип, который нужно сериализовать</typeparam>
        /// <param name="obj">Имя типа, который нужно сериализовать</param>

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
            // ловим исключения
            catch (IOException)
            {

            }
        }
        /// <summary>
        /// Метод выполняет десериализацию 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Тип, который хотим получить после десериализации</returns>
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
            // ловим исключения
            catch (IOException)
            {

            }
            return default(T);

        }
        /// <summary>
        /// Метод формирует имя с помощью StringBuilder
        /// </summary>
        /// <returns>Строка = имени объекта </returns>
        public static string BuildName()
        {
            //диапазон проверить
            int length = random.Next(6, 11);
            StringBuilder name = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                // с помощью random определяем, будет ли следующий символ имени заглавным
                if (random.NextDouble() < 0.5)
                    name.Append((char)random.Next('A', 'Z' + 1));
                else name.Append((char)random.Next('a', 'z' + 1));
            }
            return name.ToString();
        }
        /// <summary>
        /// Метод генерирует свойство объекта - факультет, к которому он будет относиться 
        /// </summary>
        /// <returns>Переменная типа Faculty</returns>
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

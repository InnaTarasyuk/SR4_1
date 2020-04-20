using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace StudentLibrary
{
    [DataContract]
    public class Student
    {
        // readonly?
        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public double Mark { get; private set; }
        [DataMember]
        public Faculty Faculty { get; private set; }

        public Student()
        {
        }
        public Student(string name, double mark, Faculty faculty)
        {
            if (IsNameCorrect(name) == true)
                Name = name;
            else throw new ArgumentException("Имя неверное");
            if (mark >= 4 && mark < 10)
                Mark = mark;
            else throw new ArgumentOutOfRangeException("Значение оценки неверное");
            // проверить значение
            Faculty = faculty;
        }
        //}
        /// <summary>
        /// Проверяет корректность имение
        /// </summary>
        /// <param name="name">Имя для проверки</param>
        /// <returns>Правда, если корректно, иначе ложь</returns>
        static bool IsNameCorrect(string name)
        {
            if (name.Length >= 6 && name.Length <= 10)
            {
                foreach (char ch in name)
                {
                    if (!(ch >= 'A' && ch <= 'Z') && !(ch >= 'a' && ch <= 'z'))
                        return false;
                }
                return true;
            }
            else return false;
        }
        public override string ToString() => $"<{Faculty}> Student <{Name}>: Mark <{Mark:f3}>";
        public static Student operator +(Student firstStudent, Student secondStudent)
        {
            int max = Math.Max(firstStudent.Name.Length, secondStudent.Name.Length);
            int length = (firstStudent.Name.Length + secondStudent.Name.Length) / 2;
            string name = string.Empty;
            // реализация случая равенства длин 2-ух имен произвольная
            // я решила реализовать его так же, как и случай, в котором первое имя длиннее
            if (max == firstStudent.Name.Length || (firstStudent.Name.Length == secondStudent.Name.Length))
                name = firstStudent.Name.Substring(0, length / 2) + secondStudent.Name.Substring(0, length / 2);
            else if (max == secondStudent.Name.Length)
                name = secondStudent.Name.Substring(0, length / 2) + firstStudent.Name.Substring(0, length / 2);
            if (firstStudent.Faculty != secondStudent.Faculty) throw new ArgumentException("Факультеты не совпадают");
            double mark = (firstStudent.Mark + secondStudent.Mark) / 2.0;
            return new Student(name, mark, firstStudent.Faculty);
        }
    }
}

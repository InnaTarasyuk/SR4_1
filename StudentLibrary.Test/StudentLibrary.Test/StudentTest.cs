using System;
using Xunit;
using StudentLibrary;
using System.Runtime.Serialization.Json;
using System.IO;
namespace StudentLibrary.Test
{
    public class StudentTest
    {
        [Fact]
        // Метод проверки корректности работы метода ToString()
        public void TestToString()
        {
            Student student = new Student("Michael", 8, Faculty.CS);
            string expectedString = $"<CS> Student <Michael>: Mark <{8:f3}>";
            Assert.Equal(student.ToString(), expectedString);

        }
        [Fact]
        // Метод проверки корректности работы сериализации и десериализации 
        public void TestSerialize()
        {
            Student student = new Student("Michael", 8, Faculty.CS);
            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Student));
                serializer.WriteObject(ms, student);
                ms.Seek(0, SeekOrigin.Begin);
                Assert.Equal(student, serializer.ReadObject(ms));
            }
        }
        [Fact]
        // Метод проверки корректности работы перегрузки оператора сложения двух объектов
        public void TestPlusOperator()
        {

            Student student1 = new Student("AAAAAA", 6, Faculty.CS);
            Student student2 = new Student("BBBBBBBB", 8, Faculty.CS);
            Student actualStudent = student1 + student2;

            Student expectedStudent = new Student("BBBAAA", 7, Faculty.CS);
            Assert.Equal(expectedStudent, actualStudent);

        }
    }
}

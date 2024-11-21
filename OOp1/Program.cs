using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOp1
{

  

    
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Student> students = new List<Student>
            {
                new Student(1, "An", 16),
                new Student(2, "Binh", 18),
                new Student(3, "Chau", 14),
                new Student(4, "Dat", 17),
                new Student(5, "Anh", 15)
            };
            //1.
            Console.WriteLine("Danh sach sinh vien:");
            foreach (var student in students)
            {
                student.Display();
            }
            //2.
            var filteredStudents = students.Where(s => s.Age >= 15 && s.Age <= 18).ToList();
            Console.WriteLine("\nSinh viên co tuoi tu 15 den 18:");
            foreach (var student in filteredStudents)
            {
                student.Display();
            }
           //3.
            var studentsWithA = students.Where(s => s.Name.StartsWith("A")).ToList();
            Console.WriteLine("\nSinh vien co ten bat dau bang chu 'A':");
            foreach (var student in studentsWithA)
            {
                student.Display();
            }
            //4.
            int totalAge = students.Sum(s => s.Age);
            Console.WriteLine($"\nTong tuoi cua sinh vien: {totalAge}");

            //5.
            var youngestStudent = students.OrderBy(s => s.Age).First();
            Console.WriteLine("\nSinh vien tre nhat:");
            youngestStudent.Display();

            //6.
            var sortedStudents = students.OrderBy(s => s.Age).ToList();
            Console.WriteLine("\nDanh sach sinh vien theo tuoi tang dan:");
            foreach (var student in sortedStudents)
            {
                student.Display();
            }

            Console.ReadLine();
        }
    }
}

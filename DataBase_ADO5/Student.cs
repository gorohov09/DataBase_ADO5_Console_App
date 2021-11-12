using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase_ADO5
{
    public class Student
    {
        public int Id { get; private set; }

        public string FIO { get; private set; }

        public DateTime Birthday { get; private set; }

        public string University { get; private set; }

        public string GroupNumber { get; private set; }

        public int Course { get; private set; }

        public double AverageScore { get; private set; }

        public Student(int Id, string FIO, DateTime Birthday, string University, string GroupNumber, int Course, double AverageScore)
        {
            this.Id = Id;
            this.FIO = FIO;
            this.Birthday = Birthday;
            this.University = University;
            this.GroupNumber = GroupNumber;
            this.Course = Course;
            this.AverageScore = AverageScore;
        }

        public override string ToString()
        {
            return $"Студент №{Id}\n" +
                   $"ФИО: {FIO}\n" +
                   $"Дата рождения: {Birthday.ToLongDateString()}\n" +
                   $"Университет: {University}\n" +
                   $"Группа: {GroupNumber}\n" +
                   $"Средний балл: {AverageScore}\n";
        }

    }
}

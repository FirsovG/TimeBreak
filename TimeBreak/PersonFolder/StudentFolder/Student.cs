using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeBreak.PersonFolder.StudentFolder;

namespace TimeBreak
{
    // Nachfolger der Kalsse Person
    public class Student : Person
    {
        #region Field
        private StudentClass studentClass;
        private StudentAttendance studentAttendance;
        #endregion

        #region Constructor
        // Standartkonstruktor für den Schüler der auf den Konstruktor der Mutterklasse basiert
        public Student(string username, string surname, string firstname, StudentClass studentClass, DateTime creationTime)
        {
            this.Surname = surname;
            this.Firstname = firstname;
            this.Username = username;
            this.studentAttendance = new StudentAttendance(creationTime);
            this.Attendance = studentAttendance;
            this.studentClass = studentClass;
        }
        #endregion

        #region Property
        public StudentClass StudentClass { get => studentClass; set => studentClass = value; }
        public StudentAttendance StudentAttendance { get => studentAttendance; set => studentAttendance = value; }
        #endregion
    }
}

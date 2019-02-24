using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeBreak
{
    //Abstrakte Mutterklasse für den Lehrer und Schüler
    public abstract class Person
    {
        #region Fields
        // Geschützte Felder um es den Nachvolgern weiterzuteilen
        private string surname; //Nachname
        private string firstname; //Vorname
        private string username; // Matrikelnummer
        private Attendance attendance; // ein Platzhalter für die Anwesenheitsklasse
        #endregion

        #region Constructor 
        // Standartkonstruktor auf den die Nachfolger basieren werden
        public Person(string username, string surname, string firstname, DateTime creationTime)
        {
            this.surname = surname;
            this.firstname = firstname;
            this.username = username;
            this.attendance = new Attendance(creationTime);
        }

        public Person()
        {
            
        }
        #endregion

        #region Propertys
        // Offene eigenschaften um es den Nachvolgern weiterzuteilen
        public Attendance Attendance { get => attendance; set => attendance = value; }
        public string Surname { get => surname; set => surname = value; }
        public string Firstname { get => firstname; set => firstname = value; }
        public string Username { get => username; set => username = value; }
        #endregion

    }
}

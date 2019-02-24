
//Benutzung von using um die Klassenpfad kürzer zu machen
using TimeBreak.PersonFolder.TeacherFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeBreak
{
    // Nachfolgerklasse der Personklasse
    public class Teacher : Person
    {

        #region Fields
        #endregion

        #region Constructor 
        // Standartkonstruktor für den Lehrer der auf den Konstruktor der Mutterklasse basiert
        public Teacher(string matrikelNummer, string name, string vorname, DateTime creationTime) : base(matrikelNummer, name, vorname, creationTime)
        {
        }
        #endregion

        #region Functions
        // Funktion die Schüller sucht
        public void Kontrollieren(DBConnector databaseConnection)
        {
            // Dialogfenster der Namen und Vornamen abfragt
            StudentSearch studentSearch = new StudentSearch(databaseConnection);
            studentSearch.ShowDialog();

            // Temporäre variblen
            string surname = "";
            string firstname = "";

            // Wenn der Dialog erfolgreich abgelaufen ist werden die Variablen rausgezogen
            // und die genauere Daten werden als ein Tupel rausgezogen und in den Displaydialog angezeigt
            if (studentSearch.DialogResult == true)
            {
                surname = studentSearch.Surname.Text;
                firstname = studentSearch.Firstname.Text;

                Tuple<string, string, string, string> Data = databaseConnection.GetStudentInfo(surname, firstname);
                StudentInfo studentInfo = new StudentInfo(Data);
                studentInfo.ShowDialog();

            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeBreak.PersonFolder.StudentFolder
{
    // Arbeitfläche im Fall das der user eine Schüller ist
    public partial class StudentWorkspace : Page
    {
        #region Fields
        Student student;
        DBConnector databaseConnection;
        #endregion

        #region Construct
        public StudentWorkspace(Student student, DBConnector databaseConnection)
        {
            InitializeComponent();
            this.student = student;
            this.databaseConnection = databaseConnection;
        }
        #endregion

        #region UI_Eventhandlers
        private void ClickButton_Start(object sender, RoutedEventArgs e)
        {


            //Prüft wie viele pausen gemacht wurden, ob der jenige grade in der Pause ist
            if (student.StudentAttendance.BreakCount >= 4)
            {
                MessageBox.Show("Du hast zu viele Pausen gemacht");
            }
            else if (student.StudentAttendance.LastBreakTimestamp > 3601)
            {
                MessageBox.Show("Du hast zu lange Pause gemacht");
            }
            else
            {
                // Dialog der fragt wo der jenige Pause machen möchte
                BreakDialog breakDialog = new BreakDialog();
                breakDialog.ShowDialog();
                // Wenn der Dialog erfolgreich abgelaufen ist werden rausgezogene Daten in die Funtion übergeben
                if (breakDialog.DialogResult == true)
                {
                    student.StudentAttendance.MakeBreak(breakDialog.Place, databaseConnection, student.Username);
                    // Der Button tauscht den Fallhandler der beim druck von Button ausgeführt wird
                    ClickButton.Content = "Stop";
                    ClickButton.Click -= ClickButton_Start;
                    ClickButton.Click += ClickButton_Stop;
                }
            }
        }

        //Fall um den Pausenschluss zu bearbeiten
        private void ClickButton_Stop(object sender, RoutedEventArgs e)
        {
            student.StudentAttendance.GetBack();
            // Tauscht auf den ausgangsfallhandler
            ClickButton.Content = "Start";
            ClickButton.Click -= ClickButton_Stop;
            ClickButton.Click += ClickButton_Start;
            // Begrüsst den Schüller
            MessageBox.Show("Willkommen zurück");
        }
        #endregion
    }
}

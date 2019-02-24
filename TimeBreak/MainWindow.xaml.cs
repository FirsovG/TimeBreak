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
using MySql.Data.MySqlClient;

namespace TimeBreak
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        private DBConnector databaseConnection;
        private Person user;
        private StudentClass studentClass;
        private const string classRoom = "C207"; 
        #endregion

        #region Constuctor
        public MainWindow()
        {
            // Inzialisiert die UI komponente
            InitializeComponent();
            Taskbar.Content = new Design.Taskbar(this);

            //Erstellung von temporären Variablen um die dann aus der Datenbank rauszuziehen
            string username         = null;
            string userSurname      = null;
            string userFirstname    = null;
            bool userIsTecher       = false;
            string className        = null;
            DateTime timeOfCreation = DateTime.Now;

            //Erstellung einer verbindung zur eine Datenbank (extra variable um den namen der Datenbank schnell zu ändern)
            string serverAddress  = "127.0.0.1";
            string serverPort     = "3306"; 
            string serverUsername = "root"; //standardmäßig root
            string serverPassword = "";     //standardmäßig leer
            string database       = "dbtimebreak";

            string MySQLConnectionString = $"datasource={serverAddress};port={serverPort};username={serverUsername};password={serverPassword};database={database}";
            databaseConnection = new DBConnector(MySQLConnectionString, classRoom);

            //Aufrufen von den Login Fenster
            LoginWindow login = new LoginWindow(databaseConnection);
            login.ShowDialog();

            //Wenn die Handlung fertig ist, ziehen wir die Daten in temporäre Variable
            //sonst wenn die Handlung unterbrochen wurde schliesst sich das program
            if (login.DialogResult == true)
            {
                username = login.Data.Item1;
                userSurname = login.Data.Item3;
                userFirstname = login.Data.Item4;
                userIsTecher = login.Data.Item6;
                className = login.Data.Item5;
                timeOfCreation = login.Data.Item2;
            }
            else
            {
                this.Close();
            }

            //Erstellung eine Schulklasse
            studentClass = new StudentClass(className);

            /*
             * 
             * Fragt ab ob es sich um ein Lehrer handelt, wenn ja 
             * wird ein Objekt der Klasse Lehrer erstellt, oder 
             * ein Objekt der Klasse Schueler wenn es nicht der Fall ist.
             * 
             */
            // Name von den Bild
            string userImage;

            //Wenn die Datenbank sagt das der user ein Lehrer ist wird eine Klasse Lehrer erstellt, sonst Schüller
            if (userIsTecher)
            {
                // Ein Objekt Lehrer wird erstellt
                user = new Teacher(username, userSurname, userFirstname, timeOfCreation);
                // Dateiname von den Bild
                userImage = "teacher.png";
                // Objekt von der Klasse Person wird in Objekt der Klasse Teacher 
                Teacher teacher = (Teacher)user;
                // Arbeitsfläche wird erstellt
                Workspace.Content = new PersonFolder.TeacherFolder.TeacherWorkspace(teacher, databaseConnection);
            }
            else
            {
                user = new Student(username, userSurname, userFirstname, studentClass, timeOfCreation);
                userImage = "student.png";
                Student student = (Student)user;
                // Unter den Namen wird die Klasse angezeigt
                PersonDescription.Text = student.StudentClass.ClassName;
                Workspace.Content = new PersonFolder.StudentFolder.StudentWorkspace(student, databaseConnection);
            }

            //Öffnet ein Bild in der UI
            ProfilePhoto.Source = new BitmapImage(new Uri($@"/Image/{userImage}", UriKind.Relative));
            //Änderung von den Text in UI(Name und Klasse)
            PersonName.Text = user.Firstname + " " + user.Surname;

        }
        #endregion

        #region UI_Eventhandler
        // Fall wenn sich das Fenster schließt
        private void Window_Closed(object sender, EventArgs e)
        {
            // Wenn ein Benutzer sich angemeldet hat wird die Abmeldezeit in die Datenbank eingetragen
            if (user != null)
            {
                databaseConnection.LogOut(user.Username, user.Attendance.ArriveTime);
            }
            
            databaseConnection.Connection.Close();
        }
        #endregion
    }
}

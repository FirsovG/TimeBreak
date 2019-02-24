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
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;

namespace TimeBreak
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    /// 

    public partial class LoginWindow : Window
    {
        #region Fields
        private DBConnector databaseConnection;
        private Tuple<string, DateTime, string, string, string, bool> data;
        private string classroom;
        #endregion

        #region Constructor
        //Konstruktor der einen Datenbankanschluss einstellt
        public LoginWindow(DBConnector connector)
        {
            InitializeComponent();
            this.databaseConnection = connector;
            Taskbar.Content = new Design.Taskbar(this);
        }
        #endregion

        #region Propertys
        public Tuple<string, DateTime, string, string, string, bool> Data { get => data; set => data = value; }
        #endregion

        #region UI_Eventhandler
        // Wenn der Button login gedrückt wird ruft sich dieser Event/Fall
        // Der Prüft nach ob die eingegebene Daten in der Datenbank vorhanden sind
        // und packt die in ein Tupel der zurückkommt
        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {

            data = databaseConnection.LogIn(loginUsername.Text, loginPassword.Password);

            //Wenn der Tupel lehr ist wird ein Nachtricht angezeigt
            if(data.Item1 == null)
            {
                MessageBox.Show("Matrikelnummer oder Passwort ist falsch");
            }
            else
            {
                this.DialogResult = true;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.DialogResult = false;
        }
        #endregion

        #region Design
        /*
        private void LoginUsername_MouseEnter(object sender, MouseEventArgs e)
        {
            UsernameLabel.Visibility = Visibility.Hidden;
        }

        private void LoginPassword_MouseEnter(object sender, MouseEventArgs e)
        {
            PasswordLabel.Visibility = Visibility.Hidden;
        }

        private void LoginUsername_MouseLeave(object sender, MouseEventArgs e)
        {
            if ((sender as TextBox).Text == "")
            {
                UsernameLabel.Visibility = Visibility.Visible;
            }
        }

        private void LoginPassword_MouseLeave(object sender, MouseEventArgs e)
        {
            if ((sender as PasswordBox).Password == "")
            {
                UsernameLabel.Visibility = Visibility.Visible;
            }
        }
        */
        #endregion
    }
}

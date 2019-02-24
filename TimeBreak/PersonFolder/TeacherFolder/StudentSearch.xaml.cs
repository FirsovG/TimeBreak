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

namespace TimeBreak.PersonFolder.TeacherFolder
{
    /// <summary>
    /// Логика взаимодействия для StudentSearch.xaml
    /// </summary>
    public partial class StudentSearch : Window
    {
        #region Field
        private DBConnector databaseConnection;
        #endregion

        #region Constructor
        public StudentSearch(DBConnector databaseConnection)
        {
            InitializeComponent();
            this.databaseConnection = databaseConnection;
            Taskbar.Content = new Design.Taskbar(this);
        }
        #endregion

        #region UI_Eventhanlers
        private void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            // Beim druck von den OkButton löst sich ein Fall der nachprüft ob der jenige der eingegeben wurde exestiert
            int personCount = databaseConnection.CheckStudent(Surname.Text, Firstname.Text);
            if (personCount > 0)
            {
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Keine treffer");
            }
        }

        // Wenn der CancelButton gedückt wurde schlisst sich das Fenster
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}

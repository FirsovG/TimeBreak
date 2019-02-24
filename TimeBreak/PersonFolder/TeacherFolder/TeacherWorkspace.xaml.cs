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

namespace TimeBreak.PersonFolder.TeacherFolder
{
    /// <summary>
    /// Логика взаимодействия для TeacherWorkspace.xaml
    /// </summary>
    public partial class TeacherWorkspace : Page
    {
        #region Fields
        private Teacher teacher;
        private DBConnector databaseConnection;
        #endregion

        #region Constructor
        public TeacherWorkspace(Teacher teacher, DBConnector databaseConnection)
        {
            this.teacher = teacher;
            this.databaseConnection = databaseConnection;

            InitializeComponent();
        }
        #endregion

        #region UI_Eventhanler
        private void ClickButton_Click(object sender, RoutedEventArgs e)
        {
            teacher.Kontrollieren(databaseConnection);
        }
        #endregion

        private void FileDialogButton_Click(object sender, RoutedEventArgs e)
        {
            CsvImport csvImport = new CsvImport(databaseConnection);
        }
    }
}

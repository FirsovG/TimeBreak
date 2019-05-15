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
    /// Логика взаимодействия для StudentInfo.xaml
    /// </summary>
    public partial class StudentInfo : Window
    {
        #region Fields
        private Tuple<string, string, string, string, string> data;
        #endregion

        #region Constructor
        //Konstruktor der ein Tuple bekommt
        public StudentInfo(Tuple<string, string, string, string, string> Data)
        {
            this.data = Data;
            // Zum zeigen da
            // Ein Fall wird gestartet wenn das Fenster geladen ist
            this.Loaded += new RoutedEventHandler(AddText_Loaded);
            InitializeComponent();
            Taskbar.Content = new Design.Taskbar(this);
        }
        #endregion

        #region UI_Eventhandlers
        // Der Fall Schreibt die Daten in die UI Zellen
        /* Es ist ein anderer Weg die Daten in UI reinzuschreiben
         * In den Mainwindow wurden die Daten nach Inizialiserungsfunktion reingespeichert */
        void AddText_Loaded(object sender, RoutedEventArgs e)
        {
            Firstname.Content += data.Item2;
            Surname.Content += data.Item1;
            Username.Content += data.Item3;
            StudentClass.Content += data.Item4;
            Abode.Content += data.Item5;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}

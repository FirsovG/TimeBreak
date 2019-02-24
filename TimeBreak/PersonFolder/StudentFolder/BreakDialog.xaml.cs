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

namespace TimeBreak
{
    /// <summary>
    /// Логика взаимодействия для PauseDialog.xaml
    /// </summary>
    public partial class BreakDialog : Window
    {
        #region Field
        private string place;
        #endregion

        #region Constructor
        public BreakDialog()
        {
            InitializeComponent();
            Taskbar.Content = new Design.Taskbar(this);
        }
        #endregion

        #region UI_Eventhandler
        private void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            // Wenn der Button ok gedrückt wird, kommt es zu eine Abrage welcher RadioButton wurde Gedruckt
            // Der Inhalt des Felds namens platz hängt von den ausgewählten Radiobutton
            if(A_Gebaeude.IsChecked == true)
            {
                place = "A-Gebäude";
            }
            else if(B_Gebaeude.IsChecked == true)
            {
                place = "B-Gebäude";
            }
            else if (C_Gebaeude.IsChecked == true)
            {
                place = "C-Gebäude";
            }
            else
            {
                place = "Drausen";
            }

            this.DialogResult = true;
        }
        #endregion

        #region Property
        public string Place { get => place; }
        #endregion
    }
}

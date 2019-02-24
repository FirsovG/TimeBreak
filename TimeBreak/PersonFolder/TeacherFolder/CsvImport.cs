using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

// Aus diese Bibliothek nehmen wir den FileDioalog
using Microsoft.Win32;
// Bibliothek um in unseren Fall was aus eine Datei zu lesen
using System.IO;

namespace TimeBreak.PersonFolder.TeacherFolder
{
    class CsvImport
    {
        #region Fields
        private string filePath;
        private string[][] data = new string[101][];
        private int rowCounter;

        private DBConnector databaseConnection;
        #endregion

        #region Constructor
        public CsvImport(DBConnector databaseConnection)
        {

            this.databaseConnection = databaseConnection;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Csv Dateien|*.csv";
            openFileDialog.DefaultExt = "*.csv";
            openFileDialog.ShowDialog();

            this.filePath = openFileDialog.FileName;
            if (filePath != "")
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {

                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        if (rowCounter == 0)
                        {
                            rowCounter++;
                            continue;
                        }

                        data[rowCounter - 1] = values;

                        rowCounter++;
                    }
                    databaseConnection.ImportPeople(data, rowCounter - 1);
                    MessageBox.Show("Die Benutzer wurden Erfolgreich importiert");
                }
            }
            else
            {
                MessageBox.Show("Die Datei wurde nicht gefunden");
            }
        }
        #endregion

        #region Propertys
        public string FilePath { get => filePath; set => filePath = value; }
        #endregion
    }
}

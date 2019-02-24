using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace TimeBreak
{
    public class DBConnector
    {
        #region Field
        // Feld für die Verbindung mit dem Sql server
        private MySqlConnection connection;
        private string classRoom;
        private int attendanceId;
        private int startUnixTimestamp;
        #endregion

        #region Constructor
        public DBConnector(string args, string classRoom)
        {
            // Guckt nach ob man sich zur Datebank verbinden kann
            // Wenn nicht wird ein Meldefenster angezeigt und das Programm schließt sich
            this.classRoom = classRoom;
            try
            {
                connection = new MySqlConnection(args);
                connection.Open();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        #endregion

        #region Functions
        // Funktion für die Form LoginWindow
        // Prüft den eingegebene Matrikelnummer und Passwort 
        // Bei überinstimmung gibt ein Tuple zurück
        public Tuple<string, DateTime, string, string, string, bool> LogIn(string username, string password)
        {
            // Wenn die Connection zu ist macht er die auf
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            // Sql-Abfrage ob es eine übereinstimmung gibt(wenn ja dann wird eine 1 zurückgegeben)
            string query = "SELECT COUNT(1) FROM tab_Menschen WHERE M_Matrikelnummer=@Username AND M_Passwort=@Password";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Username", username);
            sqlCommand.Parameters.AddWithValue("@Password", password);

            // Führt die Sql anfrage durch und convertier den rückgabewert in eine Ganzzahl
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());

            // Wenn es eine übereinstimmung gibt werden die Dateien rausgezogen und es wird ein neuer eintrag in die Anwesenheit gemacht
            if (count == 1)
            {
                // Sql-Abfrage um die Daten von der Person zu bekommen
                query = "SELECT M_Vorname, M_Nachname, M_Klasse, M_istLehrer FROM tab_Menschen WHERE M_Matrikelnummer=@Username AND M_Passwort=@Password";
                sqlCommand = new MySqlCommand(query, connection);
                sqlCommand.Parameters.AddWithValue("@Username", username);
                sqlCommand.Parameters.AddWithValue("@Password", password);

                // Sql-Leser um die Daten in die Klasse einzupeichern
                MySqlDataReader reader = sqlCommand.ExecuteReader();

                // Temporäre Variablen
                DateTime startTime = DateTime.Now;
                string surname = "";
                string firstname = "";
                string StudentClass = "";
                bool isTeacher = false;

                // Speichern von Daten in die Variablen
                while (reader.Read())
                {
                    surname = reader["M_Nachname"].ToString();
                    firstname = reader["M_Vorname"].ToString();
                    StudentClass = reader["M_Klasse"].ToString();
                    isTeacher = (bool)reader["M_istLehrer"];
                }
                
                // Erstellung von Tuple in der die Daten sind 
                Tuple<string, DateTime, string, string, string, bool> Data = new Tuple<string, DateTime, string, string, string, bool>
                    (username, startTime, surname, firstname, StudentClass, isTeacher);
                reader.Close();

                // Sql-Kommand welches die Anwesenheit einträgt 
                query = "INSERT INTO tab_anwesenheit(A_Datum, A_Klassenraum, M_Matrikelnummer, A_Anfangszeit) VALUES (@Date, @ClassRoom, @Username, @DateStart)";
                sqlCommand = new MySqlCommand(query, connection);
                sqlCommand.Parameters.AddWithValue("@Date", startTime.ToString("yyyy-MM-dd"));
                sqlCommand.Parameters.AddWithValue("@ClassRoom", classRoom);
                sqlCommand.Parameters.AddWithValue("@Username", username);
                sqlCommand.Parameters.AddWithValue("@DateStart", startTime.ToString("HH:mm:ss"));
                sqlCommand.ExecuteScalar();

                query = "SELECT A_ID, A_Gesamtzeit FROM tab_anwesenheit WHERE M_Matrikelnummer=@Username AND A_Datum=@Date";
                sqlCommand = new MySqlCommand(query, connection);
                sqlCommand.Parameters.AddWithValue("@Username", username);
                sqlCommand.Parameters.AddWithValue("@Date", startTime.ToString("yyyy-MM-dd"));

                reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    int tmp = Convert.ToInt32(reader["A_ID"]);
                    if (tmp > attendanceId)
                    {
                        attendanceId = tmp;
                        
                        if (!DBNull.Value.Equals(reader["A_Gesamtzeit"]))
                        {
                            startUnixTimestamp = Convert.ToInt32(reader["A_Gesamtzeit"]);
                        }
                    }
                }

                // Schlißt die Connection und gibt den Tuple zurück
                connection.Close();
                return Data;
            }
            // Schlißt die Connection und gibt ein Tuple zurück
            connection.Close();
            return new Tuple<string, DateTime, string, string, string, bool>(null, DateTime.Now, null, null, null, false);
        }

        public int GetLastBreakId(string username, DateTime startTime)
        {

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            string query = "SELECT P_ID FROM tab_pause WHERE M_Matrikelnummer=@Username AND A_Datum=@Date";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Username", username);
            sqlCommand.Parameters.AddWithValue("@Date", startTime.ToString("yyyy-MM-dd"));

            MySqlDataReader reader = sqlCommand.ExecuteReader();

            int breakId = 0;

            while (reader.Read())
            {
                if (!DBNull.Value.Equals(reader["P_ID"]))
                {
                    int tmp = Convert.ToInt32(reader["P_ID"]);
                    if (tmp > breakId)
                    {
                        breakId = tmp;
                    }
                }
                else
                {
                    return 0;
                }
            }
            reader.Close();
            connection.Close();
            return breakId;
        }

        public int GetLastBreakTimestamp(int breakId, string username, DateTime startTime)
        {

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            string query = "SELECT P_Gesamtzeit FROM tab_pause WHERE P_ID=@BreakId AND M_Matrikelnummer=@Username AND A_Datum=@Date";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@BreakId", breakId);
            sqlCommand.Parameters.AddWithValue("@Username", username);
            sqlCommand.Parameters.AddWithValue("@Date", startTime.ToString("yyyy-MM-dd"));

            MySqlDataReader reader = sqlCommand.ExecuteReader();

            int breakTimeStartUnixTimestamp = 0;

            while (reader.Read())
            {
                if (!DBNull.Value.Equals(reader["P_Gesamtzeit"]))
                {
                    breakTimeStartUnixTimestamp = Convert.ToInt32(reader["P_Gesamtzeit"]);
                }
            }

            reader.Close();
            connection.Close();
            return breakTimeStartUnixTimestamp;
        }

        //Funktion um die Pausenzeiten einzutragen (ohne Endzeit)
        public void MakeBreak(string username, DateTime startTime, string abode)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            string query = "INSERT INTO tab_pause(A_ID, A_Datum, M_Matrikelnummer, P_Anfangszeit, P_Aufenthaltsort) VALUES(@A_Id, @Date, @Username, @P_Start, @P_Platz);";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@A_Id", attendanceId);
            sqlCommand.Parameters.AddWithValue("@Date", startTime.ToString("yyyy-MM-dd"));
            sqlCommand.Parameters.AddWithValue("@Username", username);
            sqlCommand.Parameters.AddWithValue("@P_Start", startTime.ToString("HH:mm:ss"));
            sqlCommand.Parameters.AddWithValue("@P_Platz", abode);
            sqlCommand.ExecuteScalar();

            connection.Close();
        }
        // Funktion um die Pausenenden einzutragen
        public void GetBackFromTheBreak(int breakId, string userId, DateTime startTime, int breakTimestamp)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            string query = "UPDATE tab_pause SET P_Endzeit=@P_Stop, P_Gesamtzeit=@UnixTimestamp WHERE A_Datum=@Date AND M_Matrikelnummer=@Username AND P_ID=@ID";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@P_Stop", DateTime.Now.ToString("HH:mm:ss"));
            sqlCommand.Parameters.AddWithValue("@UnixTimestamp", DateTime.Now.Subtract(startTime).TotalSeconds + breakTimestamp);
            sqlCommand.Parameters.AddWithValue("@Date", startTime.ToString("yyyy-MM-dd"));
            sqlCommand.Parameters.AddWithValue("@Username", userId);
            sqlCommand.Parameters.AddWithValue("@ID", breakId);
            sqlCommand.ExecuteScalar();
            connection.Close();
        }

        //Checkt ob es ein Mensch mit diesen Nachnamen und Vornamen exestirert
        public int CheckStudent(string surname, string firstname)
        {

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            // Nur Studenten werden Analysiert
            string query = "SELECT COUNT(1) FROM tab_Menschen WHERE M_Nachname=@Surname AND M_Vorname=@Firstname AND M_istLehrer=false";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Surname", surname);
            sqlCommand.Parameters.AddWithValue("@Firstname", firstname);

            int studentCount = Convert.ToInt32(sqlCommand.ExecuteScalar());

            connection.Close();
            return studentCount;
        }

        // Eine Funktion die ein Tupel zurückgibt in den Daten über den Student stehen
        public Tuple<string, string, string, string> GetStudentInfo(string surname, string firstname)
        {

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            string query = "SELECT M_Matrikelnummer, M_Klasse FROM tab_Menschen WHERE M_Nachname=@Surname AND M_Vorname=@Firstname AND M_istLehrer=false";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Surname", surname);
            sqlCommand.Parameters.AddWithValue("@Firstname", firstname);

            // Sql-Leser um die Daten in die Klasse einzupeichern
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            // Temporäre Variablen
            string username = "";
            string studentClass = "";

            // Speichern von Daten in die Variablen
            while (reader.Read())
            {
                username = reader["M_Matrikelnummer"].ToString();
                studentClass = reader["M_Klasse"].ToString();
            }

            // Erstellung von Tuple in der die Daten sind 
            Tuple<string, string, string, string> Data = new Tuple<string, string, string, string>
                (surname, firstname, username, studentClass);

            reader.Close();
            connection.Close();

            return Data;
        }

        public void ImportPeople(string[][] data, int rowCount)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            string query = "";
            MySqlCommand sqlCommand;

            for (int i = 0; i < rowCount; i++)
            {
                //Wenn das Matrikelnummer mit eine Zahl anfängt wird noch ein Schueler reingeschrieben
                if (char.IsDigit(data[i][2][0]))
                {
                    query = $"INSERT INTO tab_Menschen VALUES(@Username,@Password,@Firstname,@Surname,0,@StudentClass)";
                    sqlCommand = new MySqlCommand(query, connection);
                    sqlCommand.Parameters.AddWithValue("@Username",     data[i][2]);
                    sqlCommand.Parameters.AddWithValue("@Password",     data[i][3]);
                    sqlCommand.Parameters.AddWithValue("@Firstname",    data[i][0]);
                    sqlCommand.Parameters.AddWithValue("@Surname",      data[i][1]);
                    sqlCommand.Parameters.AddWithValue("@StudentClass", data[i][4]);
                    sqlCommand.ExecuteScalar();
                }
                else
                {
                    query = $"INSERT INTO tab_Menschen(M_Matrikelnummer, M_Passwort, M_Vorname, M_Nachname, M_istLehrer) " +
                        $"VALUES(@Username,@Password,@Firstname,@Surname,1)";
                    sqlCommand = new MySqlCommand(query, connection);
                    sqlCommand.Parameters.AddWithValue("@Username",  data[i][2]);
                    sqlCommand.Parameters.AddWithValue("@Password",  data[i][3]);
                    sqlCommand.Parameters.AddWithValue("@Firstname", data[i][0]);
                    sqlCommand.Parameters.AddWithValue("@Surname",   data[i][1]);
                    sqlCommand.ExecuteScalar();
                }
            }
            connection.Close();
        }

        public void LogOut(string username, DateTime startTime)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            string query = "UPDATE tab_anwesenheit SET A_Endzeit=@DateStop, A_Gesamtzeit=@UnixTimestamp WHERE A_ID=@AttendanceId AND A_Datum=@Date AND M_Matrikelnummer=@User";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@DateStop", DateTime.Now.ToString("HH:mm:ss"));
            sqlCommand.Parameters.AddWithValue("@UnixTimestamp", DateTime.Now.Subtract(startTime).TotalSeconds + startUnixTimestamp);
            sqlCommand.Parameters.AddWithValue("@AttendanceId", attendanceId);
            sqlCommand.Parameters.AddWithValue("@Date", startTime.ToString("yyyy-MM-dd"));
            sqlCommand.Parameters.AddWithValue("@User", username);
            sqlCommand.ExecuteScalar();

            connection.Close();
        }
        #endregion

        #region Property
        public MySqlConnection Connection { get => connection; set => connection = value; }
        #endregion
    }
}

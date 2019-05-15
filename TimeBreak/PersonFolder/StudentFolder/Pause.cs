using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeBreak
{
    public class Pause
    {

        #region Fields
        private DateTime startTime;
        private string abode;
        private int breakId;
        private int breakTimestamp;
        private string username;
        

        private DBConnector databaseConnection;
        #endregion

        #region Constructor
        public Pause(int breakId, int breakTimestamp, string abode, DBConnector databaseConnection, string username, DateTime startTime)
        {
            this.username = username;
            this.startTime = startTime;
            this.abode = abode;
            this.databaseConnection = databaseConnection;
            this.breakId = breakId;
            this.breakTimestamp = breakTimestamp;
            databaseConnection.MakeBreak(username, startTime, abode);
        }
        #endregion

        #region Function
        public void GetBack()
        {
            databaseConnection.GetBackFromTheBreak(breakId, username, startTime, breakTimestamp);
        }
        #endregion
    }
}

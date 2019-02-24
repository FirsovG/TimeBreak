using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TimeBreak
{
    public class Attendance
    {

        #region Fields
        private DateTime arriveTime;
        #endregion

        #region Constructor
        public Attendance(DateTime creationTime)
        {
            this.arriveTime = creationTime;
        }
        #endregion


        #region Propertys
        public DateTime ArriveTime { get => arriveTime; set => arriveTime = value; }
        #endregion
    }
}

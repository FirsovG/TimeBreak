using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeBreak.PersonFolder.StudentFolder
{
    public class StudentAttendance : Attendance
    {

        #region Fields
        private const int maxBreakCount = 4;
        private Pause[] breakTimes = new Pause[maxBreakCount];
        private bool takeABreak = false;
        private int breakCount = 0;
        private int breakId = 0;
        private int lastBreakId = 0;
        private int lastBreakTimestamp = 0;
        DateTime startTime = DateTime.Now;
        #endregion

        #region Constructor
        public StudentAttendance(DateTime creationTime, DBConnector databaseConnection, string username) : base(creationTime)
        {
            this.takeABreak = false;
            lastBreakId = databaseConnection.GetLastBreakId(username, startTime, out breakId, out breakCount);
            lastBreakTimestamp = databaseConnection.GetLastBreakTimestamp(lastBreakId, username, startTime);
        }
        #endregion

        #region Functions
        public void MakeBreak(string aufenthaltsort, DBConnector databaseConnection, string username)
        {
            lastBreakId = databaseConnection.GetLastBreakId(username, startTime, out breakId, out breakCount);
            lastBreakTimestamp = databaseConnection.GetLastBreakTimestamp(lastBreakId, username, startTime);
            breakTimes[breakCount] = new Pause(breakId, lastBreakTimestamp, aufenthaltsort, databaseConnection, username, startTime);
            takeABreak = true;
        }

        public void GetBack()
        {
            breakTimes[breakCount].GetBack();
            takeABreak = false;
            breakCount++;
        }
        #endregion

        #region Propertys
        public int BreakCount { get => breakCount; set => breakCount = value; }
        public bool TakeABreak { get => takeABreak; set => takeABreak = value; }
        public int LastBreakTimestamp { get => lastBreakTimestamp; set => lastBreakTimestamp = value; }
        #endregion

    }
}

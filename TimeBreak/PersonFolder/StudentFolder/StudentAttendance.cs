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
        private Pause[] breakTimes = new Pause[4];
        private bool takeABreak = false;
        private int breakCount = 0;
        private int lastBreakTimestamp = 0;
        #endregion

        #region Constructor
        public StudentAttendance(DateTime creationTime) : base(creationTime)
        {
            this.takeABreak = false;
        }
        #endregion

        #region Functions
        public void MakeBreak(string aufenthaltsort, DBConnector databaseConnection, string username)
        {
            DateTime startTime = DateTime.Now;
            breakCount = databaseConnection.GetLastBreakId(username, startTime) + 1;
            lastBreakTimestamp = databaseConnection.GetLastBreakTimestamp(breakCount - 1, username, startTime);
            breakTimes[breakCount - 1] = new Pause(breakCount, lastBreakTimestamp, aufenthaltsort, databaseConnection, username, startTime);
            takeABreak = true;
        }

        public void GetBack()
        {
            breakTimes[breakCount - 1].GetBack();
            takeABreak = false;
        }
        #endregion

        #region Propertys
        public int BreakCount { get => breakCount; set => breakCount = value; }
        public bool TakeABreak { get => takeABreak; set => takeABreak = value; }
        public int LastBreakTimestamp { get => lastBreakTimestamp; set => lastBreakTimestamp = value; }
        #endregion

    }
}

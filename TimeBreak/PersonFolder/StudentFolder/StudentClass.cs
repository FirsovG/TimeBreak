using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeBreak
{
    public class StudentClass
    {
        #region Filed
        private string className;
        #endregion

        #region Constructor
        public StudentClass(string name)
        {
            this.className = name;
        }
        #endregion

        #region Property
        public string ClassName
        {
            get
            {
                return className;
            }

            set
            {
                className = value;
            }
        }
        #endregion
    }
}

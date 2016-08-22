using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSNews
{
    class DBConnection
    {
        public DBOpeartion cobject;
        public DBConnection()
        {
            cobject = new DBOpeartion("Your_Connection_String");
        }
    }
}

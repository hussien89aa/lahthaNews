using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace NewsApp.Classes
{
    public class DBConnection
    {
        public DBOpeartion cobject;
        public DBConnection()
        {
            cobject = new DBOpeartion(WebConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString);
        }
    }
}
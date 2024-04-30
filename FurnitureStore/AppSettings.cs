using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureStore
{
    public static class AppSettings
    {
        private static int userId;


        public static int UserId
        {
            get { return userId; }
            set { userId = value; }
        }
    }
}

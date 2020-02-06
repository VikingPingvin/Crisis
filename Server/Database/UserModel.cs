using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisis.Database.Model
{
    public class UserModel
    {

        public string emailAdress { get; set; }

        public string passwordHash { get; set; }

        public string[] warnings { get; set; }

        public bool isGameMaster { get; set; }
    }
}

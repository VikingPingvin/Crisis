using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crisis.Database.Model;

namespace Crisis.Database
{
    public interface IDatabaseHandler
    {

        List<UserModel> listAllUsers();

        UserModel getUser();

        bool addUser(UserModel model);
    }
}

using Crisis.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Crisis.Database
{
    class LocalJsonDatabase : IDatabaseHandler
    {
        private string _dbRootFolder = System.IO.Directory.GetCurrentDirectory();
        private string _dbName = "userDb.json";
        private string _dbFullPath;
        public LocalJsonDatabase()
        {
            string fullPath = Path.Combine(_dbRootFolder, _dbName);

        }

        public bool addUser(UserModel model)
        {
            if (validateUserModel(model))
            {
                var jsonData = File.ReadAllText(@_dbFullPath);
                var userList = JsonConvert.DeserializeObject<List<UserModel>>(jsonData) ?? new List<UserModel>();
                userList.Add(model);

                using(StreamWriter file = File.CreateText(_dbFullPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, userList);

                    return true;
                }
            }

            return false;
        }

        // Todo: Validation probably should not happen in server
        private bool validateUserModel(UserModel model)
        {

            return true;
        }

        public UserModel getUser()
        {
            throw new NotImplementedException();
        }

        public List<UserModel> listAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}

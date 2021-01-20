using DesignDBLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDBLibrary.DataAccess
{
    public interface IDataConnection
    {
        void AddUser(UserModel NewUser);
        void DeleteUser(UserModel OldUser);
        void UpdateUser(string ThisUser);
        UserModel GetUser(string userName);
        List<UserModel> GetUsers_All();
    }
}

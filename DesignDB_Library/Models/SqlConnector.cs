using DesignDBLibrary.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDBLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        private const string db = "DEV";

        public void AddUser(UserModel NewUser)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@UserName", NewUser.UserName);
                p.Add("@PW", NewUser.PW);
                p.Add("@Priviledge", NewUser.Priviledge);
                p.Add("ActiveDesigner", NewUser.ActiveDesigner);
                p.Add("ID", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);


                //TODO - Add try catch to handle duplicate users
                connection.Execute("dbo.spUser_Add", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteUser(UserModel OldUser)
        {
            throw new NotImplementedException();
        }

        public List<UserModel> GetUsers_All()
        {
            List<UserModel> output;  

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                output = connection.Query<UserModel>("dbo.spUsers_GetAll").ToList();

                foreach (UserModel User in output)
                {
                    var p = new DynamicParameters();
                    p.Add("@UserName", User.UserName);
                    p.Add("@PW", User.PW);
                    p.Add("@Priviledge", User.Priviledge);
                    p.Add("@ActiveDesigner", User.ActiveDesigner);
                    p.Add("@ID", User.Id);

                }
            }

            return output;

        }


        public void UpdateUser(UserModel ThisUser)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@UserName", ThisUser.UserName);
                p.Add("@PW", ThisUser.PW);
                p.Add("@Priviledge", ThisUser.Priviledge);
                p.Add("@ActiveDesigner", ThisUser.ActiveDesigner);


                connection.Execute("dbo.spUser_Update", p, commandType: CommandType.StoredProcedure);               
            }
        }


        public UserModel GetUser(string userName)
        {
            throw new NotImplementedException();
        }
    }
}

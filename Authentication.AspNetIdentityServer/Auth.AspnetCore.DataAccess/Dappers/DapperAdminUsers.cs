using Auth.AspnetCore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Dapper;

namespace Auth.AspnetCore.DataAccess.Dappers
{
    public class DapperAdminUsers
    {
        private readonly SqlConnection _connection;

        public DapperAdminUsers(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<AdminUsers> FindByLoginAsync(string loginName, string password)
        {
            string sql = "SELECT * FROM dbo.AdminUsers WHERE (LoginName=@LoginName or Mobile=@LoginName OR Email=@LoginName) AND PassWord=@PassWord;";
            return await _connection.QuerySingleOrDefaultAsync<AdminUsers>(sql, new
            {
                LoginName = loginName,
                PassWord = password
            });
        }
    }
}
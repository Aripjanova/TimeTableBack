
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using WebApplication;
using MySql.Data.MySqlClient;

namespace  WebApplication.Models
{
    public class TimeTableQuery
    {
        public AppDb Db { get; }

        public TimeTableQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Groups> FindOneGroups(int id_groups)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_groups`, `name`, `id_class` FROM `ClassStudents` WHERE `id_groups` = @id_groups";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_groups",
                DbType = DbType.Int32,
                Value = id_groups,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }
  

        public async Task<List<Groups>> LatestPostsGroups()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_groups`, `name`, `id_class` FROM `ClassStudents` ORDER BY `id_groups` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllGroups()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `ClassStudents`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Groups>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Groups>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Groups(Db)
                    {
                        id_groups = reader.GetInt32(0),
                        name = reader.GetString(1),
                        id_class = reader.GetInt32(2),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
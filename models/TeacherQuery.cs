using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using WebApplication;
using MySql.Data.MySqlClient;

namespace  WebApplication.Models
{
    public class TeacherQuery
    {
        public AppDb Db { get; }

        public TeacherQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Teacher> FindOneTeacher(int id_teacher)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_teacher`, `first_name`, `last_name` FROM `teacher` WHERE `id_teacher` = @id_teacher";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_teacher",
                DbType = DbType.Int32,
                Value = id_teacher,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Teacher>> LatestPostsTeacher()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_teacher`, `first_name`, `last_name` FROM `teacher` ORDER BY `id_teacher` ASC ";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllTeacher()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `teacher`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Teacher>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Teacher>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Teacher(Db)
                    {
                        id_teacher = reader.GetInt32(0),
                        first_name = reader.GetString(1),
                        last_name = reader.GetString(2),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
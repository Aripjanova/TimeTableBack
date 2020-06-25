
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using WebApplication;
using MySql.Data.MySqlClient;

namespace  WebApplication.Models
{
    public class ClassRoomQuery
    {
        public AppDb Db { get; }

        public ClassRoomQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<ClassRoom> FindOneRoom(int id_classRoom)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_classRoom`, `ID`, `name` FROM `ClassRoom` WHERE `id_classRoom` = @id_classRoom";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_classRoom",
                DbType = DbType.Int32,
                Value = id_classRoom,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<ClassRoom>> LatestPostsClassRoom()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_classRoom`, `ID`, `name` FROM `ClassRoom` ORDER BY `id_classRoom` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllClassRooms()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `ClassRoom`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<ClassRoom>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<ClassRoom>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new ClassRoom(Db)
                    {
                        id_classRoom = reader.GetInt32(0),
                        ID= reader.GetInt32(1),
                        name = reader.GetString(2),
                       
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
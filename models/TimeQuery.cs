
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using WebApplication;
using MySql.Data.MySqlClient;

namespace  WebApplication.Models
{
    public class TimeQuery
    {
        public AppDb Db { get; }

        public TimeQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Time> FindOneTime(int id_time_all)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_time_all`, `id_timeName`, `id_days` FROM `TimesAll` WHERE `id_time_all` = @id_time_all";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_time_all",
                DbType = DbType.Int32,
                Value = id_time_all,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Time>> LatestPostsTime()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_time_all`, `id_timeName`, `id_days` FROM `TimesAll` ORDER BY `id_time_all` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllTimes()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `TimesAll`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Time>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Time>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Time(Db)
                    {
                        id_time_all = reader.GetInt32(0),
                        id_timeName = reader.GetInt32(1),
                        id_days = reader.GetInt32(2)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
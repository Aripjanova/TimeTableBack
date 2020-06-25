using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using WebApplication;
using MySql.Data.MySqlClient;

namespace  WebApplication.Models
{
    public class SchedulerQuery
    {
        public AppDb Db { get; }

        public SchedulerQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Scheduler> FindOneShedulerBlock(int id_block)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_block`, `day`, `hour`,`group` ,`teacher` FROM `schedule` WHERE `id_block` = @id_block";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_block",
                DbType = DbType.Int32,
                Value = id_block,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Scheduler>> Sheduler()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_block`, `day`, `hour`,`group`,`teacher` FROM `schedule` ORDER BY `group` DESC ";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllSchedule()
        {
            using var transaction = Db.Connection.BeginTransaction();
            
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `schedule`";
            cmd.Transaction = transaction;
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Scheduler>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Scheduler>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Scheduler(Db)
                    {
                        id_block = reader.GetInt32(0),
                        day= reader.GetInt32(1),
                        hour= reader.GetInt32(2),
                        group= reader.GetInt32(3),
                        teacher= reader.GetInt32(4),
                       
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
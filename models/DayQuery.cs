
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using WebApplication;
using MySql.Data.MySqlClient;

namespace  WebApplication.Models
{
    public class DayQuery
    {
        public AppDb Db { get; }

        public DayQuery(AppDb db)
        {
            Db = db;
        }

   

        public async Task<List<Day>> GetAllDays()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_day`, `name`,`dayscol` FROM `Days` ORDER BY `id_day` ASC ";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<Day> FindOneDay(int id_block)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_day`, `name`, `dayscol` FROM `Day` WHERE `id_day` = @id_day";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_day",
                DbType = DbType.Int32,
                Value = id_block,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        private async Task<List<Day>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Day>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Day(Db)
                    {
                        id_day = reader.GetInt32(0),
                        name = reader.GetString(1),
                        dayscol = reader.GetString(2),
                        
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
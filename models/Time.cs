


using System;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApplication.Models
{
    public class Time
    {
        public int id_time_all { get; set; }
        public int id_timeName { get; set; }
        public int id_days { get; set; }
       
        internal AppDb Db { get; set; }

        public Time()
        {
        }

        internal Time(AppDb db)
        {
            Db = db;
        }

        public async Task InsertTime()
        {
            Console.WriteLine(id_time_all+id_timeName);
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `TimesAll` (`id_timeName`, `id_days`) VALUES (@id_timeName @id_days);";
         //   INSERT INTO `timetable`.`ClassStudents` (`name`, `id_class`) VALUES ('5', '4');

          //  @"INSERT INTO Tasks(Text,Created) VALUES (@Text,STR_TO_DATE(@Date, '%Y/%m/%d'));";            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            id_time_all = (int) cmd.LastInsertedId;
        }

        public async Task UpdateTime()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `TimesAll` SET `id_timeName` = @id_timeName , `id_days` = @id_days WHERE `id_time_all` = @id_time_all;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteTime()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `TimesAll WHERE `id_time_all` = @id_time_all;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_time_all",
                DbType = DbType.Int32,
                Value = id_time_all,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_timeName",
                DbType = DbType.String,
                Value = id_timeName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_days",
                DbType = DbType.Int32,
                Value = id_days,
            });
        }
    }
}
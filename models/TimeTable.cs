

using System;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApplication.Models
{
    public class TimeTable
    {
        public int id_block { get; set; }
        public string id_classRoom { get; set; }
        public int id_time_all { get; set; }

        internal AppDb Db { get; set; }

        public TimeTable()
        {
        }

        internal TimeTable(AppDb db)
        {
            Db = db;
        }

        public async Task InsertTimeTable()
        {
            Console.WriteLine(id_block+id_classRoom);
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `TimeTable` (`id_block`,`id_classRoom`, `id_time_all`) VALUES (@id_block @id_classRoom,@id_time_all);";
         //   INSERT INTO `timetable`.`ClassStudents` (`name`, `id_class`) VALUES ('5', '4');

          //  @"INSERT INTO Tasks(Text,Created) VALUES (@Text,STR_TO_DATE(@Date, '%Y/%m/%d'));";            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
           
        }

        public async Task UpdateTimeTable()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `TimeTable` SET `id_block` = @id_block , `id_classRoom` = @id_classRoom,`id_time_all` = @id_time_all ";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteTable()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `TimeTable` WHERE `id_block` = @id_block;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_block",
                DbType = DbType.Int32,
                Value = id_block,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_classRoom",
                DbType = DbType.String,
                Value = id_classRoom,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_time_all",
                DbType = DbType.Int32,
                Value = id_time_all,
            });
        }
    }
}
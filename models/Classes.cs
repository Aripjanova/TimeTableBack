using System;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApplication.Models
{
    public class Groups
    {
        public int id_groups { get; set; }
        public string name { get; set; }
        public int id_class { get; set; }

        internal AppDb Db { get; set; }

        public Groups()
        {
        }

        internal Groups(AppDb db)
        {
            Db = db;
        }

        public async Task InsertGroup()
        {
            Console.WriteLine(id_class+name);
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `ClassStudents` (`name`, `id_class`) VALUES (@name ,@id_class);";
            //cmd.CommandText = @"INSERT INTO `sheduler` (`day`, `hour`,`group`,`teacher`) VALUES (@day,@hour,@group ,@teacher);";

         //   INSERT INTO `timetable`.`ClassStudents` (`name`, `id_class`) VALUES ('5', '4');

          //  @"INSERT INTO Tasks(Text,Created) VALUES (@Text,STR_TO_DATE(@Date, '%Y/%m/%d'));";            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            id_groups = (int) cmd.LastInsertedId;
        }

        public async Task UpdateGroup()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `ClassStudents` SET `name` = @name , `id_class` = @id_class WHERE `id_groups` = @id_groups;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteGroup()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `ClassStudents` WHERE `id_groups` = @id_groups;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_groups",
                DbType = DbType.Int32,
                Value = id_groups,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@name",
                DbType = DbType.String,
                Value = name,
            });
            
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_class",
                DbType = DbType.Int32,
                Value = id_class,
            });
        }
    }
}
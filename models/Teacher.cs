

using System;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApplication.Models
{
    public class Teacher
    {
        public int id_teacher { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }

        internal AppDb Db { get; set; }

        public Teacher()
        {
        }

        internal Teacher(AppDb db)
        {
            Db = db;
        }

        public async Task InsertTeacher()
        {
            Console.WriteLine("Aikaiaka"+@first_name+@last_name);
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `teacher` (`first_name`, `last_name`) VALUES (@first_name,@last_name);";
         //   INSERT INTO `timetable`.`ClassStudents` (`name`, `id_class`) VALUES ('5', '4');

          //  @"INSERT INTO Tasks(Text,Created) VALUES (@Text,STR_TO_DATE(@Date, '%Y/%m/%d'));";            BindParams(cmd);
         BindId(cmd);
         BindParams(cmd);
          await cmd.ExecuteNonQueryAsync();
            id_teacher = (int) cmd.LastInsertedId;
        
        }

        public async Task UpdateTeacher()
        {
            using var cmd = Db.Connection.CreateCommand();
            Console.WriteLine("fffd"+first_name+last_name);
            cmd.CommandText = @"UPDATE `teacher` SET `first_name` = @first_name , `last_name` = @last_name WHERE `id_teacher` = @id_teacher;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_teacher",
                DbType = DbType.Int32,
                Value = id_teacher,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@first_name",
                DbType = DbType.String,
                Value = first_name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@last_name",
                DbType = DbType.Int32,
                Value = last_name,
            });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteTeacher()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `teacher` WHERE `id_teacher` = @id_teacher;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_teacher",
                DbType = DbType.Int32,
                Value = id_teacher,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@first_name",
                DbType = DbType.String,
                Value = first_name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@last_name",
                DbType = DbType.Int32,
                Value = last_name,
            });
        }
    }
}
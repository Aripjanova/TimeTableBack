
using System;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApplication.Models
{
    public class Subject
    {
        public int id_subject { get; set; }
        public string name { get; set; }
       
        internal AppDb Db { get; set; }

        public Subject()
        {
        }

        internal Subject(AppDb db)
        {
            Db = db;
        }

        public async Task InsertSubject()
        {
            Console.WriteLine(id_subject+name);
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `subject` (`name`) VALUES (@name);";
         //   INSERT INTO `timetable`.`ClassStudents` (`name`, `id_class`) VALUES ('5', '4');

          //  @"INSERT INTO Tasks(Text,Created) VALUES (@Text,STR_TO_DATE(@Date, '%Y/%m/%d'));";            BindParams(cmd);
          cmd.Parameters.Add(new MySqlParameter
          {
              ParameterName = "@name",
              DbType = DbType.String,
              Value = name,
          });
          await cmd.ExecuteNonQueryAsync();
         

                id_subject= (int) cmd.LastInsertedId;
        }

        public async Task UpdateSubject()
        {
            Console.WriteLine("Aika"+id_subject);
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `subject` SET `name` = @name  WHERE `id_subject` = @id_subject;";
            
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteSubject()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `subject` WHERE `id_subject` = @id_subject;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_subject",
                DbType = DbType.Int32,
                Value = id_subject,
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
           
        }
    }
}
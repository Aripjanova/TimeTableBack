
using System;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApplication.Models
{
    public class ClassRoom
    {
       
        private AppDb _db;
        public int id_classRoom { get; set; }
        public int ID { get; set; }

        public string name
        { get;set; }

        internal AppDb Db
        {
            get => _db;
            set => _db = value;
        }

        public ClassRoom()
        {
        }

        internal ClassRoom(AppDb db)
        {
            Db = db;
        }

        public async Task InsertClassRoom()
        {
            Console.WriteLine("aloooooo"+ID+name);
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `ClassRoom` (`ID`, `name`) VALUES (1,name );";
         //   INSERT INTO `timetable`.`ClassStudents` (`name`, `id_class`) VALUES ('5', '4');

          //  @"INSERT INTO Tasks(Text,Created) VALUES (@Text,STR_TO_DATE(@Date, '%Y/%m/%d'));";            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            id_classRoom = (int) cmd.LastInsertedId;
        }

        public async Task UpdateClassRoom()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `ClassRoom` SET `ID` = @id , `name` = @name WHERE `id_classRoom` = @id_classRoom;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteClassRoom()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `ClassRoom` WHERE `id_classRoom` = @id_classRoom;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_classRoom",
                DbType = DbType.Int32,
                Value = id_classRoom,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ID",
                DbType = DbType.Int32,
                Value = ID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@name",
                DbType = DbType.String,
                Value = name,
            });
        }
    }
}
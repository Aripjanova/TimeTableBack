
using System;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApplication.Models
{
    public class Block
    {
        public int id_block { get; set; }
        public int id_teacher { get; set; }
        public int id_subject { get; set; }
        public int id_typeBlock { get; set; }
        public int id_groups { get; set; }
        public int countBlock { get; set; }

        internal AppDb Db { get; set; }

        public Block()
        {
        }

        internal Block(AppDb db)
        {
            Db = db;
        }

        public async Task InsertBlock()
        {
            Console.WriteLine(id_teacher+id_subject);
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Block` (`id_teacher`, `id_subject`,`id_typeBlock`,`id_groups`,`countBlock`) VALUES (@id_teacher @id_subject,@id_typeBlock,@id_groups,@countBlock);";
         //   INSERT INTO `timetable`.`ClassStudents` (`name`, `id_class`) VALUES ('5', '4');

          //  @"INSERT INTO Tasks(Text,Created) VALUES (@Text,STR_TO_DATE(@Date, '%Y/%m/%d'));";            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            id_block = (int) cmd.LastInsertedId;
        }

        public async Task UpdateBlock()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `Block` SET `id_teacher` = @id_teacher , `id_subject` = @id_subject ,`id_typeBlock` = @id_typeBlock , `id_groups` = @id_groups,`countBlock` = @countBlock WHERE `id_block` = @id_block;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteBlock()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Block` WHERE `id_block` = @id_block;";
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
                ParameterName = "@id_teacher",
                DbType = DbType.String,
                Value = id_teacher,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_subject",
                DbType = DbType.Int32,
                Value = id_subject,
            });
        }
    }
}

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
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int id_subject { get; set; }
        public string name { get; set; }
        public int id_typeBlock { get; set; }
        public string TypeBlock { get; set; }
        public int id_groups { get; set; }
        public string gname { get; set; }
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
            Console.WriteLine("id_teacher"+id_teacher);
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Blok` (`id_teacher`,`id_subject`,`id_typeBlock`,`id_groups`,`countBlock`) VALUES (@id_teacher,@id_subject,@id_typeBlock,@id_groups,@countBlock)";
         //   INSERT INTO `timetable`.`ClassStudents` (`name`, `id_class`) VALUES ('5', '4');

          //  @"INSERT INTO Tasks(Text,Created) VALUES (@Text,STR_TO_DATE(@Date, '%Y/%m/%d'));";            BindParams(cmd);
          cmd.Parameters.Add(new MySqlParameter
          {
              ParameterName = "@id_block",
              DbType = DbType.Int32,
              Value = id_block,
          });
          cmd.Parameters.Add(new MySqlParameter
          {
              ParameterName = "@id_teacher",
              DbType = DbType.Int32,
              Value = id_teacher,
          });
        
          cmd.Parameters.Add(new MySqlParameter
          {
              ParameterName = "@id_subject",
              DbType = DbType.Int32,
              Value = id_subject,
          });
        
          cmd.Parameters.Add(new MySqlParameter
          {
              ParameterName = "@id_typeBlock",
              DbType = DbType.Int32,
              Value = id_typeBlock,
          });
        
          cmd.Parameters.Add(new MySqlParameter
          {
              ParameterName = "@id_groups",
              DbType = DbType.Int32,
              Value = id_groups,
          });
        
          cmd.Parameters.Add(new MySqlParameter
          {
              ParameterName = "@countBlock",
              DbType = DbType.Int32,
              Value = countBlock,
          });
          await cmd.ExecuteNonQueryAsync();
          id_block= (int) cmd.LastInsertedId;
        }

        public async Task UpdateBlock()
        {
            Console.WriteLine("id_block"+id_block);
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `Blok` SET `id_teacher` = @id_teacher,`id_subject`=@id_subject,`id_typeBlock`=@id_typeBlock, `id_groups`=@id_groups,`countBlock`=@countBlock WHERE `id_block` = @id_block;";
            
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteBlock()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Blok` WHERE `id_block` = @id_block;";
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
                DbType = DbType.Int32,
                Value = id_teacher,
            });
        
        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@id_subject",
            DbType = DbType.Int32,
            Value = id_subject,
        });
        
        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@id_typeBlock",
            DbType = DbType.Int32,
            Value = id_typeBlock,
        });
        
        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@id_groups",
            DbType = DbType.Int32,
            Value = id_groups,
        });
        
        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@countBlock",
            DbType = DbType.Int32,
            Value = countBlock,
        });
        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@first_name",
            DbType = DbType.String,
            Value = first_name,
        });
        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@name",
            DbType = DbType.String,
            Value = name,
        });
        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@last_name",
            DbType = DbType.String,
            Value = last_name,
        });
        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@TypeBlock",
            DbType = DbType.String,
            Value = TypeBlock,
        });
        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@gname",
            DbType = DbType.String,
            Value = gname,
        });

           
        }
    }
}
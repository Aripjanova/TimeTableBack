
using System;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApplication.Models
{
    public class Day
    {
        public int id_day { get; set; }
        public string name { get; set; }
        public string dayscol { get; set; }
        internal AppDb Db { get; set; }

        public Day()
        {
        }

        internal Day(AppDb db)
        {
            Db = db;
        }
 
 
        public async Task UpdateDays()
        {
            Console.WriteLine("Days id in update"+id_day+dayscol);
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `Days` SET `name` = @name, `dayscol`=@Dayscol  WHERE `id_day` = @id_day;";
            
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }
       
        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_day",
                DbType = DbType.Int32,
                Value = id_day,
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
                ParameterName = "@dayscol",
                DbType = DbType.String,
                Value = dayscol,
            });
           
        }
    }
}

using System;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApplication.Models
{
    public class Scheduler
    {
        public int id_block { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
        public int group { get; set; }
        public int teacher { get; set; }
     //   public int type_block { get; set; }
       
        internal AppDb Db { get; set; }

        public Scheduler()
        {
        }
        internal Scheduler(AppDb db)
        {
            Db = db;
        }

        public async Task AddBlock()
        {
            Console.WriteLine("in add block "+@group+" " +@teacher);
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `sheduler` (`day`, `hour`,`group`,`teacher`) VALUES (@day,@hour,@group ,@teacher);";
         
            await cmd.ExecuteNonQueryAsync();
            id_block = (int) cmd.LastInsertedId;
        }

        public async Task UpdateScheduleBlock()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `schedule` SET `day` = @day , `hour` = @hour,`group` = @group , `teacher` = @teacher WHERE `id_block` = @id_block;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteInSchedule()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `schedule` WHERE `id_block` = @id_block;";
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
                ParameterName = "@day",
                DbType = DbType.Int32,
                Value = day,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@hour",
                DbType = DbType.Int32,
                Value = hour,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@group",
                DbType = DbType.Int32,
                Value = group,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@teacher",
                DbType = DbType.Int32,
                Value = teacher,
            });
        }
    }
}
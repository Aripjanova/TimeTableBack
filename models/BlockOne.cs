
   
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using WebApplication;
using MySql.Data.MySqlClient;

namespace  WebApplication.Models
{
    public class BlockOne
    {
        public AppDb Db { get; }

        public BlockOne(AppDb db)
        {
            Db = db;
        }

        public async Task<Block> FindOneBlock(int id_block)
        {
            Console.WriteLine("ppp"+id_block);
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_block`,`id_teacher`, `id_subject`, `id_typeBlock`,`id_groups`,`countBlock` FROM `Blok` WHERE `id_block` = @id_block";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_block",
                DbType = DbType.Int32,
                Value = id_block,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        
        public async Task DeleteAllBlock()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Blok`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Block>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Block>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Block(Db)
                    {
                         id_block = reader.GetInt32(0),
                           id_teacher = reader.GetInt32(1),
                               id_subject = reader.GetInt32(2),
                               id_typeBlock=reader.GetInt32(3),
                               id_groups = reader.GetInt32(4),
                               countBlock = reader.GetInt32(5)
                    };
                   
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
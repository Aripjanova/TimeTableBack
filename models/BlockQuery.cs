
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using WebApplication;
using MySql.Data.MySqlClient;

namespace  WebApplication.Models
{
    public class Blokquery
    {
        public AppDb Db { get; }

        public Blokquery(AppDb db)
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


        public async Task<List<Block>> LatestPostsBlock()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT b.id_block,t.first_name,t.last_name,s.name,tb.Typeblock,g.name,b.countBlock FROM Blok b,teacher  t,subject s, TypeBlock tb, ClassStudents g
            WHERE b.id_teacher=t.id_teacher and b.id_subject=s.id_subject and b.id_typeBlock=tb.id_typeBlock and b.id_groups=g.id_groups ORDER BY g.id_groups ASC ";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
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
                         first_name = reader.GetString(1),
                         last_name=reader.GetString(2),
                         name=reader.GetString(3),
                         TypeBlock=reader.GetString(4),
                         gname=reader.GetString(5),
                         countBlock = reader.GetInt32(6)
                  /*      id_teacher = reader.GetInt32(3),
                        id_subject = reader.GetInt32(4),
                        id_typeBlock=reader.GetInt32(5),
                        id_groups = reader.GetInt32(6),
                        countBlock = reader.GetInt32(7)*/
                    };
                   
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
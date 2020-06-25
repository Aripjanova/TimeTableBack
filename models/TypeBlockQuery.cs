/*
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using WebApplication;
using MySql.Data.MySqlClient;

namespace  WebApplication.Models
{
    public class TypeBlockQuery
    {
        public AppDb Db { get; }

        public TypeBlockQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<TypeBlocks> FindTypeBlock(int id_typeBlock)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_typeBlock`, `TypeBlock` FROM `TypeBlock`";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_typeBlock",
                DbType = DbType.Int32,
                Value = id_typeBlock,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }
  

        public async Task<List<TypeBlocks>> LatestPostsGroups()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_groups`, `name`, `id_class` FROM `ClassStudents` ORDER BY `id_groups` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllGroups()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `ClassStudents`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<TypeBlocks>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<TypeBlocks>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new TypeBlocks(Db)
                    {
                        id_typeBlock = reader.GetInt32(0),
                        TypeBlock = reader.GetString(1),
                        
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}*/
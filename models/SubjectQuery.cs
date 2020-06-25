
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using WebApplication;
using MySql.Data.MySqlClient;

namespace  WebApplication.Models
{
    public class SubjectQuery
    {
        public AppDb Db { get; }

        public SubjectQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Subject> FindOneSubject(int id_subject)
        {
           
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_subject`, `name` FROM `subject` WHERE `id_subject` = @id_subject";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_subject",
                DbType = DbType.Int32,
                Value = id_subject,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Subject>> LatestPostsSubject()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id_subject`, `name` FROM `subject` ORDER BY `name` ASC ";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllSubject()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `subject`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Subject>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Subject>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Subject(Db)
                    {
                        id_subject = reader.GetInt32(0),
                        name = reader.GetString(1),
                        
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
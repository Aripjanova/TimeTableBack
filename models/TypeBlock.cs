using System;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

/*namespace WebApplication.Models
{
    public class TypeBlocks
    {
        public int id_typeBlock { get; set; }
        public string TypeBlock { get; set; }
       
        internal AppDb Db { get; set; }

        public TypeBlocks()
        {
        }

        internal TypeBlocks(AppDb db)
        {
            Db = db;
        }



        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_typeBlock",
                DbType = DbType.Int32,
                Value = id_typeBlock,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@TypeBlock",
                DbType = DbType.String,
                Value = TypeBlock,
            });
           
        }
    }
}*/
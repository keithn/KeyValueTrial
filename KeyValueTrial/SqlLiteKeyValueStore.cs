using System;
using System.Linq;
using Microsoft.Data.Sqlite;
using RepoDb;

namespace KeyValueTrial
{
    class SqlLiteKeyValueStore : IDisposable, IValueStore
    {
        public void Dispose()
        {
            _connection?.Dispose();
        }

        private SqliteConnection _connection;

        public SqlLiteKeyValueStore()
        {
            SqLiteBootstrap.Initialize();
            _connection = new SqliteConnection("Data Source=kv.db");
            _connection.Open();
            
            var result = _connection.ExecuteQuery<int>("select count(*) from sqlite_master where name='kv' and type='table'").ToList();
            Console.WriteLine($"Has kv table?{result.First()}");
            if (result.First() == 0)
            {
                _connection.ExecuteNonQuery("CREATE TABLE kv (Id TEXT primary key, Data BLOB);");
            }
        }

        public void Put(string key, string value)
        {
            _connection.ExecuteNonQuery("INSERT INTO kv(Id, Data) VALUES (@Id, @Data) ON CONFLICT(Id) DO UPDATE SET Data=@Data;", new {Id = key, Data = value});
        }

        public string Get(string key)
        {
            return _connection.ExecuteQuery<string>("SELECT Data from kv where Id=@Id", new {Id = key}).FirstOrDefault();
        }

        public void Clear()
        {
            _connection.ExecuteNonQuery("DELETE FROM kv");
        }
        public void Begin()
        {
            _connection.ExecuteNonQuery("BEGIN");
        }
        public void End()
        {
            _connection.ExecuteNonQuery("END");
        }

       
    }
}
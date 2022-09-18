using BlazorDapperApp.Services.Interfaces;
using Dapper;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace BlazorDapperApp.Services
{
    public class DapperService : IDapperService
    {
        private readonly IConfiguration _config;
        private readonly string _db;

        public DapperService(IConfiguration config)
        {
            _config = config;
            _db = _config.GetConnectionString("DefaultConnection");
        }

        public DbConnection GetConnection()
        {
            return new SqlConnection(_db);
        }

        public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_db);
            return db.Execute(sp, parms, commandType: commandType);
        }

        public T? Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_db);
            return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
        }

        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_db);
            return db.Query<T>(sp, parms, commandType: commandType).ToList();
        }

        public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result; 
            using IDbConnection db = new SqlConnection(_db);
            try
            {
                if(db.State == ConnectionState.Closed) db.Open();
                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
            catch(Exception)
            {
                throw; 
            }
            finally
            {
                if (db.State == ConnectionState.Open) db.Close(); 
            }
            return result; 
        }

        public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_db);
            try
            {
                if (db.State == ConnectionState.Closed) db.Open();
                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open) db.Close();
            }
            return result;
        }
    }
}
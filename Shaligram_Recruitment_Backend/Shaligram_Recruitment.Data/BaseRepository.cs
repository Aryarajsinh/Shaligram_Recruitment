using Shaligram_Recruitment.Model.Config;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using System.Data.SqlClient;

namespace Shaligram_Recruitment.Data
{
    public class BaseRepository
    {
        public readonly IOptions<DataConfig> _ConnectionString;
        public readonly IConfiguration configuration;

        #region
        public BaseRepository(IOptions<DataConfig> connectionString, IConfiguration config = null)
        {
            configuration = config;
        }
        #endregion

        #region SQl Methods
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            string conString = ConfigurationExtensions.GetConnectionString(this.configuration, "DefaultConnection");
            using (SqlConnection _db = new SqlConnection(conString))
            {
                await _db.OpenAsync();
                return await _db.QueryFirstOrDefaultAsync<T>(sql, param, commandType: CommandType.StoredProcedure);
            }

        }

        public async Task<int> ExecuteAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            string conString = ConfigurationExtensions.GetConnectionString(configuration, "DefaultConnection");
            using (SqlConnection _db = new SqlConnection(conString))
            {
                await _db.OpenAsync();
                return await _db.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            string conString = ConfigurationExtensions.GetConnectionString(this.configuration, "DefaultConnection");
            using (SqlConnection con = new SqlConnection(conString))
            {
                await con.OpenAsync();
                return await con.QueryAsync<T>(sql, param, commandType: CommandType.StoredProcedure);
            }
        }
        #endregion
    }
}

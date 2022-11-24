using AdventureWorks.Models;
using Microsoft.Data.SqlClient;

namespace AdventureWorks.Data
{
    public class Repository
    {
        public SqlCommand Cmd = new SqlCommand();
        
        public Repository(IConfiguration configuration)
        {
            var connection = new SqlConnection();
            connection.ConnectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Repository (open connection)");
            }
            Cmd.Connection = connection;
            Cmd.CommandType = System.Data.CommandType.Text;
        }

        internal bool Purge_Log()
        {
            var requete = $@"delete [Logs]";
            try
            {
                Cmd.CommandText = requete;
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, $"Repository.Purge_Log {requete}");
                return false;
            }
            return true;
        }

        internal List<LogData> Get_log()
        {
            var requete = $@"select [Message], Exception, TimeStamp, Level from [Logs] order by [TimeStamp] desc"; // where level='Error'";
            var data = new List<LogData>();
            SqlDataReader rd = null;
            try
            {
                Cmd.CommandText = requete;
                rd = Cmd.ExecuteReader();
                while (rd.Read())
                {
                    data.Add(new LogData
                    {
                        Message = rd.GetString(0),
                        MessageTemplate = rd.GetString(1),
                        TimeStamp = rd.GetDateTime(2),
                        Level = rd.GetString(3)
                    });
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, $"Repository.Get_log {requete}");
            }
            finally
            {
                rd.Close();
            }

            return data;
        }
    }
}

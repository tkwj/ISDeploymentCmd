using System;
using System.Data;
using System.Data.SqlClient;

namespace ISDeploymentCmd
{
    public partial class CatalogProc
    {
        public int DeployProject(string folderName, string projectName, byte[] projectStream)
        {

            using (var connection = new SqlConnection(ConnectionString))
            {
                int folderCreated = CreateFolder(folderName, connection);
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "catalog.deploy_project";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@folder_name", SqlDbType.NVarChar).Value = folderName;
                    cmd.Parameters.Add("@project_name", SqlDbType.NVarChar).Value = projectName;
                    cmd.Parameters.Add("@project_stream", SqlDbType.VarBinary).Value = projectStream;
                    cmd.Parameters.Add("@operation_id", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                    if (connection.State!=ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public int CreateFolder(string folderName, SqlConnection connection = null)
        {
            if (connection.Equals(null))
            {
                connection = new SqlConnection(ConnectionString);
            }
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = "catalog.create_folder";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@folder_name", SqlDbType.NVarChar).Value = folderName;
                cmd.Parameters.Add("@folder_id", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                connection.Open();
                try
                {
                    int rc = Convert.ToInt32(cmd.ExecuteScalar());
                    return rc;
                }
                catch (SqlException SqlEx)
                {
                    // if folder exists error don't error out
                    if (SqlEx.Number== 27190)
                    {
                        return 0;
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
                
            }
           
        }
    }
};

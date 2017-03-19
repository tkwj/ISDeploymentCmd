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
    }
};

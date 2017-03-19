using System;
using System.Data;
using System.Data.SqlClient;

namespace ISDeploymentCmd
{
    public partial class CatalogProc
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public int CreateFolder(string folderName, SqlConnection connection = null)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                Exception ex = new Exception("FolderName must be supplied");
                throw ex;
            }

            if (connection == null)
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
                catch (SqlException sqlEx)
                {
                    // if folder exists error don't error out
                    if (sqlEx.Number == 27190)
                    {
                        return 0;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
           
        }
    }
};

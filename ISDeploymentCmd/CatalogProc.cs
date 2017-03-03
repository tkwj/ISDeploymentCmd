using System;

namespace ISDeploymentCmd
{
    public partial class CatalogProc
    {
        public CatalogProc(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }
    }
}
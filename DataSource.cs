using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IserviceColections_MapWhen
{
    public class DataSource
    {
        private SqlConnectionStringBuilder builder;
        private static DataSource _intance;
        
        protected DataSource()
        {
            builder = new SqlConnectionStringBuilder();
        }

        public static DataSource Intances()
        {
            _intance = _intance ?? new DataSource();

            return _intance;
        }

        public string GetDataSourceSever()
        {
            builder.DataSource = @"DESKTOP-OQFEFQ5";

            builder.InitialCatalog = "Students";

            builder.IntegratedSecurity = true;

            return builder.ConnectionString;
        }

    }
}

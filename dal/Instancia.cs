using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
//using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Client;


namespace dal_oracle
{
    public sealed class Instancia
    {
        static readonly Instancia _instancia = new Instancia();
        static readonly OracleConnection cnn = new OracleConnection();

        static Instancia() { }

        public static Instancia GetInstancia
        {

            get
            {

                return _instancia;
            }


        }

        public OracleConnection GetConnection(string strCnn)
        {
            try
            {
                cnn.ConnectionString = strCnn;
                //cnn.Open();
            }
            catch (Exception e)
            {
                throw e;
            }

            return cnn;

        }

        //public SqlConnection cnn { getPrivilegios; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;


namespace dal_sqlserver
{
    public sealed class Instancia
    {
        static readonly Instancia _instancia = new Instancia();
        static readonly MySqlConnection cnn = new MySqlConnection();

        static Instancia() { }

        public static Instancia GetInstancia
        {

            get
            {

                return _instancia;
            }


        }

        public MySqlConnection GetConnection(string strCnn)
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

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal_sqlserver
{
    public class Helper
    {


        private static SqlConnection getConnection()
        {

            //Obtiene la instacia activa de la conexion y de no existir la crea median patron singleton mejorado

            return Instancia.GetInstancia.GetConnection(ConfigurationManager.ConnectionStrings["SQLConnStr"].ConnectionString);

        }

        private static SqlConnection getConnectionCC()
        {

            //Obtiene la instacia activa de la conexion y de no existir la crea median patron singleton mejorado
            return Instancia.GetInstancia.GetConnection(ConfigurationManager.ConnectionStrings["SQL_LED_ACCESO"].ConnectionString);

        }

        private static string strCnnApp
        {
            //Obtiene la cadena de conexion para la base 
            get { return ConfigurationManager.ConnectionStrings["SQLConnStr"].ConnectionString; }

        }

        private static string strCnnAppCC
        {
            //Obtiene la cadena de conexion para la base 
            get { return ConfigurationManager.ConnectionStrings["SQL_LED_ACCESO"].ConnectionString; }

        }



        /// <summary>
        /// Ejecuta un procedimiento almacenado, comunmente utilizado para insert, update y delete
        /// </summary>
        /// <param name="strNombreSP">Procedimiento almacenado que se desea utilizar</param>
        /// <param name="_parametros">Diccionario con los parametros que se le entregan al metodo. Las key del dictionary deben ser las mismas que se utilizan dentro del SP para identificar el parametro</param>
        /// <returns></returns>
        public static int executeProcedure(string strNombreSP, Dictionary<string, object> _parametros)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = strNombreSP;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnStr"].ConnectionString))
                {
                    cmd.Connection = connection;

                    if (_parametros != null & _parametros.Count > 0)
                    {
                        foreach (KeyValuePair<string, object> value in _parametros)
                        {
                            cmd.Parameters.AddWithValue(value.Key, value.Value);
                        }
                    }

                    cmd.Connection.Open();
                    return cmd.ExecuteNonQuery();
                }


            }
            catch (SqlException ex)
            {


                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);

                if (ex.Number == 2292)
                    throw new Exception("El registro que intenta eliminar está relacionado con otro registro en la base de datos.");
                else
                    throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {


                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;

            }

            finally
            {
                cmd.Connection.Close();

            }
        }

        public static IList<Dictionary<string, object>> getObject(string strNombreSP)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = strNombreSP;
            Dictionary<string, object> _Valor = new Dictionary<string, object>();
            IList<Dictionary<string, object>> listValores = new List<Dictionary<string, object>>();

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnStr"].ConnectionString))
                {
                    cmd.Connection = connection;
                    
                    //cmd.Connection = getConnection();
                    cmd.Connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        _Valor = new Dictionary<string, object>();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            _Valor.Add(dr.GetName(i), dr[i]);
                        }
                        listValores.Add(_Valor);
                    }

                    dr.Close();
                    return listValores;
                }


            }
            catch (SqlException ex)
            {


                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;

            }
            catch (Exception ex)
            {


                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;

            }
           
            finally
            {
                cmd.Connection.Close();

            }
        }

        public static IList<Dictionary<string, object>> getObject(string strNombreSP, Dictionary<string, object> _parametros)
        {

            //int contParametros = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = strNombreSP;
            Dictionary<string, object> _Valor = new Dictionary<string, object>();
            IList<Dictionary<string, object>> listValores = new List<Dictionary<string, object>>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnStr"].ConnectionString))
                {

                    cmd.Connection = connection;

                    if (_parametros != null & _parametros.Count > 0)
                    {
                        //SqlParameter[] _params = new SqlParameter[_parametros.Count];
                        //SqlParameter _param;
                        foreach (KeyValuePair<string, object> value in _parametros)
                        {
                            cmd.Parameters.AddWithValue(value.Key, value.Value);
                        }
                        //cmd.Parameters.AddRange(_params);
                    }

                    cmd.Connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        _Valor = new Dictionary<string, object>();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            _Valor.Add(dr.GetName(i), dr[i]);
                        }
                        listValores.Add(_Valor);
                    }
                    dr.Close();
                    return listValores;



                }

            }
            catch (SqlException ex)
            {


                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();

            }
        }

        /// <summary>
        /// Ejecuta un select y devuelve un dictionary solo key y value solo se debe tener la precaucion que el primer campo del select debe ser ID y el Segundo Value
        /// </summary>
        /// <param name="strNombreSP"></param>
        /// <param name="_parametros"></param>
        /// <returns>Devuelve un dictionary ideal para carga de combos y listas de id y valor</returns>
        public static Dictionary<string, string> getObject_KeyValue(string strNombreSP, Dictionary<string, object> _parametros)
        {

            int contParametros = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = strNombreSP;
            Dictionary<string, string> _Valor = new Dictionary<string, string>();


            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnStr"].ConnectionString))
                {
                    cmd.Connection = connection;

                    if (_parametros != null & _parametros.Count > 0)
                    {
                        SqlParameter[] _params = new SqlParameter[_parametros.Count];
                        SqlParameter _param;
                        foreach (KeyValuePair<string, object> value in _parametros)
                        {
                            _param = new SqlParameter();
                            _param.ParameterName = value.Key;
                            _param.Value = value.Value;
                            _params[contParametros] = _param;

                            contParametros++;
                        }
                        cmd.Parameters.AddRange(_params);
                    }

                    cmd.Connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {

                        _Valor.Add(dr[0].ToString(), dr[1].ToString());

                    }
                    dr.Close();
                    return _Valor;

                }

            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();

            }
        }

        /// <summary>
        /// Ejecuta un select y devuelve un dictionary solo key y value solo se debe tener la precaucion que el primer campo del select debe ser ID y el Segundo Value
        /// </summary>
        /// <param name="strNombreSP"></param>
        /// <returns>Devuelve un dictionary ideal para carga de combos y listas de id y valor</returns>
        public static Dictionary<string, string> getObject_KeyValue(string strNombreSP)
        {


            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = strNombreSP;
            Dictionary<string, string> _Valor = new Dictionary<string, string>();


            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnStr"].ConnectionString))
                {
                    cmd.Connection = connection;



                    cmd.Connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {

                        _Valor.Add(dr[0].ToString(), dr[1].ToString());

                    }
                    dr.Close();
                    return _Valor;
                }



            }
            catch (SqlException ex)
            {


                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();

            }
        }

        /// <summary>
        /// Ejecuta un SP generalmente cuando se requiere devolver solo un valor y es retornado tipo Object
        /// </summary>
        /// <param name="strNombreSP">Nombre de procedimiento almacenado</param>
        /// <param name="_parametros">Dictionary de tipo (string,object) donde se envia el nombre del parametro y el valor en el mismo orden</param>
        /// <returns>Un object con el valor solicitado</returns>
        public static object getScalar(string strNombreSP, Dictionary<string, object> _parametros)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = strNombreSP;

            object result = new object();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnStr"].ConnectionString))
                {
                    cmd.Connection = connection;

                    if (_parametros != null & _parametros.Count > 0)
                    {
                        //OracleParameter[] _params = new OracleParameter[_parametros.Count];
                        //OracleParameter _param;
                        foreach (KeyValuePair<string, object> value in _parametros)
                        {
                            cmd.Parameters.AddWithValue(value.Key, value.Value);
                        }
                        //cmd.Parameters.AddRange(_params);
                    }

                    cmd.Connection.Open();

                    result = cmd.ExecuteScalar();

                    return result;
                }


            }
            catch (SqlException ex)
            {


                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();

            }
        }

        /// <summary>
        /// Ejecuta un SP generalmente cuando se requiere devolver solo un valor y es retornado tipo Object
        /// </summary>
        /// <param name="strNombreSP">Nombre de procedimiento almacenado</param>
        /// <param name="_parametros">Dictionary de tipo (string,object) donde se envia el nombre del parametro y el valor en el mismo orden</param>
        /// <returns>Un object con el valor solicitado</returns>
        public static object getScalar(string strNombreSP, Dictionary<string, object> _parametros, string nombre_parametro_salida)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = strNombreSP;

            object result = new object();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnStr"].ConnectionString))
                {
                    cmd.Connection = connection;

                    if (_parametros != null & _parametros.Count > 0)
                    {
                        //OracleParameter[] _params = new OracleParameter[_parametros.Count];
                        //OracleParameter _param;
                        foreach (KeyValuePair<string, object> value in _parametros)
                        {
                            cmd.Parameters.AddWithValue(value.Key, value.Value);
                        }
                        //cmd.Parameters.AddRange(_params);
                    }

                    cmd.Connection.Open();

                    result = cmd.ExecuteScalar();

                    return result;
                }
            }
            catch (SqlException ex)
            {


                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();

            }
        }

        /// <summary>
        /// Ejecuta un SP generalmente cuando se requiere devolver solo un valor y es retornado tipo Object
        /// </summary>
        /// <param name="strNombreSP">Nombre de procedimiento almacenado</param>
        /// <returns>Un object con el valor solicitado</returns>
        public static object getScalar(string strNombreSP)
        {


            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = strNombreSP;


            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnStr"].ConnectionString))
                {
                    cmd.Connection = connection;
                    cmd.Connection.Open();

                    return cmd.ExecuteScalar();

                }

            }
            catch (SqlException ex)
            {


                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();

            }
        }

        /// <summary>
        /// Ejecuta un Select, devuelve una lista segun el tipo enviado
        /// </summary>
        /// <typeparam name="TEntity">Entidad a la cual desea ser mapeada</typeparam>
        /// <param name="strNombreSP">Nombre del sp</param>
        /// <param name="_parametros">Parametros</param>
        /// <returns></returns>
        public static IList<TEntity> getObject<TEntity>(string strNombreSP, Dictionary<string, object> _parametros) where TEntity : class, new()
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = strNombreSP;
            Dictionary<string, object> _Valor = new Dictionary<string, object>();
            IList<Dictionary<string, object>> listValores = new List<Dictionary<string, object>>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnStr"].ConnectionString))
                {
                    cmd.Connection = connection;

                    if (_parametros != null & _parametros.Count > 0)
                    {
                        //OracleParameter[] _params = new OracleParameter[_parametros.Count];
                        //OracleParameter _param;
                        foreach (KeyValuePair<string, object> value in _parametros)
                        {
                            cmd.Parameters.AddWithValue(value.Key, value.Value);
                        }
                        //cmd.Parameters.AddRange(_params);
                    }

                    cmd.Connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        _Valor = new Dictionary<string, object>();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            _Valor.Add(dr.GetName(i), dr[i]);
                        }
                        listValores.Add(_Valor);
                    }
                    dr.Close();

                    Mapper<TEntity> mapper = new Mapper<TEntity>();
                    IList<TEntity> _list = mapper.Map(listValores);

                    return _list;
                }

            }
            catch (SqlException ex)
            {


                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();

            }
        }

        /// <summary>
        /// Ejecuta un Select, devuelve una lista segun el tipo enviado
        /// </summary>
        /// <typeparam name="TEntity">Entidad a la cual desea ser mapeada</typeparam>
        /// <param name="strNombreSP">Nombre del sp</param>
        /// <param name="_parametros">Parametros</param>
        /// <returns></returns>
        public static TEntity getObject<TEntity>(string strNombreSP, Dictionary<string, object> _parametros, bool isMappingSingle) where TEntity : class, new()
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = strNombreSP;
            Dictionary<string, object> _Valor = new Dictionary<string, object>();
            IList<Dictionary<string, object>> listValores = new List<Dictionary<string, object>>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnStr"].ConnectionString))
                {
                    cmd.Connection = connection;

                    if (_parametros != null & _parametros.Count > 0)
                    {
                        //OracleParameter[] _params = new OracleParameter[_parametros.Count];
                        //OracleParameter _param;
                        foreach (KeyValuePair<string, object> value in _parametros)
                        {
                            //_param = new OracleParameter();
                            //_param.ParameterName = value.Key;
                            //_param.Value = value.Value;
                            //_params[contParametros] = _param;

                            cmd.Parameters.AddWithValue(value.Key, value.Value);

                            //contParametros++;
                        }
                        //cmd.Parameters.Add(_params);
                    }

                    cmd.Connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        _Valor = new Dictionary<string, object>();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            _Valor.Add(dr.GetName(i), dr[i]);
                        }
                        listValores.Add(_Valor);
                    }
                    dr.Close();

                    if (_Valor.Count > 0)
                    {
                        Mapper<TEntity> mapper = new Mapper<TEntity>();
                        TEntity _list = mapper.Map(_Valor, true);
                        return _list;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (SqlException ex)
            {


                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();

            }

        }

        /// <summary>
        /// Ejecuta un Select, devuelve una lista segun el tipo enviado
        /// </summary>
        /// <typeparam name="TEntity">Entidad a la cual desea ser mapeada</typeparam>
        /// <param name="strNombreSP"></param>
        /// <returns>IList TEntity </returns>
        public static IList<TEntity> getObject<TEntity>(string strNombreSP) where TEntity : class, new()
        {


            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = strNombreSP;
            Dictionary<string, object> _Valor = new Dictionary<string, object>();
            IList<Dictionary<string, object>> listValores = new List<Dictionary<string, object>>();

            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnStr"].ConnectionString))
                {
                    cmd.Connection = connection;
                    cmd.Connection.Open();

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        _Valor = new Dictionary<string, object>();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            _Valor.Add(dr.GetName(i), dr[i]);
                        }
                        listValores.Add(_Valor);
                    }
                    dr.Close();


                    Mapper<TEntity> mapper = new Mapper<TEntity>();
                    IList<TEntity> _list = mapper.Map(listValores);


                    return _list;

                }

            }
            catch (SqlException ex)
            {


                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();

            }
        }

        /// <summary>
        /// Ejecuta un Select, devuelve una lista segun el tipo enviado
        /// </summary>
        /// <typeparam name="TEntity">Entidad a la cual desea ser mapeada</typeparam>
        /// <param name="strNombreSP"></param>
        /// <returns>IList TEntity </returns>
        public static IList<TEntity> getObjectCC<TEntity>(string strNombreSP) where TEntity : class, new()
        {


            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = strNombreSP;

            Dictionary<string, object> _Valor = new Dictionary<string, object>();
            IList<Dictionary<string, object>> listValores = new List<Dictionary<string, object>>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnStr"].ConnectionString))
                {
                    cmd.Connection = connection;
                    cmd.Connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        _Valor = new Dictionary<string, object>();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            _Valor.Add(dr.GetName(i), dr[i]);
                        }
                        listValores.Add(_Valor);
                    }
                    dr.Close();


                    Mapper<TEntity> mapper = new Mapper<TEntity>();
                    IList<TEntity> _list = mapper.Map(listValores);


                    return _list;
                }

            }
            catch (SqlException ex)
            {


                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();

            }
        }

        /// <summary>
        /// Ejecuta un Select, devuelve una lista segun el tipo enviado
        /// </summary>
        /// <typeparam name="TEntity">Entidad a la cual desea ser mapeada</typeparam>
        /// <param name="strNombreSP"></param>
        /// <returns>IList TEntity </returns>
        public static TEntity getObject<TEntity>(string strNombreSP, bool isMappingSingle) where TEntity : class, new()
        {


            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = strNombreSP;
            Dictionary<string, object> _Valor = new Dictionary<string, object>();
            //IList<Dictionary<string, object>> listValores = new List<Dictionary<string, object>>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnStr"].ConnectionString))
                {
                    cmd.Connection = connection;
                    cmd.Connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        _Valor = new Dictionary<string, object>();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            _Valor.Add(dr.GetName(i), dr[i]);
                        }
                        //listValores.Add(_Valor);
                    }
                    dr.Close();


                    Mapper<TEntity> mapper = new Mapper<TEntity>();
                    TEntity _list = mapper.Map(_Valor, true);


                    return _list;
                }
            }
            catch (SqlException ex)
            {


                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR, " + ex.Message);
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();

            }
        }


    }
}

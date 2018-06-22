using dal_sqlserver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test_gui.Models
{
    public class TestAccesoDatos
    {
        /// <summary>
        /// Obtiene los datos a traves de parametros en un objeto desde la base de datos, 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntidadTest GetDatos(int id)
        {
            try
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("ID", id);

                //Se envia como parametros el nombre del sp y los parametros de entrada del SP en un diccionario
                //ademas se envia el parametro true para indicarle al metodo que la respuesta es solo un registro
                EntidadTest _retorno = (EntidadTest)Helper.getObject<EntidadTest>("SP_GET_TEST_X_ID", parametros, true);
                return _retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene una lista con todos los datos de la tabla
        /// </summary>
        /// <returns></returns>
        public List<EntidadTest> GetDatos()
        {
            try
            {
                List<EntidadTest> _retorno = (List<EntidadTest>)Helper.getObject<EntidadTest>("SP_GET_TEST");
                return _retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
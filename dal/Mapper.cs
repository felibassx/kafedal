using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dal
{
    public class Mapper<TEntity> where TEntity : class, new()
    {


        public List<TEntity> Map(IList<Dictionary<string, object>> table)
        {
            List<TEntity> entities = new List<TEntity>();
            //var columnNames = table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            var properties = (typeof(TEntity)).GetProperties()
                                              .Where(x => x.GetCustomAttributes(typeof(SourceNamesAttribute), true).Any())
                                              .ToList(); //Solo toma propiedades que tiene SourceNamesAttribute; ignora las demas
            foreach (Dictionary<string, object> row in table)
            {
                TEntity entity = new TEntity();
                foreach (var prop in properties)
                {
                    Map(typeof(TEntity), row, prop, entity);
                }
                entities.Add(entity);
            }

            return entities;
        }

        public TEntity Map(Dictionary<string, object> table, bool isMappingSingle)
        {
            //TEntity entities = new TEntity();
            //var columnNames = table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            var properties = (typeof(TEntity)).GetProperties()
                                              .Where(x => x.GetCustomAttributes(typeof(SourceNamesAttribute), true).Any())
                                              .ToList(); //Solo toma propiedades que tiene SourceNamesAttribute; ignora las demas
                                                         //foreach (Dictionary<string, object> row in table)
                                                         //{
            TEntity entity = new TEntity();
            foreach (var prop in properties)
            {
                Map(typeof(TEntity), table, prop, entity);
            }
            //entities.Add(entity);
            //}

            return entity;
        }

        public void Map(Type type, Dictionary<string, object> row, PropertyInfo prop, object entity)
        {
            List<string> columnNames = MappingHelper.GetSourceNames(type, prop.Name);

            foreach (var columnName in columnNames)
            {
                if (!String.IsNullOrWhiteSpace(columnName))
                {

                    object propertyValue;

                    if (row.TryGetValue(columnName, out propertyValue))
                    {
                        if (propertyValue != DBNull.Value)
                        {
                            MappingHelper.ParsePrimitive(prop, entity, row[columnName]);
                            break; //Asume que el primer match contiene la DATA de la columna
                        }
                    }
                }
            }
        }


    }
}

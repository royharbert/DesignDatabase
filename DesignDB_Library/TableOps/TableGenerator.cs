using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.TableOps
{
    /// <summary>
    /// Creates Data Table from typed list
    /// Useful in User defined table type operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TableGenerator<T>
    { 
        private DataTable _table = new DataTable();

        public List<T> List
        {            
            set 
            {                
                Type T = value[0].GetType();
                List<T> _list = (List<T>)value;
                if (_list.Count > 0)
                {                    
                    PropertyInfo[] properties = T.GetProperties();
                    foreach (var property in properties)
                    {
                        _table.Columns.Add(property.Name, property.PropertyType);
                    }

                    foreach (var item in _list)
                    {
                        DataRow row = _table.NewRow();
                        for (int k = 0; k < properties.Length; k++)
                        {
                            try
                            {
                                row[k]= properties[k].GetValue(item);
                            }
                            catch (Exception)
                            {
                                row[k] = DBNull.Value;
                            }
                        }
                        _table.Rows.Add(row);                 
                    }
                }
            }
        }

        public DataTable table 
        { get 
            {
                return _table;
            } 
        }

    }
}

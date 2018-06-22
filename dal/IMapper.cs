using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal
{
    public interface IMapper<T>
    {
        List<T> Map(DataTable table);
    }
}

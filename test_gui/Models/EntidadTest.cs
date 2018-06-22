using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using dal_sqlserver;

namespace test_gui.Models
{
    public class EntidadTest
    {
        [SourceNames("ID", "ID")]
        public int ID { get; set; }

        [SourceNames("DESCRIPCION", "DESCRIPCION")]
        public string DESCRIPCION { get; set; }
        
    }
}
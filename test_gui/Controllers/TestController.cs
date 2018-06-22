using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace test_gui.Controllers
{
    public class TestController : Controller
    {

        Models.TestAccesoDatos _dal = new Models.TestAccesoDatos();
        // GET: Test
        public ActionResult Index()
        {
            ViewBag.DatosTest = _dal.GetDatos();
            ViewBag.DatosTestPorId = _dal.GetDatos(1);
            return View();
        }
    }
}
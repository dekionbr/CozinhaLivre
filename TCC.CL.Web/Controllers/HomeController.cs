using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC.CL.Core.Business;

namespace TCC.CL.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var model = ReceitaBusiness.ObterLista().Where(x => x.Destaque && x.Parente == null).ToList();
            return View(model);
        }

        public ActionResult Categorias()
        {
            var lista = CategoriaBusiness.ObterListaDeValor();
            return PartialView("_Categorias", lista);
        }
    }
}

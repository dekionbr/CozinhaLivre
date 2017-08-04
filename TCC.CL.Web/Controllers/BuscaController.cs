using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TCC.CL.Core.Business;
using TCC.CL.Core.Entities;
using TCC.CL.Web.Models;
using TCC.CL.Core.Helper;

namespace TCC.CL.Web.Controllers
{
    public class BuscaController : BaseController
    {
        //
        // GET: /Base/

        public virtual ActionResult Index(string q, int pagAtual = 1)
        {
            var Busca = ReceitaBusiness.ObterTodos(pagAtual, q);
            
            BuscaBusiness.AddRange(q, Busca.ToList());

            return View(Busca);
        }

        public virtual ActionResult search(string q)
        {
            IList<Receita> Buscas = ReceitaBusiness.ObterTodos(q);
            IList<ListaReceitas> lista = new List<ListaReceitas>();
            foreach (var item in Buscas)
            {
                lista.Add(new ListaReceitas()
                {
                    Nome = item.Titulo,
                    Resumo = item.Resumo(15).RemoveHtml(),
                    UrlImagem = item.GetUrlImagem(50,50),
                    IsPatrocinado = item.IsPatrocinado,
                    Link = Url.Action("Receita", "Receitas", new { id = item.Id }),
                    Categorias = item.Categorias.Select(x => x.Valor).ToArray()
                });
            }

            var json = new JavaScriptSerializer().Serialize(lista);

            return JavaScript(json);
        }

        public virtual ActionResult Patrocinio(string q)
        {
            var lista = ReceitaBusiness.ObterListaPatrocinada(q);
            return PartialView("_Patrocinio", lista);
        }

    }

}

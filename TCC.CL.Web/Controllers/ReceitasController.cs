using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC.CL.Core.Business;
using TCC.CL.Core.Entities;

namespace TCC.CL.Web.Controllers
{
    public class ReceitasController : BaseController
    {
        //
        // GET: /Receita

        public virtual ActionResult Index()
        {
            return RedirectToAction("Index", "Busca");
        }

        //
        // GET: /Receita

        public virtual ActionResult Receita(int id)
        {
            var modelo = ReceitaBusiness.Obter(id);

            if (modelo != null)
                return View(modelo);
            else
                return RedirectToAction("Index", "Busca");
        }

        //
        //

        [HttpPost]
        public virtual ActionResult IncluirComentario(int id, FormCollection forms)
        {
            Receita receita = ReceitaBusiness.Obter(id);
            var usuario = UsuarioBusiness.Obter(User.Identity.Name);
            
        
            var comentario = new Comentario(receita);

            if (User != null)
            {
                var Autor = AutorBusiness.ObterPorUsuario(usuario.Id);
                comentario.Autor = usuario;
                comentario.NomeAnonimo = Autor.ToString() ?? usuario.Login;
            }
            else {

                comentario.NomeAnonimo = forms["Nome"];
            }

            comentario.Titulo = forms["Titulo"];
            comentario.Conteudo = forms["ConteudoComentario"];

            ComentarioBusiness.Add(comentario);

            return RedirectToAction("Receita", new { id = id });
        }
    }
}

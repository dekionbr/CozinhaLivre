using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC.CL.Core.Business;
using TCC.CL.Core.Entities;

namespace TCC.CL.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = TCC.CL.Core.Seguranca.RoleManager.Funcionario)]
    public class ComentariosController : Controller
    {
        //
        // GET: /Admin/Comentarios/

        public ActionResult Index(int pageindex = 0)
        {
            Paginacao<Comentario> comentarios = ComentarioBusiness.ObterTodos(pageindex);
            return View(comentarios);
        }

        //
        // GET: /Admin/Comentarios/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Admin/Comentarios/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Comentarios/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Comentarios/Editar/5

        public ActionResult Editar(int id)
        {
            var comentario = ComentarioBusiness.Obter(id);

            return View("Edit", comentario);
        }

        //
        // POST: /Admin/Comentarios/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                var comentario = ComentarioBusiness.Obter(id);

                comentario.Titulo = collection["Titulo"];
                comentario.Conteudo = collection["Conteudo"];
                comentario.Ativo = collection["Ativo"] == "on";

                ComentarioBusiness.Update(comentario);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Comentarios/Delete/5

        public ActionResult Delete(int id)
        {
            ComentarioBusiness.Deletar(id);

            return RedirectToAction("Index");
        }

        //
        // POST: /Admin/Comentarios/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Liberar(int id)
        {
            ComentarioBusiness.Ativar(id);

            return RedirectToAction("Index");
        }
    }
}

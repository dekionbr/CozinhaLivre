using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC.CL.Core.Business;
using TCC.CL.Core.Entities;
using TCC.CL.Core.Common;
using TCC.CL.Core.Enumeration;
using TCC.CL.Web.Helpers;
using NHibernate.Linq;
using NHibernate;

namespace TCC.CL.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = TCC.CL.Core.Seguranca.RoleManager.Autor)]
    public class ReceitasController : Controller
    {

        private Autor Autor
        {
            get
            {
                var user = User.ObterUsuario();
                return AutorBusiness.ObterPorUsuario(user.Id);
            }
        }

        //
        // GET: /Admin/Receitas/

        public ActionResult Index(int pagAtual = 1)
        {
            Paginacao<Receita> receitas = ReceitaBusiness.ObterTodos(pagAtual, !User.IsInRole(TCC.CL.Core.Seguranca.RoleManager.Autor));

            return View(receitas);
        }

        public ActionResult MinhasReceitas(int pagAtual = 1)
        {
            Paginacao<Receita> receitas = ReceitaBusiness.ObterTodos(pagAtual, Autor);

            return View(receitas);
        }

        //
        // GET: /Admin/Receitas/Incluir

        public ActionResult Incluir()
        {
            ViewBag.Categorias = CategoriaBusiness.ObterLista();
            ViewBag.Title = "Incluir Receita";
            ViewBag.BtnValue = "Incluir";
            Autor Autor = AutorBusiness.ObterPorUsuario(User.ObterUsuario().Id);
            return View("Edit", new Receita(Autor));
        }

        //
        // POST: /Admin/Receitas/Create


        //
        // GET: /Admin/Receitas/Editar/5

        public ActionResult Editar(int id)
        {
            Receita receita = ReceitaBusiness.Obter(id);

            ViewBag.Title = "Editar Receita";
            ViewBag.BtnValue = "Editar";
            ViewBag.Categorias = CategoriaBusiness.ObterLista();

            Autor autor = AutorBusiness.ObterPorUsuario(User.ObterUsuario().Id);
            if (receita.Autor != autor)
                receita.Imagens = new List<Imagem>();

            return View("Edit", receita);
        }

        //
        // POST: /Admin/Receitas/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateOrEdit(FormCollection collection)
        {
            try
            {
                int id = 0;

                Receita OldReceita = null;
                Receita receita = null;

                if (int.TryParse(collection["Id"], out id) && id != 0)
                {
                    OldReceita = ReceitaBusiness.Obter(id);
                    receita = (Receita)OldReceita.Clone();
                }
                else
                {
                    receita = new Receita();
                }

                receita.Titulo = collection["Titulo"];
                receita.SubTitulo = collection["SubTitulo"];
                receita.Conteudo = collection["Conteudo"];
                receita.Data = DateTime.Parse(collection["Data"]);
                receita.Destaque = collection["Destaque"] == "on";

                if (collection["Categorias"] != null)
                {
                    int[] ids = collection["Categorias"].Split(',').Select(x => int.Parse(x)).ToArray<int>();

                    List<Categoria> categorias = CategoriaBusiness.ObterLista(ids);

                    foreach (var item in categorias)
                    {
                        if (!receita.Categorias.Contains(item))
                            receita.Categorias.Add(item);
                    }
                }
                else
                {
                    receita.Categorias.Clear();
                }

                Autor autor = AutorBusiness.ObterPorUsuario(User.ObterUsuario().Id);
                bool isNovoAutor = receita.Autor != autor;

                //receita.Imagem = collection["foto"];
                receita.Status = receita.Data > DateTime.Now ? autor.IsPatrocinador ? Status.Publicado : Status.Criado : Status.Agendado;
                receita.Autor = autor;

                if ((isNovoAutor) || receita.Avalicoes == null)
                    receita.Avalicoes = new List<Avaliacao>();

                if ((isNovoAutor) || (receita.Imagens == null && Request.Files.Count > 0))
                    receita.Imagens = new List<Imagem>();

                List<string> arquivosSalvos = new List<string>();

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase arquivo = Request.Files[i];

                    //Suas validações ......

                    //Salva o arquivo
                    if (arquivo.ContentLength > 0)
                    {
                        var NAME = receita.Slug + "-" + i + Path.GetExtension(arquivo.FileName);
                        var uploadPath = Server.MapPath("~/Content/Uploads");

                        if (!Directory.Exists(uploadPath))
                            Directory.CreateDirectory(uploadPath);

                        string caminhoArquivo = Path.Combine(@uploadPath, NAME);

                        string caminhoVirtual = string.Format("~/Content/Uploads/{0}", NAME).ToAbsoluteUrl();
                        arquivo.SaveAs(caminhoArquivo);
                        arquivosSalvos.Add(caminhoVirtual);
                        Imagem imagem = new Imagem(caminhoVirtual);
                        receita.AddImagem(imagem);
                    }
                }


                ViewData["Message"] = String.Format("{0} arquivo(s) salvo(s) com sucesso.",
                    arquivosSalvos.Count);

                if (OldReceita == null || isNovoAutor)
                    ReceitaBusiness.Save(receita);
                else
                    ReceitaBusiness.AddParent(receita, OldReceita);

            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }

            return RedirectToAction("Index");
        }


        public ActionResult DeleteImagem(int idImagem)
        {
            bool returno = ReceitaBusiness.DeleteImagem(idImagem);

            if (returno)
                return Json(new { mensagem = "Imagem apagada com sucesso", erro = false });

            return Json(new { mensagem = "Ouve um erro desconhecido", erro = true });
        }

        //
        // GET: /Admin/Receitas/Retornar/5

        public ActionResult Retornar(int id)
        {
            try
            {
                ReceitaBusiness.Retornar(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Erro");
            }
        }

        //
        // GET: /Admin/Receitas/Deletar/5

        public ActionResult Deletar(int id)
        {
            try
            {
                ReceitaBusiness.Deletar(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Erro");
            }
        }

        public ActionResult Categoria(int pagAtual = 1)
        {
            IList<Categoria> categorias = CategoriaBusiness.ObterLista(pagAtual);
            return View(categorias);
        }

        public ActionResult IncluirCategoria(FormCollection collection)
        {
            Categoria cat = new Categoria(collection["Valor"]);

            CategoriaBusiness.Add(cat);

            return RedirectToAction("Categoria");
        }

    }

}

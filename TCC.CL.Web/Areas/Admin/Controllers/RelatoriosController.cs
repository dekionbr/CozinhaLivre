using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCC.CL.Core.Business;
using TCC.CL.Core.DTO;
using TCC.CL.Core.Entities;
using TCC.CL.Core.Enumeration;

namespace TCC.CL.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = TCC.CL.Core.Seguranca.RoleManager.Administrador)]
    public class RelatoriosController : Controller
    {
        //
        // GET: /Admin/Relatorios/

        public ActionResult Index()
        {
            ViewBag.Categorias = RelatoriosBusiness.ObterQuantidadeCategoria();
            return View();
        }

        public ActionResult Receitas(int[] Patrocinadores = null, int[] Autores = null, int Corte = 0)
        {
            Models.AutorModel receitas = new Models.AutorModel()
            {
                Patrocinadores = PessoaJuridicaBusiness.ObterLista(Patrocinadores),
                Autores = AutorBusiness.ObterLista(Autores),
                Corte = (ModeloDeCorteEnum)Corte
            };

            return View(receitas);
        }

        public ActionResult Acessos(int[] usuarioIds = null, int[] navegadores = null, bool inAnonimo = true, int Corte = 0)
        {

            Models.AcessoModel acessos = new Models.AcessoModel()
            {
                Usuarios = UsuarioBusiness.ObterLista(usuarioIds),
                Navegadores = NavegadorEnum.Chrome,
                InAnonimos = inAnonimo,
                Corte = (ModeloDeCorteEnum)Corte
            };

            return View(acessos);
        }

        public ActionResult ReceitasPorBuscas(string[] term = null, bool inBuscas = true, int Corte = 0)
        {
            Models.CategoriaModel categorias = new Models.CategoriaModel()
            {
                Buscas = BuscaBusiness.ObterLista(term),
                InBuscas = true,
                Corte = (ModeloDeCorteEnum)Corte
            };

            return View(categorias);
        }

        #region charts
        
        [HttpPost]
        public ActionResult ObterDadosReceitas(int[] autoresId = null, ModeloDeCorteEnum corte = ModeloDeCorteEnum.Anual, string dataInicial = null, string dataFinal = null, int topItens = 5)
        {
            var lista = RelatoriosBusiness.ObterRelatorioReceitas(corte, autoresId, dataInicial, dataFinal, topItens);
            var json = new JavaScriptSerializer().Serialize(lista);
            return JavaScript(json);
        }

        [HttpPost]
        public ActionResult ObterDadosAcessos(bool inAnonimos, int[] navegadores, string[] usuariosId = null, string dataInicial = null, string dataFinal = null, ModeloDeCorteEnum corte = ModeloDeCorteEnum.Anual, int topItens = 5)
        {
            var lista = RelatoriosBusiness.ObterRelatorioAcessos(inAnonimos, navegadores, usuariosId, dataInicial, dataFinal, corte, topItens);
            var json = new JavaScriptSerializer().Serialize(lista);
            return JavaScript(json);       
        }

        [HttpPost]
        public ActionResult ObterDadosBuscas(bool inBuscas, string[] buscas, ModeloDeCorteEnum corte = ModeloDeCorteEnum.Anual, string dataInicial = null, string dataFinal = null, int topItens = 5)
        {
            var lista = RelatoriosBusiness.ObterRelatorioReceitasPorBuscas(inBuscas, buscas, corte, dataInicial, dataFinal, topItens);
            var json = new JavaScriptSerializer().Serialize(lista);
            return JavaScript(json);
        }
        
        #endregion

        #region Selection Json

        [HttpPost]
        public ActionResult ObterAutores(int[] patrocinadores)
        {
            var autores = RelatoriosBusiness.ObterAutores(patrocinadores).Select(x => x.ID).ToArray();
            var json = new JavaScriptSerializer().Serialize(autores);
            return JavaScript(json);
        }

        #endregion

        #region Export Excel

        public ActionResult AutoresExcel()
        {
            gridExcel(RelatoriosBusiness.ObterAutoresExcel());

            return RedirectToAction("Index");
        }

        public ActionResult FuncionariosExcel()
        {
            gridExcel(RelatoriosBusiness.ObterFuncionariosExcel());

            return RedirectToAction("Index");
        }

        public ActionResult PatrocinadoresExcel()
        {
            gridExcel(RelatoriosBusiness.ObterPatrocinadoresExcel());

            return RedirectToAction("Index");
        }

        private void gridExcel(DataTable source)
        {
            var grid = new GridView();
            grid.DataSource = source;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}ExcelFile.xls", source.TableName));
            Response.ContentType = "application/ms-excel";

            Response.Charset = Encoding.ASCII.EncodingName;
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            
        }

        #endregion
    }
}

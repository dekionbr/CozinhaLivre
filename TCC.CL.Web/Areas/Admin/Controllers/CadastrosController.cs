using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC.CL.Core.Business;
using TCC.CL.Core.Entities;
using TCC.CL.Core.Enumeration;
using TCC.CL.Web.Helpers;

namespace TCC.CL.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = TCC.CL.Core.Seguranca.RoleManager.Autor)]
    public class CadastrosController : Controller
    {
        //
        // GET: /Admin/Cadastros/

        public ActionResult Index()
        {
            return View();
        }

        #region Autor
        //
        // GET: /Admin/Cadastros/Autor/5
        [Authorize(Roles = TCC.CL.Core.Seguranca.RoleManager.Autor)]
        public ActionResult Autor(int id)
        {
            var autor = AutorBusiness.Obter(id);
            if (autor.Telefones.Count == 0)
            {
                autor.Telefones.Add(new Telefone() { Key = "Telefone_Comercial", Value = "" });
                autor.Telefones.Add(new Telefone() { Key = "Telefone_Residencial", Value = "" });
                autor.Telefones.Add(new Telefone() { Key = "Telefone_Pessoal", Value = "" });
            }

            return View(autor);
        }

        [HttpGet]
        [Authorize(Roles = TCC.CL.Core.Seguranca.RoleManager.Autor)]
        public ActionResult IncluirAutor()
        {
            var mode = new Autor();
            mode.Telefones.Add(new Telefone() { Key = "Telefone_Comercial", Value = "" });
            mode.Telefones.Add(new Telefone() { Key = "Telefone_Residencial", Value = "" });
            mode.Telefones.Add(new Telefone() { Key = "Telefone_Pessoal", Value = "" });

            return View("Autor", mode);
        }

        [HttpPost]
        [Authorize(Roles = TCC.CL.Core.Seguranca.RoleManager.Autor)]
        public ActionResult IncluirAutor(int id, FormCollection collection)
        {
            Autor Autor = new Autor();

            try
            {

                if (collection.HasKeys())
                {
                    if (id != 0)
                        Autor = AutorBusiness.Obter(id);

                    Autor.Nome = collection["Nome"];
                    Autor.DataNascimento = DateTime.Parse(collection["DataNascimento"]);
                    Autor.UF = (UF)int.Parse(collection["UF"]);
                    Autor.Cidade = collection["Cidade"];
                    Autor.Bairro = collection["Bairro"];
                    Autor.Logradouro = collection["Logradouro"];
                    Autor.Numero = int.Parse(collection["Numero"]);
                    Autor.Complemento = collection["Complemento"];
                    Autor.Cep = collection["CEP"];
                    Autor.Ativo = false;

                    Autor.Usuario.Grupo = Grupo.Autor;

                    if (!string.IsNullOrEmpty(collection["Usuario.Senha"]) &&
                        !string.IsNullOrEmpty(collection["ConfirmaSenha"]) &&
                        (collection["Usuario.Senha"] == collection["ConfirmaSenha"]))
                    {
                        if (User.IsInRole(TCC.CL.Core.Seguranca.RoleManager.Administrador) || Autor.Id == 0 || User.ObterAutor().Id == Autor.Id)
                            Autor.Usuario.Login = collection["Usuario.Login"];

                        Autor.Usuario.Senha = collection["Usuario.Senha"];
                        Autor.Usuario.EMail = collection["Usuario.EMail"];
                    }

                    foreach (var key in collection.AllKeys.Where(x => x.Contains("Telefone")))
                        if (!string.IsNullOrEmpty(collection[key]))
                            if (Autor.Telefones.Where(x => x.Key == key).Count() > 0)
                                Autor.Telefones.FirstOrDefault(x => x.Key == key).Value = collection[key];
                            else if (!string.IsNullOrEmpty(collection[key]))
                                Autor.Telefones.Add(new Telefone(key, collection[key]));

                    AutorBusiness.Add(Autor);
                }

            }
            catch
            {
                if (Autor.Usuario.Grupo != Grupo.Autor)
                    return RedirectToAction("Autores");
                else
                    return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (Autor.Usuario.Grupo != Grupo.Autor)
                return RedirectToAction("Autores");
            else
                return RedirectToAction("Index", "Home", new { area = "" });
        }

        //
        // GET: /Admin/Cadastros/Autores
        [Authorize(Roles = TCC.CL.Core.Seguranca.RoleManager.Funcionario)]
        public ActionResult Autores(int pagAtual = 1)
        {
            var autores = AutorBusiness.ObterLista(pagAtual);
            return View(autores);
        }

        #endregion


        //
        // GET: /Admin/Cadastros/Funcionario/5
        [Authorize(Roles = TCC.CL.Core.Seguranca.RoleManager.Funcionario)]
        public ActionResult Funcionario(int id)
        {
            var funcionario = FuncionarioBusiness.Obter(id);
            if (funcionario.Telefones.Count == 0)
            {
                funcionario.Telefones.Add(new Telefone() { Key = "Telefone_Residencial", Value = "" });
                funcionario.Telefones.Add(new Telefone() { Key = "Telefone_Pessoal", Value = "" });
            }


            return View(funcionario);
        }

        [HttpGet]
        [Authorize(Roles = TCC.CL.Core.Seguranca.RoleManager.Funcionario)]
        public ActionResult IncluirFuncionario()
        {
            var mode = new Funcionario();
            mode.Telefones.Add(new Telefone() { Key = "Telefone_Residencial", Value = "" });
            mode.Telefones.Add(new Telefone() { Key = "Telefone_Pessoal", Value = "" });

            return View("Funcionario", mode);
        }

        [HttpPost]
        public ActionResult IncluirFuncionario(int id, FormCollection collection)
        {
            Funcionario Funcionario = new Funcionario();
            try
            {
                if (collection.HasKeys())
                {
                    if (id != 0)
                        Funcionario = FuncionarioBusiness.Obter(id);

                    Funcionario.Nome = collection["Nome"];
                    Funcionario.DataNascimento = DateTime.Parse(collection["DataNascimento"]);
                    Funcionario.CPF = collection["CPF"];
                    Funcionario.RG = collection["RG"];

                    if (User.IsInRole(TCC.CL.Core.Seguranca.RoleManager.Administrador))
                    {
                        TipoFuncionarioEnum tipoFuncionario = (TipoFuncionarioEnum)int.Parse(collection["TipoFuncionario"]);
                        Funcionario.TipoFuncionario = tipoFuncionario;
                        Funcionario.Usuario.Grupo = tipoFuncionario == TipoFuncionarioEnum.Administrador ? Grupo.Administrador : Grupo.Funcionario;
                    }

                    Funcionario.UF = (UF)int.Parse(collection["UF"]);
                    Funcionario.Cidade = collection["Cidade"];
                    Funcionario.Bairro = collection["Bairro"];
                    Funcionario.Logradouro = collection["Logradouro"];
                    Funcionario.Numero = int.Parse(collection["Numero"]);
                    Funcionario.Complemento = collection["Complemento"];
                    Funcionario.Cep = collection["CEP"];
                    Funcionario.Ativo = true;

                    if (!string.IsNullOrEmpty(collection["Usuario.Senha"]) &&
                        !string.IsNullOrEmpty(collection["ConfirmaSenha"]) &&
                        (collection["Usuario.Senha"] == collection["ConfirmaSenha"]))
                    {
                        Funcionario.Usuario.Login = collection["Usuario.Login"];
                        Funcionario.Usuario.Senha = collection["Usuario.Senha"];
                    }

                    foreach (var key in collection.AllKeys.Where(x => x.Contains("Telefone")))
                        if (Funcionario.Telefones.Where(x => x.Key.Equals(key)).Count() > 0 && id != 0)
                            Funcionario.Telefones.FirstOrDefault(x => x.Key.Equals(key)).Value = collection[key];
                        else if (!string.IsNullOrEmpty(collection[key]))
                            Funcionario.Telefones.Add(new Telefone(key, collection[key]));

                    FuncionarioBusiness.Add(Funcionario);

                }

            }
            catch
            {
                if (Funcionario.TipoFuncionario != TipoFuncionarioEnum.Moderador)
                    return RedirectToAction("Funcionarios");
                else
                    return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (Funcionario.TipoFuncionario != TipoFuncionarioEnum.Moderador)
                return RedirectToAction("Funcionarios");
            else
                return RedirectToAction("Index", "Home", new { area = "" });
        }

        //
        // GET: /Admin/Cadastros/Funcionarios
        [Authorize(Roles = TCC.CL.Core.Seguranca.RoleManager.Funcionario)]
        public ActionResult Funcionarios(int pagAtual = 1)
        {
            var funcionarios = FuncionarioBusiness.ObterLista(pagAtual);

            return View(funcionarios);
        }

        //
        // GET: /Admin/Cadastros/Patrocinador/5
        [Authorize(Roles = TCC.CL.Core.Seguranca.RoleManager.Funcionario)]
        public ActionResult Patrocinador(int id)
        {
            var patrocinador = PessoaJuridicaBusiness.Obter(id);
            if (patrocinador.Telefones.Count == 0)
            {
                patrocinador.Telefones.Add(new Telefone() { Key = "Telefone_Comercial", Value = "" });
                patrocinador.Telefones.Add(new Telefone() { Key = "Telefone_Fax", Value = "" });
            }

            return View(patrocinador);
        }

        [HttpGet]
        [Authorize(Roles = TCC.CL.Core.Seguranca.RoleManager.Funcionario)]
        public ActionResult IncluirPatrocinador()
        {
            var mode = new PessoaJuridica();
            mode.Telefones.Add(new Telefone() { Key = "Telefone_Comercial", Value = "" });
            mode.Telefones.Add(new Telefone() { Key = "Telefone_Fax", Value = "" });

            return View("Patrocinador", mode);
        }

        [HttpPost]
        public ActionResult IncluirPatrocinador(int id, FormCollection collection)
        {
            try
            {
                PessoaJuridica Patrocinador = new PessoaJuridica();
                if (collection.HasKeys())
                {
                    if (id != 0)
                        Patrocinador = PessoaJuridicaBusiness.Obter(id);

                    Patrocinador.RazaoSocial = collection["RazaoSocial"];
                    Patrocinador.CNPJ = collection["CNPJ"];
                    Patrocinador.NomeFantasia = collection["NomeFantasia"];
                    Patrocinador.UF = (UF)int.Parse(collection["UF"]);
                    Patrocinador.Cidade = collection["Cidade"];
                    Patrocinador.Bairro = collection["Bairro"];
                    Patrocinador.Logradouro = collection["Logradouro"];
                    Patrocinador.Numero = int.Parse(collection["Numero"]);
                    Patrocinador.Complemento = collection["Complemento"];
                    Patrocinador.Cep = collection["CEP"];

                    foreach (var key in collection.AllKeys.Where(x => x.Contains("Telefone")))
                        if (Patrocinador.Telefones.Where(x => x.Key == key).Count() > 0)
                            Patrocinador.Telefones.FirstOrDefault(x => x.Key == key).Value = collection[key];
                        else if (!string.IsNullOrEmpty(collection[key]))
                            Patrocinador.Telefones.Add(new Telefone(key, collection[key]));

                    PessoaJuridicaBusiness.Add(Patrocinador);

                }

            }
            catch
            {
                return RedirectToAction("Patrocinadores");
            }

            return RedirectToAction("Patrocinadores");
        }

        //
        // GET: /Admin/Cadastros/Patrocinadores
        [Authorize(Roles = TCC.CL.Core.Seguranca.RoleManager.Funcionario)]
        public ActionResult Patrocinadores(int pagAtual = 1)
        {
            var patrocinadores = PessoaJuridicaBusiness.ObterLista(pagAtual);
            return View(patrocinadores);
        }

    }
}

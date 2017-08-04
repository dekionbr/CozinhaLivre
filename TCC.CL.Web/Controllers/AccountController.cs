using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TCC.CL.Core.Business;
using TCC.CL.Core.Entities;
using TCC.CL.Core.Enumeration;

namespace TCC.CL.Web.Controllers
{
    public class AccountController : BaseController
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            ViewData["massage"] = TempData["massage"];
            return View();
        }

        public ActionResult LogIn(string Login, string Senha)
        {
            Session.Clear(); Session.Abandon();
            if (UsuarioBusiness.LogIn(Login, Senha))
            {
                var url = FormsAuthentication.GetRedirectUrl(Login, true);
                FormsAuthentication.SetAuthCookie(Login, true);
                return Redirect(url);
            }

            TempData["massage"] = "Login e ou Senha incorretos.";

            return RedirectToAction("Index");
        }

        public ActionResult CadastreSe()
        {
            return View();
        }

        public ActionResult CadastroAutor()
        {
            return PartialView("ParcialCadastreSeAutor", new Autor());
        }

        public ActionResult CadastroPatrocinador()
        {
            return PartialView("ParcialCadastreSePatrocinador", new PessoaJuridica());
        }

        public ActionResult IncluirAutor(FormCollection collection)
        {
            try
            {
                if (collection.HasKeys())
                {
                    var autor = new Autor();
                    autor.Nome = collection["Nome"];
                    autor.DataNascimento = DateTime.Parse(collection["DataNascimento"]);
                    autor.UF = (UF)int.Parse(collection["UF"]);
                    autor.Cidade = collection["Cidade"];
                    autor.Bairro = collection["Bairro"];
                    autor.Logradouro = collection["Logradouro"];
                    autor.Numero = int.Parse(collection["Numero"]);
                    autor.Complemento = collection["Complemento"];
                    autor.Cep = collection["CEP"];
                    autor.Ativo = false;
                    
                    //if (!string.IsNullOrEmpty(collection["TelefoneComercial"]))
                    //    autor.Telefones.Add(new Telefone() { Key = "TelefoneComercial", Value = collection["TelefoneComercial"] });

                    //if (!string.IsNullOrEmpty(collection["TelefonePessoal"]))
                    //    autor.Telefones.Add(new Telefone() { Key = "TelefonePessoal", Value = collection["TelefonePessoal"] });

                    //if (!string.IsNullOrEmpty(collection["TelefoneResidencial"]))
                    //    autor.Telefones.Add(new Telefone() { Key = "TelefoneResidencial", Value = collection["TelefoneResidencial"] });

                    foreach (var key in collection.AllKeys.Where(x => x.Contains("Telefone")))
                        if (autor.Telefones.Where(x => x.Key == key).Count() > 0)
                            autor.Telefones.FirstOrDefault(x => x.Key == key).Value = collection[key];
                        else
                            autor.Telefones.Add(new Telefone(key, collection[key]));

                    autor.Usuario.Grupo = Grupo.Autor;
                    autor.Usuario.Login = collection["Usuario.Login"];
                    autor.Usuario.Senha = collection["Usuario.Senha"];
                    autor.Usuario.EMail = collection["Usuario.EMail"];

                    AutorBusiness.Add(autor);

                    RedirectToAction("SucessoCadastro", autor);

                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Erro", "Index", ex);
            }

            return RedirectToAction("SucessoCadastro");
        }

        public ActionResult SucessoCadastro()
        {
            return View();
        }

        public ActionResult IncluirPatrocinador(FormCollection collection)
        {
            try
            {
                var Patrocinador = new PessoaJuridica();

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

                //if (!string.IsNullOrEmpty(collection["Telefone_Comercial"]))
                //    Patrocinador.Telefones.Add(new Telefone() { Key = "TelefoneComercial", Value = collection["TelefoneComercial"] });

                //if (!string.IsNullOrEmpty(collection["TelefonePessoal"]))
                //    Patrocinador.Telefones.Add(new Telefone() { Key = "TelefonePessoal", Value = collection["TelefonePessoal"] });

                //if (!string.IsNullOrEmpty(collection["TelefoneResidencial"]))
                //    Patrocinador.Telefones.Add(new Telefone() { Key = "TelefoneResidencial", Value = collection["TelefoneResidencial"] });

                foreach (var key in collection.AllKeys.Where(x => x.Contains("Telefone")))
                    if (Patrocinador.Telefones.Where(x => x.Key == key).Count() > 0)
                        Patrocinador.Telefones.FirstOrDefault(x => x.Key == key).Value = collection[key];
                    else
                        Patrocinador.Telefones.Add(new Telefone(key, collection[key]));

                var Autor = new Autor();
                Autor.Nome = collection["Nome"];

                if (!string.IsNullOrEmpty(collection["Telefone_Autor"]))
                    Autor.Telefones.Add(new Telefone() { Key = "Telefone_Pessoal", Value = Telefone.LimparCaracteresEspeciais(collection["Telefone_Autor"]) });
                Autor.Usuario.Grupo = Grupo.AutorPatrocinado;
                Autor.Usuario.Login = collection["Login"];
                Autor.Usuario.Senha = collection["Senha"];
                Autor.Usuario.EMail = collection["Email"];

                if (!string.IsNullOrEmpty(collection["TelefoneResidencial"]))
                    Autor.Empresa = Patrocinador;

                Patrocinador.Autores.Add(Autor);

                PessoaJuridicaBusiness.Add(Patrocinador);

            }
            catch (Exception ex)
            {
                return RedirectToAction("Erro", "Index", ex);
            }

            return RedirectToAction("SucessoCadastro");
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear(); Session.Abandon();
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
    }
}

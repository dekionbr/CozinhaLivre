using System;
using System.Collections.Generic;
using System.Linq;
using TCC.CL.Core.Enumeration;
using NHibernate.Linq;
using TCC.CL.Core.Validators;
using FluentValidation.Attributes;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;
using TCC.CL.Core.Helper;

namespace TCC.CL.Core.Entities
{

    [Serializable]
    public class Receita : BaseDomain<Receita, int>
    {
        public Receita()
        {
            this.Categorias = new List<Categoria>();
            this.Avalicoes = new List<Avaliacao>();
        }

        public Receita(Autor autor)
        {
            this.Categorias = new List<Categoria>();
            this.Avalicoes = new List<Avaliacao>();
            this.Autor = autor;
        }

        private IList<Imagem> imagens = new List<Imagem>();

        public virtual string Titulo { get; set; }
        public virtual string SubTitulo { get; set; }
        public virtual string Conteudo { get; set; }
        public virtual string ConteudoResumido { get; set; }
        public virtual IList<Categoria> Categorias { get; set; }
        public virtual Autor Autor { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual Status Status { get; set; }
        public virtual IList<Avaliacao> Avalicoes { get; set; }
        public virtual Receita Parente { get; set; }
        public virtual IList<Comentario> Comentarios { get; set; }
        public virtual bool Destaque { get; set; }
        public virtual int Audiencia { get; set; }
        public virtual IList<Imagem> Imagens
        {
            get
            {
                return imagens;
            }
            set
            {
                imagens = value;
            }
        }

        public virtual bool IsPatrocinado { get { return Autor.IsPatrocinador; } }

        public virtual string NomeAutor { get { return Autor.Nome; } }

        public virtual string GetUrlImagem(int largura, int altura, string url = null, int imageId = 0)
        {
            if (imagens.Count() > 0)
            {
                var image = imagens.FirstOrDefault(x => x.Id == imageId) ?? imagens.First();

                if (image != null)
                    return string.Format("{0}?w={1}&h={2}", image.Url, largura, altura);
            }

            return string.Format("//placehold.it/{0}x{1}", largura, altura);
        }

        public virtual string Slug
        {
            get
            {
                StringBuilder sbReturn = new StringBuilder();
                var arrayText = Titulo.Normalize(NormalizationForm.FormD).ToCharArray();
                foreach (char letter in arrayText)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                        sbReturn.Append(letter);
                }

                var TituloSemAcento = sbReturn.ToString();

                string pattern = @"(?i)[^0-9a-záéíóúàèìòùâêîôûãõç\s]";
                Regex rgx = new Regex(pattern);
                return rgx.Replace(TituloSemAcento, "").ToLower().Replace(" ", "-");
            }
        }

        public virtual string Resumo(int wordsCount = 15)
        {
            if (string.IsNullOrEmpty(ConteudoResumido))
            {
                string[] words = Conteudo.RemoveHtml().Split(' ');
                return string.Join(" ", words.Take(wordsCount).ToArray()).ToString();
            }
            else
            {
                return ConteudoResumido;
            }
        }

        public virtual IEnumerable<int> CategoriasIds { get { return Categorias.Select(x => x.Id); } }

        public virtual void AddImagem(Imagem imagem)
        {
            imagem.Receita = this;
            imagens.Add(imagem);
        }

        public virtual Receita Clone()
        {
            Receita receita = DeepClone(this);
            receita.Id = 0;

            return receita;
        }
    }
}

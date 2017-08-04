using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.CL.Core.Entities
{
    [Serializable]
    public class Comentario : BaseDomain<Comentario, int>
    {
        public Comentario()
        {
            this.Data = DateTime.Today;
        }

        public Comentario(Receita receita)
        {
            this.Receita = receita;
            this.Data = DateTime.Today;
        }

        public Comentario(Receita receita, Autor autor)
        {
            this.Receita = receita;
            this.Autor = Autor;
            this.Data = DateTime.Today;
        }

        public virtual Usuario Autor { get; set; }
        public virtual Receita Receita { get; set; }
        public virtual string NomeAnonimo { get; set; }
        public virtual string Titulo { get; set; }
        public virtual string Conteudo { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual Comentario Parente { get; set; }
        public virtual IList<Comentario> Filhos { get; set; }

        public virtual bool Ativo { get; set; }
    }
}

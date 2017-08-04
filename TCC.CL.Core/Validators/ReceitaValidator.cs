using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.CL.Core.Entities;

namespace TCC.CL.Core.Validators
{
    public class ReceitaValidator<T> : AbstractValidator<T> where T : Receita
    {
        public ReceitaValidator()
        {
            RuleFor(x => x.Conteudo)
                .Length(0, 5000)
                .WithMessage("Conteúdo não pode estar vazio e tem um limite de 5000 letras");

            RuleFor(x => x.Titulo)
                .Length(3, 60)
                .WithMessage("Minimo de letras no título é 3 e o máximo é 60");

            Custom(receita => { 
                string[] palavras = receita.ConteudoResumido.Split(' ');
                return palavras.Length > 15 ? new ValidationFailure("Resumo", "O Máximo de palavras para o campo resumo é 15") : null;
            });
        }
    }
}

using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.CL.Core.Entities;

namespace TCC.CL.Core.Validators
{
    public class UsuarioValidator<T> : AbstractValidator<T> where T : Usuario
    {
        public UsuarioValidator() {
            RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage("Campo login não pode ser vazio");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Campo senha não pode ser vazio");
        
        }
    }
}

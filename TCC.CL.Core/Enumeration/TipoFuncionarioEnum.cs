using System;
using System.ComponentModel;


namespace TCC.CL.Core.Enumeration
{
    public enum TipoFuncionarioEnum
    {
        [Description("Administrador do Sistema")]
        Administrador = 1,
        [Description("Moderador do Site")]
        Moderador = 2
    }
}

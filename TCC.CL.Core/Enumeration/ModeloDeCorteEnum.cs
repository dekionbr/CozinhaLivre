using System;
using System.ComponentModel;

namespace TCC.CL.Core.Enumeration
{
    public enum ModeloDeCorteEnum : int
    {
        [Description("Diário")]
        Diario,
        [Description("Semanal")]
        Semanal,
        [Description("Mensal")]
        Mensal,
        [Description("Trimestral")]
        Trimestral,
        [Description("Semestral")]
        Semestral,
        [Description("Anual")]
        Anual
    }    
}

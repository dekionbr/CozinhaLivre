using System;
using System.ComponentModel;

namespace TCC.CL.Core.Enumeration
{
    public enum NavegadorEnum
    {
        [Description("Chrome")]
        Chrome,
        [Description("FireFox")]
        FireFox,
        [Description("IE 11")]
        IE11,
        [Description("Microsoft Edge")]
        Edge,
        [Description("Netscape")]
        Netscape,
        [Description("Opera")]
        Opera,
        [Description("Safari")]
        Safari,
        [Description("Outros")]
        Outros
    }
}

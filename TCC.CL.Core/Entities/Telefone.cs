using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TCC.CL.Core.Entities
{
    [Serializable]
    public class Telefone : BaseDomain<Telefone, int>
    {
        private static Telefone _this;

        public Telefone()
        {
            _this = this;
        }

        public Telefone(string key, string value)
        {
            // TODO: Complete member initialization
            this.Key = key;
            this.Value = LimparCaracteresEspeciais(value);
        }

        public virtual string Key { get; set; }

        public virtual string Value { get; set; }

        public static string ObterTelefone()
        {
            var r = Regex.Match(_this.Value, @"^\([1-9]{2}\) (?:[2-8][0-9]|9[1-9])[0-9]{2,3}\-[0-9]{4}$");

            if (r.Success)
                return r.Value;

            return _this.Value;
        }

        public static void InserirTelefone(string value)
        {
            _this.Value = LimparCaracteresEspeciais(value);
        }

        public virtual string ObterTelefoneFormatado() {
            return ObterTelefone();
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.CL.Core.Helper
{
    public static class EnumHelper
    {
        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">Enum Type</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>Retorna a descrição do Enum</returns>
        /// <example>string desc = myEnumVariable.GetDescription<myEnum>();</example>
        public static string GetDescription(this object objVal)
        {
            var type = objVal.GetType();
            var memInfo = type.GetMember(objVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute),
                false);

            return ((DescriptionAttribute)attributes[0]).Description;
        }

        public static Int32 ToInt(this Enum enumVal) 
        {
            return Convert.ToInt32(enumVal);
        }
    }
}

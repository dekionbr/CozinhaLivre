using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI.WebControls;
using TCC.CL.Core.Entities;
using TCC.CL.Core.Helper;

namespace TCC.CL.Web.Helpers
{
    public static class Html
    {
        public static MvcHtmlString RenderHtml(this HtmlHelper html, string source)
        {
            var sb = new StringBuilder();

            sb.Append(source);

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString RemoveHtml(this HtmlHelper html, string source)
        {
            return MvcHtmlString.Create(source.RemoveHtml());
        }

        public static MvcHtmlString Paginacao<T>(this HtmlHelper html, Paginacao<T> itens)
        {
            var sb = new StringBuilder();
            sb.Append("<ul class=\"pagination text-center\" role=\"navigation\" aria-label=\"Pagination\">");
            int inicio = 1;
            if (itens.PaginaAtual - 4 > inicio)
                inicio = itens.PaginaAtual - 4;

            if (inicio > 1)
            {
                sb.Append("<li class=\"pagination-previous\">");
                CriaItemPagina(sb, 1, "Previous", true, "page");
                sb.Append("</li>");
            }

            if (inicio + 8 > itens.TotalPaginas && itens.TotalPaginas - 8 > 1)
                inicio = itens.TotalPaginas - 8;

            for (var i = inicio; i <= inicio + 8 && i <= itens.TotalPaginas; i++)
            {
                if (i == itens.PaginaAtual)
                    sb.Append("<li class=\"current\">");
                else
                    sb.Append("<li>");

                CriaItemPagina(sb, i, i.ToString(), i == itens.PaginaAtual, "You're on page");
                sb.Append("</li>");
            }

            if (inicio + 8 < itens.TotalPaginas)
            {
                sb.Append("<li class=\"pagination-next\">");
                CriaItemPagina(sb, itens.TotalPaginas, "Next", true, "page");
                sb.Append("</li>");
            }

            sb.Append("</ul>");
            return MvcHtmlString.Create(sb.ToString());
        }

        private static void CriaItemPagina(StringBuilder sb, int indice, string label, bool atual = false, string labelspan = "")
        {
            var pagina = new TagBuilder("a");

            var span = new TagBuilder("span");
            span.SetInnerText(labelspan);
            span.MergeAttribute("class", "show-for-sr");

            if (atual)
                pagina.InnerHtml = span.ToString() + " " + label;
            else
                pagina.InnerHtml = label;

            pagina.MergeAttribute("href", "?pagAtual=" + indice.ToString());
            pagina.MergeAttribute("aria-label", "pagina " + indice.ToString());



            sb.Append(pagina.ToString());
        }

        public static MvcHtmlString DropDownEnumList<TEnum>(this HtmlHelper html, string nome, TEnum MyEnum) where TEnum : struct, IConvertible
        {
            return DropDownEnumList(html, nome, MyEnum);
        }

        public static MvcHtmlString DropDownEnumList<TEnum>(this HtmlHelper html, string nome, TEnum MyEnum, object htmlAttributes) where TEnum : struct, IConvertible
        {
            IList<SelectListItem> valores = new List<SelectListItem>();

            foreach (TEnum item in Enum.GetValues(typeof(TEnum)))
                valores.Add(new SelectListItem { Text = item.GetDescription(), Value = Convert.ToInt32(item).ToString(), Selected = item.Equals(MyEnum) });


            return html.DropDownList(nome, valores, "Selecione", htmlAttributes);

        }

        public static MvcHtmlString DropDownEnumListFor<TModel, TValue, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression,
                                                    TEnum MyEnum) where TEnum : struct, IConvertible
        {
            return DropDownEnumListFor<TModel, TValue, TEnum>(html, expression, MyEnum, null);
        }

        public static MvcHtmlString DropDownEnumListFor<TModel, TValue, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression,
                                                    TEnum MyEnum, object htmlAttributes) where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("é preciso ser um enumerado para utilizar esse componente");
            }

           var itens = from enumValue in Enum.GetValues(typeof(TEnum)).Cast<object>()
                       select new SelectListItem
                       {
                        Text = enumValue.GetDescription(),
                        Value = ((int)enumValue).ToString(),
                        Selected = enumValue.Equals(MyEnum)
                       };

            return html.DropDownListFor(expression, itens, "Selecione", htmlAttributes);
        }

        private static IEnumerable<object> ObtemListaEnum<TEnum>() where TEnum : struct, IConvertible
        {
            foreach (TEnum x in Enum.GetValues(typeof(TEnum)))
                yield return new { Text = x.GetDescription(), Value = Convert.ToInt32(x).ToString() };
        }
    }
}
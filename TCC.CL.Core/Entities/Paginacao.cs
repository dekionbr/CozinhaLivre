using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.CL.Core.Entities
{
    public class Paginacao<T> : IList<T>
    {
        public Paginacao(int paginaAtual, int total, IList<T> itens)
        {
            if (itens == null)
                throw new ArgumentNullException("itens");
            this.PaginaAtual = paginaAtual;
            this.TotalItens = total;
            this.Itens = itens;
        }

        public int PaginaAtual { get; private set; }
        public int TotalItens { get; private set; }
        public int TotalPaginas
        {
            get
            {
                if (TotalItens <= 0)
                    return 1;
                else
                {
                    return (int)Math.Ceiling((double)TotalItens / PaginacaoExtensions.TamanhoPagina);
                }
            }
        }

        private IList<T> Itens { get; set; }

        public int IndexOf(T item)
        {
            return Itens.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            Itens.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Itens.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return Itens[index];
            }
            set
            {
                Itens[index] = value;
            }
        }

        public void Add(T item)
        {
            Itens.Add(item);
        }

        public void Clear()
        {
            Itens.Clear();
        }

        public bool Contains(T item)
        {
            return Itens.Contains<T>(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Itens.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return Itens.Count; }
        }

        public bool IsReadOnly
        {
            get { return Itens.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            return Itens.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Itens.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Itens.GetEnumerator();
        }
    }

    public static class PaginacaoExtensions
    {
        public const int TamanhoPagina = 10;
        public static Paginacao<T> ToListPaginado<T>(this IQueryable<T> query, int paginaAtual)
        {
            if (paginaAtual <= 0)
                return new Paginacao<T>(0, -1, query.ToList<T>());
            else
            {
                var count =
                    query.Count();

                var itens = query
                        .Skip((paginaAtual - 1) * TamanhoPagina)
                        .Take(TamanhoPagina)
                        .ToList<T>();

                return new Paginacao<T>(paginaAtual, count, itens);
            }

        }
    }
}

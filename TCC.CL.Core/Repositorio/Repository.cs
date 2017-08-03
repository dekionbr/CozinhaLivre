using System;
using System.Linq;
using System.Configuration;
using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using TCC.CL.Core.Infraestrutura;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace TCC.CL.Core.Repositorio
{
    public class Repository<TKey, T> : IPersistRepository<T>,
       IReadOnlyRepository<TKey, T> where T : class, IEntityKey<TKey>
    {
        protected readonly ISession _session;

        public Repository(ISession session)
        {
            _session = session;
        }

        public bool Add(T entity)
        {
            _session.Save(entity);
            return true;
        }

        public TKey Save(T entity)
        {
            _session.Save(entity);
            return entity.Id;
        }

        public bool Add(System.Collections.Generic.IEnumerable<T> itens)
        {
            foreach (T item in itens)
            {
                _session.Save(item);
            }
            return true;
        }

        public bool Update(T entity)
        {
            _session.Update(entity);
            return true;
        }

        public bool Updates(System.Collections.Generic.IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                _session.Update(entity);
            }
            return true;
        }

        public bool AddOrUpdate(T entity)
        {
            _session.SaveOrUpdate(entity);
            return true;
        }

        public bool Delete(T entity)
        {
            _session.Delete(entity);
            return true;
        }

        public bool Delete(System.Collections.Generic.IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                _session.Delete(entity);
            }
            return true;
        }

        public bool Delete(TKey Id)
        {
            T entity = FindBy(Id);
            if (!entity.Equals(null))
            {
                _session.Delete(entity);
                return true;
            }
            return false;
        }

        private ICriteria SetCriteria()
        {
            return _session.CreateCriteria<T>();
        }

        private ICriteria SetPropertiesBack(params string[] properties)
        {

            ProjectionList projects = Projections.ProjectionList();

            for (int i = 0; i < properties.Length; i++)
            {
                projects.Add(Projections.Property(properties[i]), properties[i]);
            }

            return SetCriteria().SetProjection(projects);

        }

        
        public IQueryable<T> PropertiesBack(int skipFilds, int maxFilds, System.Linq.Expressions.Expression<Func<T, bool>> expression, params string[] properties)
        {
            return SetPropertiesBack(properties)
                            .Add(Restrictions.Where(expression))
                            .SetFirstResult(skipFilds)
                            .SetMaxResults(maxFilds)
                            .SetResultTransformer(Transformers.AliasToBean<T>())
                            .Future<T>().AsQueryable();
        }

        public IQueryable<T> PropertiesBack(System.Linq.Expressions.Expression<Func<T, bool>> expression, params string[] properties)
        {
            return PropertiesBack(0, int.MaxValue, expression, properties);
        }

        public IQueryable<T> PropertiesBack(params string[] properties)
        {
            return PropertiesBack(null, properties);
        }

        public IQueryable<T> All()
        {
            return _session.Query<T>();
        }

        public virtual T FindBy(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return FilterBy(expression).First();
        }

        public virtual T FindBy(TKey id)
        {
            return _session.Get<T>(id);
        }

        public IQueryable<T> FilterBy(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return All().Where(expression).AsQueryable();
        }     

        public void Commit()
        {
            if (!_session.Transaction.IsActive)
            {
                throw new InvalidOperationException("Transação não foi iniciada");
            }
            _session.Transaction.Commit();
        }

        public void RollBack()
        {
            if (!_session.Transaction.IsActive)
            {
                throw new InvalidOperationException("Transação não foi iniciada");
            }
            _session.Transaction.Rollback();
        }
    }
}

using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using TCC.CL.Core.Map;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Dialect;
using System.Security;
using System.Security.Permissions;
using NHibernate.Cache;
using TCC.CL.Core.Entities;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using TCC.CL.Core.Map;

namespace TCC.CL.Core
{

    public static class Session
    {
        private static ISessionFactory currentFactory;
        private static ISessionFactory CurrentFactory
        {
            get
            {
                if (currentFactory == null || currentFactory.IsClosed)
                {
                    CreateSessionFactory();
                }
                return currentFactory;
            }
        }

        public static bool IsClosed()
        {
            return CurrentFactory.IsClosed;
        }

        public static void CreateSessionFactory()
        {
            try
            {

                var connStrKey = "DefaultConnection";

                currentFactory = Fluently.Configure()
                        .Database(
                            MySQLConfiguration.Standard.Dialect<MySQL5Dialect>().ConnectionString(
                                    c => c.FromConnectionStringWithKey(connStrKey)
                                )
                                .ShowSql()
                                .Raw("transaction-isolation", "READCOMMITTED")
                        )
                        .Cache(x => x.UseQueryCache().ProviderClass<HashtableCacheProvider>())
                        .Mappings(m => m.FluentMappings
                            .AddFromAssemblyOf<UsuarioMap>()
                            .Conventions.AddFromAssemblyOf<UsuarioMap>()
                    //.ExportTo(@"C:\map")

                        )
                        .ExposeConfiguration(c =>
                        {
                            c.SetProperty("hbm2ddl.keywords", "auto-quote");
                            c.SetProperty("current_session_context_class", "web");
                            c.SetProperty("command_timeout", (TimeSpan.FromMinutes(10).TotalSeconds).ToString());
                            c.SetProperty("cache.use_query_cache", "true");
                        })
                       .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                       .BuildSessionFactory();

                //var cfg = new Configuration();
                ////new SchemaExport(cfg).Execute(false, true, false, false);

                //currentFactory = cfg.Configure()                
                //                    .BuildSessionFactory();

                //.AddAssembly(typeof(Avaliacao).Assembly)
                //.AddAssembly(typeof(Autor).Assembly)
                //.AddAssembly(typeof(Categoria).Assembly)
                //.AddAssembly(typeof(Comentario).Assembly)
                //.AddAssembly(typeof(Funcionario).Assembly)
                //.AddAssembly(typeof(Pessoa).Assembly)
                //.AddAssembly(typeof(PessoaFisica).Assembly)
                //.AddAssembly(typeof(PessoaJuridica).Assembly)
                //.AddAssembly(typeof(Receita).Assembly)
                //.AddAssembly(typeof(Telefone).Assembly)
                //.AddAssembly(typeof(Usuario).Assembly)




            }
            catch (HibernateConfigException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal static ISession Current
        {
            get
            {
                //if (currentSession == null || !currentSession.IsConnected || !currentSession.IsOpen)
                //    currentSession = currentFactory.OpenSession();
                //return currentSession;

                if (!CurrentSessionContext.HasBind(CurrentFactory))
                {
                    CurrentSessionContext.Bind(CurrentFactory.OpenSession());
                }

                return CurrentFactory.GetCurrentSession();

            }
        }

        public static void DisposeSession()
        {
            var session = CurrentSessionContext.Unbind(CurrentFactory);
            if (session != null)
            {
                if (session.IsConnected && session.Connection != null)
                    session.Connection.Close();
                if (session.IsOpen)
                    session.Close();
                session.Dispose();
            }
        }

        public static ISession OpenSession()
        {
            return CurrentFactory.OpenSession();
        }
    }
}

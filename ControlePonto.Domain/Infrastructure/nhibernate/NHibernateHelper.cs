using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using ControlePonto.Domain.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.nhibernate
{
    public class NHibernateHelper
    {
        private const string connectionString = "Server=localhost;Database=db_ponto_artplas;User ID=root;Password=;";
                
        private static FluentConfiguration _fluentConfiguration;
        private static ISessionFactory _sessionFactory;        

        public static FluentConfiguration getFluentConfiguration()
        {
            if (_fluentConfiguration == null)
            {
                _fluentConfiguration = Fluently.Configure()
                    .Database(
                        MySQLConfiguration.Standard.ConnectionString(connectionString)
                    )
                    .Mappings(
                        m => m.FluentMappings.AddFromAssemblyOf<ControlePonto.Infrastructure.nhibernate.mapping.UsuarioMap>()
                    );
            }
            return _fluentConfiguration;
        }

        public static ISessionFactory getSessionFactory()
        {
            if (_sessionFactory == null)
            {
                _sessionFactory = getFluentConfiguration()
                    .BuildSessionFactory();
            }
            return _sessionFactory;
        }

        public static ISession openSession()
        {
            return getSessionFactory().OpenSession();
        }        
    }
}

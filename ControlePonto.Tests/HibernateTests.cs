using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlePonto.Infrastructure.nhibernate;
using FluentNHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace ControlePonto.Tests
{
    [TestClass]
    public class HibernateTests
    {
        [TestMethod]
        [TestCategory("Hibernate")]
        public void testCriarBancoDados()
        {
            FluentConfiguration config = NHibernateHelper.getFluentConfiguration();

            config.ExposeConfiguration(
                              c => new SchemaExport(c).Execute(true, true, false))
                         .BuildConfiguration();
        }
    }
}

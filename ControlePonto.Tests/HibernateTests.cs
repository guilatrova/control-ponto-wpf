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
        public void criarBancoDados()
        {
            FluentConfiguration config = NHibernateHelper.getFluentConfiguration();

            config.ExposeConfiguration(
                              c => new SchemaExport(c).Execute(true, true, false))
                         .BuildConfiguration();
        }

        [TestMethod]
        public void hibernatePodeConfigurarHostDoBancoDados()
        {            
            Assert.AreEqual("localhost", NHibernateHelper.Host);

            NHibernateHelper.Host = "127.0.0.1";
            Assert.AreEqual("127.0.0.1", NHibernateHelper.Host);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidHostException))]
        public void hibernateDeveValidarHost()
        {
            NHibernateHelper.Host = "192.168.0.5";            
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void hibernateNaoDevePermitirTrocaDeHost()
        {
            //Obrigatório reiniciar aplicação
            NHibernateHelper.Host = "127.0.0.1";
            NHibernateHelper.Host = "localhost";
        }
    }
}

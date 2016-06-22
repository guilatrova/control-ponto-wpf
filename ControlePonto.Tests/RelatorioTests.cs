using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Domain.services.relatorio;
using ControlePonto.Tests.mocks.repository;
using ControlePonto.Domain.ponto.folga;
using ControlePonto.Tests.mocks;

namespace ControlePonto.Tests
{
    [TestClass]
    public class RelatorioTests
    {
        private IPontoDiaRepository pontoRepository;
        private Funcionario funcionario;

        [TestInitialize]
        public void setUp()
        {
            pontoRepository = new PontoDiaMockRepository();
            funcionario = new FuncionarioFactory().criarFuncionario("Gui", "gui", "123456", "", "41617099864");
        }

        #region Helper Methods

        private DiaFolga criarFolgaEm(IPontoDiaRepository repository, int dia, int mes, int ano)
        {
            return criarFolgaEm(repository, new DateTime(ano, mes, dia));
        }

        private DiaFolga criarFolgaEm(IPontoDiaRepository repository, DateTime data)
        {
            var service = (PontoServiceMock) FactoryHelper.criarPontoService(funcionario, null, repository, true);

            return
                service.darFolgaPara(funcionario, data, "desc");
        }

        #endregion

        [TestMethod]
        public void relatorioDeveGerarCalendario()
        {
            var relatorio = new RelatorioService(pontoRepository);
            var inicio = new DateTime(2016, 6, 1);
            var fim = new DateTime(2016, 6, 30);
            var calendario = relatorio.gerarCalendario(funcionario, inicio, fim);

            Assert.AreEqual(funcionario, calendario.Funcionario);
            Assert.AreEqual(inicio, calendario.PeriodoInicio);
            Assert.AreEqual(fim, calendario.PeriodoFim);
            Assert.AreEqual(30, calendario.Dias.Count);
        }

        [TestMethod]
        public void relatorioDeveContarDiasFolga()
        {
            var repository = new PontoDiaMockRepository();
            var relatorio = new RelatorioService(repository);
            var inicio = new DateTime(2016, 6, 1);
            var fim = new DateTime(2016, 6, 30);            
            var dataFolga1 = new DateTime(2016, 6, 2);
            var dataFolga2 = new DateTime(2016, 6, 18);

            var folga1 = criarFolgaEm(repository, dataFolga1);
            var folga2 = criarFolgaEm(repository, dataFolga2);
            var calendario = relatorio.gerarCalendario(funcionario, inicio, fim);

            var folgasNoPeriodo = calendario.Dias.Where(x => x.TipoDia == ETipoDiaCalendario.FOLGA).ToList();            

            Assert.AreEqual(2, folgasNoPeriodo.Count());
            Assert.AreEqual(dataFolga1, folgasNoPeriodo[0].Data);
            Assert.AreEqual(dataFolga2, folgasNoPeriodo[1].Data);
        }
    }
}

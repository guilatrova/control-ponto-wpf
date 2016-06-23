using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Domain.services.relatorio;
using ControlePonto.Tests.mocks.repository;
using ControlePonto.Domain.ponto.folga;
using ControlePonto.Tests.mocks;
using ControlePonto.Domain.feriado;
using ControlePonto.Infrastructure.utils;

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

        private Feriado criarFeriadoEm(string nome, int dia, int mes, int ano)
        {
            var factory = new FeriadoFactory();
            return factory.criarFeriadoEspecifico(nome, dia, mes, ano);
        }

        private RelatorioService criarRelatorioService(IPontoDiaRepository pontoRepository = null, IFeriadoRepository feriadoRepository = null)
        {
            if (pontoRepository == null)
                pontoRepository = new PontoDiaMockRepository();

            if (feriadoRepository == null)
                feriadoRepository = new FeriadoMockRepository();

            return new RelatorioService(pontoRepository, new FeriadoService(feriadoRepository));
        }

        #endregion

        [TestMethod]
        public void relatorioDeveGerarCalendario()
        {
            var relatorio = criarRelatorioService();
            var inicio = new DateTime(2016, 6, 1);
            var fim = new DateTime(2016, 6, 30);
            var calendario = relatorio.gerarCalendario(funcionario, inicio, fim);

            Assert.AreEqual(funcionario, calendario.Funcionario);
            Assert.AreEqual(inicio, calendario.PeriodoInicio);
            Assert.AreEqual(fim, calendario.PeriodoFim);
            Assert.AreEqual(30, calendario.Dias.Count);
        }

        [TestMethod]
        public void relatorioDeveRetornarDiasFolga()
        {
            var repository = new PontoDiaMockRepository();
            var relatorio = criarRelatorioService(repository);
            var inicio = new DateTime(2016, 6, 1);
            var fim = new DateTime(2016, 6, 30);            
            var dataFolga1 = new DateTime(2016, 6, 2);
            var dataFolga2 = new DateTime(2016, 6, 18);

            var folga1 = criarFolgaEm(repository, dataFolga1);
            var folga2 = criarFolgaEm(repository, dataFolga2);
            var calendario = relatorio.gerarCalendario(funcionario, inicio, fim);

            var folgasNoPeriodo = calendario.getFolgas();                

            Assert.AreEqual(2, folgasNoPeriodo.Count);
            Assert.AreEqual(dataFolga1, folgasNoPeriodo[0].Data);
            Assert.AreEqual(dataFolga2, folgasNoPeriodo[1].Data);
            Assert.AreEqual(30, calendario.Dias.Count);
        }

        [TestMethod]
        public void relatorioDeveRetornarFeriados()
        {
            var feriadoRepository = new FeriadoMockRepository();
            var nomeFeriado = "Dia de festa";
            var feriado = criarFeriadoEm(nomeFeriado, 1, 6, 2016);
            var relatorio = criarRelatorioService(null, feriadoRepository);
            var inicio = new DateTime(2016, 6, 1);
            var fim = new DateTime(2016, 6, 30);   

            feriadoRepository.save(feriado);
            var calendario = relatorio.gerarCalendario(funcionario, inicio, fim);

            var feriadosNoPeriodo = calendario.getFeriados();

            Assert.AreEqual(1, feriadosNoPeriodo.Count);
            Assert.AreEqual(nomeFeriado, feriadosNoPeriodo[0].Nome);
            Assert.AreEqual(new DateTime(2016, 6, 1), feriadosNoPeriodo[0].Data);
            Assert.AreEqual(30, calendario.Dias.Count);
        }

        [TestMethod]
        public void relatorioDeveRetornarFeriadosTrabalhados()
        {
            //Arranging: Feriado
            var feriadoRepository = new FeriadoMockRepository();            
            var nomeFeriado = "Dia de festa";
            var feriado = criarFeriadoEm(nomeFeriado, 1, 6, 2016);

            //Arranging: Dia de trabalho
            var pontoRepository = new PontoDiaMockRepository();
            var horarioEntrada = new DateTime(2016, 6, 1, 9, 0, 0);
            var horarioSaida = new DateTime(2016, 6, 1, 18, 0, 0);
            var horarios = new DataHoraMockListStrategy(
                horarioEntrada.Date,
                horarioEntrada,
                horarioSaida);
            var pontoService = FactoryHelper.criarPontoService(funcionario, new DataHoraMockStrategy(1, 6, 2016), pontoRepository);
            var dia = pontoService.iniciarDia();
            pontoService.encerrarDia(dia);

            //Arranging: Relatório Service
            var relatorio = criarRelatorioService(pontoRepository, feriadoRepository);
            var inicio = new DateTime(2016, 6, 1);
            var fim = new DateTime(2016, 6, 30);

            //Act
            feriadoRepository.save(feriado);
            var calendario = relatorio.gerarCalendario(funcionario, inicio, fim);
            var feriadosNoPeriodo = calendario.getFeriados();
            var pontosNoPeriodo = calendario.getPontos();

            //Assert
            Assert.AreEqual(30, calendario.Dias.Count);

            Assert.AreEqual(1, feriadosNoPeriodo.Count);
            Assert.AreEqual(nomeFeriado, feriadosNoPeriodo[0].Nome);
            Assert.AreEqual(new DateTime(2016, 6, 1), feriadosNoPeriodo[0].Data);            

            Assert.AreEqual(1, pontosNoPeriodo.Count);
            //Assert.AreEqual(horarioEntrada.TimeOfDay, pontosNoPeriodo[0].Entrada);
            //Assert.AreEqual(horarioSaida.TimeOfDay, pontosNoPeriodo[0].Saida);
            Assert.AreEqual(new TimeSpan(9, 0, 0), pontosNoPeriodo[0].PontoDia.calcularHorasTrabalhadas());
        }
    }
}

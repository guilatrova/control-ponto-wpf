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
using ControlePonto.Domain.ponto.trabalho;
using System.Collections.Generic;
using ControlePonto.Domain.jornada;

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

        private Feriado criarFeriadoEm(IFeriadoRepository repository, string nome, int dia, int mes, int ano)
        {
            var factory = new FeriadoFactory();
            var feriado = factory.criarFeriadoEspecifico(nome, dia, mes, ano);

            repository.save(feriado);
            return feriado;
        }

        private RelatorioService criarRelatorioService(IPontoDiaRepository pontoRepository = null, IFeriadoRepository feriadoRepository = null, IJornadaTrabalhoRepository jornadaRepository = null)
        {
            if (pontoRepository == null)
                pontoRepository = new PontoDiaMockRepository();

            if (feriadoRepository == null)
                feriadoRepository = new FeriadoMockRepository();

            if (jornadaRepository == null)
                jornadaRepository = new JornadaTrabalhoMockRepository();

            return new RelatorioService(pontoRepository, new FeriadoService(feriadoRepository), jornadaRepository);
        }

        private DiaTrabalho criarPontoTrabalhoDoDia(IPontoDiaRepository pontoRepository, IFeriadoRepository feriadoRepository, int dia, int mes, int ano, int hora = 9, int minuto = 0)
        {
            var dt = new DataHoraMockStrategy(new DateTime(ano, mes, dia, hora, minuto, 0));
            return FactoryHelper
                .criarPontoFactory(pontoRepository, feriadoRepository)
                .criarDiaTrabalho(dt, new SessaoLoginMock(funcionario));
        }

        private PontoService criarPontoService(IPontoDiaRepository pontoRepository, IFeriadoRepository feriadoRepository, int dia, int mes, int ano, int hora)
        {
            return criarPontoService(pontoRepository, feriadoRepository, new DataHoraMockStrategy(dia, mes, ano, hora));
        }

        private PontoService criarPontoService(IPontoDiaRepository pontoRepository, IFeriadoRepository feriadoRepository, IDataHoraStrategy dataHora)
        {
            return
                FactoryHelper.criarPontoService(funcionario, dataHora, pontoRepository, false, feriadoRepository);
        }

        private JornadaTrabalho criarJornadaTrabalho(IJornadaTrabalhoRepository jornadaRepository)
        {
            return
                new JornadaTrabalhoFactory(jornadaRepository).criarJornadaTrabalho();
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
            var feriado = criarFeriadoEm(feriadoRepository, nomeFeriado, 1, 6, 2016);
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
            var feriado = criarFeriadoEm(feriadoRepository, nomeFeriado, 1, 6, 2016);

            //Arranging: Dia de trabalho
            var pontoRepository = new PontoDiaMockRepository();
            var dia = criarPontoTrabalhoDoDia(pontoRepository, feriadoRepository, 1, 6, 2016, 9); //Horário de entrada
            var pontoService = criarPontoService(pontoRepository, feriadoRepository, 1, 6, 2016, 18); //Horário de saída
            pontoService.encerrarDia(dia);

            //Arranging: Relatório Service
            var relatorio = criarRelatorioService(pontoRepository, feriadoRepository);
            var inicio = new DateTime(2016, 6, 1);
            var fim = new DateTime(2016, 6, 30);

            //Act            
            var calendario = relatorio.gerarCalendario(funcionario, inicio, fim);
            var feriadosNoPeriodo = calendario.getFeriados();
            var pontosNoPeriodo = calendario.getDiasTrabalhados();

            //Assert
            Assert.AreEqual(30, calendario.Dias.Count);

            Assert.AreEqual(1, feriadosNoPeriodo.Count);
            Assert.AreEqual(nomeFeriado, feriadosNoPeriodo[0].Nome);
            Assert.AreEqual(new DateTime(2016, 6, 1), feriadosNoPeriodo[0].Data);            

            Assert.AreEqual(1, pontosNoPeriodo.Count);
            Assert.AreEqual(new TimeSpan(9, 0, 0), (pontosNoPeriodo[0].PontoDia as DiaTrabalho).Inicio);
            Assert.AreEqual(new TimeSpan(18, 0, 0), (pontosNoPeriodo[0].PontoDia as DiaTrabalho).Fim);
            Assert.AreEqual(new TimeSpan(9, 0, 0), pontosNoPeriodo[0].PontoDia.calcularHorasTrabalhadas());
        }

        [TestMethod]
        public void relatorioDeveContarHorasExtrasSeparandoPorValor()
        {
            #region Arranging: Feriado

            var feriadoRepository = new FeriadoMockRepository();
            var nomeFeriado = "Dia de festa";
            var feriado = criarFeriadoEm(feriadoRepository, nomeFeriado, 1, 6, 2016);

            #endregion

            #region Arranging: Dias de trabalho

            var pontoRepository = new PontoDiaMockRepository();
            var horariosEntrada = new DataHoraMockListStrategy(
                new DateTime(2016, 6, 1, 10, 0, 0), //Feriado     -> 100%
                new DateTime(2016, 6, 2, 10, 0, 0), //Quinta      -> 75%
                new DateTime(2016, 6, 3, 10, 0, 0), //Sexta       -> 75%
                new DateTime(2016, 6, 4, 10, 0, 0), //Sábado      -> 75%
                new DateTime(2016, 6, 5, 10, 0, 0));//Domingo     -> 100%
            var horariosSaida = new DataHoraMockListStrategy(true,
                new DateTime(2016, 6, 1, 18, 0, 0), //Feriado     -> 100%
                new DateTime(2016, 6, 2, 18, 0, 0), //Quinta      -> 75%
                new DateTime(2016, 6, 3, 18, 0, 0), //Sexta       -> 75%
                new DateTime(2016, 6, 4, 18, 0, 0), //Sábado      -> 75%
                new DateTime(2016, 6, 5, 18, 0, 0));//Domingo     -> 100%

            var diasDeTrabalho = new List<DiaTrabalho>();            
            while(horariosEntrada.Count > 0)
            {
                diasDeTrabalho.Add(
                    iniciarEncerrarDia(pontoRepository, feriadoRepository, horariosEntrada.getDataHoraAtual(), horariosSaida));
                horariosSaida.dequeue();
            }

            #endregion

            #region Arranging: Jornada de trabalho

            var jornadaRepository = new JornadaTrabalhoMockRepository();
            var jornada = criarJornadaTrabalho(jornadaRepository);
            jornada.cadastrarDia(DayOfWeek.Monday, DayOfWeek.Friday, new TimeSpan(10, 0, 0), new TimeSpan(15, 0, 0), new TimeSpan(0, 0, 0));
            jornadaRepository.save(jornada);

            #endregion

            //Arranging: Relatório Service
            var relatorio = criarRelatorioService(pontoRepository, feriadoRepository, jornadaRepository);
            var inicio = new DateTime(2016, 6, 1);
            var fim = new DateTime(2016, 6, 30);

            //Act            
            var calendario = relatorio.gerarCalendario(funcionario, inicio, fim);
            var feriadosNoPeriodo = calendario.getFeriados();
            var diasTrabalhadosNoPeriodo = calendario.getDiasTrabalhados();

            //Assert
            Assert.AreEqual(30, calendario.Dias.Count);

            Assert.AreEqual(1, feriadosNoPeriodo.Count);
            Assert.AreEqual(nomeFeriado, feriadosNoPeriodo[0].Nome);
            Assert.AreEqual(new DateTime(2016, 6, 1), feriadosNoPeriodo[0].Data);

            Assert.AreEqual(5, diasTrabalhadosNoPeriodo.Count);            
            Assert.AreEqual(new TimeSpan(25, 0, 0), calendario.calcularHorasExtras());
            Assert.AreEqual(new TimeSpan(14, 0, 0), calendario.calcularHorasExtras(75));
            Assert.AreEqual(new TimeSpan(11, 0, 0), calendario.calcularHorasExtras(100));
        }

        private DiaTrabalho iniciarEncerrarDia(IPontoDiaRepository pontoRepository, IFeriadoRepository feriadoRepository, DateTime entrada, IDataHoraStrategy dataHora)
        {
            var diaTrab = criarPontoTrabalhoDoDia(pontoRepository, feriadoRepository, entrada.Day, entrada.Month, entrada.Year, entrada.Hour);
            criarPontoService(pontoRepository, feriadoRepository, dataHora).encerrarDia(diaTrab);
            return diaTrab;
        }
    }
}

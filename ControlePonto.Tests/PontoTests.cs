using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.jornada;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.services.ponto;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Infrastructure.utils;
using ControlePonto.Tests.mocks;
using ControlePonto.Tests.mocks.repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Tests
{
    [TestClass]
    public class PontoTests
    {
        private PontoFactory factory;
        private TipoIntervaloFactory tipoIntervaloFactory;
        private TipoIntervalo tipoAlmoco;
        private SessaoLogin sessaoLogin;
        private Funcionario funcionario;

        [TestInitialize]
        public void setUp()
        {
            funcionario = new FuncionarioFactory().criarFuncionario("Jhon Doe", "doe", "123456", "", "41617099864");
            sessaoLogin = new SessaoLoginMock(funcionario);            
            factory = new PontoFactory();
            tipoIntervaloFactory = new TipoIntervaloFactory(new NomeIntervaloJaExisteSpecification(new TipoIntervaloMockRepository()));
            tipoAlmoco = tipoIntervaloFactory.criarTipoIntervalo("ALMOÇO");
        }

        private PontoDia criarPontoDoDia(int dia, int mes, int ano, int hora = 9, int minuto = 0)
        {
            var dt = new DataHoraMockStrategy(new DateTime(ano, mes, dia, hora, minuto, 0));
            return factory.criarPonto(dt, sessaoLogin);
        }

        private PontoService criarService(IDataHoraStrategy dataHoraStrategy, IPontoDiaRepository repository = null, Funcionario logado = null)
        {
            var sessao = sessaoLogin;
            if (logado != null)
                sessao = new SessaoLoginMock(logado);

            if (repository == null)
                repository = new PontoDiaMockRepository();

            return new PontoService(factory,
                dataHoraStrategy, 
                new FuncionarioPossuiPontoAbertoSpecification(repository),
                new FuncionarioJaTrabalhouHojeSpecification(repository),
                sessao,
                repository);
        }

        private JornadaTrabalho criarJornada()
        {
            return new JornadaTrabalhoFactory(new JornadaTrabalhoMockRepository()).criarJornadaTrabalho();
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void pontoSoDeveSerCriadoPelaFactory()
        {
            var ponto = factory.criarPonto(new DataHoraMockStrategy(22, 8, 2014), sessaoLogin);
            Assert.IsNotNull(ponto);

            ponto = new PontoDia(new DateTime(2014, 8, 22), new TimeSpan(19, 30, 0), null);
        }

        [TestMethod]
        public void pontoDeveSerCriadoComDataHoraCorreta()
        {
            var esperado = new DataHoraMockStrategy(22, 8, 2014, 19, 30);
            var ponto = factory.criarPonto(esperado, sessaoLogin);

            Assert.AreEqual(new DateTime(2014, 8, 22, 0, 0, 0), ponto.Data);
            Assert.AreEqual(new TimeSpan(19, 30, 0), ponto.Inicio);
        }

        [TestMethod]
        public void pontoDeveSerEncerradoComDataHoraCorreta()
        {
            var fimEsperado = new DataHoraMockStrategy(new DateTime(2014, 8, 22, 22, 30, 0));
            var service = criarService(fimEsperado);

            var ponto = service.iniciarDia();
            service.encerrarDia(ponto);

            Assert.AreEqual(new TimeSpan(22, 30, 0), ponto.Fim);
        }

        [TestMethod]
        public void pontoDeveSerCriadoAssociadoAoFuncionario()
        {
            var ponto = factory.criarPonto(new DataHoraMockStrategy(22, 8, 2016), sessaoLogin);

            Assert.AreEqual(funcionario, ponto.Usuario);
        }

        [TestMethod]
        public void pontoDeveContabilizarIntervalos()
        {
            var entradaAlmoco = new DateTime(2014, 8, 22, 12, 30, 0);
            var saidaAlmoco = new DateTime(2014, 8, 22, 13, 30, 0);
            var entradaLanche = new DateTime(2014, 8, 22, 16, 0, 0);
            var saidaLanche = new DateTime(2014, 8, 22, 16, 15, 0);            
            var tipoLanche = tipoIntervaloFactory.criarTipoIntervalo("LANCHE");
            var dtMock = new DataHoraMockListStrategy(
                    new DateTime(2014, 8, 22, 9, 0, 0),
                    entradaAlmoco,
                    saidaAlmoco,
                    entradaLanche,
                    saidaLanche);

            var ponto = factory.criarPonto(dtMock, sessaoLogin);

            ponto.registrarIntervalo(tipoAlmoco, dtMock);
            ponto.registrarIntervalo(tipoAlmoco, dtMock);
            ponto.registrarIntervalo(tipoLanche, dtMock);
            ponto.registrarIntervalo(tipoLanche, dtMock);
            
            Assert.AreEqual(entradaAlmoco.TimeOfDay, ponto.getIntervalo(tipoAlmoco).Entrada);
            Assert.AreEqual(saidaAlmoco.TimeOfDay, ponto.getIntervalo(tipoAlmoco).Saida);
            Assert.AreEqual(entradaLanche.TimeOfDay, ponto.getIntervalo(tipoLanche).Entrada);
            Assert.AreEqual(saidaLanche.TimeOfDay, ponto.getIntervalo(tipoLanche).Saida);
        }

        [TestMethod]
        [ExpectedException(typeof(IntervaloEmAbertoException))]
        public void diaNaoDeveEncerrarComPontoAberto()
        {
            var inicioDia = new DateTime(2014, 8, 22, 9, 0, 0);
            var entradaAlmoco = new DateTime(2014, 8, 22, 12, 30, 0);
            var encerramentoDia = new DateTime(2014, 8, 22, 15, 0, 0);
            
            var dtMock = new DataHoraMockListStrategy(
                    inicioDia,
                    entradaAlmoco,
                    encerramentoDia);

            var service = criarService(dtMock);
            var ponto = service.iniciarDia();

            ponto.registrarIntervalo(tipoAlmoco, dtMock);
            service.encerrarDia(ponto);            
        }

        [TestMethod]
        [ExpectedException(typeof(DiaEmAbertoException))]
        public void diaNaoDeveIniciarSeHouverPontoAbertoEmDiasAnteriores()
        {
            var pontoAntigo = criarPontoDoDia(10, 6, 2016);
            var repositorio = new PontoDiaMockRepository();

            repositorio.save(pontoAntigo);

            //Vou simular o dia de hoje
            var service = criarService(new DataHoraMockStrategy(13, 6, 2016), repositorio);
            var pontoHoje = service.iniciarDia();
        }

        [TestMethod]
        public void diaPodeIniciarSeFuncionarioNaoPossuirPontoEmAberto()
        {
            var funcionarioCorreto = new FuncionarioFactory().criarFuncionario("Thais", "tatacs", "123456", "", "41617099864");
            var pontoAntigo = criarPontoDoDia(10, 6, 2016);
            var repositorio = new PontoDiaMockRepository();
            var inicioDoDia = new DataHoraMockStrategy(13, 6, 2016);

            repositorio.save(pontoAntigo);

            try
            {
                var service = criarService(inicioDoDia, repositorio);
                service.iniciarDia();
                Assert.Fail();
            }
            catch (DiaEmAbertoException ex)
            {
                Assert.AreEqual("O ponto do dia 10/06/2016 não foi encerrado", ex.Message);
            }

            var serviceCorreto = criarService(inicioDoDia, repositorio, funcionarioCorreto);
            var ponto = serviceCorreto.iniciarDia();

            Assert.AreEqual(new DateTime(2016, 6, 13), ponto.Data.Date);
        }

        [TestMethod]
        [ExpectedException(typeof(PontoDiaJaExisteException))]
        public void funcionarioNaoPodeTerDoisPontosParaMesmoDia()
        {
            var rep = new PontoDiaMockRepository();
            var mesmoDia = new DataHoraMockStrategy(22, 8, 2014);
            var service = criarService(mesmoDia, rep);

            var ponto = service.iniciarDia();
            service.encerrarDia(ponto);

            rep.save(ponto);

            service.iniciarDia();
        }

        [TestMethod]
        [ExpectedException(typeof(IntervaloJaRegistradoException))]
        public void pontoNaoDeveRegistrarIntervalosRepetidos()
        {
            var saidaAlmoco = new DateTime(2014, 8, 22, 12, 30, 0);
            var horarios = new DataHoraMockListStrategy(
                new DateTime(2014, 8, 22, 9, 0, 0), //Inicio do dia
                new DateTime(2014, 8, 22, 12, 30, 0), //Entrada almoço
                saidaAlmoco,
                new DateTime(2014, 8, 22, 17, 00, 0) //Horário de saída/erro
            );
            var service = criarService(horarios);

            var ponto = service.iniciarDia();
            ponto.registrarIntervalo(tipoAlmoco, horarios);
            ponto.registrarIntervalo(tipoAlmoco, horarios);
            ponto.registrarIntervalo(tipoAlmoco, horarios);
        }

        [TestMethod]
        [ExpectedException(typeof(IntervaloNaoRegistradoException))]
        public void pontoDeveAlertarIntervalosNaoRegistradosQuandoForSolicitadoPeloIntervalo()
        {
            var service = criarService(new DataHoraMockStrategy(22, 8, 2014));
            var ponto = service.iniciarDia();

            ponto.getIntervalo(tipoAlmoco);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void saidaDoIntervaloNaoDeveSerAlterado()
        {
            var entradaAlmoco = new DateTime(2014, 8, 22, 12, 30, 0);
            var saidaAlmoco = new DateTime(2014, 8, 22, 12, 30, 0);
            var horarios = new DataHoraMockListStrategy(
                new DateTime(2014, 8, 22, 9, 0, 0), //Inicio do dia
                entradaAlmoco,
                saidaAlmoco,
                new DateTime(2014, 8, 22, 17, 00, 0) //Horário de saída/erro
            );
            var service = criarService(horarios);

            var ponto = service.iniciarDia();
            ponto.registrarIntervalo(tipoAlmoco, horarios);
            ponto.registrarIntervalo(tipoAlmoco, horarios);

            var intervalo = ponto.getIntervalo(tipoAlmoco);
            intervalo.Saida = entradaAlmoco.TimeOfDay;
        }

        [TestMethod]
        public void pontoDeveCalcularHorasExtras()
        {
            var ponto = criarPontoDoDia(22, 8, 2014);                                           //09:00 - INÍCIO
            ponto.registrarIntervalo(tipoAlmoco, new DataHoraMockStrategy(22, 8, 2014, 12, 15));//12:15 - ALMOÇO
            ponto.registrarIntervalo(tipoAlmoco, new DataHoraMockStrategy(22, 8, 2014, 13, 00));//13:00 - SAÍDA ALMOÇO
            criarService(new DataHoraMockStrategy(22, 8, 2014, 19, 00)).encerrarDia(ponto);     //19:00 - FIM

            var jornada = criarJornada();
            jornada.cadastrarDia(DayOfWeek.Sunday, DayOfWeek.Saturday, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0), new TimeSpan(1, 0, 0));

            /* DESCRIÇÃO | ENTRADA | SAÍDA | DURAÇÃO | TOTAL
             * TRABALHO  |  09:00  | 19:00 | 10:00   | 10:00
             * ALMOÇO    |  12:15  | 13:00 | 00:45   | 09:15
             *                               EXTRA   | 01:15 (Jornada configurada para 8 horas) */            
            Assert.AreEqual(new TimeSpan(1, 15, 0), ponto.calcularHorasExtras(jornada));
        }

        [TestMethod]
        [ExpectedException(typeof(DiaEmAbertoException))]
        public void pontoNaoDeveCalcularHorasExtrasQuandoEstiverAberto()
        {
            var ponto = criarPontoDoDia(22, 8, 2014);                                           //09:00 - INÍCIO
            ponto.registrarIntervalo(tipoAlmoco, new DataHoraMockStrategy(22, 8, 2014, 12, 15));//12:15 - ALMOÇO

            var jornada = criarJornada();
            jornada.cadastrarDia(DayOfWeek.Sunday, DayOfWeek.Saturday, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0), new TimeSpan(1, 0, 0));
            ponto.calcularHorasExtras(jornada);
        }

        [TestMethod]
        public void pontoDeveCalcularHorasExtrasDoDiaCorreto()
        {
            var ponto = criarPontoDoDia(11, 6, 2016, 10, 0); //Sábado
            ponto.registrarIntervalo(tipoAlmoco, new DataHoraMockStrategy(22, 8, 2014, 12, 00));
            ponto.registrarIntervalo(tipoAlmoco, new DataHoraMockStrategy(22, 8, 2014, 12, 30));
            criarService(new DataHoraMockStrategy(22, 8, 2014, 19, 00)).encerrarDia(ponto);

            var jornada = criarJornada();
            //Configurado para 6 horas (7 horas - 1 hora de folga)
            jornada.cadastrarDia(DayOfWeek.Saturday, new TimeSpan(10, 0, 0), new TimeSpan(17, 0, 0), new TimeSpan(1, 0, 0));
            Assert.AreEqual(new TimeSpan(2, 30, 0), ponto.calcularHorasExtras(jornada));
        }

        [TestMethod]
        public void pontoDeveCalcularHorasTrabalhadas()
        {
            var ponto = criarPontoDoDia(22, 8, 2014);
            ponto.registrarIntervalo(tipoAlmoco, new DataHoraMockStrategy(22, 8, 2014, 12));
            ponto.registrarIntervalo(tipoAlmoco, new DataHoraMockStrategy(22, 8, 2014, 13));
            criarService(new DataHoraMockStrategy(22, 8, 2014, 18)).encerrarDia(ponto);

            Assert.AreEqual(new TimeSpan(8, 0, 0), ponto.calcularHorasTrabalhadas());
        }

        [TestMethod]
        public void pontoDeveCalcularHorasExtrasEmDiasDeFolga()
        {
            var ponto = criarPontoDoDia(12, 6, 2016, 10, 0); //Domingo
            criarService(new DataHoraMockStrategy(12, 6, 2016, 12, 00)).encerrarDia(ponto);

            var jornada = criarJornada();
            Assert.AreEqual(new TimeSpan(2, 0, 0), ponto.calcularHorasExtras(jornada));
            Assert.AreEqual(new TimeSpan(2, 0, 0), ponto.calcularHorasTrabalhadas());
        }

        [TestMethod]
        [ExpectedException(typeof(IntervaloEmAbertoException))]
        public void pontoNaoDeveEntrarEmDuasPausasAoMesmoTempo()
        {
            var dia = new DateTime(2014, 8, 22, 9, 0, 0);
            var entradaAlmoco = new DateTime(2014, 8, 22, 12, 00, 0);
            var entradaLanche = new DateTime(2014, 8, 22, 12, 30, 0);
            var horarios = new DataHoraMockListStrategy(
                dia,
                dia,
                entradaAlmoco,
                entradaLanche               
            );
            var service = criarService(horarios);

            var ponto = service.iniciarDia();
            ponto.registrarIntervalo(tipoAlmoco, horarios);
            ponto.registrarIntervalo(tipoIntervaloFactory.criarTipoIntervalo("LANCHE"), horarios);
        }
    }
}

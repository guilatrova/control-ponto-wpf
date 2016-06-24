using ControlePonto.Domain.factories;
using ControlePonto.Domain.factories.services;
using ControlePonto.Domain.feriado;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.services.relatorio;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.WPF.window.ponto.folga;

namespace ControlePonto.WPF.window.ponto
{
    public class PontoWindowFactory
    {
        private static RelatorioService criarRelatorioService()
        {
            return new RelatorioService(
                RepositoryFactory.criarPontoRepository(),
                new FeriadoService(RepositoryFactory.criarFeriadoRepository()),
                RepositoryFactory.criarJornadaTrabalhoRepository());
        }

        public static PontoWindow criarPontoWindow(DiaTrabalho ponto, PontoService pontoService)
        {
            return new PontoWindow(new PontoViewModel(
                (Funcionario)SessaoLogin.getSessao().UsuarioLogado,
                ponto,
                pontoService,
                RepositoryFactory.criarTipoIntervaloRepository()));
        }

        public static ControleFolgaWindow criarFolgaWindow()
        {
            return new ControleFolgaWindow(new ControleFolgaViewModel(
                RepositoryFactory.criarUsuarioRepository(),
                criarRelatorioService(),
                PontoServiceFactory.criarPontoService()
            ));
        }
    }
}

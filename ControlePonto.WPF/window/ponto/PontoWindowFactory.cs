using ControlePonto.Domain.factories;
using ControlePonto.Domain.factories.services;
using ControlePonto.Domain.feriado;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.services.persistence;
using ControlePonto.Domain.services.relatorio;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.WPF.window.ponto.folga;
using ControlePonto.WPF.window.ponto.funcionario;

namespace ControlePonto.WPF.window.ponto
{
    public class PontoWindowFactory
    {
        private static RelatorioService criarRelatorioService(UnitOfWork unitOfWork)
        {
            return new RelatorioService(
                RepositoryFactory.criarPontoRepository(),
                new FeriadoService(RepositoryFactory.criarFeriadoRepository()),
                RepositoryFactory.criarJornadaTrabalhoRepository(),
                unitOfWork);
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
                criarRelatorioService(UnitOfWorkFactory.criarUnitOfWork()),
                PontoServiceFactory.criarPontoService()
            ));
        }

        public static PontoFuncionarioWindow criarPontoDoFuncionarioWindow(DiaTrabalho ponto)
        {
            return new PontoFuncionarioWindow(new PontoFuncionarioViewModel(ponto));
        }
    }
}

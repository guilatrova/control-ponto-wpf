using ControlePonto.Domain.factories;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario.funcionario;

namespace ControlePonto.WPF.window.ponto
{
    public class PontoWindowFactory
    {
        public static PontoWindow criarPontoWindow(DiaTrabalho ponto, PontoService pontoService)
        {
            return new PontoWindow(new PontoViewModel(
                (Funcionario)SessaoLogin.getSessao().UsuarioLogado,
                ponto,
                pontoService,
                RepositoryFactory.criarTipoIntervaloRepository()));
        }
    }
}

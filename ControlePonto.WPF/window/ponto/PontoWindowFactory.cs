using ControlePonto.Domain.factories;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.services.ponto;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Infrastructure.nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.WPF.window.ponto
{
    public class PontoWindowFactory
    {
        public static PontoWindow criarPontoWindow()
        {
            return new PontoWindow(new PontoViewModel(
                (Funcionario)SessaoLogin.getSessao().UsuarioLogado,
                criarPontoService(),
                RepositoryFactory.criarTipoIntervaloRepositorio()));
        }

        private static PontoService criarPontoService()
        {
            var repo = RepositoryFactory.criarPontoRepositorio();
                
            return new PontoService(new PontoFactory(),
                new DataHoraServerStrategy(),
                new FuncionarioPossuiPontoAbertoSpecification(repo),
                new FuncionarioJaTrabalhouHojeSpecification(repo),
                SessaoLogin.getSessao(),
                repo);
        }
    }
}

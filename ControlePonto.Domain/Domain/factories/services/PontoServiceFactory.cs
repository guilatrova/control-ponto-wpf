using ControlePonto.Domain.ponto;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.services.ponto;
using ControlePonto.Infrastructure.nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.factories.services
{
    public class PontoServiceFactory
    {
        public static PontoService criarPontoService()
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

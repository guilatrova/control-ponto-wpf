using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.login;
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
            var repo = RepositoryFactory.criarPontoRepository();

            return new PontoService(
                criarPontoFactory(repo),
                new DataHoraServerStrategy(),
                new FuncionarioPossuiPontoAbertoSpecification(repo),
                new FuncionarioJaTrabalhouHojeSpecification(repo),
                SessaoLogin.getSessao(),
                repo);
        }

        private static PontoFactory criarPontoFactory(IPontoDiaRepository repo)
        {
            return new PontoFactory(repo, FeriadoServiceFactory.criarFeriadoService());
        }
    }
}

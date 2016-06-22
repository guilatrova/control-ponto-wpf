using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Tests.mocks;
using ControlePonto.Tests.mocks.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Tests
{
    public static class FactoryHelper
    {
        public static PontoService criarPontoService(SessaoLogin sessao, IDataHoraStrategy dataHoraStrategy = null, IPontoDiaRepository repository = null, bool mock = false)
        {
            if (dataHoraStrategy == null)
                dataHoraStrategy = new DataHoraMockStrategy(DateTime.Today);            

            if (repository == null)
                repository = new PontoDiaMockRepository();

            if (mock)
            {
                return new PontoServiceMock(criarPontoFactory(repository),
                    dataHoraStrategy,
                    new FuncionarioPossuiPontoAbertoSpecification(repository),
                    new FuncionarioJaTrabalhouHojeSpecification(repository),
                    sessao,
                    repository);
            }

            return new PontoService(criarPontoFactory(repository),
                dataHoraStrategy,
                new FuncionarioPossuiPontoAbertoSpecification(repository),
                new FuncionarioJaTrabalhouHojeSpecification(repository),
                sessao,
                repository);
        }

        public static PontoService criarPontoService(Funcionario logado, IDataHoraStrategy dataHoraStrategy = null, IPontoDiaRepository repository = null, bool mock = false)
        {
            return criarPontoService(new SessaoLoginMock(logado), dataHoraStrategy, repository, mock);
        }

        public static PontoFactory criarPontoFactory(IPontoDiaRepository repository = null)
        {
            if (repository == null)
                repository = new PontoDiaMockRepository();

            return new PontoFactory(repository);
        }
    }
}

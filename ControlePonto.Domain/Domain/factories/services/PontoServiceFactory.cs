﻿using ControlePonto.Domain.ponto;
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
            var pontoRepository = RepositoryFactory.criarPontoRepository();
            var tipoIntervaloRepository = RepositoryFactory.criarTipoIntervaloRepository();

            return new PontoService(
                criarPontoFactory(pontoRepository),
                new DataHoraServerStrategy(),
                new FuncionarioPossuiPontoAbertoSpecification(pontoRepository),
                new FuncionarioJaTrabalhouHojeSpecification(pontoRepository),
                SessaoLogin.getSessao(),
                pontoRepository,
                tipoIntervaloRepository);
        }

        private static PontoFactory criarPontoFactory(IPontoDiaRepository repo)
        {
            return new PontoFactory(repo, FeriadoServiceFactory.criarFeriadoService());
        }
    }
}

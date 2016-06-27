using ControlePonto.Domain.services.relatorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.factories.services
{
    public static class RelatorioServiceFactory
    {
        public static RelatorioService criarRelatorioService()
        {
            return new RelatorioService(
                RepositoryFactory.criarPontoRepository(),
                FeriadoServiceFactory.criarFeriadoService(),
                RepositoryFactory.criarJornadaTrabalhoRepository(),
                UnitOfWorkFactory.criarUnitOfWork()
            );
        }
    }
}

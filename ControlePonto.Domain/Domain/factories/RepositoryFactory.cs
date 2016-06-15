using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.usuario;
using ControlePonto.Infrastructure.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.factories
{
    public class RepositoryFactory
    {
        public static IUsuarioRepositorio criarUsuarioRepositorio()
        {
            return new UsuarioHibernateRepositorio();
        }

        public static IPontoDiaRepository criarPontoRepositorio()
        {
            return new PontoDiaHibernateRepository();
        }

        public static ITipoIntervaloRepository criarTipoIntervaloRepositorio()
        {
            return new TipoIntervaloHibernateRepositorio();
        }
    }
}

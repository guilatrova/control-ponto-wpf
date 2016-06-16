using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.jornada;
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
        public static IUsuarioRepositorio criarUsuarioRepository()
        {
            return new UsuarioHibernateRepositorio();
        }

        public static IPontoDiaRepository criarPontoRepository()
        {
            return new PontoDiaHibernateRepository();
        }

        public static ITipoIntervaloRepository criarTipoIntervaloRepository()
        {
            return new TipoIntervaloHibernateRepositorio();
        }

        public static IJornadaTrabalhoRepository criarJornadaTrabalhoRepository()
        {
            return new JornadaTrabalhoHibernateRepository();
        }
    }
}

using ControlePonto.Domain.intervalo;
using ControlePonto.Infrastructure.nhibernate;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.repositorio
{
    public class TipoIntervaloHibernateRepositorio : ITipoIntervaloRepositorio
    {
        public List<TipoIntervalo> findAll()
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                return
                    session.CreateCriteria<TipoIntervalo>().List<TipoIntervalo>().ToList();
            }
        }
    }
}

using ControlePonto.Domain.intervalo;
using ControlePonto.Infrastructure.nhibernate;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.repository
{
    public class TipoIntervaloHibernateRepository : ITipoIntervaloRepository
    {
        public List<TipoIntervalo> findAll()
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                return
                    session.CreateCriteria<TipoIntervalo>().List<TipoIntervalo>().ToList();
            }
        }

        public TipoIntervalo findByName(string nome)
        {
            throw new NotImplementedException();
        }


        public uint save(TipoIntervalo tipoIntervalo)
        {            
            throw new NotImplementedException();
        }
    }
}

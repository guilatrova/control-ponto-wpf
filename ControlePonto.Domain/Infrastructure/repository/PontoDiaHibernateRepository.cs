using ControlePonto.Domain.ponto;
using ControlePonto.Domain.usuario;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Infrastructure.nhibernate;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.repository
{
    public class PontoDiaHibernateRepository : IPontoDiaRepository
    {
        public ulong save(PontoDia ponto)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                session.SaveOrUpdate(ponto);
                session.Flush();
                return ponto.Id;
            }
        }

        public List<PontoDia> findPontosAbertos(Funcionario funcionario)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                return session.CreateCriteria<PontoDia>()
                    .Add(Restrictions.And(
                        Restrictions.Eq("isAberto", true),
                        Restrictions.Eq("Usuario", funcionario)
                    ))
                    .List<PontoDia>().ToList();
            }
        }

        public bool existePontoDia(Funcionario funcionario, DateTime date)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                return
                session.CreateCriteria<PontoDia>()
                    .Add(Restrictions.Eq("Data", date.Date))
                    .Add(Restrictions.Eq("Usuario", (Usuario)funcionario))
                    .SetProjection(Projections.RowCount())
                    .UniqueResult<int>() > 0;
            }
        }


        public PontoDia findPontoAberto(Funcionario funcionario, DateTime date)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                return session.CreateCriteria<PontoDia>()
                    .Add(Restrictions.Eq("isAberto", true))
                    .Add(Restrictions.Eq("Usuario", funcionario))
                    .Add(Restrictions.Eq("Data", date.Date))
                    .SetFetchMode("Intervalos", FetchMode.Eager)
                    .SetFetchMode("Intervalos.TipoIntervalo", FetchMode.Eager)
                    .UniqueResult<PontoDia>();
            }
        }
    }
}

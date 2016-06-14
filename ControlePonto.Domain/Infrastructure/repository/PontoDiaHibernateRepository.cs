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
                return session.QueryOver<PontoDia>()
                    .Where(x => x.Data == date.Date && x.Usuario.Equals((Usuario)funcionario))
                    .RowCount() > 0;
            }
        }
    }
}

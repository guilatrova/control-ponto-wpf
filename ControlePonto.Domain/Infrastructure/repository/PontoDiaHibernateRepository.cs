using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
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
            using (ITransaction trx = session.BeginTransaction())
            {
                session.SaveOrUpdate(ponto);
                trx.Commit();
                return ponto.Id;
            }
        }

        public List<DiaTrabalho> findPontosAbertos(Funcionario funcionario)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                return session.CreateCriteria<DiaTrabalho>()
                    .Add(Restrictions.IsNull("Fim"))
                    .Add(Restrictions.Eq("Usuario", funcionario))
                    .List<DiaTrabalho>().ToList();
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


        public DiaTrabalho findPontoAberto(Funcionario funcionario, DateTime date)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                return session.CreateCriteria<DiaTrabalho>()
                    .Add(Restrictions.IsNull("Fim"))
                    .Add(Restrictions.Eq("Usuario", funcionario))
                    .Add(Restrictions.Eq("Data", date.Date))
                    .SetFetchMode("Intervalos", FetchMode.Eager)
                    .SetFetchMode("Intervalos.TipoIntervalo", FetchMode.Eager)
                    .UniqueResult<DiaTrabalho>();
            }
        }


        public List<PontoDia> findPontosNoIntervalo(Funcionario funcionario, DateTime inicio, DateTime fim, bool lazyLoadTrabalho = true, bool lazyLoadFolga = true)
        {
            FetchMode fetchDiaTrabalho = lazyLoadTrabalho ? FetchMode.Lazy : FetchMode.Eager;
            FetchMode fetchDiaFolga = lazyLoadFolga ? FetchMode.Lazy : FetchMode.Eager;

            using (ISession session = NHibernateHelper.openSession())
            {
                return
                    session
                        .CreateCriteria<PontoDia>()
                        .SetFetchMode("DiaTrabalho", fetchDiaTrabalho)
                        .SetFetchMode("DiaFolga", fetchDiaFolga)
                        .Add(Restrictions.Eq("Usuario", funcionario))
                        .Add(Restrictions.Between("Data", inicio, fim))
                        .List<PontoDia>().ToList();
            }
        }
    }
}

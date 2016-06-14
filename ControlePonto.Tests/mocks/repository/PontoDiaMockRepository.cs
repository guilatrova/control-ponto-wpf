using ControlePonto.Domain.ponto;
using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Tests.mocks.repository
{
    public class PontoDiaMockRepository : IPontoDiaRepository
    {
        private List<PontoDia> listRep = new List<PontoDia>();

        public ulong save(PontoDia ponto)
        {
            listRep.Add(ponto);
            return (ulong)listRep.Count();
        }

        public List<PontoDia> findPontosAbertos(Funcionario funcionario)
        {
            return
                listRep.Where(x => x.isAberto && x.Usuario == funcionario).ToList();
        }


        public bool existePontoDia(Funcionario funcionario, DateTime date)
        {
            return
                listRep.Any(x => x.Data == date.Date && x.Usuario == funcionario);
        }
    }
}

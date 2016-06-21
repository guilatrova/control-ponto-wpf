using ControlePonto.Domain.folga;
using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlePonto.Tests.mocks
{
    public class FolgaMockRepository : IFolgaRepository
    {
        private List<Folga> lstRep = new List<Folga>();

        public uint save(Folga folga)
        {
            lstRep.Add(folga);
            return (uint)lstRep.Count;
        }

        public Folga findFolgaForDate(Funcionario funcionario, DateTime date)
        {
            return
                lstRep.SingleOrDefault(x => x.Data.Equals(date) && x.Funcionario.Nome == funcionario.Nome);
        }

        public List<Folga> findFolgaFromMonth(int month)
        {
            return
                lstRep.Where(x => x.Data.Month == month)
                .ToList();
        }
    }
}

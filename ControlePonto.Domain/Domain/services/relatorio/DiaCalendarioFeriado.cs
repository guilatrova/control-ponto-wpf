using ControlePonto.Domain.feriado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.relatorio
{
    public class DiaCalendarioFeriado : DiaCalendario, IDiaCalendarioFeriado
    {
        private Feriado feriado;

        public string Nome
        {
            get { return feriado.Nome; }
        }

        public override ETipoDiaCalendarioPonto TipoDia
        {
            get { return ETipoDiaCalendarioPonto.FERIADO; }
        }

        public DiaCalendarioFeriado(Feriado feriado) : base(feriado.getData())
        {
            this.feriado = feriado;
        }
    }
}

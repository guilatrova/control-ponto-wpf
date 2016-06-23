using ControlePonto.Domain.ponto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlePonto.Domain.services.relatorio
{
    public abstract class DiaCalendario
    {
        public DateTime Data { get; private set; }

        public abstract ETipoDiaCalendarioPonto TipoDia { get; }

        public DiaCalendario(DateTime data)
        {
            this.Data = data;
        }
    }
}

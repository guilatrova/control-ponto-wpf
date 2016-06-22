using ControlePonto.Domain.ponto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlePonto.Domain.services.relatorio
{
    public class DiaCalendarioPonto
    {
        public PontoDia PontoDia { get; set; }

        public DateTime Data { get; private set; }

        public ETipoDiaCalendario TipoDia
        {
            get 
            {
                if (PontoDia == null)
                    return ETipoDiaCalendario.FALTOU;

                if (PontoDia.Tipo == ETipo.TRABALHO)
                    return ETipoDiaCalendario.TRABALHO;

                return ETipoDiaCalendario.FOLGA;
            }
        }

        public DiaCalendarioPonto(PontoDia pontoDia)
        {
            this.PontoDia = pontoDia;
            this.Data = pontoDia.Data;
        }

        public DiaCalendarioPonto(DateTime data)
        {
            this.Data = data;
        }
    }
}

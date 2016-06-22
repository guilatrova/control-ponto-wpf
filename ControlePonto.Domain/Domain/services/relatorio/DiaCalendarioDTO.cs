using ControlePonto.Domain.ponto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlePonto.Domain.services.relatorio
{
    public class DiaCalendarioDTO
    {
        private PontoDia pontoDia;

        public DateTime Data { get; private set; }

        public ETipoDiaCalendario TipoDia
        {
            get 
            {
                if (pontoDia == null)
                    return ETipoDiaCalendario.FALTOU;

                if (pontoDia.Tipo == ETipo.TRABALHO)
                    return ETipoDiaCalendario.TRABALHO;

                return ETipoDiaCalendario.FOLGA;
            }
        }

        public DiaCalendarioDTO(PontoDia pontoDia)
        {
            this.pontoDia = pontoDia;
            this.Data = pontoDia.Data;
        }

        public DiaCalendarioDTO(DateTime data)
        {
            this.Data = data;
        }
    }
}

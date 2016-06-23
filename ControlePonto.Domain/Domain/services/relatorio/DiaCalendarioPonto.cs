using ControlePonto.Domain.ponto;
using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.relatorio
{
    public class DiaCalendarioPonto : DiaCalendario, IDiaCalendarioTrabalho
    {
        public PontoDia PontoDia { get; set; }

        public override ETipoDiaCalendarioPonto TipoDia
        {
            get
            {
                if (PontoDia.Tipo == ETipoPonto.TRABALHO)
                    return ETipoDiaCalendarioPonto.TRABALHO;

                return ETipoDiaCalendarioPonto.FOLGA;
            }
        }

        public DiaCalendarioPonto(PontoDia pontoDia)
            : base(pontoDia.Data)
        {
            Check.Require(pontoDia.Tipo != ETipoPonto.FERIADO_TRABALHADO,
                "Feriado trabalhado não deve ser criado desta forma.");            
            this.PontoDia = pontoDia;
        }
        
        public TimeSpan Entrada
        {
            get { throw new NotImplementedException(); }
        }

        public TimeSpan Saida
        {
            get { throw new NotImplementedException(); }
        }
    }
}

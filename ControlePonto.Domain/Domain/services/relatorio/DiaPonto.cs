using ControlePonto.Domain.jornada;
using ControlePonto.Domain.ponto;
using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.relatorio
{
    public class DiaPonto : DiaRelatorio, IDiaComPonto
    {
        public PontoDia PontoDia { get; set; }

        public override ETipoDiaRelatorio TipoDia
        {
            get
            {
                if (PontoDia.Tipo == ETipoPonto.TRABALHO)
                    return ETipoDiaRelatorio.TRABALHO;

                return ETipoDiaRelatorio.FOLGA;
            }
        }

        public DiaPonto(PontoDia pontoDia)
            : base(pontoDia.Data)
        {
            Check.Require(pontoDia.Tipo != ETipoPonto.FERIADO_TRABALHADO,
                "Feriado trabalhado não deve ser criado desta forma.");
            Check.Require(pontoDia != null, "O ponto deve ser válido");

            this.PontoDia = pontoDia;
        }

        public TimeSpan calcularHorasExtras(JornadaTrabalho jornadaAtiva)
        {
            return PontoDia.calcularHorasExtras(jornadaAtiva);
        }

        public double calcularValorHoraExtra()
        {
            return PontoDia.calcularValorHoraExtra();
        }

        public TimeSpan calcularHorasDevedoras(JornadaTrabalho jornadaAtiva)
        {
            return PontoDia.calcularHorasDevedoras(jornadaAtiva);
        }
    }
}

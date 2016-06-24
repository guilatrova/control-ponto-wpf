using ControlePonto.Domain.feriado;
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
    public class DiaFeriadoTrabalhado : DiaRelatorio, IDiaFeriado, IDiaComPonto
    {
        public PontoDia PontoDia { get; private set; }
        public Feriado Feriado { get; private set; }

        public string Nome
        {
            get { return Feriado.Nome; }
        }

        public override ETipoDiaRelatorio TipoDia
        {
            get { return ETipoDiaRelatorio.FERIADO_TRABALHADO; }
        }

        public DiaFeriadoTrabalhado(PontoDia pontoDia, Feriado feriado)
            : base(pontoDia.Data)
        {
            Check.Require(pontoDia.Data == feriado.getData(), 
                "O dia do feriado deve ser o mesmo do dia de trabalho");
            Check.Require(feriado != null, "O feriado deve ser válido");
            Check.Require(pontoDia != null, "O ponto deve ser válido");

            PontoDia = pontoDia;
            Feriado = feriado;
        }

        public TimeSpan calcularHorasExtras(JornadaTrabalho jornadaAtiva)
        {
            return PontoDia.calcularHorasExtras(jornadaAtiva);
        }

        public TimeSpan calcularHorasDevedoras(JornadaTrabalho jornadaAtiva)
        {
            return PontoDia.calcularHorasDevedoras(jornadaAtiva);
        }


        public double calcularValorHoraExtra()
        {
            return PontoDia.calcularValorHoraExtra();
        }
    }
}

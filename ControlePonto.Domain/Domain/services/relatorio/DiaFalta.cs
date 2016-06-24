using ControlePonto.Domain.jornada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.relatorio
{
    public class DiaFalta : DiaRelatorio, ICalculoHoraDevedora
    {
        public override ETipoDiaRelatorio TipoDia
        {
            get { return ETipoDiaRelatorio.FALTOU; }
        }

        public DiaFalta(DateTime date) : base(date) { }

        public TimeSpan calcularHorasDevedoras(JornadaTrabalho jornadaAtiva)
        {
            return jornadaAtiva.getDia(Data.Date.DayOfWeek).calcularHorasTrabalhoEsperado();
        }
    }
}

using ControlePonto.Domain.ponto;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlePonto.Domain.services.relatorio
{
    public class CalendarioPonto
    {
        public Funcionario Funcionario { get; set; }

        public DateTime PeriodoInicio { get; private set; }

        public DateTime PeriodoFim { get; private set; }

        public List<DiaCalendarioDTO> Dias { get; private set; }

        public CalendarioPonto(Funcionario funcionario, DateTime inicio, DateTime fim, List<DiaCalendarioDTO> todosDias)
        {
            this.Funcionario = funcionario;
            this.PeriodoInicio = inicio;
            this.PeriodoFim = fim;
            this.Dias = todosDias;

            int difDias = (fim - inicio).Days + 1;

            Check.Ensure(Dias.Count == difDias, string.Format(
                "Uma quantidade errada de dias foi gerada para o período: {0} e {1}. Esperado: {2} Encontrado: {3}", 
                inicio.ToShortDateString(), 
                fim.ToShortDateString(),
                difDias,
                Dias.Count));
        }
    }
}

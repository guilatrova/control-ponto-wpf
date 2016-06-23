using ControlePonto.Domain.jornada;
using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto.trabalho
{
    public class DiaTrabalhoComum : DiaTrabalho
    {
        public DiaTrabalhoComum(DateTime data, TimeSpan inicio, Funcionario funcionario) 
            : base(data, inicio, funcionario) { }

        public override ETipoPonto Tipo
        {
            get { return ETipoPonto.TRABALHO; }
        }

        public override double calcularValorHoraExtra()
        {
            if (Data.DayOfWeek == DayOfWeek.Sunday)
                return 100;
            return 75;
        }
    }
}

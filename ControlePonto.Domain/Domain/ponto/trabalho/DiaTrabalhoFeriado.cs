using ControlePonto.Domain.feriado;
using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto.trabalho
{
    public class DiaTrabalhoFeriado : DiaTrabalho
    {
        public Feriado Feriado { get; private set; }

        public override ETipoPonto Tipo
        {
            get { return ETipoPonto.FERIADO_TRABALHADO; }
        }

        public DiaTrabalhoFeriado(Feriado feriado, TimeSpan inicio, Funcionario funcionario) :
            base (feriado.getData(), inicio, funcionario) 
        {
            this.Feriado = feriado;
        }

        public override double calcularValorHoraExtra()
        {
            return 100;
        }
    }
}

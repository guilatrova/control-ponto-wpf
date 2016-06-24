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
        public virtual Feriado Feriado { get; protected set; }

        public override ETipoPonto Tipo { get; protected set; }

        protected DiaTrabalhoFeriado() { }

        public DiaTrabalhoFeriado(Feriado feriado, TimeSpan inicio, Funcionario funcionario) :
            base (feriado.getData(), inicio, funcionario) 
        {
            this.Tipo = ETipoPonto.FERIADO_TRABALHADO;
            this.Feriado = feriado;
        }

        public override double calcularValorHoraExtra()
        {
            return 100;
        }
    }
}

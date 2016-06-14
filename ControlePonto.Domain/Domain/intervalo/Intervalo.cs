using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.intervalo
{
    public class Intervalo : Entity<ulong>
    {
        public virtual TimeSpan Entrada { get; protected set; }
        public virtual TimeSpan? Saida { get; set; }
        public virtual bool Fechado { get; protected set; }
        public virtual TipoIntervalo TipoIntervalo { get; protected set; }

        public Intervalo(TipoIntervalo tipo, TimeSpan entrada)
        {
            TipoIntervalo = tipo;
            Entrada = entrada;
            Saida = null;
            Fechado = false;
        }
    }
}

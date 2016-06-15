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

        private TimeSpan? _saida;
        public virtual TimeSpan? Saida 
        {
            get
            {
                return _saida;
            }
            set
            {
                if (_saida.HasValue)
                    throw new InvalidOperationException("Não é possível alterar o horário de saída de um intervalo");
                _saida = value;
            }
        }
        public virtual bool isFechado
        {
            get
            {
                return Saida.HasValue;
            }
        }
        public virtual TipoIntervalo TipoIntervalo { get; protected set; }

        protected Intervalo() { }

        public Intervalo(TipoIntervalo tipo, TimeSpan entrada)
        {
            TipoIntervalo = tipo;
            Entrada = entrada;
            Saida = null;
        }
    }
}

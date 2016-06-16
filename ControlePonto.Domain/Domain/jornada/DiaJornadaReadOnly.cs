using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.jornada
{
    public class DiaJornadaReadOnly : DiaJornada
    {
        protected internal DiaJornadaReadOnly(DiaJornada diaJornada) : 
            base(diaJornada.DiaSemana)
        {
            _entrada = diaJornada.EntradaEsperada;
            _saida = diaJornada.SaidaEsperada;
            _folga = diaJornada.FolgaEsperada;
        }

        private TimeSpan _entrada;
        public override TimeSpan EntradaEsperada
        {
            get { return _entrada; }
            set { throw new InvalidOperationException(); }
        }

        private TimeSpan _saida;
        public override TimeSpan SaidaEsperada
        {
            get { return _saida; }
            set { throw new InvalidOperationException(); }
        }

        private TimeSpan _folga;
        public override TimeSpan FolgaEsperada
        {
            get { return _folga; }
            set { throw new InvalidOperationException(); }
        }
    }
}

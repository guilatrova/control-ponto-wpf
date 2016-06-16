using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.jornada
{
    public class JornadaTrabalho : Entity<uint>
    {
        public virtual ICollection<DiaJornada> Dias { get; protected set; }

        private TimeSpan NAO_DEFINIDO = new TimeSpan(0, 0, 0);

        public JornadaTrabalho() 
        {
            Dias = new List<DiaJornada>(7);
            for (DayOfWeek day = DayOfWeek.Sunday; day <= DayOfWeek.Saturday; day++)
                Dias.Add(new DiaJornada(day, NAO_DEFINIDO, NAO_DEFINIDO, NAO_DEFINIDO));
        }

        public virtual void cadastrarDia(DayOfWeek week, TimeSpan entradaEsperada, TimeSpan saidaEsperada, TimeSpan horasFolga)
        {
            Check.Require(saidaEsperada >= entradaEsperada,
                string.Format("A saída esperada deve ser após a entrada. Entrada:{0} Saída:{1}", entradaEsperada, saidaEsperada));

            var dia = Dias.Single(x => x.DiaSemana == week);
            dia.EntradaEsperada = entradaEsperada;
            dia.SaidaEsperada = saidaEsperada;
            dia.FolgaEsperada = horasFolga;
        }

        public virtual void cadastrarDia(DayOfWeek from, DayOfWeek until, TimeSpan entradaEsperada, TimeSpan saidaEsperada, TimeSpan horasFolga)
        {
            Check.Require(from < until, "O intervalo de dias da semana está incorreto");            

            for (DayOfWeek i = from; i <= until; i++)
                cadastrarDia(i, entradaEsperada, saidaEsperada, horasFolga);            
        }

        public virtual DiaJornada getDia(DayOfWeek week)
        {
            return Dias.Single(x => x.DiaSemana == week).asReadOnly();
        }
    }
}

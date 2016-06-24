using ControlePonto.Domain.jornada;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
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
        #region Propriedades

        public Funcionario Funcionario { get; set; }

        public DateTime PeriodoInicio { get; private set; }

        public DateTime PeriodoFim { get; private set; }

        public List<DiaCalendario> Dias { get; private set; }        

        #endregion

        private JornadaTrabalho jornadaAtiva;

        public CalendarioPonto(Funcionario funcionario, DateTime inicio, DateTime fim, JornadaTrabalho jornadaAtiva, List<DiaCalendario> todosDias)
        {
            this.Funcionario = funcionario;
            this.PeriodoInicio = inicio;
            this.PeriodoFim = fim;
            this.jornadaAtiva = jornadaAtiva;
            this.Dias = todosDias;

            int difDias = (fim - inicio).Days + 1;

            Check.Ensure(Dias.Count == difDias, string.Format(
                "Uma quantidade errada de dias foi gerada para o período: {0} e {1}. Esperado: {2} Encontrado: {3}", 
                inicio.ToShortDateString(), 
                fim.ToShortDateString(),
                difDias,
                Dias.Count));
        }

        public List<IDiaCalendarioFeriado> getFeriados()
        {
            return
                Dias
                .Where(x => x.TipoDia == ETipoDiaCalendarioPonto.FERIADO || 
                    x.TipoDia == ETipoDiaCalendarioPonto.FERIADO_TRABALHADO)
                .Cast<IDiaCalendarioFeriado>()
                .ToList();
        }

        public List<IDiaCalendarioTrabalho> getDiasTrabalhados()
        {
            return
                Dias
                .Where(x => x.TipoDia == ETipoDiaCalendarioPonto.TRABALHO ||
                    x.TipoDia == ETipoDiaCalendarioPonto.FERIADO_TRABALHADO)
                .Cast<IDiaCalendarioTrabalho>()
                .ToList();
        }

        public List<DiaCalendarioPonto> getFolgas()
        {
            return Dias.Where(x => x.TipoDia == ETipoDiaCalendarioPonto.FOLGA)
                .Cast<DiaCalendarioPonto>()
                .ToList();
        }

        public TimeSpan calcularHorasExtras(double valorHoraExtra)
        {
            return
                new TimeSpan(
                    getDiasTrabalhados()
                        .Select(x => x.PontoDia)
                        .Cast<DiaTrabalho>()
                        .Where(x => x.calcularValorHoraExtra() == valorHoraExtra)
                        .Sum(x => x.calcularHorasExtras(jornadaAtiva).Ticks)
                );
        }

        public TimeSpan calcularHorasExtras()
        {
            return
                new TimeSpan(
                    getDiasTrabalhados()
                        .Select(x => x.PontoDia)
                        .Cast<DiaTrabalho>()
                        .Sum(x => x.calcularHorasExtras(jornadaAtiva).Ticks)
                );
        }
    }
}
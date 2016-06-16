using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlePonto.Domain.jornada;
using ControlePonto.Infrastructure.utils;

namespace ControlePonto.Tests
{
    [TestClass]
    public class JornadaTrabalhoTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void diaJornadaTrabalhoNaoPodeSerAlterado()
        {
            var jornada = new JornadaTrabalho();
            jornada.cadastrarDia(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0), new TimeSpan(1, 0, 0));

            Assert.AreEqual(new TimeSpan(18, 0, 0), jornada.getDia(DayOfWeek.Monday).SaidaEsperada);
            jornada.getDia(DayOfWeek.Monday).SaidaEsperada = new TimeSpan(10, 0, 0);
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void horarioDeSaidaNaoDeveSerAntesDoDeEntrada()
        {
            var jornada = new JornadaTrabalho();
            jornada.cadastrarDia(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(8, 0, 0), new TimeSpan(1, 0, 0));
        }

        [TestMethod]
        public void jornadaDeveCalcularHorasTrabalhoEsperado()
        {
            var jornada = new JornadaTrabalho();
            jornada.cadastrarDia(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0), new TimeSpan(1, 0, 0));

            Assert.AreEqual(new TimeSpan(8,0 ,0), jornada.getDia(DayOfWeek.Monday).calcularHorasTrabalhoEsperado());
        }
    }
}

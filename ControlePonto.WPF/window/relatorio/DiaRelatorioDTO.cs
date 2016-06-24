using ControlePonto.Domain.ponto.folga;
using ControlePonto.Domain.services.relatorio;
using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ControlePonto.WPF.window.relatorio
{
    public class DiaRelatorioDTO
    {
        public DiaRelatorio DiaRelatorio { get; private set; }

        public DateTime Data
        { get { return DiaRelatorio.Data; } }

        public string Info
        {
            get
            {
                switch (DiaRelatorio.TipoDia)
                {
                    case ETipoDiaRelatorio.FALTOU:
                        return "Funcionário faltou";

                    case ETipoDiaRelatorio.FERIADO:
                    case ETipoDiaRelatorio.FERIADO_TRABALHADO:
                        return "FERIADO: " + (DiaRelatorio as IDiaFeriado).Nome;

                    case ETipoDiaRelatorio.FOLGA:
                        return "FOLGA: " + ((DiaRelatorio as DiaPonto).PontoDia as DiaFolga).Descricao;

                    default:
                        return DiaSemanaTradutor.traduzir(Data.DayOfWeek);                        
                }                
            }
        }

        public Brush Cor
        {
            get
            {
                if (DiaRelatorio.TipoDia == ETipoDiaRelatorio.FALTOU)
                    return Brushes.Red;
                return Brushes.Black;
            }
        }

        public string HorasTrabalhadas
        {
            get
            {                
                if (DiaRelatorio is IDiaComPonto)
                    return (DiaRelatorio as IDiaComPonto).calcularHorasTrabalhadas().ToString();
                return "";
                //return new TimeSpan();
            }
        }

        public string HorasDevedoras
        {
            get
            {                
                TimeSpan devedoras = new TimeSpan();
                if (DiaRelatorio is ICalculoHoraDevedora)
                    devedoras = (DiaRelatorio as ICalculoHoraDevedora).calcularHorasDevedoras();

                if (devedoras == new TimeSpan())
                    return "";
                return devedoras.ToString();
            }
        }

        public string HorasExtras75
        {
            get
            {
                return getHoraExtrarPorValor(75);
            }
        }

        public string HorasExtras100
        {
            get
            {
                return getHoraExtrarPorValor(100);
            }
        }

        public DiaRelatorioDTO(DiaRelatorio diaRelatorio)
        {
            this.DiaRelatorio = diaRelatorio;
        }

        private string getHoraExtrarPorValor(double valor)
        {
            if (DiaRelatorio is ICalculoHoraExtra)
            {
                var calculo = (DiaRelatorio as ICalculoHoraExtra);
                if (calculo.calcularValorHoraExtra() == valor)
                    return calculo.calcularHorasExtras().ToString();
            }
            return "";
        }
    }
}

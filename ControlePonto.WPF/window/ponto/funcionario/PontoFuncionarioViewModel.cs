using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ControlePonto.WPF.window.ponto.funcionario
{
    public class PontoFuncionarioViewModel : ViewModelBase
    {
        private DiaTrabalho ponto;        

        public PontoFuncionarioViewModel(DiaTrabalho ponto)
        {
            this.ponto = ponto;            

            this.Data = ponto.Data.ToShortDateString();
            this.Funcionario = ponto.Funcionario.Nome;
            this.Entrada = string.Format("Entrada às {0}", ponto.Inicio);
            this.Saida = string.Format("Saída às {0}", ponto.Fim);

            Intervalos = ponto.Intervalos.ToList();

            FecharCommand = new RelayCommand(() => requestView(CLOSE));
        }

        public string Data { get; private set; }
        public string Funcionario { get; private set; }
        public string Entrada { get; private set; }

        private List<Intervalo> _intervalos;
        public List<Intervalo> Intervalos
        {
            get { return _intervalos; }
            set { _intervalos = value; }
        }
        
        public string Saida { get; private set; }
        public ICommand FecharCommand { get; private set; }


        protected override string validar(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}

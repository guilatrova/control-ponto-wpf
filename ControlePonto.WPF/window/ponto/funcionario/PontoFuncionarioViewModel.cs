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

namespace ControlePonto.WPF.window.consulta.funcionario
{
    public class PontoFuncionarioViewModel : ViewModelBase
    {
        private DiaTrabalho ponto;

        public PontoFuncionarioViewModel(DiaTrabalho ponto, bool allowEdit = false)
        {
            this.ponto = ponto;            

            this.Data = ponto.Data.ToShortDateString();
            this.Funcionario = ponto.Funcionario.Nome;
            this.Entrada = ponto.Inicio;
            this.Saida = ponto.Fim ?? new TimeSpan(0, 0, 0);
            this.AllowEdit = allowEdit;

            Intervalos = ponto.Intervalos.ToList();

            FecharCommand = new RelayCommand(() => requestView(CLOSE));
        }

        public string Data { get; private set; }
        public string Funcionario { get; private set; }
        public TimeSpan Entrada { get; private set; }
        public bool AllowEdit { get; private set; }

        private List<Intervalo> _intervalos;
        public List<Intervalo> Intervalos
        {
            get { return _intervalos; }
            set { _intervalos = value; }
        }
        
        public TimeSpan Saida { get; private set; }
        public ICommand FecharCommand { get; private set; }


        protected override string validar(string propertyName)
        {
            return null;
            throw new NotImplementedException();
        }
    }
}

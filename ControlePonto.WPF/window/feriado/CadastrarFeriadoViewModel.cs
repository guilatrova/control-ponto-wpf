using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.WPF.window.feriado
{
    public class CadastrarFeriadoViewModel : ViewModelBase
    {
        public CadastrarFeriadoViewModel()
        {

        }

        #region Propriedades

        public bool TipoFixo { get; set; }
        public bool TipoRelativo { get; set; }
        public bool TipoEspecifico { get; set; }

        public string Nome { get; set; }

        public DateTime Data { get; set; }

        public string DataStringFormat { get { return ""; } }

        #region Relativo

        public int[] SequenciasDiaSemana 
        { 
            get
            {
                return new int[] { 1, 2, 3, 4, 5 };
            }
        }

        private int _sequenciaDiaEscolhido;
        public int SequenciaDiaEscolhido
        {
            get { return _sequenciaDiaEscolhido; }
            set 
            {
                SetField(ref _sequenciaDiaEscolhido, value); 
            }
        }
        
        public List<string> DiasSemana { get; private set; }
        public string DiaSemanaEscolhido { get; set; }

        #endregion

        #endregion

        protected override string validar(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}

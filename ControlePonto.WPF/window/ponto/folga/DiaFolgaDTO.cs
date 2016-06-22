using ControlePonto.Domain.ponto.folga;
using ControlePonto.Domain.services.relatorio;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.WPF.window.ponto.folga
{
    public class DiaFolgaDTO : NotifyPropertyChangedBase
    {
        private const string FUNCIONARIO_NAO_TRABALHOU = "Funcionário não trabalhou neste dia";        

        private bool _isDiaFolga;
        public bool IsDiaFolga
        {
            get { return _isDiaFolga; }
            set
            {
                if (SetField(ref _isDiaFolga, value))
                {
                    if (value)
                        Descricao = "";
                    else
                        Descricao = FUNCIONARIO_NAO_TRABALHOU;
                }
            }
        }

        public DateTime Data { get; private set; }

        private string _descricao;
        public string Descricao
        {
            get { return _descricao; }
            set { SetField(ref _descricao, value); }
        }

        public int DescricaoMax
        {
            get
            {
                return DiaFolga.MAX_DESCRICAO_LENGTH;
            }
        }

        public bool IsEnabled
        {
            get
            {
                if (DateTime.Today > Data)
                    return false;
                return !diaExiste;
            }
        }

        public bool IsReadOnly
        {
            get 
            {
                return diaExiste;
            }
        }

        private bool diaExiste;

        public DiaFolgaDTO(DiaCalendarioPonto diaCalendario)
        {
            this.Data = diaCalendario.Data;
            diaExiste = true;

            switch (diaCalendario.TipoDia)
            {
                case ETipoDiaCalendario.FOLGA:
                    _isDiaFolga = true;
                    this.Descricao = (diaCalendario.PontoDia as DiaFolga).Descricao;
                    break;

                case ETipoDiaCalendario.TRABALHO:
                    this.Descricao = "Funcionário trabalhou neste dia";
                    break;

                default:
                    this.Descricao = FUNCIONARIO_NAO_TRABALHOU;
                    diaExiste = false;
                    break;
            }
        }
        
    }
}

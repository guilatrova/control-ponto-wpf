using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.folga
{
    public class FolgaJaExisteException : Exception
    {
        public DateTime Date { get; private set; }
        public string Descricao { get; private set; }

        public FolgaJaExisteException(Funcionario funcionario, DateTime data, string descricao) :
            base(string.Format("Já existe uma folga para o {0} no dia {1}: {2}", funcionario.Nome, data.ToShortDateString(), descricao))
        {
            this.Date = data;
            this.Descricao = descricao;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.folga
{
    public class FolgaDiaTrabalhadoException : Exception
    {
        public DateTime Date { get; private set; }

        public FolgaDiaTrabalhadoException(DateTime date) : 
            base(string.Format("Não é possível dar folga para um dia que o funcionário já iniciou trabalho"))
        {
            this.Date = date;
        }
    }
}

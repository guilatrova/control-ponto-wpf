using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.intervalo
{
    public class TipoIntervalo : Entity<uint>
    {
        public const int MAX_NOME_LENGTH = 25;

        public virtual string Nome { get; protected set; }//TODO unique

        public TipoIntervalo(string nome)
        {
            Nome = nome;
        }
    }
}

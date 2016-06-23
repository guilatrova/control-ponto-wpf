using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.relatorio
{
    public enum ETipoDiaCalendarioPonto : int
    {
        TRABALHO,        
        FERIADO,
        FERIADO_TRABALHADO,
        FOLGA,
        FALTOU
    }
}

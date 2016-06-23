using ControlePonto.Domain.ponto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.relatorio
{
    public interface IDiaCalendarioTrabalho
    {
        PontoDia PontoDia { get; }

        TimeSpan Entrada { get; }

        TimeSpan Saida { get; }
    }
}

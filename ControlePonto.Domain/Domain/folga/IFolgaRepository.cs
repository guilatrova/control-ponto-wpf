using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlePonto.Domain.folga
{
    public interface IFolgaRepository
    {
        uint save(Folga folga);

        Folga findFolgaForDate(Funcionario funcionario, DateTime date);

        List<Folga> findFolgaFromMonth(int month);
    }
}

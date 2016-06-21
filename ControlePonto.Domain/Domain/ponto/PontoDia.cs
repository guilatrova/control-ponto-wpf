using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.jornada;
using ControlePonto.Domain.usuario;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto
{
    public abstract class PontoDia : Entity<ulong>
    {
        //http://stackoverflow.com/questions/18089383/abstract-classes-and-fluent-nhibernate
        public virtual DateTime Data { get; protected set; }

        public virtual Usuario Usuario { get; protected set; }

        public virtual ETipo Tipo { get; protected set; }
    }
}

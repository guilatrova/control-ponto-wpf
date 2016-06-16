using ControlePonto.Domain.jornada;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.nhibernate.mapping
{
    public class JornadaTrabalhoMap : ClassMap<JornadaTrabalho>
    {
        public JornadaTrabalhoMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.Dias)                
                .Not.LazyLoad();
        }
    }
}

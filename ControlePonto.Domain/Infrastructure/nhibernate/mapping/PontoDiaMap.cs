using ControlePonto.Domain.ponto;
using FluentNHibernate.Mapping;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.nhibernate.mapping
{
    public class PontoDiaMap : ClassMap<PontoDia>
    {
        public PontoDiaMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Data).Not.Nullable();
            Map(x => x.Inicio)
                .CustomType(typeof(TimeAsTimeSpanTypeClone))                
                .Not.Nullable();
            Map(x => x.Fim)
                .CustomType(typeof(TimeAsTimeSpanTypeClone));
            Map(x => x.isAberto);

            HasMany(x => x.Intervalos)
                .Cascade.All();

            References(x => x.Usuario).Not.Nullable();
        }
    }
}

using ControlePonto.Domain.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.factories
{
    public class RepositorioFactory
    {
        public static IUsuarioRepositorio criarUsuarioRepositorio()
        {
            //return new UsuarioHibernateRepositorio();
            throw new NotImplementedException();
        }
    }
}

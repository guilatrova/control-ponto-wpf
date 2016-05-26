using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.usuario.funcionario
{
    public class Funcionario : Usuario
    {
        public string CPF { get; private set; }
        public string RG { get; private set; }

        protected Funcionario() { }

        public Funcionario(string nome, string login, string senha, string cpf = null, string rg = null)
        {
            checkPreConstructor();

            this.Nome = nome;
            this.Login = login;
            this.Senha = senha;
            this.RG = rg;
            this.CPF = cpf;
        }
    }
}

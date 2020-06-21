using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Services
{
    public class MeuServico : IMeuService
    {
        public string Saudacao(string nome)
        {
            return $"Bem vindo, {nome} \n \n {DateTime.Now}";
        }
    }
}

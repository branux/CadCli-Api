using System.Collections.Generic;

namespace CadCli.Dominio.Entidades
{
    public class Empresa
    {
        public Empresa()
        {
            Clientes = new List<Cliente>();
        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CNPJ { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}

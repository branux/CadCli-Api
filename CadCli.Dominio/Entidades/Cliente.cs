using CadCli.Dominio.Enums;
using System;

namespace CadCli.Dominio.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }

        public string Nome { get; set; }
        public DateTime Nascimento { get; set; }
        public Sexo Sexo { get; set; }

        public int? EmpresaId { get; set; }
        public virtual Empresa Empresa { get; set; }
    }
}

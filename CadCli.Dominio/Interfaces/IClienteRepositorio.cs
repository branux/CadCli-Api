using CadCli.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace CadCli.Dominio.Interfaces
{
    public interface IClienteRepositorio : IDisposable
    {
        Cliente Salvar(Cliente cliente);
        Cliente Excluir(int id);

        Cliente ObterPorId(int id);
        IEnumerable<Cliente> Todos();
    }
}

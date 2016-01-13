using CadCli.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace CadCli.Dominio.Interfaces
{
    public interface IEmpresaRepositorio : IDisposable
    {
        Empresa Salvar(Empresa empresa);
        Empresa Excluir(int id);

        Empresa ObterPorId(int id);
        Empresa ObterPorIdComClientes(int id);

        IEnumerable<Empresa> Todos();
    }
}

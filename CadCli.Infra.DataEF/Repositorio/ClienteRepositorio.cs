using CadCli.Dominio.Interfaces;
using System.Collections.Generic;
using CadCli.Dominio.Entidades;
using CadCli.Infra.DataEF.Contexto;
using System.Data.Entity;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace CadCli.Infra.DataEF.Repositorio
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly CadCliContexto _ctx;

        public ClienteRepositorio()
        {
            _ctx = new CadCliContexto();
        }


        public Cliente ObterPorId(int id)
        {
            return
                _ctx.Clientes
                .Include(tab => tab.Empresa)
                .FirstOrDefault(d => d.Id == id);
        }

        public Cliente Salvar(Cliente cliente)
        {
            if (cliente.Id == 0)
            {
                _ctx.Clientes.Add(cliente);
            }
            else
            {
                _ctx.Entry(cliente).State = EntityState.Modified;
            }

            SalvarContexto();

            return cliente;
        }

        public IEnumerable<Cliente> Todos()
        {
            return _ctx.Clientes.Include(tab => tab.Empresa);
        }

        private void SalvarContexto()
        {
            _ctx.SaveChanges();
        }


        public void Dispose()
        {
            _ctx.Dispose();
        }

        public Cliente Excluir(int id)
        {
            var cliente = ObterPorId(id);

            if (cliente == null)
                return null;


            _ctx.Clientes.Remove (cliente);
            SalvarContexto();

            return cliente;
        }
    }
}

using CadCli.Dominio.Interfaces;
using System.Collections.Generic;
using CadCli.Dominio.Entidades;
using CadCli.Infra.DataEF.Contexto;
using System.Data.Entity;
using System.Linq;

namespace CadCli.Infra.DataEF.Repositorio
{
    public class EmpresaRepositorio : IEmpresaRepositorio
    {

        private readonly CadCliContexto _ctx;

        public EmpresaRepositorio()
        {
            _ctx = new CadCliContexto();
        }

        public Empresa Excluir(int id)
        {
            var empresa = ObterPorId(id);

            if (empresa == null)
                return null;


            _ctx.Empresas.Remove(empresa);
            SalvarContexto();

            return empresa;
        }

        private void SalvarContexto()
        {
            _ctx.SaveChanges();
        }

        public Empresa ObterPorId(int id)
        {
            return _ctx.Empresas.Find(id);
        }

        public  Empresa ObterPorIdComClientes(int id)
        {
            return 
                _ctx.Empresas
                .Include(cli=>cli.Clientes)
                .FirstOrDefault(d=>d.Id==id);
        }

        public Empresa Salvar(Empresa empresa)
        {
            if (empresa.Id == 0)
            {
                _ctx.Empresas.Add(empresa);
            }
            else
            {
                _ctx.Entry(empresa).State = EntityState.Modified;
            }

            SalvarContexto();
            return empresa;
        }

        public IEnumerable<Empresa> Todos()
        {
            return _ctx.Empresas;

        }

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}

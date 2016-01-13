using CadCli.Dominio.Entidades;
using CadCli.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CadCli.Servico.Api.Controllers
{
    public class EmpresasController : ApiController
    {

        private readonly IEmpresaRepositorio _repo;

        public EmpresasController(IEmpresaRepositorio repo)
        {
            _repo = repo;
        }


        public async Task<IHttpActionResult> Get()
        {
            var dados = await Task.FromResult(_repo.Todos().ToList());
            return Ok(dados);
        }


        public async Task<IHttpActionResult> Get(int id)
        {
            var dados = await Task.FromResult(_repo.ObterPorId(id));

            if (dados == null)
                return NotFound();

            return Ok(dados);
        }

        [Route("api/empresas/{id:int}/clientes")]
        public async Task<IHttpActionResult> GetCli(int id)
        {
            var dados = await Task.FromResult(_repo.ObterPorIdComClientes(id));

            if (dados == null)
                return NotFound();

            return Ok(dados);
        }




        public async Task<IHttpActionResult> Post(Empresa empresa)
        {
            if (empresa == null)
                return BadRequest("Empresa não pode ser nula");

            if (empresa.Id != 0)
                return BadRequest("Id da empresa inválido");

            try
            {
                await Task.FromResult(_repo.Salvar(empresa));

                return CreatedAtRoute("DefaultApi", new { id = empresa.Id }, empresa);

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.ToString().Contains("UQ_dbo.Empresa.CNPJ"))
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Já existe uma empresa com este cnpj"));

                return InternalServerError(ex);
            }
        }

        public async Task<IHttpActionResult> Put(int id, Empresa empresa)
        {

            if (empresa == null)
                return BadRequest("Empresa não pode ser nula");

            if (id != empresa.Id)
                return BadRequest("Os id's não conferem");

            await Task.FromResult(_repo.Salvar(empresa));

            return StatusCode(HttpStatusCode.NoContent);
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            var empresa = await Task.FromResult(_repo.Excluir(id));

            if (empresa == null)
                return NotFound();

            return Ok(empresa);
        }


        protected override void Dispose(bool disposing)
        {
            _repo.Dispose();
            base.Dispose(disposing);
        }

    }
}

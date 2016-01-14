using CadCli.Dominio.Entidades;
using CadCli.Dominio.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CadCli.Servico.Api.Controllers
{
    [RoutePrefix("api/v1/clientes")]
    public class ClientesController : ApiController
    {
        private readonly IClienteRepositorio _repo;

        public ClientesController(IClienteRepositorio repo)
        {
            _repo = repo;
        }

        [Route]
        public async Task<IHttpActionResult> Get()
        {
            var dados = await Task.FromResult(_repo.Todos().ToList());
            return Ok(dados);
        }

        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var dados = await Task.FromResult(_repo.ObterPorId(id));

            if (dados == null)
                return NotFound();

            return Ok(dados);

        }

        [Route]
        public async Task<IHttpActionResult> Post(Cliente cliente)
        {
            if (cliente == null)
                return BadRequest("Cliente não pode ser nulo");

            if (cliente.Id != 0)
                return BadRequest("Id do cliente inválido");

            try
            {
                await Task.FromResult(_repo.Salvar(cliente));
                //return CreatedAtRoute("DefaultApi", new { id = cliente.Id }, cliente);

                var location = Request == null ? "" :
                    string.Format("{0}/{1}", Request.RequestUri, cliente.Id).Replace("//","/");

                return Created(location, cliente);

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.ToString().Contains("UQ_dbo.Cliente.Nome-Nascimento"))
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Já existe um cliente com este nome e essa data de nascimento"));

                return InternalServerError(ex);
            }

        }

        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id,Cliente cliente)
        {

            if (cliente == null)
                return BadRequest("Cliente não pode ser nulo");

            if (id != cliente.Id)
                return BadRequest("Os id's não conferem");

            await Task.FromResult(_repo.Salvar(cliente));

            return StatusCode(HttpStatusCode.NoContent);
        }


        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var cliente = await Task.FromResult(_repo.Excluir(id));

            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }


        protected override void Dispose(bool disposing)
        {
            _repo.Dispose();
            base.Dispose(disposing);
        }

    }
}

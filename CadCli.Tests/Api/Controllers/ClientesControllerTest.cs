using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CadCli.Dominio.Interfaces;
using CadCli.Dominio.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CadCli.Servico.Api.Controllers;
using System.Web.Http.Results;
using System.Net;

namespace CadCli.Tests.Api.Controllers
{
    [TestClass]
    public class ClientesControllerTest
    {

        #region GET

        [TestMethod]
        [TestCategory("ClientesController / Métodos")]
        public async Task Get_deverá_retornar_todos_os_clientes()
        {
            //Arrange
            var repo = new ClienteRepositorioStub();
            var controller = new ClientesController(repo);

            //Action
            var result = await controller.Get() as OkNegotiatedContentResult<List<Cliente>>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(repo.Todos().Count(), result.Content.Count);

            controller.Dispose();
            repo.Dispose();
        }

        [TestMethod]
        [TestCategory("ClientesController / Métodos")]
        public async Task Get_com_id_deverá_retornar_o_cliente_desejado()
        {
            //Arrange
            var id = 1;
            var repo = new ClienteRepositorioStub();
            var controller = new ClientesController(repo);

            //Action
            var result = await controller.Get(id) as OkNegotiatedContentResult<Cliente>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(repo.ObterPorId(id), result.Content);

            controller.Dispose();
            repo.Dispose();
        }

        [TestMethod]
        [TestCategory("ClientesController / Métodos")]
        public async Task Get_com_id_inexistente_deverá_retornar_NOTFOUND()
        {
            //Arrange
            var id = 4;
            var repo = new ClienteRepositorioStub();
            var controller = new ClientesController(repo);

            //Action
            var result = await controller.Get(id) as NotFoundResult;

            //Assert
            Assert.IsNotNull(result);

            controller.Dispose();
            repo.Dispose();
        }

        #endregion


        #region POST

        [TestMethod]
        [TestCategory("ClientesController / Métodos")]
        public async Task Post_deverá_adicionar_o_cliente_e_retornar_o_caminho_do_mesmo()
        {
            //Arrange
            var cliente = new Cliente { Nome = "Novo Cliente", Sexo = CadCli.Dominio.Enums.Sexo.Masculino, Nascimento = new DateTime() };
            var repo = new ClienteRepositorioStub();
            var controller = new ClientesController(repo);

            //Action
            var result = await controller.Post(cliente) as CreatedNegotiatedContentResult<Cliente>;

            //Assert
            Assert.IsNotNull(result);
            //Assert.AreEqual(result.RouteName, "DefaultApi");
            //Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
            Assert.AreEqual(result.Content.Nome, cliente.Nome);

            controller.Dispose();
            repo.Dispose();
        }


        [TestMethod]
        [TestCategory("ClientesController / Métodos")]
        public async Task Post_deverá_falhar_qdo_o_cliente_for_nulo()
        {
            //Arrange
            Cliente cliente = null;
            var repo = new ClienteRepositorioStub();
            var controller = new ClientesController(repo);

            //Action
            var result = await controller.Post(cliente) as BadRequestErrorMessageResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Cliente não pode ser nulo", result.Message);

            controller.Dispose();
            repo.Dispose();
        }

        [TestMethod]
        [TestCategory("ClientesController / Métodos")]
        public async Task Post_deverá_falhar_qdo_o_cliente_já_possuir_um_Id()
        {
            //Arrange
            var cliente = new Cliente { Id = 1, Nome = "Novo Cliente", Sexo = CadCli.Dominio.Enums.Sexo.Masculino, Nascimento = new DateTime() };
            var repo = new ClienteRepositorioStub();
            var controller = new ClientesController(repo);

            //Action
            var result = await controller.Post(cliente) as BadRequestErrorMessageResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Id do cliente inválido", result.Message);

            controller.Dispose();
            repo.Dispose();
        }

        #endregion


        #region PUT

        [TestMethod]
        [TestCategory("ClientesController / Métodos")]
        public async Task Put_deverá_atualizar_o_cliente_e_retornar_NoContent()
        {
            //Arrange
            var repo = new ClienteRepositorioStub();
            var controller = new ClientesController(repo);
            var cliente = repo.ObterPorId(3);

            //Action
            var result = await controller.Put(cliente.Id, cliente) as StatusCodeResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);

            controller.Dispose();
            repo.Dispose();
        }

        [TestMethod]
        [TestCategory("ClientesController / Métodos")]
        public async Task Put_deverá_falhar_qdo_o_cliente_for_nulo()
        {
            //Arrange
            var id = 1;
            Cliente cliente = null;
            var repo = new ClienteRepositorioStub();
            var controller = new ClientesController(repo);

            //Action
            var result = await controller.Put(id, cliente) as BadRequestErrorMessageResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Cliente não pode ser nulo", result.Message);

            controller.Dispose();
            repo.Dispose();
        }

        [TestMethod]
        [TestCategory("ClientesController / Métodos")]
        public async Task Put_deverá_falhar_qdo_o_param_id_for_diferente_do_cliente_id()
        {
            //Arrange
            var id = 1;
            var repo = new ClienteRepositorioStub();
            var cliente = repo.ObterPorId(3);
            var controller = new ClientesController(repo);

            //Action
            var result = await controller.Put(id, cliente) as BadRequestErrorMessageResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Os id's não conferem", result.Message);

            controller.Dispose();
            repo.Dispose();
        }

        #endregion


        #region DELETE

        [TestMethod]
        [TestCategory("ClientesController / Métodos")]
        public async Task Delete_deverá_excluir_o_cliente_e_retornar_Ok()
        {
            //Arrange
            var id = 1;
            var repo = new ClienteRepositorioStub();
            var cliente = repo.ObterPorId(id);
            var controller = new ClientesController(repo);

            //Action
            var result = await controller.Delete(id) as OkNegotiatedContentResult<Cliente>;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNull(repo.ObterPorId(id));
            Assert.AreEqual(id, result.Content.Id);
            Assert.AreEqual(cliente, result.Content);

            controller.Dispose();
            repo.Dispose();
        }

        #endregion
    }

    /*
        "Stub é uma dependência da sua classe-alvo (Objeto em Teste), 
        agindo como um substituto, evitando que a implementação real seja executada."
    */
    public class ClienteRepositorioStub : IClienteRepositorio
    {

        public ClienteRepositorioStub()
        {
            Clientes = new List<Cliente> {
                    new Cliente { Id=1,Nome="Fulano da Silva",Sexo = CadCli.Dominio.Enums.Sexo.Masculino, Nascimento=new DateTime(1970,10,1) },
                    new Cliente { Id=2,Nome="Cicrana Pereira",Sexo = CadCli.Dominio.Enums.Sexo.Feminino, Nascimento=new DateTime(1977,1,12) },
                    new Cliente { Id=3,Nome="Beltrano Oliveira",Sexo = CadCli.Dominio.Enums.Sexo.Masculino, Nascimento=new DateTime(1999,8,20) },
                };

            _autoNumID = Clientes.Count + 1;
        }


        private int _autoNumID;

        public IList<Cliente> Clientes { get; private set; }

        public IEnumerable<Cliente> Todos()
        {
            return Clientes;
        }

        public Cliente ObterPorId(int id)
        {
            return Clientes.FirstOrDefault(d => d.Id == id);
        }

        public Cliente Salvar(Cliente cliente)
        {
            if (cliente.Id == 0)
            {
                var newId = _autoNumID;
                cliente.Id = newId;
                Clientes.Add(cliente);
                _autoNumID++;
            }
            else
            {
                var _cli = ObterPorId(cliente.Id);
                _cli = cliente;
            }

            return cliente;

        }

        public Cliente Excluir(int id)
        {
            var _cli = ObterPorId(id);
            Clientes.Remove(_cli);


            return _cli;
        }

        public void Dispose()
        {
            Clientes = null;
        }
    }
}
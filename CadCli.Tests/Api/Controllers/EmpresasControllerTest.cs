using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using CadCli.Servico.Api.Controllers;
using System.Web.Http.Results;
using System.Linq;
using CadCli.Dominio.Interfaces;
using CadCli.Dominio.Entidades;
using System.Collections.Generic;
using System.Net;

namespace CadCli.Tests.Api.Controllers
{
    [TestClass]
    public class EmpresasControllerTest
    {
        #region GET

        [TestMethod]
        [TestCategory("EmpresasController / Métodos")]
        public async Task Get_deverá_retornar_todos_as_empresas()
        {
            //Arrange
            var repo = new EmpresaRepositorioStub();
            var controller = new EmpresasController(repo);

            //Action
            var result = await controller.Get() as OkNegotiatedContentResult<List<Empresa>>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(repo.Todos().Count(), result.Content.Count);

            controller.Dispose();
            repo.Dispose();
        }

        [TestMethod]
        [TestCategory("EmpresasController / Métodos")]
        public async Task Get_com_id_deverá_retornar_a_empresa_desejada()
        {
            //Arrange
            var id = 1;
            var repo = new EmpresaRepositorioStub();
            var controller = new EmpresasController(repo);

            //Action
            var result = await controller.Get(id) as OkNegotiatedContentResult<Empresa>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(repo.ObterPorId(id), result.Content);

            controller.Dispose();
            repo.Dispose();
        }

        [TestMethod]
        [TestCategory("EmpresasController / Métodos")]
        public async Task Get_com_id_inexistente_deverá_retornar_NOTFOUND()
        {
            //Arrange
            var id = 4;
            var repo = new EmpresaRepositorioStub();
            var controller = new EmpresasController(repo);

            //Action
            var result = await controller.Get(id) as NotFoundResult;

            //Assert
            Assert.IsNotNull(result);

            controller.Dispose();
            repo.Dispose();
        }

        [TestMethod]
        [TestCategory("EmpresasController / Métodos")]
        public async Task Get_com_id_deverá_retornar_a_empresa_desejada_e_seus_clientes()
        {
            //Arrange
            var id = 1;
            var repo = new EmpresaRepositorioStub();
            var controller = new EmpresasController(repo);

            //Action
            var result = await controller.GetCli(id) as OkNegotiatedContentResult<Empresa>;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content.Clientes);
            Assert.AreEqual(repo.ObterPorId(id), result.Content);

            controller.Dispose();
            repo.Dispose();
        }

        #endregion

        #region POST

        [TestMethod]
        [TestCategory("EmpresasController / Métodos")]
        public async Task Post_deverá_adicionar_a_empresa_e_retornar_o_caminho_da_mesma()
        {
            //Arrange
            var empresa = new Empresa { Nome = "Nova Empresa",CNPJ="12345000177"};
            var repo = new EmpresaRepositorioStub();
            var controller = new EmpresasController(repo);

            //Action
            var result = await controller.Post(empresa) as CreatedNegotiatedContentResult<Empresa>;

            //Assert
            Assert.IsNotNull(result);
            //Assert.AreEqual(result.RouteName, "DefaultApi");
            //Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
            Assert.AreEqual(result.Content.Nome, empresa.Nome);

            controller.Dispose();
            repo.Dispose();
        }

        [TestMethod]
        [TestCategory("EmpresasController / Métodos")]
        public async Task Post_deverá_falhar_qdo_a_empresa_for_nulo()
        {
            //Arrange
            Empresa empresa = null;
            var repo = new EmpresaRepositorioStub();
            var controller = new EmpresasController(repo);

            //Action
            var result = await controller.Post(empresa) as BadRequestErrorMessageResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Empresa não pode ser nula", result.Message);

            controller.Dispose();
            repo.Dispose();
        }

        [TestMethod]
        [TestCategory("EmpresasController / Métodos")]
        public async Task Post_deverá_falhar_qdo_a_empresa_já_possuir_um_Id()
        {
            //Arrange
            var empresa = new Empresa { Id = 1, Nome = "Nova Empresa", CNPJ="12345000199"};
            var repo = new EmpresaRepositorioStub();
            var controller = new EmpresasController(repo);

            //Action
            var result = await controller.Post(empresa) as BadRequestErrorMessageResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Id da empresa inválido", result.Message);

            controller.Dispose();
            repo.Dispose();
        }

        #endregion

        #region PUT

        [TestMethod]
        [TestCategory("EmpresasController / Métodos")]
        public async Task Put_deverá_atualizar_a_empresa_e_retornar_NoContent()
        {
            //Arrange
            var repo = new EmpresaRepositorioStub();
            var controller = new EmpresasController(repo);
            var empresa = repo.ObterPorId(3);
            empresa.Nome = "empresa atualizada";

            //Action
            var result = await controller.Put(empresa.Id, empresa) as StatusCodeResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
            Assert.AreEqual("empresa atualizada", repo.ObterPorId(3).Nome);
            controller.Dispose();
            repo.Dispose();
        }

        [TestMethod]
        [TestCategory("EmpresasController / Métodos")]
        public async Task Put_deverá_falhar_qdo_a_empresa_for_nula()
        {
            //Arrange
            var id = 1;
            Empresa empresa = null;
            var repo = new EmpresaRepositorioStub();
            var controller = new EmpresasController(repo);

            //Action
            var result = await controller.Put(id, empresa) as BadRequestErrorMessageResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Empresa não pode ser nula", result.Message);

            controller.Dispose();
            repo.Dispose();
        }


        [TestMethod]
        [TestCategory("EmpresasController / Métodos")]
        public async Task Put_deverá_falhar_qdo_o_param_id_for_diferente_da_empresa_id()
        {
            //Arrange
            var id = 1;
            var repo = new EmpresaRepositorioStub();
            var empresa = repo.ObterPorId(3);
            var controller = new EmpresasController(repo);

            //Action
            var result = await controller.Put(id, empresa) as BadRequestErrorMessageResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Os id's não conferem", result.Message);

            controller.Dispose();
            repo.Dispose();
        }
        #endregion

        #region DELETE

        [TestMethod]
        [TestCategory("EmpresasController / Métodos")]
        public async Task Delete_deverá_excluir_a_empresa_e_retornar_Ok()
        {
            //Arrange
            var id = 1;
            var repo = new EmpresaRepositorioStub();
            var empresa = repo.ObterPorId(id);
            var controller = new EmpresasController(repo);

            //Action
            var result = await controller.Delete(id) as OkNegotiatedContentResult<Empresa>;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNull(repo.ObterPorId(id));
            Assert.AreEqual(id, result.Content.Id);
            Assert.AreEqual(empresa, result.Content);

            controller.Dispose();
            repo.Dispose();
        }

        #endregion
    }

    public class EmpresaRepositorioStub : IEmpresaRepositorio
    {

        public EmpresaRepositorioStub()
        {
            Empresas = new List<Empresa> {
                new Empresa {Id=1, Nome= "FANSOFT INF",CNPJ="10874124000175",
                    Clientes = new List<Cliente> {
                        new Cliente { Id=1,Nome="Fabiano Nalin",Sexo = CadCli.Dominio.Enums.Sexo.Masculino, Nascimento=new DateTime(1979,9,12)},
                        new Cliente { Id=2,Nome="Priscila Mitui",Sexo = CadCli.Dominio.Enums.Sexo.Feminino, Nascimento=new DateTime(1978,6,9)},
                    }
                },
                new Empresa {Id=2, Nome= "MICROSOFT DO BRASIL IMP. E COM. DE SOFT. E VIDEO GAMES LTDA.",CNPJ="04712500000107"},
                new Empresa {Id=3, Nome= "UNIAO EDUCACIONAL, CULTURAL E TECN IMPACTA - UNI.IMPACTA",CNPJ="59069914000151"},
            };
            _autoNum = 4;
        }

        private int _autoNum;

        public IList<Empresa> Empresas { get; private set; }

        public Empresa Excluir(int id)
        {
            var emp = ObterPorId(id);
            if (emp != null)
            {
                Empresas.Remove(emp);
            }
            return emp;
        }

        public Empresa ObterPorId(int id)
        {
            return Empresas.FirstOrDefault(d=>d.Id==id);
        }

        public Empresa ObterPorIdComClientes(int id) => ObterPorId(id);

        public Empresa Salvar(Empresa empresa)
        {
            if (empresa.Id == 0)
            {
                empresa.Id = _autoNum++;
                Empresas.Add(empresa);
            }
            else
            {
                var _emp = ObterPorId(empresa.Id);
                _emp = empresa;
            }

            return empresa;

        }

        public IEnumerable<Empresa> Todos()
        {
            return Empresas;
        }

        public void Dispose()
        {
            Empresas = null;
        }
    }
}

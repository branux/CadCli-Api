using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using CadCli.Dominio.Entidades;
using CadCli.Dominio.Enums;
using System.Linq;
using CadCli.Dominio.Interfaces;

namespace CadCli.Tests.Dominio.Repositorio
{
    [TestClass]
    public class IClienteRepositorioTest
    {
        [TestMethod]
        [TestCategory("IClienteRepositorio / Métodos")]
        public void Verificando_o_comportamento_do_método_ObterPorID()
        {
            //arrange
            var _id = 1;
            var mockRep = new Mock<CadCli.Dominio.Interfaces.IClienteRepositorio>();
            mockRep.Setup(mr => mr.ObterPorId(_id)).Returns(ClienteStub.ObterClientes.FirstOrDefault(d => d.Id == _id));

            //act
            var dado = mockRep.Object.ObterPorId(_id);

            Assert.AreEqual(dado.Id, _id);
        }

        [TestMethod]
        [TestCategory("IClienteRepositorio / Métodos")]
        public void Verificando_o_comportamento_do_método_Todos()
        {
            //arrange
            var mockRep = new Mock<IClienteRepositorio>();
            mockRep.Setup(mr => mr.Todos()).Returns(ClienteStub.ObterClientes);

            //act
            var dados = mockRep.Object.Todos();

            Assert.AreEqual(ClienteStub.ObterClientes.Count, dados.Count());
        }

        [TestMethod]
        [TestCategory("IClienteRepositorio / Métodos")]
        public void Verificando_o_comportamento_do_método_Excluir_cliente()
        {
            //arrange
            var id = 1;

            var mockRep = new Mock<IClienteRepositorio>();
            mockRep.Setup(mr => mr.Excluir(id)).Returns(ClienteStub.ObterClientes.First(d => d.Id == id));

            //act
            var dado = mockRep.Object.Excluir(id);

            //assert
            Assert.AreEqual(dado.Id, ClienteStub.ObterClientes.First(d => d.Id == id).Id);
        }

        [TestMethod]
        [TestCategory("IClienteRepositorio / Métodos")]
        public void Verificando_o_comportamento_do_método_Salvar_adicionando_cliente()
        {
            //arrange
            var id = 0;
            var cliente = new Cliente { Id = id, Nome = "teste de cliente", Sexo = Sexo.Masculino, Nascimento = new DateTime() };

            var mockRep = new Mock<IClienteRepositorio>();
            mockRep.Setup(mr => mr.Salvar(cliente)).Returns(() =>
            {
                cliente.Id = ClienteStub.ObterClientes.Count + 1;
                return cliente;
            });

            //act
            var dado = mockRep.Object.Salvar(cliente);

            //assert
            Assert.AreNotEqual(id, dado.Id);
            Assert.AreEqual(cliente.Nome, dado.Nome);
            Assert.AreEqual(cliente.Sexo, dado.Sexo);
            Assert.AreEqual(cliente.Nascimento, dado.Nascimento);
        }

        [TestMethod]
        [TestCategory("IClienteRepositorio / Métodos")]
        public void Verificando_o_comportamento_do_método_Salvar_atualizando_cliente()
        {
            //arrange
            var cliente = ClienteStub.ObterClientes.First();
            cliente.Nome += " - alt";

            var mockRep = new Mock<IClienteRepositorio>();
            mockRep.Setup(mr => mr.Salvar(cliente)).Returns(cliente);

            //act
            var dado = mockRep.Object.Salvar(cliente);

            //assert
            Assert.AreNotEqual(dado.Nome, ClienteStub.ObterClientes.First().Nome);
            Assert.AreEqual(dado.Id, ClienteStub.ObterClientes.First().Id);
            Assert.AreEqual(dado.Sexo, ClienteStub.ObterClientes.First().Sexo);
            Assert.AreEqual(dado.Nascimento, ClienteStub.ObterClientes.First().Nascimento);
        }

    }

    public class ClienteStub
    {

        public static IList<Cliente> ObterClientes
        {
            get
            {
                return new List<Cliente> {
                    new Cliente{Id=1,Nome="ID 1",Nascimento=new DateTime(1979,01,01),Sexo=Sexo.Feminino},
                    new Cliente{Id=2,Nome="ID 2",Nascimento=new DateTime(1966,10,11),Sexo=Sexo.Feminino},
                    new Cliente{Id=3,Nome="ID 3",Nascimento=new DateTime(1978,3,10),Sexo=Sexo.Masculino},
                };
            }
        }
    }
}

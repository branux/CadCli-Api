using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CadCli.Dominio.Entidades;
using Moq;
using CadCli.Dominio.Interfaces;
using System.Linq;

namespace CadCli.Tests.Dominio.Repositorio
{
    [TestClass]
    public class IEmpresaRepositorioTest
    {
        [TestMethod]
        [TestCategory("IEmpresaRepositorio / Métodos")]
        public void Verificando_o_comportamento_do_método_ObterPorID()
        {
            //arrange
            var _id = 1;
            var mockRep = new Mock<IEmpresaRepositorio>();
            mockRep.Setup(mr => mr.ObterPorId(_id)).Returns(EmpresaStub.ObterEmpresas.FirstOrDefault(d => d.Id == _id));

            //act
            var dado = mockRep.Object.ObterPorId(_id);

            Assert.AreEqual(dado.Id, _id);
        }

        [TestMethod]
        [TestCategory("IEmpresaRepositorio / Métodos")]
        public void Verificando_o_comportamento_do_método_Todos()
        {
            //arrange
            var mockRep = new Mock<IEmpresaRepositorio>();
            mockRep.Setup(mr => mr.Todos()).Returns(EmpresaStub.ObterEmpresas);

            //act
            var dados = mockRep.Object.Todos();

            Assert.AreEqual(EmpresaStub.ObterEmpresas.Count, dados.Count());
        }

        [TestMethod]
        [TestCategory("IEmpresaRepositorio / Métodos")]
        public void Verificando_o_comportamento_do_método_Excluir_empresa()
        {
            //arrange
            var id = 1;

            var mockRep = new Mock<IEmpresaRepositorio>();
            mockRep.Setup(mr => mr.Excluir(id)).Returns(EmpresaStub.ObterEmpresas.First(d => d.Id == id));

            //act
            var dado = mockRep.Object.Excluir(id);

            //assert
            Assert.AreEqual(dado.Id, EmpresaStub.ObterEmpresas.First(d => d.Id == id).Id);
        }

        [TestMethod]
        [TestCategory("IEmpresaRepositorio / Métodos")]
        public void Verificando_o_comportamento_do_método_Salvar_adicionando_empresa()
        {
            //arrange
            var id = 0;
            var empresa = new Empresa { Id = id, Nome = "teste de empresa", CNPJ = "10298199000122"};

            var mockRep = new Mock<IEmpresaRepositorio>();
            mockRep.Setup(mr => mr.Salvar(empresa)).Returns(() =>
            {
                empresa.Id = EmpresaStub.ObterEmpresas.Count + 1;
                return empresa;
            });

            //act
            var dado = mockRep.Object.Salvar(empresa);

            //assert
            Assert.AreNotEqual(id, dado.Id);
            Assert.AreEqual(empresa.Nome, dado.Nome);
            Assert.AreEqual(empresa.CNPJ, dado.CNPJ);
        }

        [TestMethod]
        [TestCategory("IEmpresaRepositorio / Métodos")]
        public void Verificando_o_comportamento_do_método_Salvar_atualizando_empresa()
        {
            //arrange
            var empresa = EmpresaStub.ObterEmpresas.First();
            empresa.Nome += " - alt";

            var mockRep = new Mock<IEmpresaRepositorio>();
            mockRep.Setup(mr => mr.Salvar(empresa)).Returns(empresa);

            //act
            var dado = mockRep.Object.Salvar(empresa);

            //assert
            Assert.AreNotEqual(dado.Nome, EmpresaStub.ObterEmpresas.First().Nome);
            Assert.AreEqual(dado.Id, EmpresaStub.ObterEmpresas.First().Id);
            Assert.AreEqual(dado.CNPJ, EmpresaStub.ObterEmpresas.First().CNPJ);
        }
    }

    public class EmpresaStub
    {

        public static IList<Empresa> ObterEmpresas
        {
            get
            {
                return new List<Empresa> {
                    new Empresa{Id=1,Nome="Empresa ID 1",CNPJ="99102000122"},
                    new Empresa{Id=2,Nome="Empresa ID 2",CNPJ="99101000220"},
                    new Empresa{Id=3,Nome="Empresa ID 3",CNPJ="89102000120"},
                };
            }
        }
    }
}

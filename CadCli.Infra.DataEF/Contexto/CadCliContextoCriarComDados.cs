using CadCli.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace CadCli.Infra.DataEF.Contexto
{
    public class CadCliContextoCriarComDados : CreateDatabaseIfNotExists<CadCliContexto>
    {
        protected override void Seed(CadCliContexto context)
        {
            var clientes = new List<Cliente> {
                new Cliente {
                    Nome ="Fabiano Nalin",Nascimento=new DateTime(1979,9,12),Sexo=Dominio.Enums.Sexo.Masculino,
                    Empresa = new Empresa { Nome= "FANSOFT INF",CNPJ="10874124000175"}
                },
                new Cliente { Nome="Priscila Mitui",Nascimento=new DateTime(1978,6,09),Sexo=Dominio.Enums.Sexo.Feminino},
                new Cliente { Nome="Raphael Nalin",Nascimento=new DateTime(1999,8,20),Sexo=Dominio.Enums.Sexo.Masculino},
            };

            foreach (var cli in clientes)
                context.Clientes.Add(cli);

            base.Seed(context);
        }
    }
}

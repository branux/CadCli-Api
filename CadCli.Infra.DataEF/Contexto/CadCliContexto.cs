using CadCli.Dominio.Entidades;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace CadCli.Infra.DataEF.Contexto
{
    public class CadCliContexto : DbContext
    {
        public CadCliContexto()
            : base("Name=CadCliDB")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        static CadCliContexto()
        {
            Database.SetInitializer(new CadCliContextoCriarComDados());
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public object Empresa { get; internal set; }


        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Obtendo a lista de erros
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // transformando em uma string única
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Juntando ex.Message com os erros de validação.
                var exceptionMessage = string.Concat(ex.Message, " Erros na validação: ", fullErrorMessage);

                // Throw DbEntityValidationException com os erros de validação.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Map.ClienteMap());
            modelBuilder.Configurations.Add(new Map.EmpresaMap());
        }
    }
}

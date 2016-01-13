using CadCli.Dominio.Entidades;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace CadCli.Infra.DataEF.Map
{
    public class ClienteMap : EntityTypeConfiguration<Cliente>
    {
        public ClienteMap()
        {
            //Tabela
            ToTable(nameof(Cliente));

            //PK
            HasKey(k => k.Id);

            //Campos
            Property(c => c.Id).HasColumnName("ID")
                .HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.Nome).HasColumnName("Nome")
                .HasColumnType("varchar").HasMaxLength(100).IsRequired()
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("UQ_dbo.Cliente.Nome-Nascimento", 0) { IsUnique = true }));

            Property(c => c.Nascimento).HasColumnName("Nascimento").HasColumnType("date").IsRequired()
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("UQ_dbo.Cliente.Nome-Nascimento", 1) { IsUnique = true }));

            Property(c => c.Sexo).HasColumnName("Sexo").HasColumnType("int").IsRequired();

            HasOptional(s => s.Empresa)
                .WithMany(s => s.Clientes)
                .HasForeignKey(s => s.EmpresaId);
        }
    }
}

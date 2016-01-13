using CadCli.Dominio.Entidades;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace CadCli.Infra.DataEF.Map
{
    public class EmpresaMap:EntityTypeConfiguration<Empresa>
    {
        public EmpresaMap()
        {

            //Tabela
            ToTable(nameof(Empresa));

            //PK
            HasKey(k=>k.Id);

            //Campos
            Property(c => c.Id).HasColumnName("ID")
               .HasColumnType("int")
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.Nome).HasColumnName("Nome")
                .HasColumnType("varchar").HasMaxLength(100).IsRequired();

            Property(c => c.Id).HasColumnName("ID")
               .HasColumnType("int")
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.CNPJ).HasColumnName("CNPJ")
                .HasColumnType("char").HasMaxLength(14).IsRequired()
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("UQ_dbo.Empresa.CNPJ", 0) { IsUnique = true }));

        }
    }
}

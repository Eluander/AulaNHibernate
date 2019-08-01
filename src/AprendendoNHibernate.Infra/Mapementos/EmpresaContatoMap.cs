using AprendendoNHibernate.Domain.Entidades;
using FluentNHibernate.Mapping;

namespace AprendendoNHibernate.Infra.Mapementos
{
    public class EmpresaContatoMap : ClassMap<EmpresaContato>
    {
        public EmpresaContatoMap()
        {
            Table("empresa_contato"); // Aqui posso colocar o nome exatamente como quero que fique no base de dados.

            Id(x => x.Id);

            References(x => x.Empresa) // Assim passamos a referencia pra um objeto.
                .ForeignKey("FK_Empresa_EmpresaContato") // Definindo o nome da Foreign Key.
                .Not.Nullable();

            Map(x => x.Contato)
                .Not.Nullable()
                .Length(50);
        }
    }
}

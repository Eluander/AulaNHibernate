using AprendendoNHibernate.Domain.Entidades;
using FluentNHibernate.Mapping;

namespace AprendendoNHibernate.Infra.Mapementos
{
    public class EmpresaMap : ClassMap<Empresa>
    {
        public EmpresaMap()
        {
            Table("empresa"); // Nome da tabela.

            Id(x => x.Id);

            Map(x => x.RazaoSocial)
                .Not.Nullable()
                .Length(30);

            References(x => x.Coordenador);

            HasMany(x => x.EmpresaContatos) // Assim faço o mapeamento 1*N
                .ForeignKeyConstraintName("FK_Empresa_EmpresaContato")
                .Cascade.AllDeleteOrphan(); //Faz um cascade com todas as ações e deleta os contatos que não tiver empresa vinculada.
        }
    }
}
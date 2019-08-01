using AprendendoNHibernate.Domain.Entidades;
using FluentNHibernate.Mapping;

namespace AprendendoNHibernate.Infra.Mapementos
{
    public class PessoaMap : ClassMap<Pessoa>
    {
        public PessoaMap()
        {
            Table("pessoa"); // Nome da Tabela

            Id(x => x.Id); // Assim especificamos o indentificador da tabela.

            Map(x => x.Nome)
                .Not.Nullable()  // Este faz com que o nome NÃO possa ser NULL.
                .Length(20);     // Define o tamanho de 20 caracteres.

            Map(x => x.Sobrenome)
                .Length(30);
        }
    }
}

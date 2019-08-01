using System;

namespace AprendendoNHibernate.Domain.Entidades
{
    public class Pessoa
    {
        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Sobrenome { get; set; }

        public Pessoa()
        {
        }

        public Pessoa(long id, string nome, string sobrenome)
        {
            Id = id;
            Nome = nome;
            Sobrenome = sobrenome;
        }
    }
}
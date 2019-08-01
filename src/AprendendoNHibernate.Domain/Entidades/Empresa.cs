using System.Collections.Generic;

namespace AprendendoNHibernate.Domain.Entidades
{
    public class Empresa
    {
        public virtual long Id { get; set; }
        public virtual string RazaoSocial { get; set; }
        public virtual Pessoa Coordenador { get; set; }
        public virtual IList<EmpresaContato> EmpresaContatos { get; set; }


        public Empresa()
        {
            Coordenador = Coordenador ?? new Pessoa();
            EmpresaContatos = EmpresaContatos ?? new List<EmpresaContato>();
        }

        public Empresa(long id, string razaoSocial, Pessoa coordenador)
        {
            Id = id;
            RazaoSocial = razaoSocial;
            Coordenador = coordenador;
            EmpresaContatos = new List<EmpresaContato>();
        }
    }
}

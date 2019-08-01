namespace AprendendoNHibernate.Domain.Entidades
{
    public class EmpresaContato
    {
        public virtual int Id { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual string Contato { get; set; }

        public EmpresaContato()
        {
        }
        public EmpresaContato(int id, Empresa empresa, string contato)
        {
            Id = id;
            Empresa = empresa;
            Contato = contato;
        }
    }
}
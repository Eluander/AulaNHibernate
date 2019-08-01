using AprendendoNHibernate.Domain.Entidades;
using AprendendoNHibernate.Infra.Mapementos;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.Collections.Generic;

namespace AprendendoNHibernate.Test
{
    [TestClass]
    public class PrimeiroMapeamento_Teste_02
    {
        private ISession _session;

        public PrimeiroMapeamento_Teste_02()
        {
            // Esta é a configuração para conexão com a base de dados.
            // Neste exemplo estamos usando o Banco do PostgreSQL.
            // Detalhe, assim não vamos criar a base de dados apenas manipular a base com nome 'NHibernate', então certifique-se de que a base estaja criada.

            _session = Fluently.Configure()
               .Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString("User ID=postgres;Password=Fime2404;Host=192.168.0.156;Port=5432;Database=NHibernate;"))
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true)) // o parametro TRUE aqui ao lado, faz com que ele percorra todos os Mapeamentos criados no projeto 'Infra' e atualize a base de dados.
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<PessoaMap>()) // Assim estou informando que quero usar o assembly onde este PessoaMap e mapear tudos que existem lá.
                .BuildSessionFactory()? //Aqui possi obter a interface do ISessionFactory.
                .OpenSession(); // Aqui por fim abriremos a session com o banco.
        }

        [TestInitialize]
        public void Iniciar()
        {
            // validar se a conexão esta aberta.
            Assert.IsTrue(_session.IsConnected);
        }

        /// <summary>
        /// Vamos adicionar 1 Registro na tabela Empresa.
        /// </summary>
        [TestMethod]
        public void Insert()
        {
            // Vamos buscar uma pessoa qualquer cadastrada na tabela Pessoa para adiciona-la como Coordenador da empresa.
            Pessoa coordenador = _session.QueryOver<Pessoa>().Take(1).SingleOrDefault();

            // Instanciar a Empresa e adicionar o coordenador obtido na busca.
            Empresa empresa = new Empresa(1, "Super Empresa", coordenador);

            // Fazer o Insert com transaction.
            var transaction = _session.BeginTransaction();
            _session.Save(empresa);
            transaction.Commit();
        }

        /// <summary>
        /// Vamos atualizar a empresa adicionando dois novos contatos para ela.
        /// </summary>
        [TestMethod]
        public void AtualizarContatoEmpresa_Merge()
        {
            // Get da empresa cadastrada.
            Empresa empresa = _session.Get<Empresa>((long)1);
            empresa.EmpresaContatos.Add(new EmpresaContato(1, empresa, "(99) 99999-9999"));
            empresa.EmpresaContatos.Add(new EmpresaContato(2, empresa, "atendimento@superempresa.com.br"));

            // Fazer o Insert com transaction.
            var transaction = _session.BeginTransaction();
            _session.Merge(empresa); // Perceba que o valor empresa vai alterando os contatos conforme vou fazendo os ADD... Por isso preciso fazer o merge..
            _session.Flush();

            transaction.Commit();
        }

        ///// <summary>
        ///// Vamos atualizar a empresa adicionando dois novos contatos para ela.
        ///// </summary>
        //[TestMethod]
        //public void AtualizarContatoEmpresa_SaveUpdate()
        //{
        //    // Get da empresa cadastrada.
        //    Empresa empresa = _session.Get<Empresa>((long)1);

        //    List<EmpresaContato> contatos = new List<EmpresaContato>();
        //    contatos.AddRange(empresa.EmpresaContatos);
        //    contatos.Add(new EmpresaContato(3, empresa, "(11) 1111-1111"));
        //    contatos.Add(new EmpresaContato(4, empresa, "rh@superempresa.com.br"));

        //    empresa.EmpresaContatos = contatos;

        //    // Fazer o Insert com transaction.
        //    var transaction = _session.BeginTransaction();
        //    _session.SaveOrUpdate(empresa); // Perceba que o valor empresa vai alterando os contatos conforme vou fazendo os ADD... Por isso preciso fazer o merge..
        //    _session.Flush();
        //    transaction.Commit();
        //}

        /// <summary>
        /// Listar Contatos atravez da Empresa e listar pela tabel EmpresaContato.
        /// </summary>
        [TestMethod]
        public void Lista()
        {
            Empresa empresa = _session.Get<Empresa>((long)1);

            // Obter lista de Pessoas cadastradas.
            IList<EmpresaContato> contatos = _session.QueryOver<EmpresaContato>()
                .Where(x => x.Empresa.Id == empresa.Id)
                .List();

            Assert.IsTrue(empresa.EmpresaContatos.Count == contatos.Count);
        }
    }
}

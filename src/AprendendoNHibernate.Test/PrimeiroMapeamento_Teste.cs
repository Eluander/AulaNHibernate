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
    public class PrimeiroMapeamento_Teste
    {
        private ISession _session;

        public PrimeiroMapeamento_Teste()
        {
            // Esta é a configuração para conexão com a base de dados.
            // Neste exemplo estamos usando o Banco do PostgreSQL.
            // Detalhe, assim não vamos criar a base de dados apenas manipular a base com nome 'NHibernate', então certifique-se de que a base estaja criada.

            _session = Fluently.Configure()
               .Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString("User ID=postgres;Password=123456;Host=192.168.0.5;Port=5432;Database=NHibernate;"))
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
        /// Vamos adicionar 1 Registro na tabela pessoa.
        /// </summary>
        [TestMethod]
        public void Insert()
        {
            // Instanciar a Pessoa.
            Pessoa pessoa1 = new Pessoa(1, "Ana", "Mari da Silva");

            // Fazer o Insert com transaction.
            var transaction = _session.BeginTransaction();
            _session.Save(pessoa1);
            transaction.Commit();

        }

        /// <summary>
        /// Adiionar uma lista de Pessoas.
        /// </summary>
        [TestMethod]
        public void InsertMultiplas()
        {
            // Instanciar a Pessoa.
            Pessoa pessoa2 = new Pessoa(2, "João", "Pereira");
            // Instanciar a Pessoa.
            Pessoa pessoa3 = new Pessoa(3, "Eluander J.", "F. Lopes");

            // Fazer o Insert com transaction.
            var transaction = _session.BeginTransaction();
            _session.Save(pessoa2);
            _session.Save(pessoa3);
            transaction.Commit();
        }

        /// <summary>
        /// Vamos obter a Pessoa cadastrada anteriormente.
        /// </summary>
        [TestMethod]
        public void Get()
        {
            // Obter a pessoa.
            Pessoa pessoa1 = _session.Get<Pessoa>((long)1);

            Assert.IsNotNull(pessoa1);
        }

        /// <summary>
        /// Vamos obter a pessoa para corrigir o sobrenome que foi cadastrado errado.
        /// </summary>
        [TestMethod]
        public void Update()
        {
            // Obter a Pessoa.
            Pessoa pessoa1 = _session.Get<Pessoa>((long)1);
            pessoa1.Sobrenome = "Maria da Silva";

            // Fazer o Update com transaction.
            var transaction = _session.BeginTransaction();
            _session.Update(pessoa1);
            transaction.Commit();

        }

        /// <summary>
        /// Deletar a pessoa1 cadastrada.
        /// </summary>
        [TestMethod]
        public void Delete()
        {
            // Obter a Pessoa.
            Pessoa pessoa1 = _session.Get<Pessoa>((long)1);

            // Fazer o Update com transaction.
            var transaction = _session.BeginTransaction();
            _session.Delete(pessoa1);
            transaction.Commit();
        }

        /// <summary>
        /// Listar Pessoas cadastradas.
        /// </summary>
        [TestMethod]
        public void Lista()
        {
            // Obter lista de Pessoas cadastradas.
            IList<Pessoa> pessoas = _session.QueryOver<Pessoa>().List();

            Assert.AreEqual(pessoas.Count, 2);
        }

        /// <summary>
        /// Obter 1 pessoa da lista atravéz do Id.
        /// </summary>
        [TestMethod]
        public void Where()
        {
            // Obter lista de Pessoas cadastradas.
            Pessoa pessoas = _session.QueryOver<Pessoa>().Where(x => x.Id == 3).SingleOrDefault();

            Assert.AreEqual(pessoas.Nome, "Eluander J.");
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlePonto.Domain.usuario;
using ControlePonto.Tests.mocks;
using ControlePonto.Domain.framework;
using ControlePonto.Infrastructure.utils;

namespace ControlePonto.Tests
{
    [TestClass]
    public class UsuarioTests
    {
        IUsuarioRepositorio usuarioRepositorio;
        UsuarioFactory factory;

        [TestInitialize]
        public void SetupTest()
        {
            usuarioRepositorio = new UsuarioMockRepositorio();
            factory = new UsuarioFactory(new LoginJaExisteSpecification(usuarioRepositorio), new LoginValidoSpecification(), new SenhaValidaSpecification());

            usuarioRepositorio.save(factory.criarUsuario("João", "joaozinho", "123456"));
        }

        [TestMethod]
        public void testCriarUsuario()
        {
            usuarioRepositorio.save(
                factory.criarUsuario("Guilherme", "guilherme_latrova", "latrova123")
            );

            Assert.IsNotNull(usuarioRepositorio.findByLogin("guilherme_latrova"));
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testLoginInvalido()
        {
            factory.criarUsuario("Guilherme", "guilherme#latrova@!-", "123123");
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testSenhaInvalida()
        {
            factory.criarUsuario("Guilherme", "latrova", "123_123");
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(LoginJaExisteException))]
        public void testLoginEmUso()
        {
            factory.criarUsuario("Mario", "joaozinho", "111111");
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testUsuarioSemNome()
        {
            factory.criarUsuario("", "joaozinho", "123123");
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testUsuarioSemLogin()
        {
            factory.criarUsuario("Mario", "", "123123");
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testUsuarioSemSenha()
        {
            factory.criarUsuario("Mario", "joaozinho", "");
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testCriarUsuarioExcedeuNome()
        {
            factory.criarUsuario(
                new string('a', Usuario.MAX_NOME_LENGTH + 1),
                new string('b', Usuario.MAX_LOGIN_LENGTH),
                new string('c', Usuario.MAX_SENHA_LENGTH));
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testCriarUsuarioExcedeuLogin()
        {
            factory.criarUsuario(
                new string('a', Usuario.MAX_NOME_LENGTH),
                new string('b', Usuario.MAX_LOGIN_LENGTH + 1),
                new string('c', Usuario.MAX_SENHA_LENGTH));
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testCriarUsuarioExcedeuSenha()
        {
            factory.criarUsuario(
                new string('a', Usuario.MAX_NOME_LENGTH),
                new string('b', Usuario.MAX_LOGIN_LENGTH),
                new string('c', Usuario.MAX_SENHA_LENGTH + 1));
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testCriarUsuarioNomePequeno()
        {
            factory.criarUsuario(
                new string('a', Usuario.MIN_NOME_LENGTH - 1),
                new string('b', Usuario.MIN_LOGIN_LENGTH),
                new string('c', Usuario.MIN_SENHA_LENGTH));
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testCriarUsuarioLoginPequeno()
        {
            factory.criarUsuario(
                new string('a', Usuario.MIN_NOME_LENGTH),
                new string('b', Usuario.MIN_LOGIN_LENGTH - 1),
                new string('c', Usuario.MIN_SENHA_LENGTH));
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testCriarUsuarioSenhaPequena()
        {
            factory.criarUsuario(
                new string('a', Usuario.MIN_NOME_LENGTH),
                new string('b', Usuario.MIN_LOGIN_LENGTH),
                new string('c', Usuario.MIN_SENHA_LENGTH - 1));
        }

        [TestMethod, TestCategory("Construtor")]
        [ExpectedException(typeof(PreconditionException))]
        public void criarUsuarioSomenteNaFactory()
        {
            Usuario u = new Usuario(new String('a', 100), new String('a', 100), new String('a', 100));
        }

    }
}

using GeoMottuMinimalApi.Domain.Entities;

namespace GeoMottuMinimalApi.Tests.Domain.Entities
{
    public class UsuarioEntityTests
    {
        private UsuarioEntity CriarUsuarioValido()
        {
            return new UsuarioEntity
            {
                Nome = "Usuário de Teste",
                Email = "teste.valido@mottu.com",
                Senha = "UmaSenhaForte!123",
                Role = "USER",
                FilialId = 1,
                CadastradoEm = DateTime.Now
            };
        }

        [Fact(DisplayName = "Usuario com todos os dados válidos deve passar")]
        [Trait("Entity", "Usuario")]
        public void UsuarioValidoDeveSerValido()
        {
            var usuario = CriarUsuarioValido();

            var results = ValidationHelper.ValidateObject(usuario);

            Assert.Empty(results);
        }

        [Fact(DisplayName = "Usuario sem Nome deve retornar erro 'Required'")]
        [Trait("Entity", "Usuario")]
        public void UsuarioSemNomeDeveRetornarErroRequired()
        {
            var usuario = CriarUsuarioValido();
            usuario.Nome = string.Empty;

            var results = ValidationHelper.ValidateObject(usuario);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Nome") &&
                r.ErrorMessage!.Contains("required")
            );
        }

        [Fact(DisplayName = "Usuario com Nome muito curto deve retornar erro 'MinLength'")]
        [Trait("Entity", "Usuario")]
        public void UsuarioComNomeMuitoCurtoDeveRetornarErroMinLength()
        {
            var usuario = CriarUsuarioValido();
            usuario.Nome = "Al";

            var results = ValidationHelper.ValidateObject(usuario);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Nome") &&
                r.ErrorMessage!.Contains("length")
            );
        }

        [Fact(DisplayName = "Usuario com Nome muito longo deve retornar erro 'MaxLength'")]
        [Trait("Entity", "Usuario")]
        public void UsuarioComNomeMuitoLongoDeveRetornarErroMaxLength()
        {
            var usuario = CriarUsuarioValido();
            usuario.Nome = new string('A', 101);

            var results = ValidationHelper.ValidateObject(usuario);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Nome") &&
                r.ErrorMessage!.Contains("maximum length")
            );
        }

        [Fact(DisplayName = "Usuario sem Email deve retornar erro 'Required'")]
        [Trait("Entity", "Usuario")]
        public void UsuarioSemEmailDeveRetornarErroRequired()
        {
            var usuario = CriarUsuarioValido();
            usuario.Email = string.Empty;

            var results = ValidationHelper.ValidateObject(usuario);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Email") &&
                r.ErrorMessage!.Contains("required")
            );
        }

        [Fact(DisplayName = "Usuario com Email em formato inválido deve retornar erro 'EmailAddress'")]
        [Trait("Entity", "Usuario")]
        public void UsuarioComEmailInvalidoDeveRetornarErroEmailAddress()
        {
            var usuario = CriarUsuarioValido();
            usuario.Email = "email-invalido";

            var results = ValidationHelper.ValidateObject(usuario);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Email") &&
                r.ErrorMessage!.Contains("formato válido")
            );
        }

        [Fact(DisplayName = "Usuario com Email muito longo deve retornar erro 'MaxLength'")]
        [Trait("Entity", "Usuario")]
        public void Usuario_EmailLongo_DeveRetornarErroMaxLength()
        {
            var usuario = CriarUsuarioValido();
            usuario.Email = new string('A', 141) + "@teste.com";

            var results = ValidationHelper.ValidateObject(usuario);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Email") &&
                r.ErrorMessage!.Contains("length")
            );
        }

        [Fact(DisplayName = "Usuario sem Senha deve retornar erro 'Required'")]
        [Trait("Entity", "Usuario")]
        public void UsuarioSemSenhaDeveRetornarErroRequired()
        {
            var usuario = CriarUsuarioValido();
            usuario.Senha = string.Empty;

            var results = ValidationHelper.ValidateObject(usuario);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Senha") &&
                r.ErrorMessage!.Contains("required")
            );
        }

        [Fact(DisplayName = "Usuario sem Role deve retornar erro 'Required'")]
        [Trait("Entity", "Usuario")]
        public void UsuarioSemRoleDeveRetornarErroRequired()
        {
            var usuario = CriarUsuarioValido();
            usuario.Role = string.Empty;

            var results = ValidationHelper.ValidateObject(usuario);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Role") &&
                r.ErrorMessage!.Contains("required")
            );
        }
    }
}

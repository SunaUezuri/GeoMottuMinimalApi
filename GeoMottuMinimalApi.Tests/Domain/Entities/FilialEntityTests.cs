using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;

namespace GeoMottuMinimalApi.Tests.Domain.Entities
{
    public class FilialEntityTests
    {
        private FilialEntity CriarFilialValida()
        {
            return new FilialEntity
            {
                Nome = "Filial SP",
                PaisFilial = PaisesFiliais.Brasil,
                Estado = "SP",
                Endereco = "Avenida Paulista, 1000",
                CriadoEm = DateTime.Now
            };
        }

        [Fact(DisplayName = "Deve validar Filial com dados corretos e passar")]
        [Trait("Entity", "Filial")]
        public void FilialValidaDevePassarValidacao()
        {
            var filial = CriarFilialValida();

            var resultados = ValidationHelper.ValidateObject(filial);

            Assert.Empty(resultados);
        }

        [Fact(DisplayName = "Filial sem Nome deve retornar erro 'Required'")]
        [Trait("Entity", "Filial")]
        public void FilialSemNomeDeveRetornarErroRequired()
        {
            var filial = CriarFilialValida();
            filial.Nome = string.Empty; // Inválido

            var results = ValidationHelper.ValidateObject(filial);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Nome") &&
                r.ErrorMessage!.Contains("required")
            );
        }

        [Fact(DisplayName = "Filial com Nome muito longo deve retornar erro 'StringLength'")]
        [Trait("Entity", "Filial")]
        public void FilialNomeLongoDeveRetornarErroStringLength()
        {
            var filial = CriarFilialValida();
            filial.Nome = new string('A', 51);

            var results = ValidationHelper.ValidateObject(filial);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Nome") &&
                r.ErrorMessage!.Contains("length")
            );
        }

        [Fact(DisplayName = "Filial sem Estado deve retornar erro 'Required'")]
        [Trait("Entity", "Filial")]
        public void FilialSemEstadoDeveRetornarErroRequired()
        {
            var filial = CriarFilialValida();
            filial.Estado = null;

            var results = ValidationHelper.ValidateObject(filial);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Estado") &&
                r.ErrorMessage!.Contains("required")
            );
        }

        [Fact(DisplayName = "Filial com Estado muito longo deve retornar erro 'StringLength'")]
        [Trait("Entity", "Filial")]
        public void FilialEstadoLongoDeveRetornarErroStringLength()
        {
            var filial = CriarFilialValida();
            filial.Estado = new string('A', 51);

            var results = ValidationHelper.ValidateObject(filial);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Estado") &&
                r.ErrorMessage!.Contains("length")
            );
        }

        [Fact(DisplayName = "Filial sem Endereco deve retornar erro 'Required'")]
        [Trait("Entity", "Filial")]
        public void FilialSemEnderecoDeveRetornarErroRequired()
        {
            var filial = CriarFilialValida();
            filial.Endereco = string.Empty;

            var results = ValidationHelper.ValidateObject(filial);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Endereco") &&
                r.ErrorMessage!.Contains("required")
            );
        }

        [Fact(DisplayName = "Filial com Endereco muito longo deve retornar erro 'StringLength'")]
        [Trait("Entity", "Filial")]
        public void FilialEnderecoLongoDeveRetornarErroStringLength()
        {
            var filial = CriarFilialValida();
            filial.Endereco = new string('A', 151);

            var results = ValidationHelper.ValidateObject(filial);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Endereco") &&
                r.ErrorMessage!.Contains("length")
            );
        }
    }
}

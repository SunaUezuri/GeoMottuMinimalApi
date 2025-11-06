using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;

namespace GeoMottuMinimalApi.Tests.Domain.Entities
{
    public class MotoEntityTests
    {
        private MotoEntity CriarMotoValida()
        {
            return new MotoEntity
            {
                Placa = "ABC1234",
                Chassi = "123456789ABCDEFGH",
                CodPlacaIot = "IOT-12345",
                Modelo = ModeloMoto.MottuSport,
                Motor = 125.0,
                Proprietario = "Moto Mottu",
                PatioId = 1,
                CriadoEm = DateTime.Now
            };
        }

        [Fact(DisplayName = "Moto com todos os dados válidos deve passar")]
        [Trait("Entity", "Moto")]
        public void MotoValidaDeveSerValido()
        {
            var moto = CriarMotoValida();

            var results = ValidationHelper.ValidateObject(moto);

            Assert.Empty(results);
        }


        [Fact(DisplayName = "Moto com Placa muito longa deve retornar erro 'StringLength'")]
        [Trait("Entity", "Moto")]
        public void MotoPlacaLongaDeveRetornarErroStringLength()
        {

            var moto = CriarMotoValida();
            moto.Placa = "ABC12345678";

            var results = ValidationHelper.ValidateObject(moto);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Placa") &&
                r.ErrorMessage!.Contains("length")
            );
        }

        [Fact(DisplayName = "Moto sem Chassi deve retornar erro 'Required'")]
        [Trait("Entity", "Moto")]
        public void MotoSemChassiDeveRetornarErroRequired()
        {
            var moto = CriarMotoValida();
            moto.Chassi = null;

            var results = ValidationHelper.ValidateObject(moto);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Chassi") &&
                r.ErrorMessage!.Contains("required")
            );
        }

        [Fact(DisplayName = "Moto com Chassi muito longo deve retornar erro 'StringLength'")]
        [Trait("Entity", "Moto")]
        public void MotoChassiLongoDeveRetornarErroStringLength()
        {
            var moto = CriarMotoValida();
            moto.Chassi = "123456789ABCDEFGHJ";

            var results = ValidationHelper.ValidateObject(moto);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Chassi") &&
                r.ErrorMessage!.Contains("length")
            );
        }

        [Fact(DisplayName = "Moto com CodPlacaIot muito longo deve retornar erro 'MaxLength'")]
        [Trait("Entity", "Moto")]
        public void MotoCodPlacaIotLongoDeveRetornarErroMaxLength()
        {
            var moto = CriarMotoValida();
            moto.CodPlacaIot = new string('A', 51);

            var results = ValidationHelper.ValidateObject(moto);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("CodPlacaIot") &&
                r.ErrorMessage!.Contains("length")
            );
        }

        [Fact(DisplayName = "Moto com Proprietario muito longo deve retornar erro 'StringLength'")]
        [Trait("Entity", "Moto")]
        public void MotoProprietarioLongoDeveRetornarErroStringLength()
        {
            var moto = CriarMotoValida();
            moto.Proprietario = new string('A', 151);

            var results = ValidationHelper.ValidateObject(moto);

            Assert.Single(results);
            Assert.Contains(results, r =>
                r.MemberNames.Contains("Proprietario") &&
                r.ErrorMessage!.Contains("length")
            );
        }

    }
}

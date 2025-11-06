using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoMottuMinimalApi.Tests.Domain.Entities
{
    public class PatioEntityTests
    {

        private PatioEntity CriarPatioValido()
        {
            return new PatioEntity
            {
                CapacidadeTotal = 100,
                LocalizacaoReferencia = "Perto da entrada principal",
                Tamanho = 500.50,
                TipoDoPatio = TipoPatio.Disponivel,
                FilialId = 1,
                CriadoEm = DateTime.Now
            };
        }

        [Fact(DisplayName = "Patio com todos os dados válidos deve passar")]
        [Trait("Entity", "Patio")]
        public void PatioValidoDeveSerValido()
        {
            var patio = CriarPatioValido();

            var results = ValidationHelper.ValidateObject(patio);

            Assert.Empty(results);
        }

        [Fact(DisplayName = "Patio com LocalizacaoReferencia muito longa deve retornar erro 'MaxLength'")]
        [Trait("Entity", "Patio")]
        public void PatioComLocalizacaoReferenciaMuitoLongaDeveRetornarErroMaxLength()
        {
            var patio = CriarPatioValido();
            patio.LocalizacaoReferencia = new string('A', 101);

            var results = ValidationHelper.ValidateObject(patio);

            Assert.Single(results);
            Assert.Contains(results, r => 
                r.MemberNames.Contains("LocalizacaoReferencia") && 
                r.ErrorMessage!.Contains("length"));
        }

        [Fact(DisplayName = "Patio com LocalizacaoReferencia nula deve passar (não é obrigatória)")]
        [Trait("Entity", "Patio")]
        public void PatioComLocalizacaoReferenciaNulaDevePassar()
        {
            var patio = CriarPatioValido();
            patio.LocalizacaoReferencia = null;

            var results = ValidationHelper.ValidateObject(patio);

            Assert.Empty(results);
        }
    }
}

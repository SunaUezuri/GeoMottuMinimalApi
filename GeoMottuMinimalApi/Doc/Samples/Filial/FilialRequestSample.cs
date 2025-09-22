using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Doc.Samples.Filial
{
    public class FilialRequestSample : IExamplesProvider<FilialDto>
    {
        public FilialDto GetExamples()
        {
            return new FilialDto(
                Nome: "Filial São Paulo - Centro",
                PaisesFiliais: PaisesFiliais.Brasil,
                Estado: "São Paulo",
                Endereco: "Avenida Paulista, 1578 - Bela Vista, São Paulo - SP"
            );
        }
    }
}

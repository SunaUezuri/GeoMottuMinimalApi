using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Domain.Entities;

namespace GeoMottuMinimalApi.Application.Mappers
{
    public static class FilialMapper
    {
        public static FilialEntity ToFilialEntity(this FilialDto dto)
        {
            return new FilialEntity
            {
                Nome = dto.Nome,
                PaisFilial = dto.PaisesFiliais,
                Estado = dto.Estado,
                Endereco = dto.Endereco
            };
        }
    }
}

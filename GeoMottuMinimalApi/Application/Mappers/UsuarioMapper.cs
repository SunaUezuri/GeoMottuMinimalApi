﻿using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Domain.Entities;

namespace GeoMottuMinimalApi.Application.Mappers
{
    public static class UsuarioMapper
    {
        public static UsuarioEntity ToUsuarioEntity(this UsuarioDto dto)
        {
            return new UsuarioEntity
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha,
                FilialId = dto.FilialId
            };
        }
    }
}

using GeoMottuMinimalApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GeoMottuMinimalApi.Domain.Entities
{
    [Table("TB_GEOMOTTU_MOTO")]
    [Index(nameof(Chassi), IsUnique = true)]
    [Index(nameof(Placa), IsUnique = true)]
    [Index(nameof(CodPlacaIot), IsUnique = true)]
    public class MotoEntity
    {
        [Key]
        [Column("ID_MOTO")]
        public int Id { get; set; }

        [StringLength(10)]
        [Column("PLACA_MOTO")]
        public string? Placa { get; set; } = string.Empty;

        [StringLength(17)]
        [Column("CHASSI_MOTO")]
        [Required]
        public string Chassi { get; set; }

        [Column("CD_IOT_PLACA")]
        [MaxLength(50)]
        public string? CodPlacaIot { get; set; } = string.Empty;

        [Required]
        [Column("MOTO_MODELO")]
        public ModeloMoto Modelo { get; set; }

        [Required]
        [Column("MOTOR_MOTO")]
        public double Motor { get; set; }

        [Column("MOTO_PROPRIETARIO")]
        [StringLength(150)]
        public string? Proprietario { get; set; } = string.Empty;
        public double PosicaoX { get; set; }
        public double PosicaoY { get; set; }

        public int PatioId { get; set; }
        [ForeignKey("PatioId")]
        [JsonIgnore]
        public virtual PatioEntity? Patio { get; set; }

        public DateTime CriadoEm { get; set; } = DateTime.Now;

    }
}


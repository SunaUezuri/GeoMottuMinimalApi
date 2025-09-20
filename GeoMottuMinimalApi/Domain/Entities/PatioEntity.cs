using GeoMottuMinimalApi.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoMottuMinimalApi.Domain.Entities
{
    [Table("TB_GEOMOTTU_PATIO")]
    public class PatioEntity
    {
        [Key]
        [Column("ID_PATIO")]
        public int Id { get; set; }

        [Required]
        [Column("CAPC_PATIO")]
        public int CapacidadeTotal { get; set; }

        [Column("REFERENCIA_PATIO")]
        [MaxLength(100)]
        public string? LocalizacaoReferencia { get; set; }
        [Column("TAMANHO_PATIO")]
        public double Tamanho { get; set; }

        [Required]
        public TipoPatio TipoDoPatio { get; set; }
        public ICollection<MotoEntity>? Motos { get; set; }
        public int FilialId { get; set; }
        [ForeignKey("FilialId")]
        public virtual FilialEntity Filial { get; set; }


        public DateTime CriadoEm { get; set; } = DateTime.Now;

    }
}

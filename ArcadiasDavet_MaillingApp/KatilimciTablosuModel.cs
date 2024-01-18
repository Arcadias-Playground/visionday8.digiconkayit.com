using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("KatilimciTablosu")]
    public class KatilimciTablosuModel
    {
        [Key]
        [Required(ErrorMessage = "BosUyari")]
        [MaxLength(36, ErrorMessage = "UzunlukUyari")]
        [Column("KatilimciID", Order = 0)]
        public virtual string KatilimciID { get; set; }

        [Required(ErrorMessage = "BosUyari")]
        [MaxLength(36, ErrorMessage = "UzunlukUyari")]
        [ForeignKey("KatilimciTipiTablosu")]
        [Column("KatilimciTipiID", Order = 1)]
        public virtual string KatilimciTipiID { get; set; }

        [Required(ErrorMessage = "BosUyari")]
        [MaxLength(255, ErrorMessage = "UzunlukUyari")]
        [Column("AdSoyad", Order = 2)]
        public virtual string AdSoyad { get; set; }

        [Required(ErrorMessage = "BosUyari")]
        [EmailAddress(ErrorMessage = "GecersizUyari")]
        [MaxLength(255, ErrorMessage = "UzunlukUyari")]
        [Column("ePosta", Order = 3)]
        public virtual string ePosta { get; set; }

        [Required(ErrorMessage = "BosUyari")]
        [MaxLength(255, ErrorMessage = "UzunlukUyari")]
        [Column("Telefon", Order = 4)]
        public virtual string Telefon { get; set; }

        [Required(ErrorMessage = "BosUyari")]
        [MaxLength(255, ErrorMessage = "UzunlukUyari")]
        [Column("Kurum", Order = 5)]
        public virtual string Kurum { get; set; }

        [Required(ErrorMessage = "BosUyari")]
        [MaxLength(255, ErrorMessage = "UzunlukUyari")]
        [Column("Unvan", Order = 6)]
        public virtual string Unvan { get; set; }

        [Required(ErrorMessage = "BosUyari")]
        [Column("YoneticiOnay", Order = 7)]
        public virtual bool YoneticiOnay { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
        [Column("YoneticiOnayTarihi", Order = 8)]
        public virtual DateTime? YoneticiOnayTarihi { get; set; }

        [Required(ErrorMessage = "BosUyari")]
        [Column("KatilimciOnay", Order = 9)]
        public virtual bool KatilimciOnay { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
        [Column("KatilimciOnayTarihi", Order = 10)]
        public virtual DateTime? KatilimciOnayTarihi { get; set; }

        [MaxLength(255, ErrorMessage = "UzunlukUyari")]
        [ForeignKey("KatilimciTablosu")]
        [Column("AnaKatilimciID", Order = 11)]
        public virtual string AnaKatilimciID { get; set; }

        [Required(ErrorMessage = "BosUyari")]
        [DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
        [Column("GuncellenmeTarihi", Order = 12)]
        public virtual DateTime GuncellenmeTarihi { get; set; }

        [Required(ErrorMessage = "BosUyari")]
        [DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
        [Column("EklenmeTarihi", Order = 13)]
        public virtual DateTime EklenmeTarihi { get; set; }

        public bool ePostaGonderimIstek { get; set; }
        public bool SmsGonderimIstek { get; set; }

        public virtual string BaseJsonModel()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
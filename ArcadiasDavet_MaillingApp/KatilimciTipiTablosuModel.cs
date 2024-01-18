using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Model
{
    [Table("KatilimciTipiTablosu")]
    public class KatilimciTipiTablosuModel
    {
        [Key]
        [Required(ErrorMessage = "BosUyari")]
        [MaxLength(36, ErrorMessage = "UzunlukUyari")]
        [Column("KatilimciTipiID", Order = 0)]
        public virtual string KatilimciTipiID { get; set; }

        [Required(ErrorMessage = "BosUyari")]
        [MaxLength(255, ErrorMessage = "UzunlukUyari")]
        [Column("KatilimciTipi", Order = 1)]
        public virtual string KatilimciTipi { get; set; }

        [Required(ErrorMessage = "BosUyari")]
        [Column("Kontenjan", Order = 2)]
        public virtual int Kontenjan { get; set; }

        [Required(ErrorMessage = "BosUyari")]
        [Column("MisafirKontenjan", Order = 3)]
        public virtual int MisafirKontenjan { get; set; }

        [Required(ErrorMessage = "BosUyari")]
        [Column("GirisSayisi", Order = 4)]
        public virtual int GirisSayisi { get; set; }

        [Required(ErrorMessage = "BosUyari")]
        [DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
        [Column("GuncellenmeTarihi", Order = 5)]
        public virtual DateTime GuncellenmeTarihi { get; set; }

        [Required(ErrorMessage = "BosUyari")]
        [DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
        [Column("EklenmeTarihi", Order = 6)]
        public virtual DateTime EklenmeTarihi { get; set; }


        public static int OzellikSayisi { get { return typeof(KatilimciTipiTablosuModel).GetProperties().Count(x => !x.GetAccessors()[0].IsVirtual && !x.GetAccessors()[0].IsStatic); } }

        public virtual string BaseJsonModel()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
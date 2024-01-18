using ModelRelation;

namespace Model
{
    public partial class KatilimciTablosuModel : KatilimciTablosuModelRelation
    {

        public bool ePostaGonderimIstek { get; set; }
        public bool SmsGonderimIstek { get; set; }
    }
}
using ModelRelation;
using System;

namespace Model
{
    public partial class YakaKartiCerceveTablosuModel : YakaKartiCerceveTablosuModelRelation
    {

        decimal WebWithRatio = Convert.ToDecimal(1000m / 210m);
        decimal WebHeightRatio = Convert.ToDecimal(1414m / 297m);

        public int WebFrameWidth
        {
            get
            {
                return Convert.ToInt32(Width * WebWithRatio);
            }
        }

        public int WebFrameHeight
        {
            get
            {
                return Convert.ToInt32(Height * WebHeightRatio);
            }
        }
        
        public int PrinterPaperTabDimension
        {
            get
            {
                return YaziciKagitOrtalama ? (1000 - WebFrameWidth) / 2 : 0;
            }
        }
    }
}
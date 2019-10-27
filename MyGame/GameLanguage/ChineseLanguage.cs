using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using Nez.BitmapFonts;

namespace MyGame.GameLanguage
{
    public class ChineseLanguage : IGameLanguage
    {
        public Languages language { get; set; }
        public BitmapFont font { get; set; }

        public ChineseLanguage()
        {
            language = Languages.Chinese;
            font = Core.content.Load<BitmapFont>("Fonts/STCN/STFontCN");
            //Graphics.instance.bitmapFont = font;
        }



    }
}

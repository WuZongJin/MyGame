using GameDataPipeline;
using MyGame.Common.Game;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameLanguage
{
    public class CommonTextContent
    {
        public CommonText commonText;
        public CommonTextContent()
        {

        }

        public void initialize()
        {
            if(GameSetting.defaultGameLanguage.language == Languages.Chinese)
            {
                commonText = Core.content.Load<CommonText>("");
            }
            else if (GameSetting.defaultGameLanguage.language == Languages.English)
            {
                commonText = Core.content.Load<CommonText>("");
            }
        }
    }
}

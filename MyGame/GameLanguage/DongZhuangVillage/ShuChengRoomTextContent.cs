using GameDataPipeline.TextContent.DongZhuangVillage;
using MyGame.Common.Game;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameLanguage.DongZhuangVillage
{
    public class ShuChengRoomTextContent
    {
        public ShuChengRoomText shuChengRoomText;

        public ShuChengRoomTextContent()
        {
            shuChengRoomText = new ShuChengRoomText();
        }

        public void initialize()
        {
            if (GameSetting.defaultGameLanguage.language == Languages.Chinese)
            {
                shuChengRoomText = Core.content.Load<ShuChengRoomText>("");
            }
            else if (GameSetting.defaultGameLanguage.language == Languages.English)
            {
                shuChengRoomText = Core.content.Load<ShuChengRoomText>("");
            }
        }
    }
}

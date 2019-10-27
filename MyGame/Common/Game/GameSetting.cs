using MyGame.GameLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Common.Game
{
    public static class GameSetting
    {
        public static bool isGamePause = false;

        public static IGameLanguage defaultGameLanguage = new ChineseLanguage();
    }

    public static class GameLayerSetting
    {
        public static int actorMoverLayer = 0;
        public static int tiledActorUpLayer = -5;
        public static int tiledLayer = 10;
        public static int playerUiLayer = -20;
        public static int uiLayer = -40;
        public static int lightRenderLayer = -10;
        public static int debugDrawViewLayer = -50;

    }
}

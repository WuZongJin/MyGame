using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Common.Game;
using MyGame.GlobalManages;
using MyGame.GlobalManages.GameManager;
using MyGame.Scenes.Demo.TileMapWitgLayerDepth;
using MyGame.Scenes.DongZhuanVillage;
using MyGame.Scenes.DongZhuanVillage.Maze;
using Nez;

namespace MyGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Core
    {

        #region Properties

        #endregion

        protected override void Initialize()
        {
            base.Initialize();
            exitOnEscapeKeypress = false;
            NotificationManager notificationManager = new NotificationManager();
            GameUIManager gameUIManager = new GameUIManager();
            GameActorManager gameActorManager = new GameActorManager();

            gameActorManager.initPlayer();

            Core.registerGlobalManager(gameActorManager);
            Core.registerGlobalManager(gameUIManager);
            Core.registerGlobalManager(notificationManager);


            GameResources.GameTextureResource.initialize();
            Graphics.instance.bitmapFont = GameSetting.defaultGameLanguage.font;


            var startScene =new ShuChengRoom();
           
            var entity = gameActorManager.player.setPosition(205, 285);
            startScene.initEntity(entity);
            scene = startScene;
        }




    }
}

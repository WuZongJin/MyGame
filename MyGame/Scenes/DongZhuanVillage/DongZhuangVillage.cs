using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameConponents.UtilityComponents;
using MyGame.GameEntities.NPC;
using MyGame.GameEntities.Player;
using MyGame.GameEntities.TiledObjects;
using MyGame.GlobalManages.GameManager;
using Nez;
using Nez.Farseer;
using Nez.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Scenes.DongZhuanVillage
{
    public class DongZhuangVillage:BasicScene
    {
        #region Properties
        Player player;
        TiledMap tiledMap;
        #endregion

        #region Constructor
        public DongZhuangVillage()
        {

        }
        #endregion

        #region Override Method
        public override void initialize()
        {
            base.initialize();
            addRenderer(new RenderLayerRenderer(0, GameLayerSetting.actorMoverLayer
                , GameLayerSetting.tiledActorUpLayer
                , GameLayerSetting.tiledLayer
                ));

            addRenderer(new RenderLayerRenderer(0, GameLayerSetting.debugDrawViewLayer));

            initPlayer();

            initTiledMap();
            initGrass();
            initMayor();
            initSceneTrigger();

            
            


        }
        public override void onStart()
        {
            base.onStart();
            initCamera();
        }

        private void initPlayer()
        {
            var gameActorManager = Core.getGlobalManager<GameActorManager>();
            player = gameActorManager.player;
        }

        private void initCamera()
        {
            camera.entity.addComponent(new FollowCamera(player));
            var topLeft = new Vector2(tiledMap.tileWidth, tiledMap.tileWidth);
            var bottomRight = new Vector2(tiledMap.tileWidth * (tiledMap.width - 1), tiledMap.tileWidth * (tiledMap.height - 1));
            createEntity("cameraBounds").addComponent(new CameraBounds(topLeft, bottomRight));
        }
        #endregion

        #region init TiledMap
        private void initTiledMap()
        {
            var tiledEntity = createEntity("tiledMap");
            tiledMap = content.Load<TiledMap>("TileMaps/Maps/DongZhuang/DongZhuangVillage");
            var tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap));
            tiledMapComponent.setLayersToRender(new string[] { "Tiled", "Tree","Room" });
            tiledMapComponent.setRenderLayer(GameLayerSetting.tiledLayer);

            var tileMapUpPlayerComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap));
            tileMapUpPlayerComponent.setLayersToRender(new string[] { "UpPlayer" });
            tileMapUpPlayerComponent.setRenderLayer(GameLayerSetting.tiledActorUpLayer);

            createMapCollision(tiledMap, "Collision");
        }

        #endregion

        #region init TiledMap Objects
        private void initGrass()
        {
            var objectLayer = tiledMap.getObjectGroup("GrassAndRock");
            var grassList = objectLayer.objectsWithName("grass");
            foreach (var grass in grassList)
            {
                addEntity(new Grass01().setPosition(grass.position + new Vector2(grass.width / 2, grass.height / 2)));
            }
        }

        #endregion

        #region init Npc

        private void initMayor()
        {
            var objectLayer = tiledMap.getObjectGroup("Position");
            var mayorPosition = objectLayer.objectWithName("MayorPosition");
            addEntity(new DongZhuangVillageMayor()).setPosition(mayorPosition.position);
        }

        #endregion

        #region init SceneTrigger
        private void initSceneTrigger()
        {
            var objectLayer = tiledMap.getObjectGroup("Objects"); 
            var shuChengAreaTriggerPosition = objectLayer.objectWithName("ShuChengAreaTrigger");
            var mazeArea = objectLayer.objectWithName("DongZhuangMazeArea");

            initShuChengAreaChangeTrigger(shuChengAreaTriggerPosition);
            initMazeArea(mazeArea);

        }

        private void initShuChengAreaChangeTrigger(TiledObject tiledObject)
        {
            var entity = createEntity("shuchengArea");
            entity.setPosition(tiledObject.position + new Vector2(tiledObject.width / 2f, tiledObject.height / 2f));
            entity.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic);

            var trigger = entity.addComponent<SceneChangeTriggerComponent>();
            trigger.setSize(tiledObject.width, tiledObject.height);
            trigger.setIsSensor(true);
            trigger.setCollisionCategories(CollisionSetting.tiledObjectCategory);
            trigger.setCollidesWith(CollisionSetting.playerCategory);

            trigger.onAdded += () =>
            {
                trigger.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var area = new ShuChengArea();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(area);
                        player.setPosition(96, 55);
                        return area;
                    });
                    transition.onTransitionCompleted += () =>
                    {
                        GameSetting.isGamePause = false;
                    };
                    Core.startSceneTransition(transition);

                    GameSetting.isGamePause = true;

                    return true;

                };
            };
        }

        private void initMazeArea(TiledObject tiledObject)
        {
            var entity = createEntity(tiledObject.name);
            entity.setPosition(tiledObject.position + new Vector2(tiledObject.width / 2f, tiledObject.height / 2f));
            entity.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic);

            var trigger = entity.addComponent<SceneChangeTriggerComponent>();
            trigger.setSize(tiledObject.width, tiledObject.height);
            trigger.setIsSensor(true);
            trigger.setCollisionCategories(CollisionSetting.tiledObjectCategory);
            trigger.setCollidesWith(CollisionSetting.playerCategory);

            trigger.onAdded += () =>
            {
                trigger.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var area = new DongZhuangMazeArea();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(area);
                        player.setPosition(290, 445);
                        return area;
                    });
                    transition.onTransitionCompleted += () =>
                    {
                        GameSetting.isGamePause = false;
                    };
                    Core.startSceneTransition(transition);

                    GameSetting.isGamePause = true;

                    return true;

                };
            };
        }
        #endregion

    }
}

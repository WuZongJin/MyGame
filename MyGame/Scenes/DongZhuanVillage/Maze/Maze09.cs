using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameEntities.Enemys.DZMazeBoss;
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

namespace MyGame.Scenes.DongZhuanVillage.Maze
{
    public class Maze09:BasicScene
    {
        #region Properties
        Player player;
        TiledMap tiledMap;
        #endregion

        #region Constructor
        public Maze09()
        {

        }
        #endregion
        #region Override
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
            initSceneChangeTrigger();
            initEnemy();
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
            camera.entity.setPosition(160,160);

        }
        #endregion

        #region init TiledMap
        private void initTiledMap()
        {
            var tiledEntity = createEntity("tiledMap");
            tiledMap = content.Load<TiledMap>("TileMaps/Maps/DongZhuang/DongZhuangMaze09");
            var tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap));
            tiledMapComponent.setLayersToRender(new string[] { "Tiled", "Wall" });
            tiledMapComponent.setRenderLayer(GameLayerSetting.tiledLayer);

            createMapCollision(tiledMap, "Collision");
        }
        #endregion

        #region initSecenTrigger
        private void initSceneChangeTrigger()
        {
            var objectLayer = tiledMap.getObjectGroup("Object");

            var maze10Object = objectLayer.objectWithName("maze10");
            var maze8Object = objectLayer.objectWithName("maze8");


            var maze10 = createSceneTrigger(maze10Object);
            maze10.getComponent<SceneChangeTriggerComponent>().onAdded += () =>
            {
                maze10.getComponent<SceneChangeTriggerComponent>().GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var area = new Maze10();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(area);
                        player.setPosition(288, 176);
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

            var maze8 = createSceneTrigger(maze8Object);
            maze8.getComponent<SceneChangeTriggerComponent>().onAdded += () =>
            {
                maze8.getComponent<SceneChangeTriggerComponent>().GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var area = new Maze08();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(area);
                        player.setPosition(30, 185);
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

        private Entity createSceneTrigger(TiledObject tiledObject)
        {
            var entity = createEntity(tiledObject.name).setPosition(tiledObject.position + new Vector2(tiledObject.width / 2f, tiledObject.height / 2f));
            entity.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic);

            var trigger = entity.addComponent<SceneChangeTriggerComponent>();
            trigger.setSize(tiledObject.width, tiledObject.height);
            trigger.setIsSensor(true);
            trigger.setCollisionCategories(CollisionSetting.tiledObjectCategory);
            trigger.setCollidesWith(CollisionSetting.playerCategory);

            return entity;
        }
        #endregion

        #region initEnemy
        private void initEnemy()
        {
            if((GameEvent.dzMazeEvent& DongZhuangMazeEvent.MazeBossKilled) == 0)
            {
                var boss = addEntity(new DZMazeBoss());
                boss.setPosition(160, 160);
                boss.startMove = true;

                var objectLayer = tiledMap.getObjectGroup("Object");
                var gate1Object = objectLayer.objectWithName("gate1");
                var gate2Object = objectLayer.objectWithName("gate2");

                var gate1 = addEntity(new MazeGateRight());
                gate1.setPosition(gate1Object.position+new Vector2(gate1Object.width/2f,gate1Object.height/2f));
                var gate2 = addEntity(new MazeGateLeft());
                gate2.setPosition(gate2Object.position + new Vector2(gate2Object.width / 2f, gate2Object.height / 2f));

                boss.onDeathed += () =>
                {
                    gate1.destoryedByKey();
                    gate2.destoryedByKey();
                };

            }
        }
        #endregion


    }
}

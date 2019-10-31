using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameEntities.Enemys.Octor;
using MyGame.GameEntities.Enemys.Slime;
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
    public class Maze04 : BasicScene
    {
        #region Properties
        Player player;
        TiledMap tiledMap;
        #endregion

        #region Constrcutor
        public Maze04()
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
            initGrassAndRock();
            initHole();
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
            tiledMap = content.Load<TiledMap>("TileMaps/Maps/DongZhuang/DongZhuangMaze04");
            var tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap));
            tiledMapComponent.setLayersToRender(new string[] { "Tiled","UpTiled", "Wall" });
            tiledMapComponent.setRenderLayer(GameLayerSetting.tiledLayer);

            createMapCollision(tiledMap, "Collision");
        }
        #endregion

        #region 
        private void initSceneChangeTrigger()
        {
            var objectLayer = tiledMap.getObjectGroup("Object");

            var maze1Object = objectLayer.objectWithName("maze1");
            var maze5Object = objectLayer.objectWithName("maze5");

            var maze1 = createSceneTrigger(maze1Object);
            maze1.getComponent<SceneChangeTriggerComponent>().onAdded += () =>
            {
                maze1.getComponent<SceneChangeTriggerComponent>().GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var area = new Maze01();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(area);
                        player.setPosition(295, 145);
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

            var maze5 = createSceneTrigger(maze5Object);
            maze5.getComponent<SceneChangeTriggerComponent>().onAdded += () =>
            {
                maze5.getComponent<SceneChangeTriggerComponent>().GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var area = new  Maze05();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(area);
                        player.setPosition(23, 170);
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

        #region init Grass And Rock
        private void initGrassAndRock()
        {
            var objectLayer = tiledMap.getObjectGroup("GrassAndRock");
            var grassList = objectLayer.objectsWithName("grass");
            foreach (var grass in grassList)
            {
                addEntity(new Grass01()).setPosition(grass.position + new Vector2(grass.width / 2f, grass.height / 2f));
            }

        }
        #endregion

        #region init Enemy
        private void initEnemy()
        {
            var objectLayer = tiledMap.getObjectGroup("Enemy");
            var octorList = objectLayer.objectsWithName("octor");
            var slimeObjec = objectLayer.objectWithName("slime");

            addEntity(new Slime(slimeObjec.position));

            foreach(var octor in octorList)
            {
                addEntity(new Octor()).setPosition(octor.position);
            }


        }
        #endregion

        #region initHole
        private void initHole()
        {
            var objectLayer = tiledMap.getObjectGroup("Hole");
            var holes = objectLayer.objectsWithName("hole");
            foreach (var hole in holes)
            {
                addEntity(new TiledHole(hole.position + new Vector2(hole.width / 2f, hole.height / 2f), new Vector2(hole.width, hole.height)));
            }
        }
        #endregion
    }
}

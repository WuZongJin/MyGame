using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameEntities.Enemys.Armos;
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
    public class Maze07:BasicScene
    {
        #region Properties
        Player player;
        TiledMap tiledMap;
        #endregion

        #region Constructor
        public Maze07()
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
            initHole();
            initGrassAndRock();
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
            tiledMap = content.Load<TiledMap>("TileMaps/Maps/DongZhuang/DongZhuangMaze07");
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

            var maze5Object = objectLayer.objectWithName("maze5");
            var maze8Object = objectLayer.objectWithName("maze8");


            var maze5 = createSceneTrigger(maze5Object);
            maze5.getComponent<SceneChangeTriggerComponent>().onAdded += () =>
            {
                maze5.getComponent<SceneChangeTriggerComponent>().GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var area = new Maze05();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(area);
                        player.setPosition(165, 52);
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
                        player.setPosition(152, 280);
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

        #region init GrassAndRock
        private void initGrassAndRock()
        {
            var objectLayer = tiledMap.getObjectGroup("GrassAndRock");
            var rockList = objectLayer.objectsWithName("rock");
           
            foreach (var rock in rockList)
            {
                addEntity(xRock.create()).setPosition(rock.position + new Vector2(rock.width / 2, rock.height / 2));
            }
        }
        #endregion

        #region initEnemy
        private void initEnemy()
        {
            var objectLayer = tiledMap.getObjectGroup("Enemy");
            var armosObject = objectLayer.objectWithName("armos");

            addEntity(new Armos()).setPosition(armosObject.position);
        }
        #endregion

        #region init Hole
        private void initHole()
        {
            var objectLayer = tiledMap.getObjectGroup("Hole");
            var holes = objectLayer.objectsWithName("hole");
            foreach(var hole in holes)
            {
                addEntity(new TiledHole(hole.position + new Vector2(hole.width / 2f, hole.height / 2f), new Vector2(hole.width, hole.height)));
            }
        }
        #endregion


    }
}

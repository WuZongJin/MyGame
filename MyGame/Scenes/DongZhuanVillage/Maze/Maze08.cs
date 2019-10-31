using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameEntities.Enemys.Archer;
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
    public class Maze08:BasicScene
    {
        #region Properties
        Player player;
        TiledMap tiledMap;
        #endregion

        #region Constrcutor
        public Maze08()
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
            tiledMap = content.Load<TiledMap>("TileMaps/Maps/DongZhuang/DongZhuangMaze08");
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

            var maze7Object = objectLayer.objectWithName("maze7");
            var maze9Object = objectLayer.objectWithName("maze9");


            var maze7 = createSceneTrigger(maze7Object);
            maze7.getComponent<SceneChangeTriggerComponent>().onAdded += () =>
            {
                maze7.getComponent<SceneChangeTriggerComponent>().GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var area = new Maze07();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(area);
                        player.setPosition(150, 45);
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

            var maze9 = createSceneTrigger(maze9Object);
            maze9.getComponent<SceneChangeTriggerComponent>().onAdded += () =>
            {
                maze9.getComponent<SceneChangeTriggerComponent>().GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var area = new Maze09();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(area);
                        player.setPosition(295, 166);
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
            var grassList = objectLayer.objectsWithName("grass");

            foreach (var grass in grassList)
            {
                addEntity(new Grass01().setPosition(grass.position + new Vector2(grass.width / 2, grass.height / 2)));
            }



        }
        #endregion

        #region init enemy
        private void initEnemy()
        {
            var objectLayer = tiledMap.getObjectGroup("Enemy");
            var archers = objectLayer.objectsWithName("archer");
            foreach(var archer in archers)
            {
                addEntity(new Archer()).setPosition(archer.position);
            }

        }
        #endregion

    }
}

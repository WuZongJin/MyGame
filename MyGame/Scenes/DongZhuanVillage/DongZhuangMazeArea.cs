using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameEntities.Player;
using MyGame.GameEntities.TiledObjects;
using MyGame.GlobalManages.GameManager;
using MyGame.Scenes.DongZhuanVillage.Maze;
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
    public class DongZhuangMazeArea:BasicScene
    {
        #region Properties

        Player player;
        TiledMap tiledMap;

        #endregion

        #region Constructor
        public DongZhuangMazeArea()
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
            initGrassAndRock();
            initSceneChangeTrigger();
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

        }
        #endregion

        #region init TiledMap
        private void initTiledMap()
        {
            var tiledEntity = createEntity("tiledMap");
            tiledMap = content.Load<TiledMap>("TileMaps/Maps/DongZhuang/DongzhuangMazeArea");
            var tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap));
            tiledMapComponent.setLayersToRender(new string[] { "Tiled", "UpTiled","Tree" });
            tiledMapComponent.setRenderLayer(GameLayerSetting.tiledLayer);



            createMapCollision(tiledMap, "Collision");
        }
        #endregion


        #region init Grass And Rock
        private void initGrassAndRock()
        {
            var objectLayer = tiledMap.getObjectGroup("GrassAndRock");
            var grassList = objectLayer.objectsWithName("grass");
            var rockList = objectLayer.objectsWithName("rock");
            foreach (var grass in grassList)
            {
                addEntity(new Grass01().setPosition(grass.position + new Vector2(grass.width / 2, grass.height / 2)));
            }

            foreach (var rock in rockList)
            {
                addEntity(new xRock()).setPosition(rock.position + new Vector2(rock.width / 2, rock.height / 2));
            }
        }
        #endregion

        #region init SceneChangeTrigger
        private void initSceneChangeTrigger()
        {
            var objectLayer = tiledMap.getObjectGroup("Object");
            var dzTriggerObject = objectLayer.objectWithName("DongZhuangVillageTrigger");
            var mazeObject = objectLayer.objectWithName("DongZhuangMazeTrigger");

            var dzvillageEntity = createSceneTrigger(dzTriggerObject);
            dzvillageEntity.getComponent<SceneChangeTriggerComponent>().onAdded += () =>
            {
                dzvillageEntity.getComponent<SceneChangeTriggerComponent>().GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var village = new DongZhuangVillage();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(village);
                        player.setPosition(447, 35);
                        return village;
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

            var mazeEntity = createSceneTrigger(mazeObject);
            mazeEntity.getComponent<SceneChangeTriggerComponent>().onAdded += () =>
            {
                mazeEntity.getComponent<SceneChangeTriggerComponent>().GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {

                    var maze0 = new Maze0();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(maze0);
                        player.setPosition(152, 280);
                        return maze0;
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
    }
}

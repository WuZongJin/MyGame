using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
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
    public class Maze0:BasicScene
    {
        #region Properties
        Player player;
        TiledMap tiledMap;
        #endregion

        #region Constrcutor
        public Maze0()
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
            initPot();
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
            camera.entity.setPosition(160,160);

        }
        #endregion

        #region init TiledMap
        private void initTiledMap()
        {
            var tiledEntity = createEntity("tiledMap");
            tiledMap = content.Load<TiledMap>("TileMaps/Maps/DongZhuang/DongZhuangMaze0");
            var tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap));
            tiledMapComponent.setLayersToRender(new string[] { "Tiled", "Wall"});
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

            foreach(var rock in rockList)
            {
                addEntity(new xRock()).setPosition(rock.position + new Vector2(rock.width / 2, rock.height / 2));
            }
        }
        #endregion

        #region init Object
        private void initSceneChangeTrigger()
        {
            var objectLayer = tiledMap.getObjectGroup("Object");
            var outTriggerObject = objectLayer.objectWithName("outTrigger");
            var mazeTirgger = objectLayer.objectWithName("maze1Trigger");


            var outEntity = createSceneTrigger(outTriggerObject);
            outEntity.getComponent<SceneChangeTriggerComponent>().onAdded += () =>
            {
                outEntity.getComponent<SceneChangeTriggerComponent>().GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var area = new DongZhuangMazeArea();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(area);
                        player.setPosition(105, 145);
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

            var mazeEntity =createSceneTrigger(mazeTirgger);
            mazeEntity.getComponent<SceneChangeTriggerComponent>().onAdded += () =>
            {
                mazeEntity.getComponent<SceneChangeTriggerComponent>().GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var area = new Maze01();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(area);
                        player.setPosition(150, 280);
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
            //var outEntity = createEntity("outEntity").setPosition(outTriggerObject.position+new Vector2(outTriggerObject.width/2f,outTriggerObject.height/2f));
            //outEntity.addComponent<FSRigidBody>()
            //    .setBodyType(BodyType.Dynamic);

            //var outTrigger = outEntity.addComponent<SceneChangeTriggerComponent>();
            //outTrigger.setSize(outTriggerObject.width, outTriggerObject.height);
            //outTrigger.setIsSensor(true);
            //outTrigger.setCollisionCategories(CollisionSetting.tiledObjectCategory);
            //outTrigger.setCollidesWith(CollisionSetting.playerCategory);

            //outTrigger.onAdded += ()=>{

            //};
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

        #region initbot
        private void initPot()
        {
            var objectLayer = tiledMap.getObjectGroup("Object");
            var potList = objectLayer.objectsWithName("pot");
            foreach(var pot in potList)
            {
                addEntity(new Bottle01()).setPosition(pot.position+new Vector2(pot.width/2f,pot.height/2f));
            }
        }
        #endregion

    }
}

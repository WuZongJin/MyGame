using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameEntities.Enemys.Octor;
using MyGame.GameEntities.Enemys.Slime;
using MyGame.GameEntities.Player;
using MyGame.GameEntities.TiledObjects;
using MyGame.GameResources;
using MyGame.GlobalManages;
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
    public class Maze05:BasicScene
    {
        #region Properties
        Player player;
        TiledMap tiledMap;
        #endregion

        #region Constructor
        public Maze05()
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
            initGrassAndRock();
            initEnemy();
            initGate();
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
            camera.entity.setPosition(160, 160);

        }
        #endregion

        #region init TiledMap
        private void initTiledMap()
        {
            var tiledEntity = createEntity("tiledMap");
            tiledMap = content.Load<TiledMap>("TileMaps/Maps/DongZhuang/DongZhuangMaze05");
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

            var maze4Object = objectLayer.objectWithName("maze4");
            var maze6Object = objectLayer.objectWithName("maze6");
            var maze7Object = objectLayer.objectWithName("maze7");

            var maze4 = createSceneTrigger(maze4Object);
            maze4.getComponent<SceneChangeTriggerComponent>().onAdded += () =>
            {
                maze4.getComponent<SceneChangeTriggerComponent>().GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var area = new Maze04();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(area);
                        player.setPosition(300, 170);
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

            var maze6 = createSceneTrigger(maze6Object);
            maze6.getComponent<SceneChangeTriggerComponent>().onAdded += () =>
            {
                maze6.getComponent<SceneChangeTriggerComponent>().GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var area = new Maze06();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(area);
                        player.setPosition(170, 53);
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
                        player.setPosition(152, 262);
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
            var rockList = objectLayer.objectsWithName("rock");
            foreach (var grass in grassList)
            {
                addEntity(new Grass01().setPosition(grass.position + new Vector2(grass.width / 2, grass.height / 2)));
            }

            foreach (var rock in rockList)
            {
                addEntity(xRock.create()).setPosition(rock.position + new Vector2(rock.width / 2, rock.height / 2));
            }
        }
        #endregion

        #region init Enemy
        private void initEnemy()
        {
            var objectLayer = tiledMap.getObjectGroup("Enemy");
            var octorList = objectLayer.objectsWithName("octor");
            var slimeObject = objectLayer.objectsWithName("slime");

            foreach(var slime in slimeObject)
            {
                addEntity(new Slime(slime.position));
            }
           

            foreach (var octor in octorList)
            {
                addEntity(new Octor()).setPosition(octor.position);
            }


        }
        #endregion

        #region init Gate
        private void initGate()
        {
            if((GameEvent.dzMazeEvent & DongZhuangMazeEvent.Maze5GateOpened) == 0)
            {
                var objectLayer = tiledMap.getObjectGroup("Object");
                var gateObject = objectLayer.objectWithName("gate");

                var gate = addEntity(new MazeGateUp());
                gate.setPosition(gateObject.position);

                gate.triggerEvent += () =>
                {
                    if (player.items.Keys.Where(m => m.id == GameItemId.dongzhuangmazekey).Count() > 0)
                    {
                        gate.destoryedByKey();
                        player.throwOut(player.items.Keys.Where(m => m.id == GameItemId.dongzhuangmazekey).First());
                        GameEvent.dzMazeEvent = GameEvent.dzMazeEvent | DongZhuangMazeEvent.Maze1GateOpened;
                    }
                    else
                    {
                        var uiManager = Core.getGlobalManager<GameUIManager>();
                        IList<Conmunication> conmunications = new List<Conmunication>();
                        conmunications.Add(new Conmunication("Images/headIcons/Link_Sprite", "大门需要钥匙"));
                        uiManager.createConmunication(conmunications);
                    }
                };
            }
            


        }
        #endregion
    }
}

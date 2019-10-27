using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameEntities.Items.Others;
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
    public class Maze01: BasicScene
    {
        #region Properties
        Player player;
        TiledMap tiledMap;
        #endregion

        #region Constrcutor
        public Maze01()
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
            initGate();
            addEntity(new DongZhuangMazeKeyEntity()).setPosition(100, 100);

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
            tiledMap = content.Load<TiledMap>("TileMaps/Maps/DongZhuang/DongZhuangMaze01");
            var tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap));
            tiledMapComponent.setLayersToRender(new string[] { "Tiled", "Wall" });
            tiledMapComponent.setRenderLayer(GameLayerSetting.tiledLayer);

            createMapCollision(tiledMap, "Collision");
        }
        #endregion

        #region 
        private void initSceneChangeTrigger()
        {
            var objectLayer = tiledMap.getObjectGroup("Object");

            var maze0Object = objectLayer.objectWithName("maze0");
            var maze2Object = objectLayer.objectWithName("maze2");
            var maze4Object = objectLayer.objectWithName("maze4");

            var maze0 = createSceneTrigger(maze0Object);
            maze0.getComponent<SceneChangeTriggerComponent>().onAdded += () =>
            {
                maze0.getComponent<SceneChangeTriggerComponent>().GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var area = new Maze0();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(area);
                        player.setPosition(152, 52);
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

            var maze2 = createSceneTrigger(maze2Object);
            maze2.getComponent<SceneChangeTriggerComponent>().onAdded += () =>
            {
                maze2.getComponent<SceneChangeTriggerComponent>().GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var area = new Maze02();
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
                        player.setPosition(23, 165);
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

        #region init Gate
        private void initGate()
        {
            if ((GameEvent.dzMazeEvent & DongZhuangMazeEvent.Maze1GateOpened) == 0)
            {

                var objectLayer = tiledMap.getObjectGroup("Gate");

                var gateObject = objectLayer.objectWithName("gate");

                var gate = addEntity(new MazeGateRight());
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

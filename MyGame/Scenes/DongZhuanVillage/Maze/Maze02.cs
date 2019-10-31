using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameEntities.Enemys.Bat;
using MyGame.GameEntities.Enemys.Slime;
using MyGame.GameEntities.Items.Others;
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
    public class Maze02: BasicScene
    {
        #region Properties
        Player player;
        TiledMap tiledMap;
        #endregion

        #region Constrcutor
        public Maze02()
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
            initBox();
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
            tiledMap = content.Load<TiledMap>("TileMaps/Maps/DongZhuang/DongZhuangMaze02");
            var tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap));
            tiledMapComponent.setLayersToRender(new string[] { "Tiled", "Wall" });
            tiledMapComponent.setRenderLayer(GameLayerSetting.tiledLayer);

            createMapCollision(tiledMap, "Collision");
        }
        #endregion

        #region init
        private void initSceneChangeTrigger()
        {
            var objectLayer = tiledMap.getObjectGroup("Object");
            var maze1Object = objectLayer.objectWithName("maze1");
            var maze3Object = objectLayer.objectWithName("maze3");

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

            var maze3 = createSceneTrigger(maze3Object);
            maze3.getComponent<SceneChangeTriggerComponent>().onAdded += () =>
            {
                maze3.getComponent<SceneChangeTriggerComponent>().GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var area = new Maze03();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(area);
                        player.setPosition(25, 175);
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

        #region initBox
        private void initBox()
        {
            var objectLayer = tiledMap.getObjectGroup("Object");
            var boxObject = objectLayer.objectWithName("box");

            var box = addEntity(new TreasureBox());
            box.setPosition(boxObject.position);
            if((GameEvent.dzMazeEvent& DongZhuangMazeEvent.Maze2KeyHasGeted) == 0)
            {
                box.openBox += () =>
                {
                    GameEvent.dzMazeEvent = GameEvent.dzMazeEvent | DongZhuangMazeEvent.Maze2KeyHasGeted;
                    player.pickUp(new DongZhuangMazeKey());
                };
            }
            else
            {
                box.boxHasOpend();
            }

        }
        #endregion

        #region initEnemy
        private void initEnemy()
        {
            var objectLayer = tiledMap.getObjectGroup("Enemy");
            var batObject = objectLayer.objectWithName("bat");
            var slimeObject = objectLayer.objectsWithName("slime");

            addEntity(new Bat()).setPosition(batObject.position);

            foreach(var slime in slimeObject)
            {
                addEntity(new Slime(slime.position));
            }
            
        }
        #endregion

    }
}

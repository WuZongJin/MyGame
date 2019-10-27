using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameEntities.Enemys.Bat;
using MyGame.GameEntities.Enemys.Slime;
using MyGame.GameEntities.Items.NormalRecoverMedicine;
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
    public class Cave2:BasicScene
    {
        #region Properties
        TiledMap tiledMap;
        Player player;
        #endregion

        #region Constructor
        public Cave2()
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
            initGrass();
            initTreasureBox();
            initSceneChnageTrigger();
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
            camera.entity.addComponent(new FollowCamera(player));
        }
        #endregion

        #region initTileMap
        private void initTiledMap()
        {
            var tiledEntity = createEntity("tiledMap");
            tiledMap = content.Load<TiledMap>("TileMaps/Maps/DongZhuang/cave2");
            var tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap));
            tiledMapComponent.setLayersToRender(new string[] { "Tiled", "Wall" });
            tiledMapComponent.setRenderLayer(GameLayerSetting.tiledLayer);

            var tileMapUpPlayerComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap));
            tileMapUpPlayerComponent.setLayersToRender(new string[] { "UpPlayer" });
            tileMapUpPlayerComponent.setRenderLayer(GameLayerSetting.tiledActorUpLayer).setLayerDepth(LayerDepthExt.caluelateLayerDepth(210));

            createMapCollision(tiledMap, "Collision");
        }
        #endregion

        #region initTileMap Object
        private void initGrass()
        {
            var objectLayer = tiledMap.getObjectGroup("Object");
            var grassList = objectLayer.objectsWithName("grass");
            foreach (var grass in grassList)
            {
                addEntity(new Grass01().setPosition(grass.position + new Vector2(grass.width / 2, grass.height / 2)));
            }
        }

        private void initTreasureBox()
        {
            var objectLayer = tiledMap.getObjectGroup("Position");
            var boxObject = objectLayer.objectWithName("treasureBox");
           

            var box = addEntity(new TreasureBox());
            box.setPosition(boxObject.position);

            if ((GameEvent.homeEvent& GameHomeEvent.Cave2TreasureBoxOpened) == 0)
            {
                box.openBox += () =>
                {
                    GameEvent.homeEvent = GameEvent.homeEvent | GameHomeEvent.Cave2TreasureBoxOpened;
                    player.pickUp(new NormalRecoverMedicine(), 5);
                };
            }
            else
            {
                box.openBox();
            }
        }

        #region SceneChangeTrigger
        private void initSceneChnageTrigger()
        {
            var objectLayer = tiledMap.getObjectGroup("Position");
            var cave1TriggerObject = objectLayer.objectWithName("cave1Trigger");
            var cave3TriggerObject = objectLayer.objectWithName("cave3Trigger");


            var cave1TriggerEntity = createEntity("cave1Trigger");
            cave1TriggerEntity.setPosition(cave1TriggerObject.position + new Vector2(cave1TriggerObject.width / 2, cave1TriggerObject.height / 2));
            cave1TriggerEntity.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic);

            var cave1Trigger = cave1TriggerEntity.addComponent<SceneChangeTriggerComponent>();
            cave1Trigger.setSize(cave1TriggerObject.width, cave1TriggerObject.height);
            cave1Trigger.setIsSensor(true);
            cave1Trigger.setCollisionCategories(CollisionSetting.tiledObjectCategory);
            cave1Trigger.setCollidesWith(CollisionSetting.playerCategory);

            cave1Trigger.onAdded += () =>
            {
                cave1Trigger.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var cave1 = new Cave1();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(cave1);
                        player.setPosition(230, 400);
                        return cave1;
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

            var cave3TriggerEntity = createEntity("cave3Trigger");
            cave3TriggerEntity.setPosition(cave3TriggerObject.position + new Vector2(cave3TriggerObject.width / 2, cave3TriggerObject.height / 2));
            cave3TriggerEntity.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic);

            var cave3Trigger = cave3TriggerEntity.addComponent<SceneChangeTriggerComponent>();
            cave3Trigger.setSize(cave3TriggerObject.width, cave3TriggerObject.height);
            cave3Trigger.setIsSensor(true);
            cave3Trigger.setCollisionCategories(CollisionSetting.tiledObjectCategory);
            cave3Trigger.setCollidesWith(CollisionSetting.playerCategory);

            cave3Trigger.onAdded += () =>
            {
                cave3Trigger.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var cave3 = new Cave3();

                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(cave3);
                        player.setPosition(215, 420);
                        return cave3;
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

        #region Enemy
        private void initEnemy()
        {
            var objectLayer = tiledMap.getObjectGroup("Enemy");
            var slimeList = objectLayer.objectsWithName("slime");
            var batList = objectLayer.objectsWithName("bat");
            foreach (var slime in slimeList)
            {
                addEntity(new Slime(slime.position));
            }

            foreach(var bat in batList)
            {
                addEntity(new Bat().setPosition(bat.position));
            }
        }
        #endregion

        #endregion
    }
}

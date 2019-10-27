using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameEntities.Enemys.Slime;
using MyGame.GameEntities.Player;
using MyGame.GameEntities.TiledObjects;
using MyGame.GlobalManages.GameManager;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using Nez.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Scenes.DongZhuanVillage
{
    public class Cave1 : BasicScene
    {
        #region Properties
        TiledMap tiledMap;
        Player player;
        #endregion

        #region Constructor
        public Cave1()
        {


        }
        #endregion

        #region Override
        public override void initialize()
        {
            base.initialize();
            addRenderer(new RenderLayerRenderer(0, GameLayerSetting.actorMoverLayer
                ,GameLayerSetting.tiledActorUpLayer
                , GameLayerSetting.tiledLayer
                ));

            addRenderer(new RenderLayerRenderer(0, GameLayerSetting.debugDrawViewLayer));
            initPlayer();

            initTiledMap();
            initGrass();
            initSceneChangeTrigger();
            initExpositionTree();
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



        #region initTiledMap
        private void initTiledMap()
        {
            var tiledEntity = createEntity("tiledMap");
            tiledMap = content.Load<TiledMap>("TileMaps/Maps/DongZhuang/cave");
            var tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap));
            tiledMapComponent.setLayersToRender(new string[] { "Tiled", "Wall" });
            tiledMapComponent.setRenderLayer(GameLayerSetting.tiledLayer);

            createMapCollision(tiledMap, "Collision");

        }
        #endregion


        #region initTiledMap Object
        private void initGrass()
        {
            var objectLayer = tiledMap.getObjectGroup("Object");
            var grassList = objectLayer.objectsWithName("grass");
            foreach(var grass in grassList)
            {
                addEntity(new Grass01().setPosition(grass.position + new Vector2(grass.width / 2, grass.height / 2)));
            }

        }

        #region expositionTree

        private void initExpositionTree()
        {
            if((GameEvent.homeEvent & GameHomeEvent.killShuChengCaveBoss) == 0)
            {
                var objectLayer = tiledMap.getObjectGroup("Object");
                var treeObject = objectLayer.objectWithName("tree");
                var tree = addEntity(new Barrier_Tree_Exposion());
                tree.setPosition(treeObject.position + new Vector2(treeObject.width / 2, treeObject.height / 2));
                tree.setRotation((float)Math.Atan2(1,0));

                var colliderEntity = createEntity("collision");
                colliderEntity.setPosition(treeObject.position + new Vector2(treeObject.width / 2, treeObject.height / 2));
                colliderEntity.addComponent<FSRigidBody>()
                    .setBodyType(BodyType.Static);
                var colliderShape = colliderEntity.addComponent<FSCollisionBox>()
                    .setSize(16, 48);
                colliderShape.setCollisionCategories(CollisionSetting.wallCategory);

                tree.triggerEvent = () =>
                {
                    tree.destoryByExposion();
                    colliderEntity.destroy();
                };
            }
        }
        #endregion

        #region SceneChangeTrigger
        private void initSceneChangeTrigger()
        {
            var objectLayer = tiledMap.getObjectGroup("Position");
            var leaveTriggerObject = objectLayer.objectWithName("leaveTrigger");
            var cave2TriggerObject = objectLayer.objectWithName("cave2Trigger");
            var cave3TriggerObject = objectLayer.objectWithName("cave3Trigger");

            var leaveTriggerEntity = addEntity(new TriggerAbleObject(10, "摁E：离开"));
            leaveTriggerEntity.setPosition(leaveTriggerObject.position + new Vector2(leaveTriggerObject.width / 2, leaveTriggerObject.height / 2));
            leaveTriggerEntity.triggerEvent += leaveTriggerEvent;


            var cave2TriggerEntity = createEntity("cave2Trigger");
            cave2TriggerEntity.setPosition(cave2TriggerObject.position + new Vector2(cave2TriggerObject.width / 2, cave2TriggerObject.height / 2));
            cave2TriggerEntity.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic);

            var cave2Trigger = cave2TriggerEntity.addComponent<SceneChangeTriggerComponent>();
            cave2Trigger.setSize(cave2TriggerObject.width, cave2TriggerObject.height);
            cave2Trigger.setIsSensor(true);
            cave2Trigger.setCollisionCategories(CollisionSetting.tiledObjectCategory);
            cave2Trigger.setCollidesWith(CollisionSetting.playerCategory);

            cave2Trigger.onAdded += () =>
             {
                 cave2Trigger.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                 {
                     var cave2 = new Cave2();
                     var transition = new FadeTransition(() =>
                     {
                         player.detachFromScene();
                         player.attachToScene(cave2);
                         player.setPosition(100, 65);
                         return cave2;
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
                .setBodyType(BodyType.Static);

            var cave3Trigger = cave3TriggerEntity.addComponent<SceneChangeTriggerComponent>();
            cave3Trigger.setSize(cave3TriggerObject.width, cave3TriggerObject.height);
            cave3Trigger.setIsSensor(true);
            cave3Trigger.setCollisionCategories(CollisionSetting.tiledObjectCategory);
            cave3Trigger.setCollidesWith(CollisionSetting.playerCategory);

            cave3Trigger.onAdded += () =>
            {
                cave3Trigger.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {

                    return true;
                };
            };


        }

        private void leaveTriggerEvent()
        {
            var shuchengArea = new ShuChengArea();
            var transition = new FadeTransition(() =>
            {
                player.detachFromScene();
                player.attachToScene(shuchengArea);
                player.setPosition(430, 80);
                return shuchengArea;
            });

            transition.onTransitionCompleted += () =>
            {
                GameSetting.isGamePause = false;
            };
            Core.startSceneTransition(transition);

            GameSetting.isGamePause = true;
        }

        #endregion

        #region Enemy

        private void initEnemy()
        {
            var objectLayer = tiledMap.getObjectGroup("Enemy");
            var slimeList = objectLayer.objectsWithName("slime");
            foreach(var slime in slimeList)
            {
                addEntity(new Slime(slime.position));
            }

        }
        #endregion

        #endregion
    }
}

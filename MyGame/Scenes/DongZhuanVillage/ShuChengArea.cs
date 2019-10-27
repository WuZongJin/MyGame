using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameConponents.UtilityComponents;
using MyGame.GameEntities.Enemys.Bat;
using MyGame.GameEntities.Enemys.Slime;
using MyGame.GameEntities.Player;
using MyGame.GameEntities.Player.PlayerUI;
using MyGame.GameEntities.Player.PlayerUI.ScreenUI;
using MyGame.GameEntities.TiledObjects;
using MyGame.GlobalManages;
using MyGame.GlobalManages.GameManager;
using Nez;
using Nez.Farseer;
using Nez.Tiled;
using Nez.Tweens;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Scenes.DongZhuanVillage
{
    public class ShuChengArea:BasicScene
    {
        #region Properties
        Player player;

        TiledMap tiledMap;

        TiledObject caveTrigger;
        TiledObject roomTrigger;
        TiledObject villageTrigger;

        #region Entity
        Entity roomEntryEntity;
        #endregion
        #endregion

        #region Constructor
        public ShuChengArea()
        {

        }
        #endregion

        #region Override Method
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
           
            initTiledObject();
            initRoomEntryTrigger();
            initCaveTrigger();
            initVillageTrigger();
            initTreeObject();
            initGrass();

            //addEntity(new BigSlime()).setPosition(100, 100);
            
        }

        public override void onStart()
        {
            base.onStart();
            initCamera();
        }
        #endregion

        #region Player Method
        private void initPlayer()
        {
            var gameActorManager = Core.getGlobalManager<GameActorManager>();
            player = gameActorManager.player;
           
        }
        #endregion

        #region Tiled Method
        private void initTiledMap()
        {
            var tiledEntity = createEntity("tiledMap");
            tiledMap = content.Load<TiledMap>("TileMaps/Maps/DongZhuang/ShuChengArea");
            var tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap));
            tiledMapComponent.setLayersToRender(new string[] { "Tiled","TiledObjects", "Tree", "Room" });
            tiledMapComponent.setRenderLayer(GameLayerSetting.tiledLayer);


            createMapCollision(tiledMap, "Collision");

            var objectPositionLayer = tiledMap.getObjectGroup("Position");
            villageTrigger = objectPositionLayer.objectWithName("villageTrigger");
            caveTrigger = objectPositionLayer.objectWithName("caveTrigger");
            roomTrigger = objectPositionLayer.objectWithName("roomTrigger");
            
            
        }
        #endregion

        #region TiledObject Method
        #region Room Entry Trigger
        private void initRoomEntryTrigger()
        {
            roomEntryEntity = createEntity("roomEntryEntity");
            roomEntryEntity.setPosition(new Vector2(roomTrigger.x + roomTrigger.width / 2, roomTrigger.y + roomTrigger.height / 2));
            roomEntryEntity.addComponent<FSRigidBody>()
                .setBodyType(FarseerPhysics.Dynamics.BodyType.Dynamic);

            var trigger = roomEntryEntity.addComponent<SceneChangeTriggerComponent>();
            trigger.setSize(20, 20).setIsSensor(true);
            trigger.setCollisionCategories(CollisionSetting.ItemsCategory);
            trigger.setCollidesWith(CollisionSetting.playerCategory);

            trigger.onAdded += onAdded_roomtirgger;
        }

        private void onAdded_roomtirgger()
        {
            var rigidBody = roomEntryEntity.getComponent<FSRigidBody>();
            rigidBody.body.onCollision += Body_onCollision;
        }

        private bool Body_onCollision(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            var shuchengRoom = new ShuChengRoom();
           
            var transition = new FadeTransition(() =>
            {
                player.detachFromScene();
                player.attachToScene(shuchengRoom);
                player.setPosition(120, 200);
                return shuchengRoom;
            });
            
            transition.onTransitionCompleted += () =>
            {
                GameSetting.isGamePause = false;
            };
            Core.startSceneTransition(transition);

            GameSetting.isGamePause = true;
            return true;
        }
        #endregion

        #region Expotion Tree Object

        private void initTreeObject()
        {
            if((GameEvent.homeEvent & GameHomeEvent.openTheWayToVillage) == 0)
            {
                var objectLayer = tiledMap.getObjectGroup("Object");
                var treeObject = objectLayer.objectWithName("tree");
                var tree = addEntity(new Barrier_Tree_Exposion());
                tree.setPosition(treeObject.position+new Vector2(treeObject.width / 2, treeObject.height / 2));


                var collision = this.createEntity("collision");
                collision.setPosition(treeObject.position + new Vector2(treeObject.width / 2, treeObject.height / 2));
                collision.addComponent<FSRigidBody>()
                    .setBodyType(BodyType.Static);
                var shape = collision.addComponent<FSCollisionBox>()
                        .setSize(48, 16);
                shape.setCollisionCategories(CollisionSetting.wallCategory);

                var trigger = addEntity(new TriggerAbleObject());
                trigger.setPosition(treeObject.position + new Vector2(treeObject.width / 2, treeObject.height / 2));

                trigger.triggerEvent = () =>
                {
                    var uiManager = Core.getGlobalManager<GameUIManager>();
                    IList<Conmunication> conmunications = new List<Conmunication>();
                    conmunications.Add(new Conmunication("Images/headIcons/Link_Sprite", "好像路被树挡住了，无法通过啊"));
                    conmunications.Add(new Conmunication("Images/headIcons/Link_Sprite", "我记得房子旁边的地窖里面有炸弹，利用炸弹应该可以把路炸开"));
                    uiManager.createConmunication(conmunications);
                };

                tree.triggerEvent = () =>
                {
                    GameEvent.homeEvent = GameEvent.homeEvent | GameHomeEvent.openTheWayToVillage;
                    tree.destoryByExposion();
                    collision.destroy();
                    trigger.destroy();
                };

            }
        }



        #endregion

        #region caveTrigger
        private void initCaveTrigger()
        {
            var caveEntry = addEntity(new TriggerAbleObject(16,"摁E：进入"));
            caveEntry.setPosition(caveTrigger.position+new Vector2(caveTrigger.width/2,caveTrigger.height/2));

            caveEntry.triggerEvent = () =>
            {
                var cave = new Cave1();

                var transition = new FadeTransition(() =>
                {
                    player.detachFromScene();
                    player.attachToScene(cave);
                    player.setPosition(250, 100);
                    return cave;
                });

                transition.onTransitionCompleted += () =>
                {
                    GameSetting.isGamePause = false;
                };
                Core.startSceneTransition(transition);

                GameSetting.isGamePause = true;
            };


        }
        #endregion

        #region Village Trigger
        private void initVillageTrigger()
        {
            var entity = createEntity("dongzhuangvillage");
            entity.setPosition(villageTrigger.position + new Vector2(villageTrigger.width / 2f, villageTrigger.height / 2f));
            entity.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic);

            var trigger = entity.addComponent<SceneChangeTriggerComponent>();
            trigger.setSize(villageTrigger.width, villageTrigger.height);
            trigger.setIsSensor(true);
            trigger.setCollisionCategories(CollisionSetting.tiledObjectCategory);
            trigger.setCollidesWith(CollisionSetting.playerCategory);

            trigger.onAdded += () =>
            {
                trigger.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var area = new DongZhuangVillage();
                    var transition = new FadeTransition(() =>
                    {
                        player.detachFromScene();
                        player.attachToScene(area);
                        player.setPosition(120, 915);
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
        #endregion

        #region grass
        private void initGrass()
        {
            var objectLayer = tiledMap.getObjectGroup("Object");
            var grasses = objectLayer.objectsWithName("grass");
            foreach(var grass in grasses)
            {
                addEntity(new Grass01()).setPosition(grass.position+new Vector2(grass.width/2,grass.height/2));
            }
        }
        #endregion

        private void initTiledObject()
        {
            var objectLayer = tiledMap.getObjectGroup("Object");
            var sign = objectLayer.objectWithName("sign");
            var rock01 = objectLayer.objectWithName("rock01");

            addEntity(new Rock(rock01.name, rock01));
            var signEntity = addEntity(new Sign("homeSign", sign));
            signEntity.triggerEvent += triggerEvent;
        }
        

        private void triggerEvent()
        {
            GameUIManager gameUIManager = Core.getGlobalManager<GameUIManager>();
            gameUIManager.createConfirmUI("这是我的房子");
        }
        #endregion

        #region Camera Method
        private void initCamera()
        {
            camera.entity.addComponent(new FollowCamera(player));
            
        }
        #endregion
    }
}

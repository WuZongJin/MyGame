using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameEntities.Enemys.Slime;
using MyGame.GameEntities.Items.Bombs;
using MyGame.GameEntities.Player;
using MyGame.GameEntities.TiledObjects;
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

namespace MyGame.Scenes.DongZhuanVillage
{
    public class Cave3:BasicScene
    {
        #region Properties
        TiledMap tiledMap;
        Player player;
        Entity bossAreaCollider;
        #endregion

        #region Constructor
        public Cave3()
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
            initExpositionTree();
            initSceneChnageTrigger();
            initBoss();



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
            tiledMap = content.Load<TiledMap>("TileMaps/Maps/DongZhuang/cave3");
            var tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap));
            tiledMapComponent.setLayersToRender(new string[] { "Tiled", "Wall" });
            tiledMapComponent.setRenderLayer(GameLayerSetting.tiledLayer);

            createMapCollision(tiledMap, "Collision");
        }
        #endregion

        #region initTree
        private void initExpositionTree()
        {
            if ((GameEvent.homeEvent & GameHomeEvent.killShuChengCaveBoss) == 0)
            {
                var objectLayer = tiledMap.getObjectGroup("Object");
                var treeObject = objectLayer.objectWithName("tree");
                var tree = addEntity(new Barrier_Tree_Exposion());
                tree.setPosition(treeObject.position + new Vector2(treeObject.width / 2, treeObject.height / 2));
                tree.setRotation((float)Math.Atan2(1, 0));

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
        private void initSceneChnageTrigger()
        {
            var objectLayer = tiledMap.getObjectGroup("Position");
            var cave1TriggerObject = objectLayer.objectWithName("cave1Trigger");
            var cave2TriggerObject = objectLayer.objectWithName("cave2Trigger");


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
                        player.setPosition(440, 250);
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
                        player.setPosition(780, 95);
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

        }
        #endregion

        #region initBoss
        private void initBoss()
        {
            if((GameEvent.homeEvent & GameHomeEvent.killShuChengCaveBoss) == 0)
            {
                var objectLayer = tiledMap.getObjectGroup("Object");
                var bossArea = objectLayer.objectWithName("bossArea");
                var boss = objectLayer.objectWithName("boss");

                var bigSlime = addEntity(new BigSlime());
                bigSlime.setPosition(boss.position);

                bossAreaCollider = createEntity("collision");
                Vertices verts = new Vertices(bossArea.polyPoints);
                bossAreaCollider
                    .addComponent<FSRigidBody>()
                    .setBodyType(BodyType.Static)
                    .addComponent<FSCollisionChain>()
                    .setVertices(verts)
                    .setCollisionCategories(CollisionSetting.wallCategory);

                bigSlime.onDeathed += () =>
                {
                    GameEvent.homeEvent = GameEvent.homeEvent | GameHomeEvent.killShuChengCaveBoss;
                    bossAreaCollider.destroy();
                    var box = addEntity(new TreasureBox());
                    box.setPosition(boss.position);
                    box.openBox += () =>
                    {
                        player.recipes.Add(new Recipe.BombRecipe());
                        player.pickUp(new BombComponent(),5);

                        var uiManager = Core.getGlobalManager<GameUIManager>();
                        //uiManager.createConfirmUI("获得炸弹和炸弹制作书");

                        IList<Conmunication> conmunications = new List<Conmunication>();
                        conmunications.Add(new Conmunication("Images/headIcons/Link_Sprite", "终于获得了炸弹"));
                        conmunications.Add(new Conmunication("Images/headIcons/Link_Sprite", "可以利用炸弹将地图上一些挡路的东西炸开了"));
                        conmunications.Add(new Conmunication("Images/headIcons/Link_Sprite", "而且我还在宝箱中找到了炸弹的制作书，这样子炸弹用完了可以摁Q在制作界面制作炸弹了"));
                        uiManager.createConmunication(conmunications);
                    };
                };

            }


        }
        #endregion

    }
}

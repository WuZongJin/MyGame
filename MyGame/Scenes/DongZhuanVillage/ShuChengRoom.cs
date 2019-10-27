using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameEntities.Enemys.Octor;
using MyGame.GameEntities.Items;
using MyGame.GameEntities.Items.Armors;
using MyGame.GameEntities.Items.Bombs;
using MyGame.GameEntities.Items.NormalRecoverMedicine;
using MyGame.GameEntities.Items.Weapon;
using MyGame.GameEntities.NPC;
using MyGame.GameEntities.Player;
using MyGame.GameEntities.Player.PlayerUI;
using MyGame.GameEntities.Player.PlayerUI.ScreenUI;
using MyGame.GameEntities.TiledObjects;
using MyGame.GameLanguage.DongZhuangVillage;
using MyGame.GlobalManages;
using MyGame.GlobalManages.GameManager;
using MyGame.GlobalManages.Notification;
using Nez;
using Nez.BitmapFonts;
using Nez.Farseer;
using Nez.Systems;
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
    public class ShuChengRoom:BasicScene
    {
        #region Properties
        ShuChengRoomTextContent textContent;

        Vector2 playerInitPosition;
        Vector2 boxPosition;
        Rectangle outRoomTriggerRectangle;

        Player player;
        Entity outRoomTriggerEntity;
        TiledMap tiledMap;
        #endregion

        #region Constructor
        public ShuChengRoom()
        {

        }
        #endregion


        #region override Method
        public override void initialize()
        {
            base.initialize();
            addRenderer(new RenderLayerRenderer(0, GameLayerSetting.actorMoverLayer
                , GameLayerSetting.tiledLayer
                ));

            addRenderer(new RenderLayerRenderer(0, GameLayerSetting.debugDrawViewLayer));

            initPlayer();

            initTiledMap();
            initObjects();
            initTreasureBox();

            //addEntity(new Bottle01()).setPosition(200, 200);
            //addEntity(new Grass01()).setPosition(200, 240);

            addEntity(new BombPropsEntity()).setPosition(150, 200);
            //addEntity(new MonsterOilEntity(50)).setPosition(100, 200);
            //addEntity(new Normalcuirass()).setPosition(100, 150);

            //addEntity(new NormalRecoverMedicineEntity(2)).setPosition(100, 100);
            //addEntity(new PlantFiberEntity(3)).setPosition(100, 150);
            //addEntity(new xRock()).setPosition(100, 120);

            //addEntity(new DongZhuangVillageMayor()).setPosition(100,70);
            //addEntity(new Octor()).setPosition(100, 100);
            //test();
            var gate = new MazeGateDown();
            gate.triggerEvent += () =>
            {
                if (player.weapon == null)
                {
                    var uiManager = Core.getGlobalManager<GameUIManager>();
                    IList<Conmunication> conmunications = new List<Conmunication>();
                    conmunications.Add(new Conmunication("Images/headIcons/Link_Sprite", "大门需要钥匙"));
                    uiManager.createConmunication(conmunications);
                }
                else
                {
                    gate.destoryedByKey();
                }
            };
            addEntity(gate).setPosition(100, 120);


        }

        private void test()
        {
            var mtest = createEntity("fallinHole").setPosition(150,70);
            mtest.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic);

            var box = mtest.addComponent(new SceneObjectTriggerComponentBox());
            box.setSize(100, 100).setIsSensor(true);

            box.setCollisionCategories(CollisionSetting.tiledHoleCategory);
            box.setCollidesWith(CollisionSetting.playerCategory | CollisionSetting.enemyCategory);

            box.onAdded += () =>
            {
                box.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                 {
                     var component = fixtureB.userData as Component;
                     if(component.entity is IFallinable)
                     {
                         var mposition = component.entity.position;
                         var entity = component.entity as IFallinable;
                         entity.fallinHole = box.GetFixture();
                         entity.potentialFallin = true;
                         entity.fallinReturnPosition = mposition;
                     }

                     return true;
                 };
                box.GetFixture().onSeparation += (fixtureA, fixtureB) =>
                {
                    var component = fixtureB.userData as Component;
                    if (component.entity is IFallinable)
                    {
                        var mposition = component.entity.position;
                        var entity = component.entity as IFallinable;
                        entity.fallinHole = null;
                        entity.potentialFallin = false;
                        entity.fallinReturnPosition = Vector2.Zero;
                    }
                };

            };
        }

        public override void onStart()
        {
            base.onStart();
            initCamera();
        }
        #endregion

        #region Map Text ,Data, Source Initialize 
        private void initializeData()
        {

        }

        #endregion

        #region Tiled Method
        private void initTiledMap()
        {
            var tiledEntity = createEntity("tiledMap");
            tiledMap = content.Load<TiledMap>("TileMaps/Maps/DongZhuang/ShuChengRoom");
            var tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap));
            tiledMapComponent.setLayersToRender(new string[] { "Tiled", "Wall", "RoomObject" });
            tiledMapComponent.setRenderLayer(GameLayerSetting.tiledLayer);

            createMapCollision(tiledMap, "Collision");

            var objectPositionLayer = tiledMap.getObjectGroup("Position");
            playerInitPosition = objectPositionLayer.objectWithName("initPosition").position;
            boxPosition = objectPositionLayer.objectWithName("boxPosition").position;
            

            var nextSceneTrigger = objectPositionLayer.objectWithName("nextScene");
            outRoomTriggerRectangle = new Rectangle(nextSceneTrigger.x, nextSceneTrigger.y, nextSceneTrigger.width, nextSceneTrigger.height);


        }

        #endregion

        #region Player Method
        private void initPlayer()
        {
            player = Core.getGlobalManager<GameActorManager>().player;
           
        }


        public void initEntity(Entity entity)
        {
            entity.setPosition(65,85);
            addEntity(entity);
        }
        #endregion

        #region Tiled Object Method
        private void initObjects()
        {
            outRoomTriggerEntity = createEntity("outRoomTriggerEntity");
            outRoomTriggerEntity.setPosition(outRoomTriggerRectangle.Center.ToVector2());

            var rigidBody = outRoomTriggerEntity.addComponent<FSRigidBody>()
                .setBodyType(FarseerPhysics.Dynamics.BodyType.Static)
                .setFixedRotation(true);

            var trigger = outRoomTriggerEntity.addComponent<SceneChangeTriggerComponent>();
            trigger.setSize(outRoomTriggerRectangle.Width, outRoomTriggerRectangle.Height);
            trigger.setIsSensor(true);
            trigger.setCollisionCategories(CollisionSetting.tiledObjectCategory);
            trigger.setCollidesWith(CollisionSetting.playerCategory);

            trigger.onAdded += onAdded_tirgger;
        }

        private bool Body_onCollision(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (player.weapon == null)
            {
                var uiManager = Core.getGlobalManager<GameUIManager>();
                if ((GameEvent.homeEvent & GameHomeEvent.initWeaponBoxOpened) == 0)
                {

                    IList<Conmunication> conmunications = new List<Conmunication>();
                    conmunications.Add(new Conmunication("Images/headIcons/Link_Sprite", "宝箱里有武器，还是把武器带上吧"));
                    uiManager.createConmunication(conmunications);
                }
                else
                {
                    IList<Conmunication> conmunications = new List<Conmunication>();
                    conmunications.Add(new Conmunication("Images/headIcons/Link_Sprite", "江湖险恶还是把武器先装备上吧，摁Q选择装备进行装备"));
                    uiManager.createConmunication(conmunications);
                }
                
               
                player.position += new Vector2(0, -5);
            }
            else
            {

                var shuchengArea = new ShuChengArea();


                var transition = new FadeTransition(() =>
                {
                    player.detachFromScene();
                    player.attachToScene(shuchengArea);
                    player.setPosition(345, 110);
                    return shuchengArea;
                });

                transition.onTransitionCompleted = () =>
                {
                    GameSetting.isGamePause = false;
                };

                Core.startSceneTransition(transition);

                GameSetting.isGamePause = true;
            }
            return true;
        }
        

        private void onAdded_tirgger()
        {
            var rigidBody = outRoomTriggerEntity.getComponent<FSRigidBody>();
            rigidBody.body.onCollision += Body_onCollision;
        }

        #region Box Object
        private void initTreasureBox()
        {

            var box = addEntity(new TreasureBox());
            box.setPosition(boxPosition); 
            if ((GameEvent.homeEvent & GameHomeEvent.initWeaponBoxOpened) == 0)
            {
                box.openBox += openBox;
            }
            else
            {
                box.boxHasOpend();
            }
        }

        private void openBox()
        {
            GameEvent.homeEvent = GameEvent.homeEvent|GameHomeEvent.initWeaponBoxOpened;
            PlayerUILocker.playerUILocker = PlayerUILocker.playerUILocker | PlayerUIElementLocker.EquitWindow|PlayerUIElementLocker.ItemWindow;

            var texture = Core.content.Load<Texture2D>("Images/ItemsIcon/W_Sword001");
            var weapon = new NormalswordWeapon(texture);
            player.pickUp(weapon);
            
            //player.recipes.Add(new Recipe.BombRecipe());

            GameSetting.isGamePause = true;
            var ui = Core.scene.findEntity("ui").getComponent<UICanvas>();
            Table table = ui.stage.addElement(new Table());
            table.setFillParent(true).center();
            var dialog = new Dialog("提示", Skin.createDefaultSkin());
            table.add(dialog).center();
            dialog.center();
            dialog.setSize(400, 500);
            LabelStyle labelStyle = new LabelStyle(new Color(255,255,255,0));
            var label = new Label("你获得了一把武器,请摁Q键打开菜单进行装备",labelStyle);
            label.setWrap(true);

            dialog.getContentTable().add(label).width(300).height(100);
            var button = new TextButton("确定", Skin.createDefaultSkin());
            dialog.getButtonTable().add(button).width(70).height(20).setPadBottom(10);
            button.onClicked += obj => {
                GameSetting.isGamePause = false;
                dialog.hide();
            };

        }

       
        #endregion
        #endregion

        #region Camera Method
        private void initCamera()
        {
            camera.entity.addComponent(new FollowCamera(player));
        }
        #endregion
    }


}

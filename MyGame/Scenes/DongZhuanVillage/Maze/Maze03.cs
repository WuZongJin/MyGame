﻿using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameEntities.Enemys.Archer;
using MyGame.GameEntities.Enemys.Bat;
using MyGame.GameEntities.Items.Weapon;
using MyGame.GameEntities.Player;
using MyGame.GameEntities.TiledObjects;
using MyGame.GlobalManages;
using MyGame.GlobalManages.GameManager;
using MyGame.Recipe;
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
    public class Maze03 : BasicScene
    {
        #region Properties
        Player player;
        TiledMap tiledMap;
        #endregion

        #region Constrcutor
        public Maze03()
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
            initHole(); initEnemy();
            initGrassAndRock();

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
            tiledMap = content.Load<TiledMap>("TileMaps/Maps/DongZhuang/DongZhuangMaze03");
            var tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap));
            tiledMapComponent.setLayersToRender(new string[] { "Tiled","UpTiled", "Wall" });
            tiledMapComponent.setRenderLayer(GameLayerSetting.tiledLayer);



            createMapCollision(tiledMap, "Collision");
        }
        #endregion

        #region 
        private void initSceneChangeTrigger()
        {
            var objectLayer = tiledMap.getObjectGroup("Object");

           
            var maze2Object = objectLayer.objectWithName("maze2");
        

           
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
                        player.setPosition(290, 170);
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

        #region init Hole
        private void initHole()
        {
            var objectLayer = tiledMap.getObjectGroup("Hole");
            var holeList = objectLayer.objectsWithName("hole");
            foreach(var hole in holeList)
            {
                addEntity(new TiledHole(hole.position + new Vector2(hole.width / 2f, hole.height / 2f), new Vector2(hole.width, hole.height)));
            }

        }
        #endregion

        #region init Grass And Rock
        private void initGrassAndRock()
        {
            var objectLayer = tiledMap.getObjectGroup("GrassAndRock");
            var grassList = objectLayer.objectsWithName("grass");
            foreach(var grass in grassList)
            {
                addEntity(new Grass01()).setPosition(grass.position+new Vector2(grass.width/2f,grass.height/2f));
            }

        }
        #endregion

        #region initEnemy
        private void initEnemy()
        {
            var objectLayer = tiledMap.getObjectGroup("Object");
            var enemy = objectLayer.objectWithName("enemy");

            if ((GameEvent.dzMazeEvent & DongZhuangMazeEvent.PlayerGetArrow) == 0)
            {

                var archer = addEntity(new Archer());
                archer.setPosition(enemy.position);

                archer.onDeathed += () =>
                {
                    var Tbox = scene.addEntity(new TreasureBox());
                    Tbox.setPosition(157, 176);
                    Tbox.openBox += () =>
                    {
                        GameEvent.dzMazeEvent = GameEvent.dzMazeEvent | DongZhuangMazeEvent.PlayerGetArrow;
                        player.pickUp(new NormalBowWeapon());
                        player.recipes.Add(new ArrowRecipe());

                        var uiManager = Core.getGlobalManager<GameUIManager>();
                        IList<Conmunication> conmunications = new List<Conmunication>();
                        conmunications.Add(new Conmunication("Images/headIcons/Link_Sprite", "我获得了一把弓，按Q键打开武器菜单进行查看，或按Tab键快速切换"));
                        conmunications.Add(new Conmunication("Images/headIcons/Link_Sprite", "弓可以进行远程攻击，且必须要有箭才可进行攻击"));
                        conmunications.Add(new Conmunication("Images/headIcons/Link_Sprite", "箭的配方已经加入到制造中了，没有箭的时候可以进行制造哦"));
                        uiManager.createConmunication(conmunications);


                    };
                };
            }

            var bats = objectLayer.objectsWithName("bat");
            foreach(var bat in bats)
            {
                addEntity(new Bat()).setPosition(bat.position);
            }
        }

        #endregion
    }
}

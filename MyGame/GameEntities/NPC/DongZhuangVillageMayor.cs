using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameEntities.TiledObjects;
using MyGame.GlobalManages;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.NPC
{
    public class DongZhuangVillageMayor:Entity
    {
        #region Properties
        Sprite animation;
        #endregion

        #region Constructor
        public DongZhuangVillageMayor():base("VillageMayor")
        {

        }
        #endregion

        #region Override Method
        public override void onAddedToScene()
        {
            base.onAddedToScene();

            var texture = scene.content.Load<Texture2D>("Images/Actors/DongZhuangVillageMayor");
            animation = addComponent(new Sprite(texture));
            animation.setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));

            initCollision();
            initTalkTrigger();
        }
        #endregion

        #region Collision Method
        private void initCollision()
        {
            addComponent<FSRigidBody>()
                .setBodyType(BodyType.Static);

            var shape = addComponent<FSCollisionCircle>()
                .setRadius(5);

            shape.setCollisionCategories(CollisionSetting.wallCategory);

        }

        private void initTalkTrigger()
        {
            var talkTriggerEntity = scene.addEntity(new TriggerAbleObject(16, "摁E ：谈话"));
            talkTriggerEntity.setPosition(this.position);

            talkTriggerEntity.triggerEvent = () =>
            {
                if ((GameEvent.dzVillageEvent & DongZhunagVillageEvent.AcceptVillageMayorTask) == 0)
                {
                    var uiManager = Core.getGlobalManager<GameUIManager>();
                    IList<Conmunication> conmunications = new List<Conmunication>();
                    conmunications.Add(new Conmunication("Images/Actors/DongZhuangVillageMayor", "书辰，你终于来了"));
                    conmunications.Add(new Conmunication("Images/Actors/DongZhuangVillageMayor", "刚才发生了好大的动静，该来的终究是要来了"));
                    conmunications.Add(new Conmunication("Images/Actors/DongZhuangVillageMayor", "但是在这之前你能不能先去村子的北部的洞穴里找一下我的女儿"));
                    conmunications.Add(new Conmunication("Images/Actors/DongZhuangVillageMayor", "塔进去之后还没回来，我有点担心她"));
                    conmunications.Add(new Conmunication("Images/headIcons/Link_Sprite", "没问题我去找一下她"));
                    uiManager.createConmunication(conmunications);
                    GameEvent.dzVillageEvent = GameEvent.dzVillageEvent | DongZhunagVillageEvent.AcceptVillageMayorTask;
                }
                else if((GameEvent.dzVillageEvent & DongZhunagVillageEvent.ClearDongZhuangVillageCave) == 0)
                {
                    var uiManager = Core.getGlobalManager<GameUIManager>();
                    IList<Conmunication> conmunications = new List<Conmunication>();
                    conmunications.Add(new Conmunication("Images/Actors/DongZhuangVillageMayor", "我女儿还在洞穴里快去救她吧"));
                   
                    uiManager.createConmunication(conmunications);
                }
            };
        }
        #endregion

    }
}

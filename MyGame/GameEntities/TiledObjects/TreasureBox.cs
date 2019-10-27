using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.TiledObjects
{
    public class TreasureBox:Entity,IPauseable
    {
        public enum TreasureBoxStates
        {
            Close,
            Open,
        }
        #region Properties
        Sprite<TreasureBoxStates> animation;
        bool hasOpened;
        bool collisioned;
        public Action openBox;
        Sprite eButtonSprite;
        Sprite openSprite;


        SceneObjectTriggerComponent trigger;

        public bool couldPause { get; set; }
        #endregion

        #region Constructor
        public TreasureBox() : base("treasureBox")
        {
            hasOpened = false;
            collisioned = false;
            couldPause = true;
        }
        #endregion

        #region Override
        public override void onAddedToScene()
        {
            var texture = scene.content.Load<Texture2D>("Images/ItemsObjects/treasureBox_small");
            var subtexture = Subtexture.subtexturesFromAtlas(texture, 14, 14);

            animation = addComponent(new Sprite<TreasureBoxStates>(subtexture[0]));
            animation.setRenderLayer(GameLayerSetting.actorMoverLayer)
                .setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));


            animation.addAnimation(TreasureBoxStates.Close, new SpriteAnimation(subtexture[0]));
            animation.addAnimation(TreasureBoxStates.Open, new SpriteAnimation(subtexture[1]));

            if(hasOpened)
                animation.currentAnimation = TreasureBoxStates.Open;
            else
                animation.currentAnimation = TreasureBoxStates.Close;
            animation.play(animation.currentAnimation);

            var rigidBody = addComponent<FSRigidBody>()
                .setBodyType(FarseerPhysics.Dynamics.BodyType.Dynamic);

            trigger = addComponent<SceneObjectTriggerComponent>();
            trigger.setRadius(10)
                .setIsSensor(true);

            trigger.setCollisionCategories(CollisionSetting.ItemsCategory);
            trigger.setCollidesWith(CollisionSetting.playerCategory);

            trigger.onAdded += onAddedMethod;


            var eButtontexture = scene.content.Load<Texture2D>("Images/ItemsObjects/EButton");
            eButtonSprite = addComponent(new Sprite(eButtontexture));
            eButtonSprite.setLocalOffset(new Vector2(-14, -14 - eButtonSprite.height));
            eButtonSprite.enabled = false;

            var openTexture = scene.content.Load<Texture2D>("Images/ItemsObjects/openFont");
            openSprite = addComponent(new Sprite(openTexture));
            openSprite.setLocalOffset(new Vector2(-14 + eButtonSprite.width*2, -14 - eButtonSprite.height));
            openSprite.enabled = false;
        }

        public override void update()
        {
            if (couldPause && GameSetting.isGamePause) return;
            base.update();
            if (!hasOpened && collisioned)
            {
                if (Input.isKeyDown(Keys.E))
                {
                    open();
                }
            }
        }

        #endregion

        #region Collision Method
        private void onAddedMethod()
        {
            trigger.GetFixture().onCollision += onCollision;
            trigger.GetFixture().onSeparation += onSeparation;
        }

        private bool onCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            collisioned = true;
            if(!hasOpened)
            {
                eButtonSprite.enabled = true;
                openSprite.enabled = true;
            }
                
            return true;
        }

        private void onSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            collisioned = false;
            eButtonSprite.enabled = false;
            openSprite.enabled = false;
        }

        #endregion

        #region Method
        public void boxHasOpend()
        {

            hasOpened = true;
        }

        public void open()
        {
            animation.currentAnimation = TreasureBoxStates.Open;
            animation.play(animation.currentAnimation);
            hasOpened = true;
            eButtonSprite.enabled = false;
            openSprite.enabled = false;
            openBox?.Invoke();
        }
        #endregion

    }
}

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using Nez.TextureAtlases;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.TiledObjects
{
    public class Bottle01:Entity,IPauseable
    {
        private enum BottleAnimation
        {
            Idle,
            Broke,
        }

        #region Properties
        public bool couldPause { get; set; }
        Subtexture[] animationTexture;
        Sprite<BottleAnimation> animation;
        public Action triggerEvent;
        Entity colliderEntity;
        #endregion

        #region Constructor
        public Bottle01()
        {
            couldPause = true;
            animationTexture = new Subtexture[4];
        }
        #endregion

        #region Override Method
        public override void onAddedToScene()
        {
            base.onAddedToScene();

            var texture = scene.content.Load<Texture2D>("TileMaps/MapTextures/objects");
            var texturePacker = TexturePackerAtlas.create(texture, 32, 32);
            animationTexture[0] = texturePacker.createRegion("1",64,96,32,32);
            animationTexture[1] = texturePacker.createRegion("2", 96, 96, 32, 32);
            animationTexture[2] = texturePacker.createRegion("3", 128, 96, 32, 32);
            animationTexture[3] = texturePacker.createRegion("4", 160, 96, 32, 32);

            animation = addComponent(new Sprite<BottleAnimation>(animationTexture[0]));
            animation.setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));

            animation.addAnimation(BottleAnimation.Idle, new SpriteAnimation(animationTexture[0]));
            var spriteAnimation = new SpriteAnimation(new List<Subtexture>()
            {
                animationTexture[0],
                animationTexture[1],
                 animationTexture[2],
                animationTexture[3],

            });
            spriteAnimation.loop = false;
            animation.addAnimation(BottleAnimation.Broke, spriteAnimation);
            animation.onAnimationCompletedEvent += Animation_onAnimationCompletedEvent;

            animation.currentAnimation = BottleAnimation.Idle;
            animation.play(BottleAnimation.Idle);

            var rigidBody = addComponent<FSRigidBody>()
                .setBodyType(FarseerPhysics.Dynamics.BodyType.Dynamic);

            var shape = addComponent<SceneObjectTriggerComponent>();
             shape.setRadius(5)
                ;

            shape.setCollisionCategories(CollisionSetting.tiledObjectCategory);
            shape.setCollidesWith(CollisionSetting.allAttackCategory|CollisionSetting.allAttackTypeCategory);

            shape.onAdded = onAddedMethod;

            colliderEntity = scene.createEntity("collision");
            colliderEntity.setPosition(this.position);
            colliderEntity.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Static);

            var colliderShape = colliderEntity.addComponent<FSCollisionCircle>();
            colliderShape.setRadius(4);
            colliderShape.setCollisionCategories(CollisionSetting.wallCategory);

            triggerEvent = () =>
            {
                colliderEntity.destroy();
            };
        }

        private void Animation_onAnimationCompletedEvent(BottleAnimation obj)
        {
            if (obj == BottleAnimation.Broke)
                this.destroy();
        }

        private void onAddedMethod()
        {
            var collider = getComponent<SceneObjectTriggerComponent>();
            collider.GetFixture().onCollision += onCollision;

        }
        private bool onCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            this.destoryByAttack();
            triggerEvent?.Invoke();
            return true;
        }

        private void destoryByAttack()
        {
            animation.currentAnimation = BottleAnimation.Broke;
        }
        #endregion
    }
}

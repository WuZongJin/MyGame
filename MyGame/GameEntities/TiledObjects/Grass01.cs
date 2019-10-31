using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameEntities.Items;
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
    public class Grass01 : Entity, IPauseable
    {
        private enum GrassAnimations
        {
            Idle,
            Broke,
        }

        #region properties
        public Action onBroked;
        Sprite<GrassAnimations> animations;
        Subtexture[] subtextures;

        public Action triggerEvent;
        Entity colliderEntity;
        public bool couldPause { get; set; }
        #endregion

        #region Constructor
        public Grass01():base("grass")
        {
            couldPause = true;
            subtextures = new Subtexture[5];
            onBroked += () =>
            {
                float number = Nez.Random.nextFloat();
                if (number < 0.4)
                {
                    scene.addEntity(new PlantFiberEntity()).setPosition(this.position);
                }
            };
        }
        #endregion

        #region Override 
        public override void onAddedToScene()
        {
            base.onAddedToScene();
            var texture = scene.content.Load<Texture2D>("TileMaps/MapTextures/objects");
            var texturePacker = TexturePackerAtlas.create(texture, 16, 16);
            subtextures[0] = texturePacker.createRegion("idle",32,0,16,16);
            subtextures[1] = texturePacker.createRegion("1", 64, 16, 32, 32);
            subtextures[2] = texturePacker.createRegion("2", 96, 16, 32, 32);
            subtextures[3] = texturePacker.createRegion("3", 128, 16, 32, 32);
            subtextures[4] = texturePacker.createRegion("4", 160, 16, 32, 32);

            animations = addComponent(new Sprite<GrassAnimations>(subtextures[0]));
            animations.setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));
            animations.addAnimation(GrassAnimations.Idle, new SpriteAnimation(subtextures[0]));
            var animation = new SpriteAnimation(new List<Subtexture>()
            {
                subtextures[0],
                subtextures[1],
                subtextures[2],
                subtextures[3],
                subtextures[4],
            });
            animation.loop = false;
            animations.addAnimation(GrassAnimations.Broke, animation);
            animations.onAnimationCompletedEvent += Animations_onAnimationCompletedEvent;
            animations.currentAnimation = GrassAnimations.Idle;


            var rigidBody = addComponent<FSRigidBody>()
                .setBodyType(FarseerPhysics.Dynamics.BodyType.Dynamic)
                .setIsSleepingAllowed(false);

            var shape = addComponent<SceneObjectTriggerComponent>();
            shape.setRadius(8);
               

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
                if (!colliderEntity.isDestroyed)
                    colliderEntity.destroy();
            };
        }

        private void Animations_onAnimationCompletedEvent(GrassAnimations obj)
        {
            if(obj == GrassAnimations.Broke)
            {
                onBroked?.Invoke();
                this.destroy();
            }
        }


        #endregion
        #region Collision
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
            if(animations.currentAnimation!= GrassAnimations.Broke)
                animations.currentAnimation = GrassAnimations.Broke;
        }
        #endregion
    }
}

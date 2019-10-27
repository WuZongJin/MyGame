using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameEntities.AnimationEffects.Deatheffect;
using MyGame.GameEntities.Items;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.TiledObjects
{
    public class xRock:Entity
    {
        public enum xRockAniamtion
        {
            Idle,
            Broke,
        }

        Sprite<xRockAniamtion> animations;
        Entity colliderEntity;
        public Action triggerEvent;
        int rockIndex;
        static int[] rockIndexs = new int[]
        {
            32,34,36,38,40,42,44
        };
        public xRock(int index = 32) : base("rock")
        {
            this.rockIndex = index;
        }

        public static xRock create()
        {
            var rand = Nez.Random.nextInt(7);
            return new xRock(rockIndexs[rand]);
        }

        #region Override
        public override void onAddedToScene()
        {
            base.onAddedToScene();
            var texture = GameResources.GameTextureResource.xObjectPacker.Packer.getSubtexture(rockIndex);
            var brokeTexture = GameResources.GameTextureResource.xObjectPacker.Packer.getSubtexture(rockIndex+1);
            animations = addComponent(new Sprite<xRockAniamtion>(texture));
            animations.setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));

            animations.addAnimation(xRockAniamtion.Idle, new SpriteAnimation(texture));
            animations.addAnimation(xRockAniamtion.Broke, new SpriteAnimation(brokeTexture));
            animations.currentAnimation = xRockAniamtion.Idle;


            var rigidBody = addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic);

            var shape = addComponent<SceneObjectTriggerComponent>();
            shape.setRadius(5);

            shape.setCollisionCategories(CollisionSetting.tiledObjectCategory);
            shape.setCollidesWith(CollisionSetting.expositionCategory);

            shape.onAdded += () =>
            {
                shape.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    this.destoryByExposition();
                    triggerEvent?.Invoke();
                    return true;
                };
            };

            colliderEntity = scene.createEntity("collision");
            colliderEntity.setPosition(this.position);
            colliderEntity.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Static);

            var colliderShape = colliderEntity.addComponent<FSCollisionCircle>();
            colliderShape.setRadius(6);
            colliderShape.setCollisionCategories(CollisionSetting.wallCategory);


            triggerEvent = () =>
            {
                if (!colliderEntity.isDestroyed)
                    colliderEntity.destroy();
            };
        }

        #endregion

        private void destoryByExposition()
        {
            if (animations.currentAnimation != xRockAniamtion.Broke)
            {
                float number = Nez.Random.nextFloat();
                if (number < 0.7)
                {
                    var entity = scene.addEntity(new RockItemEntity()).setPosition(this.position);
                }
                animations.currentAnimation = xRockAniamtion.Broke;
                scene.addEntity(new RockDestoryeffects()).setPosition(this.position);
                Core.schedule(0.2f, timer => { this.destroy(); });
            }
        }


    }
}

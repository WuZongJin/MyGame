using FarseerPhysics.Common.PhysicsLogic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GameConponents.AttackComponents;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items.Bombs
{
    public class BombEntity:Entity
    {
        enum BombAnimation
        {
            waitBoom,
            Boom,
        }
        #region Properties
        Sprite<BombAnimation> animation;
        float waitTime = 2f;
        float timer = 0f;

        bool changeColor = true;
        bool startBoom = false;
        #endregion

        #region Constructor
        public BombEntity() : base("bombEntity")
        {

        }
        #endregion


        #region Override Method
        public override void onAddedToScene()
        {
            base.onAddedToScene();
            var bombTexture = GameResources.GameTextureResource.boomTexture.Packer.getSubtexture("bomb");
            var waitBombAnimation = GameResources.GameTextureResource.boomTexture.Packer.getSpriteAnimation("waitboom");
            var boomAnimtion = GameResources.GameTextureResource.boomTexture.Packer.getSpriteAnimation("bombboom");
            boomAnimtion.loop = false;

            addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic)
                .setFixedRotation(true) ;



            addComponent<FSCollisionCircle>()
                .setRadius(7);


            animation = addComponent(new Sprite<BombAnimation>(bombTexture));
            animation.setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));

            animation.addAnimation(BombAnimation.waitBoom, waitBombAnimation);
            animation.addAnimation(BombAnimation.Boom, boomAnimtion);
            animation.currentAnimation = BombAnimation.waitBoom;
            animation.onAnimationCompletedEvent += Animation_onAnimationCompletedEvent;
        }

        private void Animation_onAnimationCompletedEvent(BombAnimation obj)
        {
            if(obj== BombAnimation.Boom)
            {
                this.destroy();
            }
        }

        public override void update()
        {
            base.update();
            animation.setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));
            if (!startBoom)
            {
                timer += Time.deltaTime;
                if (timer > waitTime)
                {
                    timer = 0f;
                    startBoom = true;
                    animation.currentAnimation = BombAnimation.Boom;
                    var shape = getComponent<FSCollisionCircle>();
                    
                    var entity = createAttackEntity();


                    FSWorld fSWorld = scene.getSceneComponent<FSWorld>();
                    SimpleExplosion simpleExplosion = new SimpleExplosion(fSWorld.world);
                    simpleExplosion.enabledOnCategories = CollisionSetting.playerCategory | CollisionSetting.enemyCategory;
                    simpleExplosion.activate(shape.GetFixture().body.position, 0.5f, 0.02f,0.035f);

                    shape.removeComponent();
                    Core.schedule(0.2f, timer => { entity.destroy(); });
                }
                if (changeColor)
                {
                    changeColor = false;
                    var time = (waitTime - timer) / 5f;
                    var sprite = getComponent<Sprite>();
                    sprite.color = Color.Red;
                    Core.schedule(0.1f, timer => { sprite.color = Color.White; });
                    Core.schedule(time,timer=>{ changeColor = true;   });
                }
            }
        }

        private Entity createAttackEntity()
        {
            var entity = Core.scene.createEntity("boomAttackEntity");
            entity.setPosition(this.position);
            entity.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic)
                .setFixedRotation(false);

            var shape = entity.addComponent<SceneObjectTriggerComponent>();
            shape.setRadius(25).setIsSensor(true);

            shape.setCollisionCategories(CollisionSetting.expositionCategory);
            shape.onAdded += ()=>
            {
                var rigidBody = entity.getComponent<FSRigidBody>();
                rigidBody.body.onCollision += Body_onCollision;
            };

            return entity;
        }


        private bool Body_onCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            var componentA = fixtureA.userData as Component;
            var componentB = fixtureB.userData as Component;
            var actorProperty = componentB.entity.getComponent<ActorPropertyComponent>();
            if (actorProperty == null)
                return true;

            actorProperty.executeDamage?.Invoke(AttackTypes.Physics, 50);

            //var rigidBody = componentB.entity.getComponent<FSRigidBody>();
            //var dir = componentB.entity.position - componentA.entity.position;
            //dir.Normalize();
            //rigidBody.body.applyLinearImpulse(dir * rigidBody.body.mass);
            return true;
        }

        
        #endregion

    }
}

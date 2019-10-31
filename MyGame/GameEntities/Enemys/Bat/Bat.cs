using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GameEntities.AnimationEffects.Deatheffect;
using Nez;
using Nez.AI.FSM;
using Nez.Farseer;
using Nez.Sprites;
using Nez.TextureAtlases;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Enemys.Bat
{
    public class Bat:Entity,IPauseable
    {
        public enum BatAnimations
        {
            Idle,
            Fly,
        }

        #region Properties
        public Sprite<BatAnimations> animation;
        public BatAnimations currentAnimtion;
        public BatAnimations preAnimation;

        public ActorPropertyComponent actorProperty;

        public Entity target = null;
        public float fieldOfView = 120.0f;
        public bool isFindTarget = false;

        public float attackTime = 1f;
        public float attackTimer = 0f;
        public float patrolTime = 3.0f;
        public float patrolTimer = 0f;

        SimpleStateMachine<BatStates> stateMachine;

        public float moveSpeed { get { return this.actorProperty.moveSpeed + actorProperty.moveSpeedUpValue + actorProperty.moveSpeed * actorProperty.moveSpeedUpRate; } }

        public bool couldPause { get; set; }
        #endregion

        #region Override 
        public override void onAddedToScene()
        {
            base.onAddedToScene();
            this.setScale(2f);

            this.actorProperty = addComponent<ActorPropertyComponent>();
            this.actorProperty.HP = this.actorProperty.MaxHP = 30;
            this.actorProperty.moveSpeed = 70f;

            var texture = scene.content.Load<Texture2D>("Images/Enemys/Bat");
            var texturePacker = new TexturePackerAtlas(texture);
            var idleTexture = texturePacker.createRegion("batidle", 0, 33, 16, 16);
            var flyTextur1 = texturePacker.createRegion("fly01", 0, 9, 16, 16);
            var flyTextur2 = texturePacker.createRegion("fly02", 16, 9, 16, 16);
            var flyTextur3 = texturePacker.createRegion("fly03", 32, 9, 16, 16);
            var flyTextur4 = texturePacker.createRegion("fly04", 48, 9, 16, 16);
            animation = addComponent(new Sprite<BatAnimations>(idleTexture));
            animation.setRenderLayer(GameLayerSetting.tiledActorUpLayer);
            animation.addAnimation(BatAnimations.Idle, new SpriteAnimation(idleTexture));
            animation.addAnimation(BatAnimations.Fly, new SpriteAnimation(new List<Subtexture>() { flyTextur1, flyTextur2, flyTextur3, flyTextur4 }));
            animation.currentAnimation = BatAnimations.Idle;


            addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic).
                setLinearDamping(0.8f)
                .setMass(1.5f);

            var enemtCollider = addComponent<FSCollisionCircle>()
                .setRadius(8);

            enemtCollider.setCollisionCategories(CollisionSetting.enemyCategory);
            enemtCollider.setCollidesWith(
                CollisionSetting.allAttackTypeCategory
                | CollisionSetting.playerAttackCategory
               );

            var damageTriger = addComponent<DamageTrigerComponent>();
            damageTriger.setRadius(10).setIsSensor(true);
            damageTriger.setCollisionCategories(
                CollisionSetting.enemyAttackCategory);
            damageTriger.setCollidesWith(CollisionSetting.playerCategory);
            damageTriger.onAdded += () =>
            {
                damageTriger.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var component = fixtureB.userData as Component;
                    var actorProperty = component.entity.getComponent<ActorPropertyComponent>();
                    actorProperty.executeDamage?.Invoke(AttackTypes.Physics, 5);
                    var rigidbody = component.entity.getComponent<FSRigidBody>();
                    var dir = component.entity.position - this.position;
                    dir.Normalize();
                    rigidbody.body.applyLinearImpulse(dir * rigidbody.body.mass);
                    return true;
                };
            };

            
            var viewFieldTrigger = addComponent<EnemyViewComponent>();
            viewFieldTrigger.setRadius(fieldOfView);
            viewFieldTrigger.setIsSensor(true);
            viewFieldTrigger.setCollisionCategories(CollisionSetting.enemyCategory);
            viewFieldTrigger.setCollidesWith(CollisionSetting.playerCategory);

            viewFieldTrigger.onAdded += () =>
            {
                viewFieldTrigger.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    target = ((Component)(fixtureB.userData)).entity;
                    isFindTarget = true;
                    return true;
                };

            };

            

            addComponent<RigidBodyVelocityLimited>();
            addComponent(new BatSampleStateMachine(this));
        }

        public override void update()
        {
            base.update();
            if (actorProperty.HP <= 0)
            {
                scene.addEntity(new Deatheffects01()).setPosition(this.position);
                this.destroy();
            }
        }
        #endregion


    }
}

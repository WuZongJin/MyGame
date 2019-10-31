using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GameEntities.AnimationEffects.Deatheffect;
using MyGame.GlobalManages.GameManager;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Enemys.DZMazeBoss
{
    public class DZMazeBoss:Entity,IPauseable
    {
        #region Proiperties
        public Action onDeathed;
        public Sprite sprite;
        public ActorPropertyComponent actorProperty;
        public FSCollisionCircle collider;
        public FSRigidBody rigidBody;
        public Entity target;

        public bool startMove = false;

        public float moveSpeed { get { return this.actorProperty.moveSpeed + actorProperty.moveSpeedUpValue + actorProperty.moveSpeed * actorProperty.moveSpeedUpRate; } }

        public bool couldPause { get; set; }
        #endregion

        #region Constructor
        public DZMazeBoss()
        {
            couldPause = true;
        }

        #endregion

        #region OverrideMethod
        public override void update()
        {
            if (couldPause && GameSetting.isGamePause)
                return;
            base.update();

            if (actorProperty.HP <= 0)
            {
                scene.addEntity(new Deatheffects02()).setPosition(this.position).setScale(2f);
                onDeathed?.Invoke();
                this.destroy();
            }
        }

        public override void onAddedToScene()
        {
            base.onAddedToScene();
            this.actorProperty = addComponent<ActorPropertyComponent>();
            this.actorProperty.HP = this.actorProperty.MaxHP = 500;
            this.actorProperty.moveSpeed = 70;

            var texture = scene.content.Load<Texture2D>("Images/Enemys/dzMazeBoss");
            sprite = addComponent(new Sprite(texture));

            initCollision();
            this.target = Core.getGlobalManager<GameActorManager>().player;

            addComponent(new DZMazeBossSimpleStateMachine(this));
            addComponent(new DZMazeBossHPBar(this)).setRenderLayer(GameLayerSetting.playerUiLayer);
        }
        #endregion

        #region Collision
        private void initCollision()
        {
            rigidBody = addComponent<FSRigidBody>()
                .setBodyType(FarseerPhysics.Dynamics.BodyType.Dynamic)
                .setLinearDamping(100f)
                .setFixedRotation(true);


            collider = addComponent<FSCollisionCircle>()
                .setRadius(8);
            collider.setCollisionCategories(CollisionSetting.enemyCategory);
            collider.setCollidesWith(CollisionSetting.wallCategory
                | CollisionSetting.allAttackTypeCategory
                | CollisionSetting.playerAttackCategory
               );

            var damageTriger = addComponent<DamageTrigerComponent>();
            damageTriger.setRadius(8).setIsSensor(true);
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

            addComponent<RigidBodyVelocityLimited>();
        }
        #endregion

    }
}

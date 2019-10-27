using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GameConponents.UtilityComponents;
using MyGame.GameEntities.AnimationEffects.Deatheffect;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Enemys.Archer
{
    public class Archer : Entity, IPauseable
    {
        #region  Properties

        public Action onDeathed;
        public ActorPropertyComponent actorProperty;
        public FSRigidBody rigidBody;
        public FSCollisionCircle collider;

        public FallinAbleComponent fallinAbleComponent;
        public Entity target = null;
        public float fieldOfView = 120.0f;
        public bool isFindTarget = false;
        #endregion

        #region Constructor
        public Archer()
        {
            couldPause = true;
        }

        public bool couldPause { get; set; }
        #endregion

        #region Override
        public override void update()
        {
            if (couldPause && GameSetting.isGamePause)
                return;
            base.update();
            if (actorProperty.HP <= 0)
            {
                scene.addEntity(new Deatheffects02()).setPosition(this.position);
                onDeathed?.Invoke();
                this.destroy();
            }


        }

        public override void onAddedToScene()
        {
            base.onAddedToScene();
            this.actorProperty = addComponent<ActorPropertyComponent>();
            this.actorProperty.HP = this.actorProperty.MaxHP = 200;
            this.actorProperty.moveSpeed = 70;

            var texture = scene.content.Load<Texture2D>("Images/Enemys/Archer");
            addComponent(new Sprite(texture)).setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));

            initCollision();

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
        }

        #endregion

        #region initCollision
        private void initCollision()
        {
            rigidBody = addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic)
                .setFixedRotation(true)
                .setLinearDamping(0.8f)
                .setMass(2f);

            collider = addComponent<FSCollisionCircle>()
               .setRadius(8);
            collider.setCollisionCategories(CollisionSetting.enemyCategory);
            collider.setCollidesWith(CollisionSetting.wallCategory
                | CollisionSetting.allAttackTypeCategory
                | CollisionSetting.playerAttackCategory
                |CollisionSetting.tiledHoleCategory
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

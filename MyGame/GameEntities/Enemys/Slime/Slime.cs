using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GameEntities.AnimationEffects.Deatheffect;
using MyGame.GameEntities.Items;
using Nez;
using Nez.AI.BehaviorTrees;
using Nez.Farseer;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.GameEntities.Enemys.Slime
{
    public class Slime:Entity,IPauseable
    {
        public enum SlimeAnimation
        {
            Idle,
            Move,
        }

        #region Properties
        public Sprite<SlimeAnimation> animation;
        SlimeAnimation currentAnimation;
        SlimeAnimation preAnimation;
        public ActorPropertyComponent actorProperty;
        DamageTrigerComponent damageTriger;
        public FSRigidBody rigidBody;
        public FSCollisionCircle collider;
        public EnemyViewComponent viewFieldTrigger;
        public EnemyComponent enemyComponent;

        public float fieldOfView = 70.0f;
        public float fieldofLostView = 100.0f;
        public float lostViewTime = 2.5f;
        public float lostViewTimer = 0f;

        public bool isFindTarget = false;
        public bool isLostTarget = false;
        public bool startLostTarget = false;
        public bool startPatrol = false;
        Vector2 patrolDirection;
        public Entity target = null;

        public float chaseTime = 3f;
        public float chaseTimer = 0f;
        public float patrolTime = 1.0f;
        public float patrolTimer = 0f;
        public float attackFrequencyTime = 0.5f;
        public float attackFrequencyTimer = 0f;

        Vector2 moveDir = Vector2.Zero;
        public Vector2 initPosition;
        public bool isBackToInitPosition
        {
            get
            {
                int size = 50;
                Rectangle rectangle = new Rectangle((int)initPosition.X - size / 2, (int)initPosition.Y - size / 2, size, size);
                return rectangle.Contains(position.ToPoint());
            }
        }

        public bool couldPause { get; set; }
        #endregion

        #region Constructor
        public Slime(Vector2 position) : base("slime")
        {
            couldPause = true;
            setTag((int)EntityTags.ENEMY);
            this.setPosition(position);
            initPosition = position;
        }
        #endregion

        #region Override Method
        public override void onAddedToScene()
        {
            base.onAddedToScene();
            var texture = scene.content.Load<Texture2D>("Images/Enemys/Slime");
            var subtexture = Subtexture.subtexturesFromAtlas(texture, 32, 32);

            initAnimation(subtexture);
            initActorProperty();
            initCollision();
            addComponent(new SlimeBehaviorTree(this));
        }
        public override void update()
        {
            if (couldPause && GameSetting.isGamePause)
                return;

            preAnimation = currentAnimation;
            base.update();
            if (moveDir != Vector2.Zero)
            {
                currentAnimation = SlimeAnimation.Move;
            }
            else
            {
                currentAnimation = SlimeAnimation.Idle;
            }

            if (currentAnimation != preAnimation)
            {
                changeAnimeion(currentAnimation);
            }
            
        }
        #endregion

        #region Animation
        private void changeAnimeion(SlimeAnimation animation)
        {
            currentAnimation = animation;
            this.animation.play(currentAnimation);
        }
        private void initAnimation(List<Subtexture> subtexture)
        {
            animation = addComponent(new Sprite<SlimeAnimation>(subtexture[3]));
            animation.addAnimation(SlimeAnimation.Idle, new SpriteAnimation(subtexture[3]));
            animation.addAnimation(SlimeAnimation.Move, new SpriteAnimation(new List<Subtexture>()
            {
                subtexture[0],
                subtexture[1],
                subtexture[2],
                subtexture[3],
                subtexture[4],
                subtexture[5],
                subtexture[6],
                subtexture[7],
            }));
            currentAnimation = SlimeAnimation.Idle;
        }
        #endregion

        #region ActorProperties
        private void initActorProperty()
        {
            actorProperty = addComponent<ActorPropertyComponent>();
            actorProperty.HP = actorProperty.MaxHP = 30;
                
            actorProperty.moveSpeed = 50f;

        }
        #endregion

        #region Collision
        private void initCollision()
        {
            rigidBody = addComponent<FSRigidBody>()
                .setBodyType(FarseerPhysics.Dynamics.BodyType.Dynamic)
                .setLinearDamping(0.8f)
                .setMass(2f);

            collider = addComponent<FSCollisionCircle>()
                .setRadius(8);
            collider.setCollisionCategories(CollisionSetting.enemyCategory);
            collider.setCollidesWith(CollisionSetting.wallCategory
                |CollisionSetting.allAttackTypeCategory
                |CollisionSetting.playerAttackCategory
               );


            damageTriger = addComponent<DamageTrigerComponent>();
            damageTriger.setRadius(16).setIsSensor(true);
            damageTriger.setCollisionCategories(
                CollisionSetting.enemyAttackCategory);
            damageTriger.setCollidesWith(CollisionSetting.playerCategory);
            damageTriger.onAdded += damageOnAdded;

            viewFieldTrigger = addComponent<EnemyViewComponent>();
            viewFieldTrigger.setRadius(fieldOfView);
            viewFieldTrigger.setIsSensor(true);
            viewFieldTrigger.setCollisionCategories(CollisionSetting.enemyCategory);
            viewFieldTrigger.setCollidesWith(CollisionSetting.playerCategory);

            viewFieldTrigger.onAdded += onAdded;

            addComponent<RigidBodyVelocityLimited>();

        }
        #endregion

        #region Collision event
        void damageOnAdded()
        {
            damageTriger.GetFixture().onCollision += damageTriger_onCollision;
        }
        private bool damageTriger_onCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            
            var component = fixtureB.userData as Component;
            var actorProperty = component.entity.getComponent<ActorPropertyComponent>();
            actorProperty.executeDamage?.Invoke(AttackTypes.Physics,5);
            var rigidbody = component.entity.getComponent<FSRigidBody>();
            var dir = component.entity.position - this.position;
            dir.Normalize();
            rigidbody.body.applyLinearImpulse(dir*rigidbody.body.mass);
            return true;
        }

       

        void onAdded()
        {
            viewFieldTrigger.GetFixture().onCollision += viewFieldTrigger_onCollision;
            viewFieldTrigger.GetFixture().onSeparation += viewFieldTrigger_onSeparation;
        }

        private bool viewFieldTrigger_onCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            var compoent = fixtureB.userData as Component;
            target = compoent.entity;
            isFindTarget = true;
            startLostTarget = false;
            isLostTarget = false;
            lostViewTimer = 0;
            return true;
        }
        private void viewFieldTrigger_onSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            isFindTarget = false;
            isLostTarget = true;
            target = null;
            startLostTarget = true;
        }

        #endregion

        #region Tree Method
        public TaskStatus patrol()
        {
            if (!startPatrol)
            {

                patrolDirection = new Vector2(Nez.Random.minusOneToOne(), Nez.Random.minusOneToOne());
                patrolDirection.Normalize();
                changeAnimeion(SlimeAnimation.Move);
                startPatrol = true;
            }
            patrolTimer += Time.deltaTime;
            if(patrolTimer>= patrolTime)
            {
                startPatrol = false;
                patrolTimer = 0f;
                return TaskStatus.Success;
            }

            moveDir = Vector2.Zero;
            moveDir = patrolDirection;
            var movement = moveDir * actorProperty.totalMoveSpeed * Time.deltaTime;
            FSCollisionResult fSCollisionResult;
            FixtureExt.collidesWithAnyFixtures(collider.GetFixture(), ref movement, out fSCollisionResult);
            rigidBody.entity.position += movement;

            return TaskStatus.Running;
        }

        public TaskStatus dead()
        {
            scene.addEntity(new Deatheffects01()).setPosition(this.position);
            var random = Nez.Random.nextFloat();
            if (random < 0.6)
            {
                scene.addEntity(new MonsterOilEntity(1)).setPosition(this.position);
            }

            destroy();
            return TaskStatus.Success;
        }

        public TaskStatus chase()
        {
            if (target == null) return TaskStatus.Failure;

            moveDir = Vector2.Zero;
            if ((int)target.position.X > (int)position.X)
                moveDir += new Vector2(1, 0);
            else if ((int)target.position.X < (int)position.X)
                moveDir += new Vector2(-1, 0);
            if ((int)target.position.Y > (int)position.Y)
                moveDir += new Vector2(0, 1);
            else if ((int)target.position.Y < (int)position.Y)
                moveDir += new Vector2(0, -1);

            var movement = moveDir * actorProperty.totalMoveSpeed * Time.deltaTime;
            FSCollisionResult fSCollisionResult;
            FixtureExt.collidesWithAnyFixtures(collider.GetFixture(), ref movement, out fSCollisionResult);
            rigidBody.entity.position += movement;

            chaseTimer += Time.deltaTime;
            if (chaseTimer >= chaseTime)
            {
                chaseTimer = 0f;
                return TaskStatus.Success;
            }

            return TaskStatus.Running;

        }

        public TaskStatus lostTarget()
        {

            if (startLostTarget)
            {
                lostViewTimer += Time.deltaTime;
                if (lostViewTimer >= lostViewTime)
                {
                    target = null;
                    lostViewTimer = 0f;
                    isLostTarget = true;
                    startLostTarget = false;
                    isFindTarget = false;
                    return TaskStatus.Success;
                }
            }
            else
            {
                return TaskStatus.Failure;
            }

            return TaskStatus.Running;
        }

        public TaskStatus back()
        {
            if (!isBackToInitPosition)
            {
                moveDir = Vector2.Zero;
                if ((int)initPosition.X > (int)position.X)
                    moveDir += new Vector2(1, 0);
                else if ((int)initPosition.X < (int)position.X)
                    moveDir += new Vector2(-1, 0);
                if ((int)initPosition.Y > (int)position.Y)
                    moveDir += new Vector2(0, 1);
                else if ((int)initPosition.Y < (int)position.Y)
                    moveDir += new Vector2(0, -1);

                var movement = moveDir * actorProperty.totalMoveSpeed * Time.deltaTime;
                FSCollisionResult fSCollisionResult;
                FixtureExt.collidesWithAnyFixtures(rigidBody.body.fixtureList[0], ref movement, out fSCollisionResult);
                rigidBody.entity.position += movement;

                return TaskStatus.Running;
            }
            else
            {
                isLostTarget = false;
            }

            return TaskStatus.Success;
        }

        public TaskStatus idle()
        {
            return TaskStatus.Running;
        }
        #endregion

    }
}

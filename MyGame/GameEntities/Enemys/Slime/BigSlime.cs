using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GameEntities.AnimationEffects.Deatheffect;
using MyGame.GlobalManages.GameManager;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyGame.GameEntities.Enemys.Slime.Slime;

namespace MyGame.GameEntities.Enemys.Slime
{
    public class BigSlime:Entity,IPauseable
    {

        #region Properties
        public Action onDeathed;
        public Sprite<SlimeAnimation> animation;
        public ActorPropertyComponent actorProperty;

        public FSRigidBody rigidBody;
        public FSCollisionCircle collider;

        public Entity target;
        public float moveSpeed { get { return this.actorProperty.moveSpeed + actorProperty.moveSpeedUpValue + actorProperty.moveSpeed * actorProperty.moveSpeedUpRate; } }

        public bool couldPause { get; set; }
        #endregion

        #region Constructor
        public BigSlime()
        {
            couldPause = false;
        }
        #endregion


        #region Override
        public override void onAddedToScene()
        {
            base.onAddedToScene();
            this.setScale(2f);
            this.actorProperty = addComponent<ActorPropertyComponent>();
            this.actorProperty.HP = this.actorProperty.MaxHP = 500;
            this.actorProperty.moveSpeed = 70;

            var texture = scene.content.Load<Texture2D>("Images/Enemys/Slime");
            var subtexture = Subtexture.subtexturesFromAtlas(texture, 32, 32);

            initAnimation(subtexture);
            initCollision();

            this.target = Core.getGlobalManager<GameActorManager>().player;
            addComponent(new BigSlimeSimpleStateMachine(this));
            addComponent(new BigSlimeHPBar(this)).setRenderLayer(GameLayerSetting.playerUiLayer); ;
        }

        public override void update()
        {
            if (couldPause && GameSetting.isGamePause)
                return;
            base.update();
            animation.setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));
            if(actorProperty.HP<=0)
            {
                scene.addEntity(new Deatheffects02()).setPosition(this.position).setScale(2f);
                onDeathed?.Invoke();
                this.destroy();
            }
        }
        #endregion

        #region Animation
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
            animation.currentAnimation = SlimeAnimation.Idle;
        }
        #endregion

        #region Collision
        private void initCollision()
        {
            rigidBody = addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic)
                .setFixedRotation(true)
                .setLinearDamping(0.8f)
                .setMass(4f);

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

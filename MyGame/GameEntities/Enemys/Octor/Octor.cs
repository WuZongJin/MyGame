using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GameEntities.AnimationEffects.Deatheffect;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Enemys.Octor
{
    public class Octor:Entity,IPauseable
    {
        public enum OctorAnimations
        {
            Attack,
            Appera,
            Hide,
        }
        #region Properties
        public Sprite<OctorAnimations> animations;
        ActorPropertyComponent actorProperty;
        OctorSimpleStateMachine stateMachine;

        public Entity colliderEntity;
        public FSCollisionCircle colliderShape;
        public Entity ViewTrigger;
        public Entity target;
        FSRigidBody rigidBody;

        float fieldOfView = 70.0f;
        public bool isFindTarget = false;
        public bool inHideBeAttacked = false;

        public float moveSpeed { get { return this.actorProperty.moveSpeed + actorProperty.moveSpeedUpValue + actorProperty.moveSpeed * actorProperty.moveSpeedUpRate; } }

        public bool couldPause { get; set; }

        #endregion

        #region Contsrtuctor
        public Octor()
        {
            couldPause = true;
        }
        #endregion

        #region Override
        public override void onAddedToScene()
        {
            base.onAddedToScene();

            this.actorProperty = addComponent<ActorPropertyComponent>();
            this.actorProperty.HP = this.actorProperty.MaxHP = 30;
            this.actorProperty.moveSpeed = 70;
            this.actorProperty.executeDamage = executeDamageMethod;

            var texture1 = scene.content.Load<Texture2D>("Images/Enemys/BlackOctor1");
            var texture2 = scene.content.Load<Texture2D>("Images/Enemys/BlackOctor2");
            var texture3 = GameResources.GameTextureResource.xObjectPacker.Packer.getSubtexture(750);

            animations = addComponent(new Sprite<OctorAnimations>(new Subtexture(texture1)));
            animations.addAnimation(OctorAnimations.Attack, new SpriteAnimation(new Subtexture(texture2)));
            animations.addAnimation(OctorAnimations.Appera, new SpriteAnimation(new Subtexture(texture1)));
            animations.addAnimation(OctorAnimations.Hide, new SpriteAnimation(texture3));

            animations.currentAnimation = OctorAnimations.Hide;

            rigidBody = addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic)
                .setLinearDamping(int.MaxValue)
                .setMass(2f);

            var collider = addComponent<FSCollisionCircle>()
                .setRadius(5);

            collider.setCollisionCategories(CollisionSetting.enemyCategory);
            collider.setCollidesWith(CollisionSetting.allAttackTypeCategory
                | CollisionSetting.playerAttackCategory);


            ViewTrigger = scene.createEntity("OctorViewTrigger").setPosition(this.position);
            ViewTrigger.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic);
            var view = ViewTrigger.addComponent<EnemyViewComponent>();
            view.setRadius(fieldOfView)
                .setIsSensor(true);

            view.setCollisionCategories(CollisionSetting.enemyCategory);
            view.setCollidesWith(CollisionSetting.playerCategory);

            view.onAdded += () =>
            {
                view.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var compoent = fixtureB.userData as Component;
                    target = compoent.entity;
                    isFindTarget = true;
                    return true;
                };

                view.GetFixture().onSeparation+=(fixtureA, fixtureB) =>{
                    target = null;
                    isFindTarget = false;
                };
            };

            colliderEntity = scene.createEntity("collision").setPosition(this.position);
            colliderEntity.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Static);

            colliderShape= colliderEntity.addComponent<FSCollisionCircle>()
                .setRadius(7);
            colliderShape.setCollisionCategories(CollisionSetting.wallCategory);


            stateMachine = addComponent(new OctorSimpleStateMachine(this));

                
        }

        public override void update()
        {
            if (couldPause && GameSetting.isGamePause)
                return;
            base.update();
            if (this.actorProperty.HP < 0)
            {
                ViewTrigger.destroy();
                colliderEntity.destroy();
                this.destroy();
            }
        }

        #endregion

        #region Method
        private void executeDamageMethod(AttackTypes attackTypes,int damage)
        {
            if(animations.currentAnimation == OctorAnimations.Hide)
            {
                scene.addEntity(new Deatheffects01()).setPosition(this.position);
                inHideBeAttacked = true;
                return;
            }

            this.actorProperty.HP -= damage;
            var damagenumEntity = Core.scene.createEntity("damageNum").setPosition(this.position);
            damagenumEntity.addComponent(new Text(Graphics.instance.bitmapFont, damage.ToString(), Vector2.Zero, new Color(255, 255, 255, 0)));
            Core.startCoroutine(damageNumEntityThrowUp(damagenumEntity));

           
            animations.color = Color.Red;
            Core.schedule(0.2f, timer => { if (!this.isDestroyed) animations.color = Color.White; });

        }

        private IEnumerator<object> damageNumEntityThrowUp(Entity entity)
        {
            float timer = 0f;
            while (true)
            {
                timer += Time.deltaTime;
                if (timer > 0.5f)
                {
                    entity.destroy();
                    yield break;
                }
                entity.position -= new Vector2(0, 1);
                yield return null;

            }
        }
        #endregion

    }
}

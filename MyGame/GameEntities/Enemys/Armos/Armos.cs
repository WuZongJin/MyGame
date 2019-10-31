using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GlobalManages.GameManager;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Enemys.Armos
{
    public class Armos:Entity, IPauseable
    {
        #region Properties

        public Sprite sprite;
        public Sprite shiled;
        public Action onDeathed;
        public ActorPropertyComponent actorProperty;
        public FSRigidBody rigidBody;
        public FSCollisionCircle collider;

        public Entity colliderEntity;

        public Entity target = null;
        public float fieldOfColse = 120.0f;
        public bool isTargetToClose = false;

        public float moveSpeed { get { return this.actorProperty.moveSpeed + actorProperty.moveSpeedUpValue + actorProperty.moveSpeed * actorProperty.moveSpeedUpRate; } }

        #endregion

        #region Constructor
        public Armos():base("armos")
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
                Death();
            }
            if (Vector2.Distance(this.position, target.position) >= fieldOfColse)
            {
                isTargetToClose = false;
            }
            else
            {
                isTargetToClose = true;
            }
        }

        public override void onAddedToScene()
        {
            base.onAddedToScene();

            target = Core.getGlobalManager<GameActorManager>().player;

            this.actorProperty = addComponent<ActorPropertyComponent>();
            this.actorProperty.HP = this.actorProperty.MaxHP = 100;
            this.actorProperty.moveSpeed = 0;

            var texture = scene.content.Load<Texture2D>("Images/Enemys/Armos");
            sprite = addComponent(new Sprite(texture));
            sprite.setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));

            var armor = scene.content.Load<Texture2D>("Images/ItemsObjects/Shield");
            shiled = addComponent(new Sprite(armor));
            //shiled.setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));
            shiled.color = new Color(255, 255, 255, 125);
            shiled.enabled = false;

            initCollision();
            addComponent(new ArmosSimpleStateMachine(this));
        }

        #endregion

        #region initCollision
        private void initCollision()
        {
            rigidBody = addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic)
                .setFixedRotation(true)
                .setIsSleepingAllowed (false)
                .setLinearDamping(0.8f)
                .setMass(2f);

            collider = addComponent<FSCollisionCircle>();
              collider.setRadius(20)
               .setIsSensor(true);
            collider.setCollisionCategories(CollisionSetting.enemyCategory);
            collider.setCollidesWith(
                CollisionSetting.playerAttackCategory
               );

            addComponent<RigidBodyVelocityLimited>();


            colliderEntity = scene.createEntity("collision").setPosition(this.position);
            colliderEntity.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Static);

            var shape = colliderEntity.addComponent<FSCollisionBox>()
                .setSize(28, 28);

            shape.setCollisionCategories(CollisionSetting.wallCategory);
        }
        #endregion

        #region Death
        void Death()
        {
            if (!colliderEntity.isDestroyed)
                colliderEntity.destroy();
            this.destroy();
            
        }
        #endregion

    }
}

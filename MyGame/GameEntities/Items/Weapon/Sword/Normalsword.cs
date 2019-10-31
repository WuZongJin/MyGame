using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GameConponents.AttackComponents;
using MyGame.GameResources;
using MyGame.GlobalManages;
using MyGame.GlobalManages.Notification;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items.Weapon
{
    public class Normalsword:Entity
    {

        #region Propeorties
        bool hasaddCollisionEvent = false;
        public Weapon weapon;
        #endregion

        #region Constructor
        public Normalsword()
        {
            this.setTag((int)EntityTags.ITEM);
        }
        #endregion

        #region Override
        public override void onAddedToScene()
        {
            var texture = Core.content.Load<Texture2D>("Images/ItemsIcon/W_Sword001");
            Weapon weapon = new NormalswordWeapon(texture);
            this.weapon = weapon;
            this.addComponent(new Sprite(texture)).setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));
            

            this.addComponent<FSRigidBody>()
                .setBodyType(FarseerPhysics.Dynamics.BodyType.Dynamic);

            

            var collision = addComponent<FSCollisionCircle>()
                    .setRadius(20);
            collision.setCollisionCategories(CollisionSetting.ItemsCategory);
            collision.setCollidesWith(CollisionSetting.wallCategory);

            var sensor = addComponent<FSCollisionCircle>()
                    .setRadius(50)
                    .setIsSensor(true);

            sensor.setCollisionCategories(CollisionSetting.ItemsCategory);
            sensor.setCollidesWith(CollisionSetting.playerCategory);


            
        }
        #endregion

        #region 
        public override void update()
        {
            base.update();
            if (!hasaddCollisionEvent)
            {
                getComponent<FSRigidBody>().body.onCollision += Body_onCollision; ;
                hasaddCollisionEvent = true;
            }
        }

        private bool Body_onCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            var picker = ((Component)fixtureB.userData).entity as Player.Player;
           
            if (picker != null)
            {
                picker.pickUp(this.weapon);
                this.destroy();
            }
            return true;
        }

        

        
        #endregion
    }


    public class NormalswordWeapon : Weapon
    {

        #region Properties
        float timer = 0;
        #endregion
        public NormalswordWeapon(Texture2D texture):base(texture, "剑", 10, 5, "一把普通的剑", 100, "攻击力:10 - 5","伤害 + 10")
        {
            id = GameItemId.noramlSworld;
            this.equit = Equit;
            this.tackOff = TackOff;
        }


        private void Equit(Equitment equitable, Player.Player equitableer)
        {
            var player = equitableer as Player.Player;
            if (player.weapon != null)
            {
                player.weapon.tackOff(player.weapon, equitableer);
            }
            player.weapon = (Weapon)equitable;
            equitable.equitableer = equitableer;
            equitableer.actorProperty.damageUpValue += 10;
            equitableer.items.Remove(equitable);
        }

        private void TackOff(Equitment equitable, Player.Player equitableer)
        {
            var player = equitableer as Player.Player;
            player.weapon = null;
            equitable.equitableer = null;
            equitableer.actorProperty.damageUpValue -= 10;
            equitableer.items.Add(equitable,1);
        }

        public override void update()
        {
            var player = equitableer as Player.Player;
            if (!isAttacked)
            {
                attack();
            }
            if(player.animation.isAnimationPlaying(player.currentAnimation)&&!player.animation.isPlaying)
            {
                timer += Time.deltaTime;
                if (timer > Attackinterval)
                {
                    timer = 0f;
                    beginAttack();
                }
                
            }               
        }

        public override void beginAttack()
        {
            isAttacked = false;
            isAttackOver = false;
            var player = equitableer as Player.Player;
            switch (player.currentDirection)
            {
                case Direction.Up:
                    player.changeAnimationState(Player.Player.PlayerAnimations.AttackUp);
                    break;
                case Direction.Down:
                    player.changeAnimationState(Player.Player.PlayerAnimations.AttackDown);

                    break;
                case Direction.Right:
                    player.changeAnimationState(Player.Player.PlayerAnimations.AttackRight);
                    break;
                case Direction.Left:
                    player.changeAnimationState(Player.Player.PlayerAnimations.AttackLeft);
                    player.animation.flipX = true;
                    break;
            }
        }

        public override void attack()
        {
            isAttacked = true;
            var player = equitableer as Player.Player;
            switch (player.currentDirection)
            {
                case Direction.Down:
                    player.scene.addEntity(new AttackEntity(this, player.position + new Vector2(0,15)));
                    break;
                case Direction.Up:
                    player.scene.addEntity(new AttackEntity(this, player.position + new Vector2(0, -15)));
                    break;
                case Direction.Left:
                    player.scene.addEntity(new AttackEntity(this, player.position + new Vector2(-15, 0)));
                    break;
                case Direction.Right:
                    player.scene.addEntity(new AttackEntity(this, player.position + new Vector2(15, 0)));
                    break;
            }
           
        }

        #region Attack Entity
        
        #endregion

        public override void endAttack()
        {
           
        }
        private class AttackEntity : Entity
        {
            #region Properties
            Weapon weapon;
            Vector2 psootion;
            #endregion
            public AttackEntity(Weapon weapon,Vector2 position) : base("playerAttackEntity")
            {
                this.weapon = weapon;
                this.setPosition(position);
            }

            public override void onAddedToScene()
            {
                setTag((int)EntityTags.ATTACK);
                
                

                addComponent<FSRigidBody>()
                    .setBodyType(BodyType.Dynamic);
                var shape = addComponent<FSCollisionBox>()
                    .setSize(30, 30)
                    .setIsSensor(true);

                shape.setCollisionCategories(CollisionSetting.playerAttackCategory);
                shape.setCollidesWith(CollisionSetting.enemyCategory|CollisionSetting.tiledObjectCategory);

                var attackTriger = addComponent(new AttackTrigerComponent(weapon));
                attackTriger.onAdded += onAdded;

                Core.schedule(0.25f, timer => { if (!isDestroyed) destroy(); });

            }

            private void onAdded()
            {
                var rigidBody = getComponent<FSRigidBody>();
                rigidBody.body.onCollision += Body_onCollision;
            }


            private bool Body_onCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
            {
                var componentA = fixtureA.userData as Component;
                var componentB = fixtureB.userData as Component;
                var actorProperty = componentB.entity.getComponent<ActorPropertyComponent>();
                if (actorProperty == null)
                    return true;

                var rigidBody = componentB.entity.getComponent<FSRigidBody>();
                var attackTriger = componentA.entity.getComponent<AttackTrigerComponent>();
                actorProperty.executeDamage?.Invoke(attackTriger.attackType,attackTriger.damage);
                var dir = componentB.entity.position - componentA.entity.position;
                dir.Normalize();
                rigidBody.body.applyLinearImpulse(dir * rigidBody.body.mass);
                return true;
            }
        }
    }

    
}

using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GameConponents.AttackComponents;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameConponents.UtilityComponents;
using MyGame.GameResources;
using MyGame.GlobalManages;
using MyGame.GlobalManages.Notification;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items.Weapon
{
    public class NormalBow:Entity
    {
        #region Properties

        public Weapon weapon;

        #endregion

        #region Constructor
        public NormalBow()
        {

        }

        #endregion

        #region Override
        public override void onAddedToScene()
        {
            base.onAddedToScene();
            var texture = Core.content.Load<Texture2D>("Images/ItemsIcon/W_Bow03");
            Weapon weapon = new NormalBowWeapon();
            addComponent(new Sprite(texture)).setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));
            this.addComponent<FSRigidBody>()
                .setBodyType(FarseerPhysics.Dynamics.BodyType.Dynamic);

            var collision = addComponent<FSCollisionCircle>()
                    .setRadius(20);
            collision.setCollisionCategories(CollisionSetting.ItemsCategory);
            collision.setCollidesWith(CollisionSetting.wallCategory);

            var sensor = addComponent<SceneObjectTriggerComponent>();
                sensor.setRadius(40)
                    .setIsSensor(true);

            sensor.setCollisionCategories(CollisionSetting.ItemsCategory);
            sensor.setCollidesWith(CollisionSetting.playerCategory);

            sensor.onAdded += () =>
            {
                sensor.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {

                    var picker = ((Component)fixtureB.userData).entity as Player.Player;

                    if (picker != null)
                    {
                        picker.pickUp(weapon, 1);
                        this.destroy();
                    }
                    return true;
                };
            };
        }

        #endregion

    }

    public class NormalBowWeapon : Weapon
    {
        #region Properties
        float timer = 0;
        float arrowSpeed = 150;
        #endregion
        #region Constructor
        public NormalBowWeapon()
        {
            itemIcon = new Nez.Textures.Subtexture(Core.content.Load<Texture2D>("Images/ItemsIcon/W_Bow03"));
            name = "普通的弓";
            maxDamage = 7;
            minDamage = 4;
            describetion = "一把普通的弓";
            saleMoney = 100;
            properties = new string[] { "伤害 + 7" };
            id = GameItemId.normalArrow;
            this.equit = Equit;
            this.tackOff = TackOff;
        }
        #endregion


        #region Equit and Tack Off
        private void Equit(Equitment equitment,Player.Player equitableer)
        {
            var player = equitableer as Player.Player;
            if (player.weapon != null)
            {
                player.weapon.tackOff(player.weapon, equitableer);
            }
            player.weapon = (Weapon)equitment;
            equitment.equitableer = equitableer;
            equitableer.actorProperty.damageUpValue += 7;
            equitableer.items.Remove(equitment);

        }
        private void TackOff(Equitment equitable, Player.Player equitableer)
        {
            var player = equitableer as Player.Player;
            player.weapon = null;
            equitable.equitableer = null;
            equitableer.actorProperty.damageUpValue -= 7;
            equitableer.items.Add(equitable, 1);
        }
        #endregion

        #region Method

        public override void update()
        {
            var player = equitableer as Player.Player;
            if (!isAttacked)
            {
                attack();
            }
            if (player.animation.isAnimationPlaying(player.currentAnimation) && !player.animation.isPlaying)
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
                    player.changeAnimationState(Player.Player.PlayerAnimations.BowAttackUp);
                    break;
                case Direction.Down:
                    player.changeAnimationState(Player.Player.PlayerAnimations.BowAttackDown);
                    break;
                case Direction.Left:
                    player.changeAnimationState(Player.Player.PlayerAnimations.BowAttackLeft);
                    break;
                case Direction.Right:
                    player.changeAnimationState(Player.Player.PlayerAnimations.BowAttackRight);
                    break;
            }
        }

        public override void attack()
        {
            isAttacked = true;
            var player = equitableer as Player.Player;
            if (player.items.Keys.Where(m => m.id == GameItemId.arrow).Count() > 0)
            {
                player.throwOut(player.items.Keys.Where(m => m.id == GameItemId.arrow).First());
                switch (player.currentDirection)
                {
                    case Direction.Up:
                        var degress = (float)Math.Atan2(0, 1);
                        addArrow(player.position).setRotation(degress).addComponent(new AutoMoveComponent(new Vector2(0, -1), arrowSpeed));
                        break;
                    case Direction.Down:
                        var degress1 = (float)Math.Atan2(0, -1);
                        addArrow(player.position).setRotation(degress1).addComponent(new AutoMoveComponent(new Vector2(0, 1), arrowSpeed));
                        break;
                    case Direction.Left:
                        var degress2 = (float)Math.Atan2(-1, 0);
                        addArrow(player.position).setRotation(degress2).addComponent(new AutoMoveComponent(new Vector2(-1, 0), arrowSpeed));
                        break;
                    case Direction.Right:
                        var degress3 = (float)Math.Atan2(1, 0);
                        addArrow(player.position).setRotation(degress3).addComponent(new AutoMoveComponent(new Vector2(1, 0), arrowSpeed));
                        break;
                }
            }
            else
            {
                var notification = Core.getGlobalManager<NotificationManager>();
                NotificationMessage message = new NotificationMessage(null,"缺少弓箭");
                notification.addNotification(message);
            }

        }

        public override void endAttack()
        {
            base.endAttack();
        }

        #endregion

        private Entity addArrow(Vector2 position)
        {
            var arrow = Core.scene.createEntity("arrow").setPosition(position);

            var texture = Core.content.Load<Texture2D>("Images/Players/Allplayer");
            var arrowTexture = new Subtexture(texture, new Rectangle(306, 2630 - 1191 - 16, 5, 16));
            arrow.addComponent(new Sprite(arrowTexture));

            arrow.addComponent(new AttackTrigerComponent(this));

            arrow.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic)
                .setIsBullet(true)
                .setIsSleepingAllowed(false)
                ;
            var trigger = arrow.addComponent<SceneObjectTriggerComponent>();
            trigger.setRadius(3)
                .setIsSensor(true);

            trigger.setCollisionCategories(CollisionSetting.playerAttackCategory);
            trigger.setCollidesWith(CollisionSetting.wallCategory
                | CollisionSetting.enemyCategory
                | CollisionSetting.tiledObjectCategory
                
                );
            trigger.onAdded += () =>
            {


                trigger.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var componentA = fixtureA.userData as Component;
                    var component = fixtureB.userData as Component;

                    var actorProperty = component.getComponent<ActorPropertyComponent>();
                    if (actorProperty != null)
                    {
                        var attackTriger = componentA.entity.getComponent<AttackTrigerComponent>();
                        actorProperty.executeDamage?.Invoke(attackTriger.attackType, attackTriger.damage);
                        var adir = component.entity.position - arrow.position;
                        adir.Normalize();
                        actorProperty.entity.getComponent<FSRigidBody>().body.applyLinearImpulse(adir * FSConvert.displayToSim);
                    }
                    arrow.destroy();
                    return true;
                };
            };

            return arrow;
        }
    }

}

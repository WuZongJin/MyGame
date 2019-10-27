using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
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

namespace MyGame.GameEntities.Items.Armors
{
    public class Normalcuirass : Entity
    {
        #region Properties
        bool hasaddCollisionEvent = false;
        Armor armor;
        #endregion

        #region Constructor
        public Normalcuirass() : base("normalcuirass")
        {
            this.setTag((int)EntityTags.ITEM);
            this.setScale(0.5f);
        }
        #endregion

        #region Override Method
        public override void onAddedToScene()
        {
            var texture = Core.content.Load<Texture2D>("Images/ItemsIcon/A_Armor04");
            armor = new Armor(texture, "胸甲", "普通的一件胸甲", 100, "最大生命值 + 100", "护甲 + 10");
            armor.equit = equitArmor;
            armor.tackOff = tackOffArmor;


            var rigidBody = addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic);

            var collision = addComponent<FSCollisionCircle>()
                .setRadius(10);

            collision.setCollisionCategories(CollisionSetting.ItemsCategory);
            collision.setCollidesWith(CollisionSetting.wallCategory);

            var shape = addComponent<FSCollisionCircle>()
                .setRadius(20)
                .setIsSensor(true);

            shape.setCollisionCategories(CollisionSetting.ItemsCategory);
            shape.setCollidesWith(CollisionSetting.playerCategory);

            addComponent(new Sprite(texture)).setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));

            armor.equit = equitArmor;
            armor.tackOff = tackOffArmor;
        }

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
                picker.pickUp(armor);
                
                this.destroy();
            }
            return true;
        }
        #endregion



        #region Method
        public void  equitArmor(Equitment equitable,Player.Player equitableer)
        {
            var player = equitableer as Player.Player;
            if (player.armor != null)
            {
                tackOffArmor(equitable, equitableer);
            }
            player.armor = equitable as Armor;
            equitable.equitableer = equitableer;
            float rate = (float)equitableer.actorProperty.HP / (float)equitableer.actorProperty.MaxHP;
            equitableer.actorProperty.MaxHP += 100;
            equitableer.actorProperty.HP = (int)(equitableer.actorProperty.MaxHP * rate);
            equitableer.actorProperty.armorUpValue += 10;
            equitableer.items.Remove(equitable);
        }

        public void tackOffArmor(Equitment equitable, Player.Player equitableer)
        {
            var player = equitableer as Player.Player;
            player.armor = null;
            equitable.equitableer = null;
            float rate = (float)equitableer.actorProperty.HP / (float)equitableer.actorProperty.MaxHP;
            equitableer.actorProperty.MaxHP -= 100;
            equitableer.actorProperty.HP = (int)(equitableer.actorProperty.MaxHP * rate);
            equitableer.actorProperty.armorUpValue -= 10;
            equitableer.items.Add(equitable,1);
        }
        #endregion
    }
}

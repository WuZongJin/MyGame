using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items
{
    public class ArrowEntity:Entity
    {
        int count;

        public ArrowEntity(int count = 1) : base("arrowEntity")
        {
            this.count = count;
            
        }

        public override void onAddedToScene()
        {
            base.onAddedToScene();
            var texture = Core.content.Load<Texture2D>("Images/Players/Allplayer");
            var subtexture = new Nez.Textures.Subtexture(texture, new Rectangle(306, 2630 - 1191 - 16, 5, 16));

            addComponent(new Sprite(subtexture)).setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));

            addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic);

            var shape = addComponent<SceneObjectTriggerComponent>();
            shape.setRadius(7).setIsSensor(true);
            shape.setCollisionCategories(CollisionSetting.ItemsCategory);
            shape.setCollidesWith(CollisionSetting.playerCategory);
            shape.onAdded = () =>
            {
                shape.GetFixture().onCollision += Body_onCollision;
            };

        }

        private bool Body_onCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            var picker = ((Component)fixtureB.userData).entity as Player.Player;

            if (picker != null)
            {
                picker.pickUp(new Arrow(), this.count);
                this.destroy();
            }
            return true;
        }

    }
}

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items.NormalRecoverMedicine
{
    public class NormalRecoverMedicineEntity:Entity
    {
        int count;
        public NormalRecoverMedicineEntity(int count = 1) : base("normalRecoverMedicine")
        {
            this.count = count;
            this.setScale(0.5f);
        }

        public override void onAddedToScene()
        {
            base.onAddedToScene();
            var texture = Core.content.Load<Texture2D>("Images/ItemsIcon/P_Red04");
            addComponent(new Sprite(texture)).setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));

            addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic)
                ;

            var shape = addComponent<SceneObjectTriggerComponent>();
                shape.setRadius(10).setIsSensor(true);

            shape.setCollisionCategories(CollisionSetting.ItemsCategory);
            shape.setCollidesWith(CollisionSetting.playerCategory);


            shape.onAdded+= onAddedMethod;


        }

        private void onAddedMethod()
        {
            var shape = getComponent<SceneObjectTriggerComponent>();
            shape.GetFixture().onCollision += Body_onCollision;
        }

        private bool Body_onCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            var picker = ((Component)fixtureB.userData).entity as Player.Player;

            if (picker != null)
            {
                picker.pickUp(new NormalRecoverMedicine(), count);
                this.destroy();
            }
            return true;
        }

    }
}

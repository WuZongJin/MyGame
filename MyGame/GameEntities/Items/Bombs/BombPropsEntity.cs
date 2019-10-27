using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
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

namespace MyGame.GameEntities.Items.Bombs
{
    public class BombPropsEntity:Entity
    {
        #region Properites
        public int count;
        public Texture2D texture;
        #endregion

        public BombPropsEntity(int count = 1) : base("bombProps")
        {
            this.count = count;
        }

        #region Override
        public override void onAddedToScene()
        {
            base.onAddedToScene();
            var Texture = GameResources.GameTextureResource.boomTexture.Packer.getSubtexture("bomb");
            addComponent(new Sprite(Texture)).setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));
            Text text = new Text();
            text.text = $"x{count}";
            addComponent(text).setLocalOffset(new Vector2(3,3)).setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));


            addComponent<FSRigidBody>()
                .setBodyType(FarseerPhysics.Dynamics.BodyType.Dynamic);

            var shape = addComponent<SceneObjectTriggerComponent>();
            shape.setRadius(10)
                .setIsSensor(true);

            shape.setCollisionCategories(CollisionSetting.ItemsCategory);
            shape.setCollidesWith(CollisionSetting.playerCategory);

            shape.onAdded += onAddedMethod;
        }
        #endregion

        #region CollisionMethod
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
                picker.pickUp(new BombComponent(),count);
                this.destroy();
            }
            return true;
        }
        #endregion

    }
}

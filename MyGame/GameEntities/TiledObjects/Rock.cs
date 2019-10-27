using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using MyGame.Common;
using MyGame.Common.Game;
using Nez;
using Nez.Farseer;
using Nez.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.TiledObjects
{
    public class Rock:Entity
    {
        TiledObject tiledObject;

        public Rock(string name, TiledObject tiledObject) : base(name)
        {
            this.tiledObject = tiledObject;
            this.setPosition(new Vector2(tiledObject.x, tiledObject.y - tiledObject.height));
        }

        #region override Method
        public override void onAddedToScene()
        {
            base.onAddedToScene();
            var tiledMap = scene.content.Load<TiledMap>("TileMaps/MapObjects/Rock01");
            var tiledMapComponent = addComponent(new TiledMapComponent(tiledMap));
            tiledMapComponent.setLayersToRender(new string[] { "object" });
            tiledMapComponent.setRenderLayer(GameLayerSetting.actorMoverLayer)
                .setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y))
                ;

            var colliderObject = tiledMap.getObjectGroup("Collision");
            var collision = colliderObject.objectWithName("collision");
           

            var rigidBody = addComponent<FSRigidBody>()
                .setBodyType(FarseerPhysics.Dynamics.BodyType.Static);

            Vertices vertices = new Vertices(collision.polyPoints);
            var collider = addComponent(new FSCollisionPolygon(vertices));
            collider.setCollisionCategories(CollisionSetting.wallCategory);
        }

        

        #endregion
    }
}

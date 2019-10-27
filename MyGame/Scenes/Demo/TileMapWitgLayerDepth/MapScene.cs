using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.GameConponents.AttackComponents;
using MyGame.GameConponents.UtilityComponents;
using MyGame.GameEntities.Enemys.Slime;
using MyGame.GameEntities.Items;
using MyGame.GameEntities.Items.Armors;
using MyGame.GameEntities.Items.Weapon;
using MyGame.GameEntities.Player;
using MyGame.GameEntities.Player.PlayerUI;
using Nez;
using Nez.DeferredLighting;
using Nez.Farseer;
using Nez.Shadows;
using Nez.Sprites;
using Nez.Textures;
using Nez.Tiled;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Scenes.Demo.TileMapWitgLayerDepth
{
    [SampleScene("Map LayerDepth", 20, "")]

    public class MapScene:DemoBasicScene
    {
        Player player;
        bool isItemWindowClosed = true;
        Window itemWindow;
        Window equitWindow;
        public override void initialize()
        {
            base.initialize();

            addRenderer(new RenderLayerRenderer(0 ,actorMoveLayer, tiledMapLayer));
            addRenderer(new RenderLayerRenderer(1, debugRenderLayer));

            addEntityProcessor(new AttackEntitySystem(new Matcher().all(typeof(AttackTrigerComponent))));
           

            var armor = addEntity(new Normalcuirass());
            armor.setPosition(new Vector2(300, 250));
            var armor2 = addEntity(new Normalcuirass());
            armor2.setPosition(new Vector2(300, 280));
            var armor3 = addEntity(new Normalcuirass());
            armor3.setPosition(new Vector2(300, 280));
            var armor4 = addEntity(new Normalcuirass());
            armor4.setPosition(new Vector2(300, 280));

            var sword = addEntity(new Normalsword());
            sword.setPosition(new Vector2(400, 200));

            var slime = addEntity(new Slime(new Vector2(500, 500)));


            player = addEntity(new Player());
            player.setPosition(new Vector2(400, 200));
            //player.addComponent(new PolyLight(200, Color.White)).setRenderLayer(lightRenderLayer);

            var tiledEntity = createEntity("tiled-map");
            var tiledMap = content.Load<TiledMap>(@"Tilemaps/Maps/xlgCave");
            var tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledMap));
            tiledMapComponent.setLayersToRender(new string[] { "Tiled","wall" });
            tiledMapComponent.renderLayer = tiledMapLayer;

            //createMapCollision(tiledMap, "collision");

            var topLeft = new Vector2(tiledMap.tileWidth, tiledMap.tileWidth);
            var bottomRight = new Vector2(tiledMap.tileWidth * (tiledMap.width - 1), tiledMap.tileWidth * (tiledMap.height - 1));
            tiledEntity.addComponent(new CameraBounds(topLeft, bottomRight));


            var texture = content.Load<Texture2D>("Images/Enemys/enemy");
            var moonEntity = createEntity("moon");
            moonEntity.setPosition(new Vector2(400, 400));
            moonEntity.addComponent(new Sprite(texture)).setRenderLayer(actorMoveLayer)
                .setLayerDepth((1000-moonEntity.position.Y)/1000);//.setMaterial( moonMaterial );

            

            camera.entity.addComponent(new FollowCamera(player));
            
        }


        public override void update()
        {
            base.update();
        }

        public void createMapCollision(TiledMap tiledMap, string objectGrounpName)
        {
            var collisionObjects = tiledMap.getObjectGroup(objectGrounpName).objects;
            foreach (var collision in collisionObjects)
            {
                switch (collision.tiledObjectType)
                {
                    case TiledObject.TiledObjectType.Polygon:
                        {
                            Vertices verts = new Vertices(collision.polyPoints);
                            createEntity(collision.name)
                                .addComponent<FSRigidBody>()
                                .setBodyType(BodyType.Static)
                                .addComponent(new FSCollisionPolygon(verts));
                        }
                        break;
                    case TiledObject.TiledObjectType.None:
                        {
                            createEntity(collision.name).setPosition(new Vector2(collision.x, collision.y) + new Vector2(collision.width / 2, collision.height / 2))
                                .addComponent<FSRigidBody>()
                                .setBodyType(BodyType.Static)
                                .addComponent<FSCollisionBox>()
                                .setSize(collision.width, collision.height);

                        }
                        break;
                    case TiledObject.TiledObjectType.Ellipse:
                        {
                            createEntity(collision.name).setPosition(new Vector2(collision.x, collision.y) - new Vector2(collision.width / 2, collision.height / 2))
                                .addComponent<FSRigidBody>()
                                .setBodyType(BodyType.Static)
                                .addComponent<FSCollisionEllipse>()
                                .setRadii(collision.width / 2, collision.height / 2);
                        }
                        break;
                    case TiledObject.TiledObjectType.Polyline:
                        {
                            Vertices verts = new Vertices(collision.polyPoints);
                            var entity = createEntity(collision.name);
                            if (verts.Count == 2)
                            {
                                entity.addComponent<FSRigidBody>()
                                    .setBodyType(BodyType.Static)
                                    .addComponent<FSCollisionEdge>()
                                    .setVertices(verts[0], verts[1]);
                            }
                            else if (verts.Count > 2)
                            {
                                entity.addComponent<FSRigidBody>()
                                    .setBodyType(BodyType.Static)
                                    .addComponent<FSCollisionChain>()
                                    .setVertices(verts);
                            }
                        }
                        break;
                    case TiledObject.TiledObjectType.Image:
                        {

                        }
                        break;
                    default:
                        throw new Exception("tiled Object Type error");

                }

            }
        }


    }
}

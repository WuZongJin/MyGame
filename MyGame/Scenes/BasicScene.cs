using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.MyUIs;
using Nez;
using Nez.Farseer;
using Nez.Tiled;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Scenes
{
    public class BasicScene:Scene, IFinalRenderDelegate
    {
        #region Properties
        ScreenSpaceRenderer screenSpaceRenderer;
        FSWorld fSWorld;
        FSDebugView fSDebugView;
        public Scene scene { get; set; }
        #endregion

        #region Constructor
        public BasicScene()
        {
            screenSpaceRenderer = new ScreenSpaceRenderer(-10, GameLayerSetting.playerUiLayer, GameLayerSetting.uiLayer);
            screenSpaceRenderer.shouldDebugRender = false;
            finalRenderDelegate = this;
        }
        #endregion

        #region override Method
        public override void initialize()
        {
            base.initialize();
            setDesignResolution(640, 360, Scene.SceneResolutionPolicy.BestFit);
            Screen.setSize(640*2, 360*2 );
            clearColor = Color.Black;

            var canvas = createEntity("ui").addComponent(new UICanvas());
            canvas.isFullScreen = true;
            canvas.setRenderLayer(GameLayerSetting.uiLayer);

            fSWorld = new FSWorld(new Vector2(0.0f, 0.0f));
            addSceneComponent(fSWorld);
            fSDebugView = createEntity("debugView").addComponent<FSDebugView>();
            fSDebugView.setRenderLayer(GameLayerSetting.debugDrawViewLayer);
            fSDebugView.debugRenderEnabled = false;
            fSDebugView.enabled = false;
        }
        #endregion

       

        #region  IFinalRender
        public void onSceneBackBufferSizeChanged(int newWidth, int newHeight)
        {
            screenSpaceRenderer.onSceneBackBufferSizeChanged(newWidth, newHeight);
        }

        public void onAddedToScene(Scene scene)
        {
            this.scene = scene;
        }

        public void handleFinalRender(RenderTarget2D finalRenderTarget, Color letterboxColor, RenderTarget2D source, Rectangle finalRenderDestinationRect, SamplerState samplerState)
        {
            Core.graphicsDevice.SetRenderTarget(null);
            Core.graphicsDevice.Clear(letterboxColor);
            Graphics.instance.batcher.begin(BlendState.Opaque, samplerState, DepthStencilState.None, RasterizerState.CullNone, null);
            Graphics.instance.batcher.draw(source, finalRenderDestinationRect, Color.White);
            Graphics.instance.batcher.end();

            screenSpaceRenderer.render(scene);
        }

        #endregion

        #region Tiled Method
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
                                .addComponent(new FSCollisionPolygon(verts))
                                .setCollisionCategories(CollisionSetting.wallCategory);
                        }
                        break;
                    case TiledObject.TiledObjectType.None:
                        {
                            createEntity(collision.name).setPosition(new Vector2(collision.x, collision.y) + new Vector2(collision.width / 2, collision.height / 2))
                                .addComponent<FSRigidBody>()
                                .setBodyType(BodyType.Static)
                                .addComponent<FSCollisionBox>()
                                .setSize(collision.width, collision.height)
                                .setCollisionCategories(CollisionSetting.wallCategory);

                        }
                        break;
                    case TiledObject.TiledObjectType.Ellipse:
                        {
                            createEntity(collision.name).setPosition(new Vector2(collision.x, collision.y) - new Vector2(collision.width / 2, collision.height / 2))
                                .addComponent<FSRigidBody>()
                                .setBodyType(BodyType.Static)
                                .addComponent<FSCollisionEllipse>()
                                .setRadii(collision.width / 2, collision.height / 2)
                                .setCollisionCategories(CollisionSetting.wallCategory); ;
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
                                    .setVertices(verts[0], verts[1])
                                    .setCollisionCategories(CollisionSetting.wallCategory); ;
                            }
                            else if (verts.Count > 2)
                            {
                                entity.addComponent<FSRigidBody>()
                                    .setBodyType(BodyType.Static)
                                    .addComponent<FSCollisionChain>()
                                    .setVertices(verts)
                                    .setCollisionCategories(CollisionSetting.wallCategory); ;
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

        #endregion
    }
}

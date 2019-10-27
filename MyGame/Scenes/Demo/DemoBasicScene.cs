using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Farseer;
using Nez.Tweens;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nez.Farseer.FSDebugView;

namespace MyGame.Scenes.Demo
{
    public class DemoBasicScene:Scene, IFinalRenderDelegate
    {
        public const int tiledMapLayer = 10;
        public const int actorMoveLayer = 0;
        public const int uiRenderLayer = -10;
        public const int debugRenderLayer = -15;
        public const int lightRenderLayer = -5;

        #region Properties
        public UICanvas canvas;
        Table table;
        List<Button> sceneButtons = new List<Button>();
        ScreenSpaceRenderer screenSpaceRenderer;

        FSWorld fSWorld;
        FSDebugView fSDebugView;

        public Scene scene { get; set ; }
        #endregion

        #region Constructor
        public DemoBasicScene()
        {
            screenSpaceRenderer = new ScreenSpaceRenderer(-10, uiRenderLayer);
            //addRenderer(screenSpaceRenderer);
            screenSpaceRenderer.shouldDebugRender = false;
            finalRenderDelegate = this;


        }
        #endregion

        #region Override Method
        public override void initialize()
        {
            base.initialize();
            setDesignResolution(640, 360, Scene.SceneResolutionPolicy.BestFit,640,360);
            Screen.setSize(640*2, 360*2);

            
            createSceneUI();
            
        }
        #endregion

        #region UI Method
        public void createSceneUI()
        {
            canvas = createEntity("ui").addComponent(new UICanvas());
            canvas.isFullScreen = true;
            canvas.setRenderLayer(uiRenderLayer);



            fSWorld = new FSWorld(new Vector2(0.0f, 0.0f));
            //fSDebugView = new FSDebugView(fSWorld.world);
            //fsDebugView.debugRenderEnabled = false;
            addSceneComponent(fSWorld);
            fSDebugView = createEntity("debugView").addComponent<FSDebugView>();
            fSDebugView.appendFlags(DebugViewFlags.ContactPoints);
            fSDebugView.setRenderLayer(debugRenderLayer);
            fSDebugView.debugRenderEnabled = false;
            fSDebugView.enabled = true;

            setupSelector();
        }

        void setupSelector()
        {
            table = canvas.stage.addElement(new Table());
            table.setFillParent(true).right().top();

            var topButtonStyle = new TextButtonStyle(new PrimitiveDrawable(Color.Black, 10f), new PrimitiveDrawable(Color.Yellow), new PrimitiveDrawable(Color.DarkSlateBlue))
            {
                downFontColor = Color.Black
            };
            table.add(new TextButton("Toggle Scene List", topButtonStyle)).setFillX().setMinHeight(30).getElement<Button>().onClicked += onToggleSceneListClicked;

            table.row().setPadTop(10);
            var checkbox = table.add(new CheckBox("Debug Render", new CheckBoxStyle
            {
                checkboxOn = new PrimitiveDrawable(30, Color.Green),
                checkboxOff = new PrimitiveDrawable(30, Color.Red)
            })).getElement<CheckBox>();
            checkbox.onChanged += enable=> fSDebugView.enabled = enable;
            checkbox.isChecked = fSDebugView.enabled;
            table.row().setPadTop(30);

            var buttonStyle = new TextButtonStyle(new PrimitiveDrawable(new Color(78, 91, 98), 10f), new PrimitiveDrawable(new Color(244, 23, 135)), new PrimitiveDrawable(new Color(168, 207, 115)))
            {
                downFontColor = Color.Black
            };

            // find every Scene with the SampleSceneAttribute and create a button for each one
            foreach (var type in getTypesWithSampleSceneAttribute())
            {
                foreach (var attr in type.GetCustomAttributes(true))
                {
                    if (attr.GetType() == typeof(SampleSceneAttribute))
                    {
                        var sampleAttr = attr as SampleSceneAttribute;
                        var button = table.add(new TextButton(sampleAttr.buttonName, buttonStyle)).setFillX().setMinHeight(30).getElement<TextButton>();
                        sceneButtons.Add(button);
                        button.onClicked += butt =>
                        {
                            // stop all tweens in case any demo scene started some up
                            TweenManager.stopAllTweens();
                            Core.startSceneTransition(new WindTransition(() => Activator.CreateInstance(type) as Scene));
                        };

                        table.row().setPadTop(10);

                        // optionally add instruction text for the current scene
                        if (sampleAttr.instructionText != null && type == GetType())
                            addInstructionText(sampleAttr.instructionText);
                    }
                }
            }

        }

        void onToggleSceneListClicked(Button butt)
        {
            foreach (var button in sceneButtons)
                button.setIsVisible(!button.isVisible());
        }

        IEnumerable<Type> getTypesWithSampleSceneAttribute()
        {
            var assembly = typeof(DemoBasicScene).Assembly;
            var scenes = assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(SampleSceneAttribute), true).Length > 0)
                    .OrderBy(t => ((SampleSceneAttribute)t.GetCustomAttributes(typeof(SampleSceneAttribute), true)[0]).order);

            foreach (var s in scenes)
                yield return s;
        }

        void addInstructionText(string text)
        {
            var instructionsEntity = createEntity("instructions");
            instructionsEntity.addComponent(new Text(Graphics.instance.bitmapFont, text, new Vector2(10, 10), Color.White))
                .setRenderLayer(uiRenderLayer);
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


        [AttributeUsage(AttributeTargets.Class)]
        public class SampleSceneAttribute : Attribute
        {
            public string buttonName;
            public int order;
            public string instructionText;


            public SampleSceneAttribute(string buttonName, int order, string instructionText = null)
            {
                this.buttonName = buttonName;
                this.order = order;
                this.instructionText = instructionText;
            }
        }


    }
}

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Common.Game;
using MyGame.GameEntities.Player;
using MyGame.GlobalManages;
using Nez;
using Nez.Sprites;
using Nez.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nez.VirtualButton;

namespace MyGame.Scenes.Demo.TileMapWitgLayerDepth
{
    [SampleScene("Coroutine", 30, "")]
    public class CoroutineMapScene : DemoBasicScene
    {
        #region 
        VirtualButton startCoroutineButton;
        bool isStopCoroutine = true;
        CoroutineManager coroutine;
        Entity entity;
        #endregion

        public CoroutineMapScene() : base()
        {

        }

        public override void initialize()
        {
            base.initialize();
            addRenderer(new RenderLayerRenderer(0, actorMoveLayer, tiledMapLayer));
            addRenderer(new RenderLayerRenderer(1, debugRenderLayer));
            //addSceneComponent<SceneEventComponent>();

            coroutine = Core.getGlobalManager<CoroutineManager>();

            var texture = content.Load<Texture2D>("Images/Enemys/enemy");
            entity = createEntity("mover");
            entity.addComponent(new Sprite(texture));

            var player = addEntity(new Player()).setPosition(200, 200);

            startCoroutineButton = new VirtualButton();
            startCoroutineButton.nodes.Add(new KeyboardKey(Keys.X));
        }

        public override void update()
        {
            base.update();
            if (startCoroutineButton.isPressed)
            {
                coroutine.startCoroutine(createCount());
            }
           
        }


        IEnumerator<object> createCount()
        {
            while (true)
            {
                entity.position += new Microsoft.Xna.Framework.Vector2(5, 5);
                if (entity.position.Length() > 200)
                    yield break;
                yield return Coroutine.waitForSeconds(3);
            }
            
        }


    }
}

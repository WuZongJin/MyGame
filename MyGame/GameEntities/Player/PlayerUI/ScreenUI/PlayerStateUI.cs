using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GlobalManages.GameManager;
using Nez;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Player.PlayerUI.ScreenUI
{
    public class PlayerStateUI : RenderableComponent,IUpdatable
    {
        #region Properties
        ActorPropertyComponent propertyComponent;
        Texture2D headIcon;
        Texture2D redColor;
        Texture2D blueColor;
        Matrix presentMatrix;
        public override float width => 200;
        public override float height => 200;
        #endregion


        #region Constructor
        public PlayerStateUI()
        {
            
        }
        #endregion

        #region Override
        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
            headIcon = Core.content.Load<Texture2D>("Images/headIcons/Link_Sprite");
            redColor = Core.content.Load<Texture2D>("Images/Color/red");
            propertyComponent = Core.getGlobalManager<GameActorManager>().player.getComponent<ActorPropertyComponent>();
        }

        #endregion





        public override void render(Graphics graphics, Camera camera)
        {
            presentMatrix = graphics.batcher.transformMatrix;
            graphics.batcher.end();
            graphics.batcher.begin();

            graphics.batcher.draw(headIcon,new Rectangle(10,10,50,50),null,Color.White);

            for(int i = 0; i < propertyComponent.HP; i+=2)
            {
                graphics.batcher.draw(redColor, new Rectangle(80 + i, 20, 1, 7), null, Color.White);
            }
            graphics.batcher.drawString(graphics.bitmapFont, $"{propertyComponent.HP} /{ propertyComponent.MaxHP}", new Vector2(80, 0), Color.White);
            for (int i = 0; i < propertyComponent.MP; i += 2)
            {
                graphics.batcher.drawRect(new Rectangle(80 + i, 45, 1, 7), Color.Blue);
            }
            graphics.batcher.drawString(graphics.bitmapFont, $"{propertyComponent.MP} /{ propertyComponent.MaxMP}", new Vector2(80, 30), Color.White);


            graphics.batcher.end();
            graphics.batcher.begin(presentMatrix);

        }

        public void update()
        {
            
        }
    }
}

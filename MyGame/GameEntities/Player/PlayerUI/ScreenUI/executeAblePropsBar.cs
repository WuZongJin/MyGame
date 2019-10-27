using Microsoft.Xna.Framework;
using MyGame.GlobalManages.GameManager;
using Nez;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Player.PlayerUI
{
    public class executeAblePropsBar:RenderableComponent
    {
        #region Properties
        Player player;
        Matrix presentMatrix;
        public override float height => 50;
        public override float width => 640;
        public static int size = 60;
        static int positionY = 650;
        static int positionX = 470;
        static int space = 70;

        string Atext = "A";
        string Stext = "S";
        string Dtext = "D";
        string Ftext = "F";
        string Gtext = "G";

        Vector2 textSpace;
        int textWidth = 8;
        int textHeight = 12;
        int padding = 10;
        #endregion

        public executeAblePropsBar()
        {
            
        }

        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
            this.player = Core.getGlobalManager<GameActorManager>().player;
        }

        public override void render(Graphics graphics, Camera camera)
        {
            presentMatrix = graphics.batcher.transformMatrix;
            graphics.batcher.end();
            graphics.batcher.begin();

            #region Abutton
            graphics.batcher.drawRect(new Rectangle(positionX - padding, positionY - padding, 340 + padding*2, size + padding*2), new Color(10, 10, 10, 128));

            if (player.AProps != null)
            {
                graphics.batcher.draw(player.AProps.itemIcon, new Rectangle(positionX, positionY, size, size), player.AProps.itemIcon.sourceRect, Color.White);
            }
            graphics.batcher.drawHollowRect(positionX, positionY, size,size, Color.AliceBlue);
            graphics.batcher.drawString(Graphics.instance.bitmapFont, Atext, new Vector2(positionX + size - textWidth, positionY + size - textHeight), Color.White);
            #endregion
            #region SButton
            if (player.SProps != null)
            {
                graphics.batcher.draw(player.SProps.itemIcon, new Rectangle(positionX + space, positionY, size, size), player.SProps.itemIcon.sourceRect, Color.White);
            }
            graphics.batcher.drawHollowRect(positionX + space, positionY, size, size, Color.AliceBlue);
            graphics.batcher.drawString(Graphics.instance.bitmapFont, Stext, new Vector2(positionX + space + size - textWidth, positionY + size - textHeight), Color.White);
            #endregion

            #region DButton
            if (player.DProps != null)
            {
                graphics.batcher.draw(player.DProps.itemIcon, new Rectangle(225 + space*2, positionY, size, size), player.DProps.itemIcon.sourceRect, Color.White);
            }
            graphics.batcher.drawHollowRect(positionX + space*2, positionY, size, size, Color.AliceBlue);
            graphics.batcher.drawString(Graphics.instance.bitmapFont, Dtext, new Vector2(positionX + space * 2 + size - textWidth, positionY + size - textHeight), Color.White);
            #endregion

            #region FButton
            if (player.FProps != null)
            {
                graphics.batcher.draw(player.FProps.itemIcon, new Rectangle(positionX + space*3, positionY, size, size), player.FProps.itemIcon.sourceRect, Color.White);
            }
            graphics.batcher.drawHollowRect(positionX + space*3, positionY, size, size, Color.AliceBlue);
            graphics.batcher.drawString(Graphics.instance.bitmapFont, Ftext, new Vector2(positionX + space * 3 + size - textWidth, positionY + size - textHeight), Color.White);
            #endregion

            #region GButton
            if (player.GProps != null)
            {
                graphics.batcher.draw(player.GProps.itemIcon, new Rectangle(225 + space*4, positionY, size, size), player.GProps.itemIcon.sourceRect, Color.White);
            }
            graphics.batcher.drawHollowRect(positionX + space*4, positionY, size, size, Color.AliceBlue);
            graphics.batcher.drawString(Graphics.instance.bitmapFont, Gtext, new Vector2(positionX + space * 4 + size - textWidth, positionY + size - textHeight), Color.White);
            #endregion





            graphics.batcher.end();
            graphics.batcher.begin(presentMatrix);
        }
    }
}

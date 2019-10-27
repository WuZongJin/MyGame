using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.GlobalManages.GameManager;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Player.PlayerUI.ScreenUI
{
    public class PlayerWeaponUI : RenderableComponent
    {
        #region Properties
        Player player;
        Matrix presentMatrix;
        Texture2D WeaponUI;



        public override float height => 50;
        public override float width => 640;
        #endregion

        public PlayerWeaponUI()
        {

        }

        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
            this.player = Core.getGlobalManager<GameActorManager>().player;
            WeaponUI = Core.content.Load<Texture2D>("Images/ItemsObjects/WeaponAttackUI");
        }


        public override void render(Graphics graphics, Camera camera)
        {
            presentMatrix = graphics.batcher.transformMatrix;
            graphics.batcher.end();
            graphics.batcher.begin();

            graphics.batcher.draw(WeaponUI, new Rectangle(1120, 580, 128, 128), null, Color.White);
            graphics.batcher.drawString(graphics.bitmapFont, "X", new Vector2(1182, 683), new Color(255, 255, 255, 0),0f,Vector2.Zero,2f,SpriteEffects.None,1);

            if(player.weapon!= null)
            {
                graphics.batcher.draw(player.weapon.itemIcon, new Rectangle(1150, 605, 64, 64), player.weapon.itemIcon.sourceRect, Color.White);
            }



            graphics.batcher.end();
            graphics.batcher.begin(presentMatrix);
        }
    }
}

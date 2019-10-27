using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public class PlayerExcuteAbleTable:Table
    {
        #region Properties

        #endregion

        #region Constructor
        public PlayerExcuteAbleTable()
        {
            var frame = Core.content.Load<Texture2D>("UI/frame_grey");

            var player = Core.getGlobalManager<GameActorManager>().player;

            var AButton = new ImageButton(new PrimitiveDrawable(Color.LightGray));
            AButton.setBackground(new SubtextureDrawable(new Nez.Textures.Subtexture(frame)));
            var SButton = new ImageButton(new PrimitiveDrawable(Color.LightGray));
            SButton.setBackground(new SubtextureDrawable(new Nez.Textures.Subtexture(frame)));
            var DButton = new ImageButton(new PrimitiveDrawable(Color.LightGray));
            DButton.setBackground(new SubtextureDrawable(new Nez.Textures.Subtexture(frame)));
            var FButton = new ImageButton(new PrimitiveDrawable(Color.LightGray));
            FButton.setBackground(new SubtextureDrawable(new Nez.Textures.Subtexture(frame)));
            var GButton = new ImageButton(new PrimitiveDrawable(Color.LightGray));
            GButton.setBackground(new SubtextureDrawable(new Nez.Textures.Subtexture(frame)));


            this.add(AButton).pad(10).size(50);
            this.add(SButton).pad(10).size(50);
            this.add(DButton).pad(10).size(50);
            this.add(FButton).pad(10).size(50);
            this.add(GButton).pad(10).size(50);

        }
        #endregion

    }
}

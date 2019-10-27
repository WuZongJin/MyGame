using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameConponents.MyRenderableComponent
{
    public class SpriteWithSize:Sprite
    {
        #region Properties
        public Rectangle destinationRectangle;
        #endregion

        public SpriteWithSize(Texture2D texture) : base(texture) { }

        public override void render(Graphics graphics, Camera camera)
        {
            graphics.batcher.draw(_subtexture, destinationRectangle, null,color, entity.transform.rotation,entity.scale, spriteEffects, _layerDepth);

        }
    }
}

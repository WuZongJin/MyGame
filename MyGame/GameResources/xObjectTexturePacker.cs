using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameResources
{
    public class xObjectTexturePacker
    {
        public TexturePackerAtlas Packer;

        public xObjectTexturePacker()
        {
            var texture = Core.content.Load<Texture2D>("Images/ItemsObjects/springObjects");
            Packer = TexturePackerAtlas.create(texture,16,16);
        }
    }
}

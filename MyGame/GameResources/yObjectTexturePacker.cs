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
    public class yObjectTexturePacker
    {
        public TexturePackerAtlas Packer;

        public yObjectTexturePacker()
        {
            var texture = Core.content.Load<Texture2D>("TileMaps/MapTextures/objects");
            Packer = TexturePackerAtlas.create(texture, 16, 16);
        }
    }
}

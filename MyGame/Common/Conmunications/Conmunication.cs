using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Common
{
    public class Conmunication
    {
        public Subtexture texture;
        public string content;

        public Conmunication(Subtexture texture,string content)
        {
            this.texture = texture;
            this.content = content;
        }

        public Conmunication(string textResource,string content)
        {
            var tx = Core.scene.content.Load<Texture2D>(textResource);
            this.texture = new Subtexture(tx);
            this.content = content;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.GameResources;
using Nez;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items
{
    public class Arrow:ItemMaterials
    {
       public Arrow()
        {
            id = GameItemId.arrow;
            var texture = Core.content.Load<Texture2D>("Images/Players/Allplayer");
            itemIcon = new Nez.Textures.Subtexture(texture, new Rectangle(306, 2630 - 1191 - 16, 5, 16));
            name = "弓箭";
            describetion = "使用弓时所需要的东西";
            properties = new string[] { "无" };
            saleMoney = 1;
        }

    }
}

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
    public class MonsterOil:ItemMaterials
    {
        public MonsterOil()
        {
            id = GameItemId.montserOil;
            itemIcon = new Subtexture(Core.content.Load<Texture2D>("Images/ItemsIcon/I_Coal"));
            name = "怪物油";
            describetion = "怪物身上的油，可以用来制作物品";
            properties = new string[] { "无" };
            saleMoney = 1;
        }
    }
}

using Microsoft.Xna.Framework.Graphics;
using MyGame.GameResources;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items.Others
{
    public class DongZhuangMazeKey : ItemComponent
    {
        public DongZhuangMazeKey()
        {
            types = ItemTypes.Other;
            id = GameItemId.dongzhuangmazekey;
            itemIcon = new Nez.Textures.Subtexture(Core.content.Load<Texture2D>("Images/ItemsIcon/I_Key01"));
            name = "钥匙";
            describetion = "东庄镇北面迷宫大门的钥匙";
            properties = new string[] { "无" };
            saleMoney = 1;
        }

    }
}

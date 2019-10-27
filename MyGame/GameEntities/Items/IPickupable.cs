using Microsoft.Xna.Framework.Graphics;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items
{
    public interface IPickupable
    {
        Subtexture itemIcon { get; set; }    //图标
        string describetion { get; set; }   //描述

        
    }
}

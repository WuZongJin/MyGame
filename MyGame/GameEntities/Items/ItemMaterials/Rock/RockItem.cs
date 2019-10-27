using MyGame.GameResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items
{
    public class RockItem:ItemMaterials
    {
        public RockItem()
        {
            id = GameItemId.rock;
            itemIcon = GameResources.GameTextureResource.xObjectPacker.Packer.getSubtexture(390);
            name = "石头";
            describetion = "石头，可在路边的石头里开采出来";
            properties = new string[] { "无" };
            saleMoney = 1;
        }
    }
}

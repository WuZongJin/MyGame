using MyGame.GameResources;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Items
{
    public class PlantFibre:ItemMaterials
    {
        public PlantFibre()
        {
            id = GameItemId.plantfibre;
            itemIcon = GameTextureResource.xObjectPacker.Packer.getSubtexture(153);
            name = "纤维";
            describetion = "纤维，通常在路边的野草上可以获得，可用于制作物品";
            properties = new string[] { "无" };
            saleMoney = 1;
        }
    }
}

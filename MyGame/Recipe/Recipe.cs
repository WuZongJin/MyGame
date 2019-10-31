using MyGame.GameEntities.Items;
using MyGame.GlobalManages.GameManager;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Recipe
{
    public class RawMaterial
    {
        public ItemComponent material;
        public int count;

    }

    public enum RecipeTypes
    {
        Alchemy,
        Medicine
    }

    public class Recipe
    {
        public RecipeTypes types;
        public string name;
        public List<RawMaterial> rawMaterials;
        public ItemComponent produce;
        public int porduceCount;

        public bool avaiableToCreate()
        {
            var player = Core.getGlobalManager<GameActorManager>().player;
            foreach(var material in rawMaterials)
            {
                if (player.items.Keys.Where(m => m.id == material.material.id).Count() > 0)
                {
                    var count = player.items[player.items.Keys.Where(m => m.id == material.material.id).First()];
                    if (count < material.count)
                        return false;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

    }


}

using MyGame.GameEntities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Recipe
{
    public class ArrowRecipe : Recipe
    {
        public ArrowRecipe()
        {
            name = "弓箭";
            types = RecipeTypes.Alchemy;
            rawMaterials = new List<RawMaterial>();
            MonsterOil monsterOil = new MonsterOil();
            PlantFibre plantFibre = new PlantFibre();
            RawMaterial rawMaterial1 = new RawMaterial();
            rawMaterial1.material = monsterOil;
            rawMaterial1.count = 2;
            RawMaterial rawMaterial2 = new RawMaterial();
            rawMaterial2.material = plantFibre;
            rawMaterial2.count = 2;
            rawMaterials.Add(rawMaterial1);
            rawMaterials.Add(rawMaterial2);
            produce = new Arrow();
            porduceCount = 5;
        }

    }
}

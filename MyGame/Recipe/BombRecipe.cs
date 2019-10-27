using MyGame.GameEntities.Items;
using MyGame.GameEntities.Items.Bombs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Recipe
{
    public class BombRecipe:Recipe
    {
        public BombRecipe()
        {
            name = "炸弹";
            types = RecipeTypes.Alchemy;
            rawMaterials = new List<RawMaterial>();
            MonsterOil monsterOil = new MonsterOil();
            PlantFibre plantFibre = new PlantFibre();
            RawMaterial rawMaterial1 = new RawMaterial();
            rawMaterial1.material = monsterOil;
            rawMaterial1.count = 5;
            RawMaterial rawMaterial2 = new RawMaterial();
            rawMaterial2.material = plantFibre;
            rawMaterial2.count = 3; 
            rawMaterials.Add(rawMaterial1);
            rawMaterials.Add(rawMaterial2);
            produce = new BombComponent();
        }

       
    }
}

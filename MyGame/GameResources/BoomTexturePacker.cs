using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameResources
{
    public class BoomTexturePacker
    {
        public TexturePackerAtlas Packer;

        public BoomTexturePacker()
        {
            var texture = Core.content.Load<Texture2D>("Images/Players/Allplayer");
            Packer = new TexturePackerAtlas(texture);
            initBoombTexture();
        }

        private void initBoombTexture()
        {
            Packer.createRegion("bomb", 20, 1921, 14, 14);

            Packer.createRegion("boomb01", 73, 1920, 15, 15);
            Packer.createRegion("boomb02", 94, 1906, 47, 43);
            Packer.createRegion("boomb03", 147, 1915, 28, 28);
            Packer.createRegion("boomb04", 183, 1915, 28, 28);
            Packer.createRegion("boomb05", 197, 1921, 18, 18);
            Packer.createRegion("boomb06", 239, 1920, 19, 19);
            Packer.createRegion("boomb07", 263, 1920, 18, 18);

            Packer.createRegion("boomb08", 289, 1918, 20, 22);
            Packer.createRegion("boomb09", 316, 1914, 23, 25);
            Packer.createRegion("boomb010", 347, 1909, 28, 32);

            Packer.createRegion("boomb011", 381, 1901, 30, 33);
            Packer.createRegion("boomb012", 418, 1905, 32, 36);
            Packer.createRegion("boomb013", 460, 1904, 30, 36);

            Packer.createRegion("boomb014", 499, 1925, 29, 36);
            Packer.createRegion("boomb015", 538, 1904, 29, 37);
            Packer.createRegion("boomb016", 575, 1905, 26, 36);

            Packer.createRegion("boomb017", 605, 1905, 26, 36);
            Packer.createRegion("boomb018", 634, 1905, 22, 36);
            Packer.createRegion("boomb019", 655, 1905, 20, 36);
            Packer.createRegion("boomb020", 678, 1905, 15, 27);

            Packer.spriteAnimationDetails = new Dictionary<string, List<int>>();
            Packer.spriteAnimationDetails.Add("waitboom", new List<int>() { 0 });
            Packer.spriteAnimationDetails.Add("bombboom", new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 });
            
        }



        
    }
}

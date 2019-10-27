using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameResources
{
    public class GameTextureResource
    {
        public static BoomTexturePacker boomTexture;
        public static xObjectTexturePacker xObjectPacker;
        public static xAnimationsTexturePacker xAnimationsTexturePacker;
                
        public static void initialize()
        {
            boomTexture = new BoomTexturePacker();
            xObjectPacker = new xObjectTexturePacker();
            xAnimationsTexturePacker = new xAnimationsTexturePacker();
        }
    }
}

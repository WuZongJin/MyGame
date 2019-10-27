using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Common
{
    public static class LayerDepthExt
    {
        public static float caluelateLayerDepth(float y)
        {
            return (1000f - y) / 1000f;
        }
    }
}

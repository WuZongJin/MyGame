using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;

namespace MyGame.GameEntities.Items
{
    public class ExecuteAbleProps:ItemComponent,IExecuteAbleItem
    {
        public ExecuteAbleProps()
        {
            types = ItemTypes.ExecuteableProp;
        }

        public virtual void excute(Entity position)
        {
            
        }
    }
}

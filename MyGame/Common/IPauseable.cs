using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Common
{
    public enum PauseTypes
    {
         CoulePause,
         NerverPause
    }
    public interface IPauseable
    {
         bool couldPause { get; set; }
    }
}

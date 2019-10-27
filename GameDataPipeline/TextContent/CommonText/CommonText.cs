using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataPipeline
{
    public class CommonText : ICommonTextContent
    {
        public string MenuGameStart { get; set ; }
        public string MenuGameSetting { get; set ; }
        public string MenuGameContinue { get; set; }
        public string MenuGameExit { get; set; }
    }
}

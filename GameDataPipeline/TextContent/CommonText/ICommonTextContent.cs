using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataPipeline
{
    public interface ICommonTextContent
    {
        string MenuGameStart { get; set; }
        string MenuGameSetting { get; set; }
        string MenuGameContinue { get; set; }
        string MenuGameExit { get; set; }


    }
}

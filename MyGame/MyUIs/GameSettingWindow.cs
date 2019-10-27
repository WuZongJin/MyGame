using Microsoft.Xna.Framework;
using Nez;
using Nez.Farseer;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.MyUIs
{
    public class GameSettingWindow:Dialog
    {
        public GameSettingWindow() : base("设置", Skin.createDefaultSkin())
        {
            this.top().left();

            var checkbox = this.getContentTable().add(new CheckBox("Debug Render", new CheckBoxStyle
            {
                checkboxOn = new PrimitiveDrawable(30, Color.Green),
                checkboxOff = new PrimitiveDrawable(30, Color.Red)
            })).getElement<CheckBox>();

            checkbox.onChanged += enable => Core.scene.findEntity("debugView").getComponent<FSDebugView>().enabled = enable;
            checkbox.isChecked = Core.scene.findEntity("debugView").getComponent<FSDebugView>().enabled;


        }
    }
}

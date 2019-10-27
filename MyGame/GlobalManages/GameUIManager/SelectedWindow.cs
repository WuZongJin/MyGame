using MyGame.Common.Game;
using MyGame.GameEntities.Items;
using MyGame.GameEntities.Player;
using MyGame.GameEntities.Player.PlayerUI;
using Nez;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GlobalManages
{
    public class SelectedWindow:Window
    {
        #region Constructor
        public SelectedWindow():base("角色菜单",Skin.createDefaultSkin())
        {
            var player = Core.scene.findEntity("player") as Player;
            debugAll();
            this.setFillParent(true).center();
            getTitleLabel().setAlignment(Align.center);
            getTitleLabel().setFontScale(3f);
            getTitleTable().top().padTop(50);
            if((PlayerUILocker.playerUILocker & PlayerUIElementLocker.ItemWindow) != 0)
            {

                var itemWindowButton = new TextButton("物品", Skin.createDefaultSkin());
                add(itemWindowButton).width(200).height(200);
                itemWindowButton.onClicked += obj =>
                {
                    this.clear();
                    this.addElement(new ItemsWindow(player));
                };
            }
            if ((PlayerUILocker.playerUILocker & PlayerUIElementLocker.EquitWindow) != 0)
            {
                var equitWindowButton = new TextButton("装备", Skin.createDefaultSkin());
                add(equitWindowButton).width(200).height(200);
                equitWindowButton.onClicked += obj =>
                 {
                     this.clear();
                     this.addElement(new EquitMentWindow(player));
                 };
            }
            if((PlayerUILocker.playerUILocker & PlayerUIElementLocker.CreateItemWindow) != 0)
            {
                var createItemWindowButton = new TextButton("建造", Skin.createDefaultSkin());
                add(createItemWindowButton).width(200).height(200);
                createItemWindowButton.onClicked += obj =>
                {
                    this.clear();
                    this.addElement(new CreateItemWindow(player));
                };

            }
            if((PlayerUILocker.playerUILocker & PlayerUIElementLocker.ExecuteablePropsWindow) != 0)
            {
                var executeablePropsWindowButton = new TextButton("道具", Skin.createDefaultSkin());
                add(executeablePropsWindowButton).width(200).height(200);
                executeablePropsWindowButton.onClicked += obj =>
                 {
                     this.clear();
                     this.addElement(new ExecuteAblePropsWindow(player));
                 };
            }
        }

        
        #endregion

        #region Override
        #endregion
    }
}

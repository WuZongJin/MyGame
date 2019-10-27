using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameEntities.Items.Bombs;
using MyGame.GameEntities.Player;
using MyGame.GameEntities.Player.PlayerUI;
using MyGame.MyUIs;
using Nez;
using Nez.Systems;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GlobalManages
{
    public class GameUIManager : GlobalManager
    {
        public enum GameUIEvent
        {
            showDialog,
            conmunication,
        }

        #region 
        Table fullScreenTable;

        Window window;
        Window gameSettingWindow;
        public bool hasOpenWindow;
        public bool hasOpenActorWindow;
        public bool hasOpenGameSettingWindow;
        Emitter<GameUIEvent> emitter;


        Queue<Conmunication> conmunications;
        Table conmunicateTable;
        #endregion

        public GameUIManager()
        {
            emitter = new Emitter<GameUIEvent>();
            conmunications = new Queue<Conmunication>();
        }

        #region override 
        public override void update()
        {
            base.update();
            if (conmunicateTable!=null)
            {
                if(Input.isKeyPressed(Keys.Space))
                {
                    if (conmunications.Count > 1)
                    {
                        conmunications.Dequeue();
                        conmunicateTable.clear();
                        var iconFrame = Core.content.Load<Texture2D>("UI/frame_grey");
                        var windowFrame = Core.content.Load<Texture2D>("UI/textBox");
                        var back = new SubtextureDrawable(new Nez.Textures.Subtexture(windowFrame));

                        var table = conmunicateTable.add(new Table()).center().getElement<Table>();
                        var image = new ImageButton(new SubtextureDrawable(this.conmunications.Peek().texture));

                        image.setBackground(new SubtextureDrawable(iconFrame));
                        table.add(image).width(130).height(130).pad(50);
                        LabelStyle labelStyle = new LabelStyle(new Color(255, 255, 255, 0));
                        Label label = new Label(this.conmunications.Peek().content, labelStyle);
                        label.setWrap(true);
                        label.setFontScale(2f);

                        table.add(label).pad(50).width(500).left().top();
                        table.add(new Label("继续：空格", labelStyle)).bottom().right().setPadBottom(40).setPadRight(50);
                        table.setBackground(back);
                    }
                    else
                    {
                        conmunications.Clear();
                        conmunicateTable.remove();
                        conmunicateTable = null;
                        GameSetting.isGamePause = false;
                    }
                }
            }
            else
            {
                if (Input.isKeyPressed(Keys.Q))
                {
                    if (hasOpenWindow && hasOpenActorWindow)
                    {
                        this.window.clear();
                        window.remove();
                        window = null;
                        hasOpenWindow = false;
                        hasOpenActorWindow = false;
                        GameSetting.isGamePause = false;
                    }
                    else
                    {
                        GameSetting.isGamePause = true;
                        createWindow();
                    }
                }
            }

            if (Input.isKeyPressed(Keys.Escape))
            {
                if (hasOpenGameSettingWindow)
                {
                    gameSettingWindow.clear();
                    gameSettingWindow.remove();
                    gameSettingWindow = null;
                    hasOpenGameSettingWindow = false;
                }
                else
                {
                    createSettingUI();
                }
            }
        }

        public void createWindow()
        {
            hasOpenWindow = true;
            hasOpenActorWindow = true;
             var ui = Core.scene.findEntity("ui").getComponent<UICanvas>();
            window = ui.stage.addElement(new SelectedWindow());
            
        }

        public void createConfirmUI(string message)
        {
            GameSetting.isGamePause = true;
            
            var ui = Core.scene.findEntity("ui").getComponent<UICanvas>();
            Table table = ui.stage.addElement(new Table());
            table.setFillParent(true).center();
            var dialog = new Dialog("提示", Skin.createDefaultSkin());
            table.add(dialog).center();
            dialog.center();
            dialog.setSize(400, 500);
            LabelStyle labelStyle = new LabelStyle(new Color(255, 255, 255, 0));
            var label = new Label(message);
            label.setWrap(true);

            dialog.getContentTable().add(label).width(300).height(100);
            var button = new TextButton("确定", Skin.createDefaultSkin());
            dialog.getButtonTable().add(button).width(70).height(20).setPadBottom(10);
            button.onClicked += obj => {
                GameSetting.isGamePause = false;
                dialog.hide();
            };
        }
        
        public void createConmunication(IList<Conmunication> conmunications)
        {
            GameSetting.isGamePause = true;
            this.conmunications.Clear();
            foreach (var item in conmunications)
            {
                this.conmunications.Enqueue(item);
            }
            var windowFrame = Core.content.Load<Texture2D>("UI/textBox");
            var iconFrame = Core.content.Load<Texture2D>("UI/frame_grey");
            var canvas = Core.scene.findEntity("ui").getComponent<UICanvas>();
            var back = new SubtextureDrawable(new Nez.Textures.Subtexture(windowFrame));
            conmunicateTable = canvas.stage.addElement(new Table());
            conmunicateTable.setFillParent(true).bottom();

            var table = conmunicateTable.add(new Table()).center().getElement<Table>();
            var image = new ImageButton(new SubtextureDrawable(this.conmunications.Peek().texture));
            
            image.setBackground(new SubtextureDrawable(iconFrame));
            table.add(image).width(130).height(130).pad(50);
            LabelStyle labelStyle = new LabelStyle(new Color(255,255,255,0));
            Label label = new Label(this.conmunications.Peek().content, labelStyle);
            label.setWrap(true);
            label.setFontScale(2f);

            table.add(label).pad(50).width(500).left().top();
            table.add(new Label("继续：空格",labelStyle)).bottom().right().setPadBottom(40).setPadRight(50);
            table.setBackground(back);

        }

        #region Player UI
        public void createPlayerUI()
        {
           

        }

        public void createPlayerHPandMP()
        {
            var canvas = Core.scene.findEntity("ui").getComponent<UICanvas>();
            var table = canvas.stage.addElement(new Table()).setFillParent(true).left().top();

            var table1 = canvas.stage.addElement(new Table()).setFillParent(true).bottom();
            table1.add(new PlayerExcuteAbleTable());
        }

        #endregion


        #region deBugUI
        private void createSettingUI()
        {
            hasOpenGameSettingWindow = true;
            var canvas = Core.scene.findEntity("ui").getComponent<UICanvas>();
            gameSettingWindow = canvas.stage.addElement(new GameSettingWindow());
        }
        #endregion

        #endregion

    }
}

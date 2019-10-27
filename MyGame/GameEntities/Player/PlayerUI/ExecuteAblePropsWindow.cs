using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.GameEntities.Items;
using MyGame.MyUIs;
using Nez;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Player.PlayerUI
{
    public class ExecuteAblePropsWindow:Window
    {

        int rowcount = 6;
        Table allContain;

        Table propsTable;
        ScrollPane propsScrollPane;
        Table propsDeatilTable;

        Table playerPropTable;
        Table playerPropContentTable;

        Player player;
        Texture2D windowFrame;
        Texture2D buttonFrame;



        public ExecuteAblePropsWindow(Player player):base("道具界面",Skin.createDefaultSkin())
        {
            getTitleLabel().setFontScale(2f).setAlignment(Align.center);
            this.setFillParent(true).left().top().padTop(50).padRight(50);
            this.player = player;

            windowFrame = Core.content.Load<Texture2D>("UI/window_frame_grey");
            buttonFrame = Core.content.Load<Texture2D>("UI/frame_grey");

            allContain = new Table();
            this.addElement(allContain).setFillParent(true);

            initplayerPropTable();
            initPropsDetailTable();

        }

        private void refreshes()
        {
            allContain.clear();
            initplayerPropTable();
            initPropsDetailTable();
        }

        #region playerPropTable
        private void initplayerPropTable()
        {
            playerPropTable = new Table();
            var back = new SubtextureDrawable(new Nez.Textures.Subtexture(windowFrame));
            back.setPadding(0, 620, 0, 540);
            playerPropTable.setBackground(back);
            playerPropTable.setSize(540, 620);
            allContain.add(playerPropTable).top().left().pad(25);

            playerPropContentTable = new Table();
            playerPropContentTable.setFillParent(true);
            playerPropTable.addElement(playerPropContentTable);

            ItemIconButton ApropsButton, SpropsButton, DpropsButton, FpropsButton, GpropsButton;

            if (player.AProps == null)
            {
                ApropsButton = new ItemIconButton(new PrimitiveDrawable(Color.DarkGray));
            }
            else
            {
                ApropsButton = createItembutton(player.AProps);
            }

            if (player.SProps == null)
            {
                SpropsButton = new ItemIconButton(new PrimitiveDrawable(Color.DarkGray));
            }
            else
            {
                SpropsButton = createItembutton(player.SProps);
            }

            if (player.DProps == null)
            {
                DpropsButton = new ItemIconButton(new PrimitiveDrawable(Color.DarkGray));
            }
            else
            {
                DpropsButton = createItembutton(player.DProps);
            }

            if (player.FProps == null)
            {
                FpropsButton = new ItemIconButton(new PrimitiveDrawable(Color.DarkGray));
            }
            else
            {
                FpropsButton = createItembutton(player.FProps);
            }

            if (player.GProps == null)
            {
                GpropsButton = new ItemIconButton(new PrimitiveDrawable(Color.DarkGray));
            }
            else
            {
                GpropsButton = createItembutton(player.GProps);
            }


            playerPropContentTable.add(ApropsButton).size(64);
            playerPropContentTable.add(SpropsButton).size(64);
            playerPropContentTable.add(DpropsButton).size(64);
            playerPropContentTable.add(FpropsButton).size(64);
            playerPropContentTable.add(GpropsButton).size(64);

        }

        private ItemIconButton createItembutton(ItemComponent equitable)
        {
            ItemIconButton equitButton;
            if (equitable == null)
            {
                equitButton = new ItemIconButton(new PrimitiveDrawable(Color.DarkGray));
                equitButton.setBackground(new SubtextureDrawable(buttonFrame));
                equitButton.item = null;
            }
            else
            {
                equitButton = new ItemIconButton(new SubtextureDrawable(equitable.itemIcon));
                equitButton.setBackground(new SubtextureDrawable(buttonFrame));
                equitButton.item = equitable;
                equitButton.OnMouseExit += playerPropsButton_OnMouseExit; ;
                equitButton.OnMouseEntry += playerPropsButton_OnMouseEntry;
                equitButton.onClicked += playerPropsButton_onClicked;
            }
            return equitButton;
        }

        private void playerPropsButton_OnMouseExit(Button obj)
        {
            var btn = obj as ItemIconButton;
            playerPropTable.removeElement(btn.entryTable);
            btn.entryTable = null;
        }

        private void playerPropsButton_OnMouseEntry(Button obj)
        {
            var windowFrame = Core.content.Load<Texture2D>("UI/frame_opaque");
            var btn = obj as ItemIconButton;
            btn.entryTable = new Table();
            btn.entryTable.debugAll();
            btn.entryTable.setFillParent(false);

            var detailTable = createItemDeatil(btn);
            btn.entryTable.addElement(detailTable);
            btn.entryTable.setSize(detailTable.preferredWidth < 200 ? 200 : detailTable.preferredWidth, detailTable.preferredHeight < 200 ? 200 : detailTable.preferredHeight);

            var back = new SubtextureDrawable(new Nez.Textures.Subtexture(windowFrame));
            back.setPadding(0, btn.entryTable.preferredWidth, 0, btn.entryTable.preferredHeight);
            btn.entryTable.setBackground(back);

            var x = btn.getX() + btn.getWidth();
            var y = btn.getY();



            Rectangle rectangle = new Rectangle((int)(playerPropTable.getX() + allContain.getX() + x), (int)(playerPropTable.getY() + allContain.getY() + y), (int)btn.entryTable.getWidth(), (int)btn.entryTable.getHeight());

            if (rectangle.Bottom > Screen.height)
            {
                y = btn.getY() + btn.getHeight() - btn.entryTable.getHeight();
            }
            if (rectangle.Right > Screen.width)
            {
                x = btn.getX() - btn.entryTable.getWidth();
            }

            playerPropTable.addElement(btn.entryTable).setPosition(x, y);

        }

        private void playerPropsButton_onClicked(Button obj)
        {
            var btn = obj as ItemIconButton;
            
            if(player.AProps.id == btn.item.id)
            {
                player.AProps = null;
            }
            else if(player.SProps.id == btn.item.id)
            {
                player.SProps = null;
            }
            else if (player.DProps.id == btn.item.id)
            {
                player.DProps = null;
            }
            else if (player.FProps.id == btn.item.id)
            {
                player.FProps = null;
            }
            else if (player.GProps.id == btn.item.id)
            {
                player.GProps = null;
            }

            refreshes();
        }
        #endregion

        #region 

        private void initPropsDetailTable()
        {
            propsTable = new Table();
            var back = new SubtextureDrawable(new Nez.Textures.Subtexture(windowFrame));
            back.setPadding(0, 620, 0, 540);
            propsTable.setBackground(back);

            allContain.add(propsTable).right().top().pad(25);

            propsDeatilTable = new Table();
            propsDeatilTable.setFillParent(true).top().left();

            ButtonGroup buttonGroup = new ButtonGroup();
            int counts = 0;

            var ItemList = player.items.Keys.Where(m => m.types == ItemComponent.ItemTypes.ExecuteableProp).ToList();
            if(player.AProps != null)
            {
                var item = ItemList.Where(m => m.id == player.AProps.id).First();
                ItemList.Remove(item);
            }
            if (player.SProps != null)
            {
                var item = ItemList.Where(m => m.id == player.SProps.id).First();
                ItemList.Remove(item);
            }
            if (player.DProps != null)
            {
                var item = ItemList.Where(m => m.id == player.DProps.id).First();
                ItemList.Remove(item);
            }
            if (player.FProps != null)
            {
                var item = ItemList.Where(m => m.id == player.FProps.id).First();
                ItemList.Remove(item);
            }
            if (player.GProps != null)
            {
                var item = ItemList.Where(m => m.id == player.GProps.id).First();
                ItemList.Remove(item);
            }

            foreach(var item in ItemList)
            {
                var imageUp = new SubtextureDrawable(item.itemIcon);
                var imageDown = new SubtextureDrawable(item.itemIcon);
                imageDown.tintColor = Color.DarkGray;
                var imageChecked = new SubtextureDrawable(item.itemIcon);
                imageChecked.tintColor = Color.DarkKhaki;
                var button = new ItemIconButton(imageUp, imageDown, imageChecked);
                button.setBackground(new SubtextureDrawable(buttonFrame));
                button.item = item;
                button.OnMouseEntry += Button_OnMouseEntry;
                button.OnMouseExit += Button_OnMouseExit;
                button.onClicked += Button_onClicked;
                buttonGroup.add(button);
                counts++;

                propsDeatilTable.add(button).size(64);
                if (counts > rowcount)
                {
                    counts = 0;
                    propsDeatilTable.row();
                }
            }
            propsScrollPane = new ScrollPane(propsDeatilTable, Skin.createDefaultSkin());

            propsTable.addElement(propsScrollPane).setPosition(30, 30);
            propsScrollPane.setSize(500, 540);


        }

        private void Button_onClicked(Button obj)
        {
            var btn = obj as ItemIconButton;
            
            if(player.AProps == null)
            {
                player.AProps = (ExecuteAbleProps)player.items.Keys.Where(m => m.id == btn.item.id).First();
            }
            else if (player.SProps == null)
            {
                player.SProps = (ExecuteAbleProps)player.items.Keys.Where(m => m.id == btn.item.id).First();
            }
            else if (player.DProps == null)
            {
                player.DProps = (ExecuteAbleProps)player.items.Keys.Where(m => m.id == btn.item.id).First();
            }
            else if (player.FProps == null)
            {
                player.FProps = (ExecuteAbleProps)player.items.Keys.Where(m => m.id == btn.item.id).First();
            }
            else if (player.GProps == null)
            {
                player.GProps = (ExecuteAbleProps)player.items.Keys.Where(m => m.id == btn.item.id).First();
            }

            refreshes();
        }

        private void Button_OnMouseExit(Button obj)
        {
            var btn = obj as ItemIconButton;
            propsTable.removeElement(btn.entryTable);
            btn.entryTable = null;
        }

        private void Button_OnMouseEntry(Button button)
        {
            var windowFrame = Core.content.Load<Texture2D>("UI/frame_opaque");
            var btn = button as ItemIconButton;
            btn.entryTable = new Table();
            btn.entryTable.setFillParent(false);

            var detailTable = createItemDeatil(btn);
            btn.entryTable.addElement(detailTable);
            btn.entryTable.setSize(detailTable.preferredWidth < 200 ? 200 : detailTable.preferredWidth, detailTable.preferredHeight < 200 ? 200 : detailTable.preferredHeight);

            var back = new SubtextureDrawable(new Nez.Textures.Subtexture(windowFrame));
            back.setPadding(0, btn.entryTable.preferredWidth, 0, btn.entryTable.preferredHeight);
            btn.entryTable.setBackground(back);

            var x = propsScrollPane.getX() + btn.getX() + btn.getWidth();
            var y = propsScrollPane.getY() + btn.getY() - propsScrollPane.getScrollY();



            Rectangle rectangle = new Rectangle((int)(propsTable.getX() + allContain.getX() + x), (int)(propsTable.getY() + allContain.getY() + y), (int)btn.entryTable.getWidth(), (int)btn.entryTable.getHeight());

            if (rectangle.Bottom > Screen.height)
            {
                y = propsScrollPane.getY() + btn.getY() + btn.getHeight() - btn.entryTable.getHeight() - propsScrollPane.getScrollY();
            }
            if (rectangle.Right > Screen.width)
            {
                x = propsScrollPane.getX() + btn.getX() - btn.entryTable.getWidth();
            }

            propsTable.addElement(btn.entryTable).setPosition(x, y);


        }

        #endregion

        private Table createItemDeatil(ItemIconButton btn)
        {
            var detailTable = new Table();
            detailTable.setFillParent(true);
            detailTable.pad(20);
            var img = detailTable.add(new Image(new SubtextureDrawable(btn.item.itemIcon))).left();
            detailTable.add(new Label(btn.item.name));

            detailTable.row();
            var desc = new Label(btn.item.describetion);
            desc.setWrap(true);
            detailTable.add(desc).setMinHeight(25).left();

            detailTable.row();
            foreach (var property in btn.item.properties)
            {
                detailTable.add(new Label(property)).setMinHeight(25).left(); ;
                detailTable.row();
            }
            detailTable.add(new Label($"售价: {btn.item.saleMoney.ToString()}")).setMinHeight(25);

            return detailTable;
        }


    }
}

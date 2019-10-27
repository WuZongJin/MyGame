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
    public class ItemsWindow:Window
    {
        #region Properties
        Player player;
        int rowcount = 6;
        Table allContain;
        Table itemsTable;
        Table recipeTable;
       
        ScrollPane detailScrollPane;
        Table detailMessageTable;
        #endregion
        public ItemsWindow(Player player):base("物品界面",Skin.createDefaultSkin())
        {
            this.player = player;

            this.setFillParent(true).left().top().padTop(50);

            var buttonCloseEnable = Core.content.Load<Texture2D>("UI/button_close_enable");
            var buttonCloseDisable = Core.content.Load<Texture2D>("UI/button_close_disable");
            Button closeButton = new Button(new SubtextureDrawable(buttonCloseEnable), new SubtextureDrawable(buttonCloseDisable),new SubtextureDrawable(buttonCloseDisable));
            getTitleTable().add(closeButton).setAlign(Align.topRight);
            getTitleLabel().setFontScale(2f).setAlignment(Align.center);


            allContain = new Table();
            this.addElement(allContain).setFillParent(true);

            initializeItemTable();
            initializaDetailTable();
            

            
        }

        private void initializaDetailTable()
        {
            recipeTable = new Table();
            recipeTable.debugAll();
            var windowFrame = Core.content.Load<Texture2D>("UI/window_frame_grey");

            var back = new SubtextureDrawable(new Nez.Textures.Subtexture(windowFrame));
            back.setPadding(0, 620, 0, 540);
            recipeTable.setBackground(back);

            allContain.add(recipeTable).top().right().pad(25);


            detailScrollPane = new ScrollPane(null, Skin.createDefaultSkin());
            recipeTable.addElement(detailScrollPane).setPosition(30, 30).fillParent = true;

            detailMessageTable = new Table();
            detailScrollPane.setWidget(detailMessageTable);

        }

       

        private void initializeItemTable()
        {
            itemsTable = new Table();
            var windowFrame = Core.content.Load<Texture2D>("UI/window_frame_grey");
            var buttonFrame = Core.content.Load<Texture2D>("UI/frame_grey");

            var back = new SubtextureDrawable(new Nez.Textures.Subtexture(windowFrame));
            back.setPadding(0, 620, 0, 540);
            itemsTable.setBackground(back);

            allContain.add(itemsTable).top().left().pad(25);

            var table = new Table();
            table.setFillParent(true).top().left();
            var itemScrollPane = new ScrollPane(table, Skin.createDefaultSkin());

            itemsTable.addElement(itemScrollPane).setPosition(30, 30).fillParent = true;

            ButtonGroup buttonGroup = new ButtonGroup();

            int counts = 0;
            var items = player.items.Keys.Where(m => m.types == ItemComponent.ItemTypes.Material|| m.types == ItemComponent.ItemTypes.ExecuteableProp).ToList();


            foreach (var item in items)
            {
                var imageUp = new SubtextureDrawable(item.itemIcon);
                var imageDown = new SubtextureDrawable(item.itemIcon);
                imageDown.tintColor = Color.DarkGray;
                var imageChecked = new SubtextureDrawable(item.itemIcon);
                imageChecked.tintColor = Color.DarkKhaki;
                var button = new ItemIconButton(imageUp, imageDown, imageChecked);
                button.item = item;
                button.setBackground(new SubtextureDrawable(buttonFrame));
                buttonGroup.add(button);
                //button.onClicked += Button_onClicked;
                button.OnMouseEntry += Button_OnMouseEntry;
                button.OnMouseExit += Button_OnMouseExit;
                counts++;

                table.add(button).size(64);
                if (counts > rowcount)
                {
                    counts = 0;
                    table.row();
                }
            }


        }

        private void Button_OnMouseExit(Button obj)
        {
            var btn = obj as ItemIconButton;
            itemsTable.removeElement(btn.entryTable);
            btn.entryTable = null;
        }

        private void Button_OnMouseEntry(Button obj)
        {
            var windowFrame = Core.content.Load<Texture2D>("UI/frame_opaque");
            var btn = obj as ItemIconButton;
            btn.entryTable = new Table();
            btn.entryTable.setFillParent(false);

            var detailTable = createItemDeatil(btn);
            btn.entryTable.addElement(detailTable);
            btn.entryTable.setSize(detailTable.preferredWidth < 200 ? 200 : detailTable.preferredWidth, detailTable.preferredHeight < 200 ? 200 : detailTable.preferredHeight);

            var back = new SubtextureDrawable(new Nez.Textures.Subtexture(windowFrame));
            back.setPadding(0, btn.entryTable.preferredWidth, 0, btn.entryTable.preferredHeight);
            btn.entryTable.setBackground(back);

            var x = detailScrollPane.getX() + btn.getX() + btn.getWidth();
            var y = detailScrollPane.getY() + btn.getY() - detailScrollPane.getScrollY();



            Rectangle rectangle = new Rectangle((int)(itemsTable.getX() + allContain.getX() + x), (int)(itemsTable.getY() + allContain.getY() + y), (int)btn.entryTable.getWidth(), (int)btn.entryTable.getHeight());

            if (rectangle.Bottom > Screen.height)
            {
                y = detailScrollPane.getY() + btn.getY() + btn.getHeight() - btn.entryTable.getHeight() - detailScrollPane.getScrollY();
            }
            if (rectangle.Right > Screen.width)
            {
                x = detailScrollPane.getX() + btn.getX() - btn.entryTable.getWidth();
            }

            itemsTable.addElement(btn.entryTable).setPosition(x, y);


        }

        //private void Button_onClicked(Button obj)
        //{
        //    detailMessageTable.clear();
        //    detailMessageTable.add(new Image(((ItemIconButton)obj).item.itemIcon));
        //    detailMessageTable.add(new Label(((ItemIconButton)obj).item.name));
        //    detailMessageTable.row();
        //    detailMessageTable.add(new Label(((ItemIconButton)obj).item.describetion));
        //    detailMessageTable.row();
        //    foreach (var pro in ((ItemIconButton)obj).item.properties)
        //    {
        //        detailMessageTable.add(new Label(pro));
        //        detailMessageTable.row();
        //    }
        //    detailMessageTable.add(new Label("售价:" + ((ItemIconButton)obj).item.saleMoney));
        //    var query = player.items.Keys.Where(m => m.id == ((ItemIconButton)obj).item.id);
        //    var item = query.First();

        //    detailMessageTable.add(new Label("数量：" + player.items[item]));

        //}

        private Table createItemDeatil(ItemIconButton btn)
        {
            var windowFrame = Core.content.Load<Texture2D>("UI/frame_opaque");
            var detailTable = new Table();
            detailTable.setFillParent(true);
            detailTable.pad(20);
            var img = detailTable.add(new Image(new SubtextureDrawable(btn.item.itemIcon))).left();
            detailTable.add(new Label(btn.item.name));

            detailTable.row();
            var desc = new Label(btn.item.describetion);
            desc.setWrap(true);
            detailTable.add(desc).setMinHeight(25).setMinWidth(200).left().setColspan(2);

            detailTable.row().height(desc.getHeight()+20);
            detailTable.add(new Label("属性:")).setMinHeight(25).left();
            detailTable.row();
            foreach (var property in btn.item.properties)
            {
                detailTable.add(new Label(property)).setMinHeight(25).left(); ;
                detailTable.row();
            }
            detailTable.add(new Label($"售价: {btn.item.saleMoney.ToString()}")).setMinHeight(25);
            var ite = player.items.Keys.Where(m => m.id == btn.item.id).First();
            detailTable.add(new Label($"数量：{player.items[ite]}"));
            return detailTable;
        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GameEntities.Items;
using MyGame.MyUIs;
using Nez;
using Nez.Textures;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Player.PlayerUI
{
    public class EquitMentWindow : Window
    {

        #region Properties
        int rowcount = 6;
        Table allContain;
        Table itemTable;
        Table itemDetailTable;
        Table equitTable;
        Table propertyTable;
        ScrollPane itemScrollPane;
        
        Player player;
        #region Frame Texture
        Texture2D windowFrame;
        Texture2D buttonFrame;

        
        int buttonSize = 64;
        #endregion
        #endregion
        public EquitMentWindow(Player player) : base("装备界面", Skin.createDefaultSkin())
        {
            getTitleLabel().setFontScale(2f).setAlignment(Align.center);
            this.setFillParent(true).left().top().padTop(50).padRight(50);
            this.player = player;
            
            allContain = new Table();
            this.addElement(allContain).setFillParent(true);

            //this.debugAll();

            initializeEquitTable();
            initializeItemTable();

        }

        #region equitTable
        private void initializeEquitTable()
        {
            equitTable = new Table();
            windowFrame = Core.content.Load<Texture2D>("UI/window_frame_grey");
            buttonFrame = Core.content.Load<Texture2D>("UI/frame_grey");

            var back = new SubtextureDrawable(new Nez.Textures.Subtexture(windowFrame));
            back.setPadding(0, 620, 0, 540);
            equitTable.setBackground(back);
            equitTable.setSize(540, 620);
            allContain.add(equitTable).top().left().pad(25);

            var helmet = createItembutton(player.helmet);

            var armor = createItembutton(player.armor);

            var shoes = createItembutton(player.shoes);

            var necklace = createItembutton(player.necklace);

            var wristbands = createItembutton(player.wristbands);

            var ring = createItembutton(player.ring);

            var weapon = createItembutton(player.weapon);

            var shield = createItembutton(player.shield);
            int addition = 80;

            var playerTexture = Core.content.Load<Texture2D>("Images/Players/player");
            var subtexture = Subtexture.subtexturesFromAtlas(playerTexture, 24, 32);
            equitTable.addElement(new Image(subtexture[0])).setPosition(198, 80).setScale(6f);

            equitTable.addElement(helmet).setPosition(30, 30).setSize(64, 64);
            equitTable.addElement(armor).setPosition(30, 30 + addition).setSize(64, 64);
            equitTable.addElement(shoes).setPosition(30, 30 + addition * 2).setSize(64, 64);


            equitTable.addElement(necklace).setPosition(446, 30).setSize(64, 64);
            equitTable.addElement(wristbands).setPosition(446, 30 + addition).setSize(64, 64);
            equitTable.addElement(ring).setPosition(446, 30 + addition * 2).setSize(64, 64);

            equitTable.addElement(weapon).setPosition(150, 320).setSize(64, 64);
            equitTable.addElement(shield).setPosition(326, 320).setSize(64, 64);

            InitPropertiesTable();

            
        }

        private void InitPropertiesTable()
        {
            propertyTable = new Table();
            propertyTable.setSize(540, 236);
            var actorProperty = player.getComponent<ActorPropertyComponent>();
            #region Damage Property
            Table damageTable = new Table();
            damageTable.add(new Label("伤害:")).right().setPadLeft(20).setPadRight(10).setMinHeight(25);
            damageTable.add(new Label($"{actorProperty.damage}")).right().setMinHeight(25);
            damageTable.add(new Label($"+{actorProperty.damageUpValue + actorProperty.damage * actorProperty.damageUpRate}", new LabelStyle(Color.Green))).left().setMinHeight(25);
            propertyTable.add(damageTable).setExpandX().left().setPadLeft(30);
            #endregion
            #region Armor Properties
            Table armorTable = new Table();
            armorTable.add(new Label("护甲:")).left().setPadLeft(20).setPadRight(10).setMinHeight(25);
            armorTable.add(new Label($"{actorProperty.armor}")).right().setMinHeight(25);
            armorTable.add(new Label($"+{actorProperty.armorUpValue}", new LabelStyle(Color.Green))).left().setMinHeight(25);
            propertyTable.add(armorTable).setExpandX().left().setPadLeft(30);
            #endregion
            propertyTable.row();
            #region Magic Properties
            Table magicTable = new Table();
            magicTable.add(new Label("魔法抗性:")).left().setPadLeft(20).setPadRight(10).setMinHeight(25);
            magicTable.add(new Label($"{actorProperty.magicResistance}")).right().setMinHeight(25);
            magicTable.add(new Label($"+{actorProperty.magicResistanceUpValue}",new LabelStyle(Color.Green))).left().setMinHeight(25);
            propertyTable.add(magicTable).setExpandX().left().setPadLeft(30);
            #endregion
            #region Fire Properties
            Table fireTable = new Table();
            fireTable.add(new Label("火焰抗性:")).left().setPadLeft(20).setPadRight(10).setMinHeight(25);
            fireTable.add(new Label($"{actorProperty.fireResistance}")).right().setMinHeight(25);
            fireTable.add(new Label($"+{actorProperty.fireResistanceUpValue}", new LabelStyle(Color.Green))).left().setMinHeight(25);
            propertyTable.add(fireTable).setExpandX().left().setPadLeft(30);
            #endregion
            propertyTable.row();
            #region forzen Properties
            Table forzenTable = new Table();
            forzenTable.add(new Label("冰冻抗性:")).left().setPadLeft(20).setPadRight(10).setMinHeight(25);
            forzenTable.add(new Label($"{actorProperty.forzenResistance}")).right().setMinHeight(25);
            forzenTable.add(new Label($"+{actorProperty.forzenResistanceUpValue}", new LabelStyle(Color.Green))).left().setMinHeight(25);
            propertyTable.add(forzenTable).setExpandX().left().setPadLeft(30);
            #endregion
            #region paraly Properties
            Table paralyTable = new Table();
            paralyTable.add(new Label("麻痹抗性:")).left().setPadLeft(20).setPadRight(10).setMinHeight(25);
            paralyTable.add(new Label($"{actorProperty.paralysisResistance}")).right().setMinHeight(25);
            paralyTable.add(new Label($"+{actorProperty.paralysisResistanceUpValue}", new LabelStyle(Color.Green))).left().setMinHeight(25);
            propertyTable.add(paralyTable).setExpandX().left().setPadLeft(30);
            #endregion
            propertyTable.row();
            #region Poison Properties
            Table poisonTable = new Table();
            poisonTable.add(new Label("中毒抗性:")).left().setPadLeft(20).setPadRight(10).setMinHeight(25);
            poisonTable.add(new Label($"{actorProperty.poisonResistance}")).right().setMinHeight(25);
            poisonTable.add(new Label($"+{actorProperty.poisonResistanceUpValue}", new LabelStyle(Color.Green))).left().setMinHeight(25);
            propertyTable.add(poisonTable).setExpandX().left().setPadLeft(30);
            #endregion
            #region Move Properties
            Table moveTable = new Table();
            moveTable.add(new Label("移动速度:")).left().setPadLeft(20).setPadRight(10).setMinHeight(25);
            moveTable.add(new Label($"{actorProperty.moveSpeed}")).right().setMinHeight(25);
            moveTable.add(new Label($"+{actorProperty.moveSpeedUpValue + actorProperty.moveSpeed*actorProperty.moveSpeedUpRate}", new LabelStyle(Color.Green))).left().setMinHeight(25);
            propertyTable.add(moveTable).setExpandX().left().setPadLeft(30);
            #endregion
            propertyTable.row();
            #region Hp
            Table hpTable = new Table();
            ProgressBar hpSlider = new ProgressBar(0, 100, 0.1f, false, Skin.createDefaultSkin());
            hpSlider.setValue(100f * ((float)actorProperty.HP / (float)actorProperty.MaxHP)) ;
            hpTable.add(new Label("血量:")).left().setPadLeft(20).setPadRight(10).setMinHeight(25);
            hpTable.add(hpSlider);
            propertyTable.add(hpTable).setExpandX().left().setPadLeft(30);
            #endregion

            #region Mp
            Table mpTable = new Table();
            ProgressBar mpSlider = new ProgressBar(0, 100, 0.1f, false, Skin.createDefaultSkin());
            mpSlider.setValue(100 * (actorProperty.MP / actorProperty.MaxMP));
            mpTable.add(new Label("魔法值:")).left().setPadLeft(20).setPadRight(10).setMinHeight(25); ;
            mpTable.add(mpSlider);
            propertyTable.add(mpTable).setExpandX().left().setPadLeft(30);
            #endregion

            equitTable.addElement(propertyTable).setPosition(0,390);


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
                equitButton.OnMouseExit += EquitButton_OnMouseExit; ;
                equitButton.OnMouseEntry += EquitButton_OnMouseEntry;
                equitButton.onClicked += EquitButton_onClicked;
            }
            return equitButton;
        }

        private void EquitButton_OnMouseExit(Button obj)
        {
            var btn = obj as ItemIconButton;
            equitTable.removeElement(btn.entryTable);
            btn.entryTable = null;
        }

        private void EquitButton_OnMouseEntry(Button obj)
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

            var x = btn.getX() + btn.getWidth();
            var y = btn.getY() ;



            Rectangle rectangle = new Rectangle((int)(itemTable.getX() + allContain.getX() + x), (int)(itemTable.getY() + allContain.getY() + y), (int)btn.entryTable.getWidth(), (int)btn.entryTable.getHeight());

            if (rectangle.Bottom > Screen.height)
            {
                y = btn.getY() + btn.getHeight() - btn.entryTable.getHeight() - itemScrollPane.getScrollY();
            }
            if (rectangle.Right > Screen.width)
            {
                x = btn.getX() - btn.entryTable.getWidth();
            }

            equitTable.addElement(btn.entryTable).setPosition(x, y);

        }

        private void EquitButton_onClicked(Button obj)
        {
            var btn = obj as ItemIconButton;
            var equitable = btn.item as Equitment;
            equitable.tackOff?.Invoke(equitable,player);
            Refeshes();
        }
        #endregion

        private void Refeshes()
        {
            allContain.clear();
            initializeEquitTable();
            initializeItemTable();
        }


        #region ItemTable
        private void initializeItemTable()
        {
            itemTable = new Table();

            var windowFrame = Core.content.Load<Texture2D>("UI/window_frame_grey");
            var buttonFrame = Core.content.Load<Texture2D>("UI/frame_grey");

            var back = new SubtextureDrawable(new Nez.Textures.Subtexture(windowFrame));
            back.setPadding(0, 620, 0, 540);
            itemTable.setBackground(back);

            allContain.add(itemTable).right().top().pad(25);


            itemDetailTable = new Table();
            itemDetailTable.setFillParent(true).top().left();
            itemScrollPane = new ScrollPane(itemDetailTable, Skin.createDefaultSkin());
            
            itemTable.addElement(itemScrollPane).setPosition(30,30);
            itemScrollPane.setSize(500, 540);

            ButtonGroup buttonGroup = new ButtonGroup();

            int counts = 0;

            var itemlist = player.items.Keys.Where(m => m.types == ItemComponent.ItemTypes.Equitment).ToList();
            foreach (var item in itemlist)
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

                itemDetailTable.add(button).minSize(64);
                if (counts > rowcount)
                {
                    counts = 0;
                    itemDetailTable.row();
                }
            }


            allContain.addElement(new Label("摁Q：关闭")).setPosition(1200, 710);

        }

        private void Button_onClicked(Button obj)
        {
            var btn = obj as ItemIconButton;
            var equitable = btn.item as Equitment;
            equitable.equit?.Invoke(equitable, player);
           
            Refeshes();
        }

        private void Button_OnMouseExit(Button obj)
        {
            var btn = obj as ItemIconButton;
            itemTable.removeElement(btn.entryTable);
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

            var x = itemScrollPane.getX() + btn.getX() + btn.getWidth();
            var y = itemScrollPane.getY() + btn.getY() - itemScrollPane.getScrollY();

            

            Rectangle rectangle = new Rectangle((int)(itemTable.getX()+ allContain.getX()+x), (int)(itemTable.getY()+ allContain.getY() + y), (int)btn.entryTable.getWidth(), (int)btn.entryTable.getHeight());

            if (rectangle.Bottom > Screen.height)
            {
                y = itemScrollPane.getY() + btn.getY() + btn.getHeight() - btn.entryTable.getHeight() - itemScrollPane.getScrollY();
            }
            if (rectangle.Right > Screen.width)
            {
                x = itemScrollPane.getX() + btn.getX() - btn.entryTable.getWidth();
            }

            itemTable.addElement(btn.entryTable).setPosition(x, y);

           
        }

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
        #endregion


    }
}

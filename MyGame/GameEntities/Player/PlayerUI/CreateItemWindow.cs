using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.GlobalManages;
using Nez;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Player.PlayerUI
{
    public class CreateItemWindow:Window
    {
        #region Porperties
        Texture2D windowFrame;

        Player player;
        Table allContain;

        Table recipeTable;
        ScrollPane recipeScrollPane;
        Table recipeContentTable;

        Cell detailCell;
        Table recipeDetailTable;
        ScrollPane recipeDetailScrollPane;
        Table recipeDetailContentTable;

        #endregion


        public CreateItemWindow(Player player):base("建造界面",Skin.createDefaultSkin())
        {
            getTitleLabel().setFontScale(2f).setAlignment(Align.center);
            this.setFillParent(true).left().top().padTop(50).padRight(50);
            this.player = player;

            windowFrame = Core.content.Load<Texture2D>("UI/window_frame_grey");
            allContain = new Table();
            //allContain.setBackground(new PrimitiveDrawable(Color.Brown));
            // allContain.debugAll();

            this.addElement(allContain).setFillParent(true);

            initRecipeTable();
            initRecipeDetailTable();

        }

        private void initRecipeTable()
        {
            recipeTable = new Table();
            
            var back = new SubtextureDrawable(new Nez.Textures.Subtexture(windowFrame));
            back.setPadding(0, 620, 0, 540);
            recipeTable.setBackground(back);
            recipeTable.setSize(540, 620);
            allContain.add(recipeTable).top().left().pad(25);

            recipeTable.add(new Label("配方"));

            recipeContentTable = new Table();
            ButtonGroup buttonGroup = new ButtonGroup();
            foreach(var recipe in player.recipes)
            {
                var button = new TextButton(recipe.name, Skin.createDefaultSkin());
                buttonGroup.add(button);
                recipeContentTable.add(button).width(50).height(20);
                button.onClicked += obj =>
                 {
                     allContain.clear();
                     initRecipeTable();
                     //allContain.removeElement(recipeDetailTable);
                     initRecipeDetailTable(recipe);
                 };
            }

            recipeScrollPane = new ScrollPane(recipeContentTable,Skin.createDefaultSkin());
            recipeTable.addElement(recipeScrollPane).setPosition(30, 30);
            recipeScrollPane.setSize(500, 540);

        }



        #region recipeDetail

        private void initRecipeDetailTable(Recipe.Recipe recipe = null)
        {
            recipeDetailTable = new Table();
            var back = new SubtextureDrawable(new Nez.Textures.Subtexture(windowFrame));
            back.setPadding(0, 620, 0, 540);
            recipeDetailTable.setBackground(back);

            detailCell = allContain.add(recipeDetailTable).top().right().pad(25);


            recipeDetailContentTable = new Table();
            if (recipe != null)
            {
                bool enable = true;
                foreach (var mater in recipe.rawMaterials)
                {
                    
                    recipeDetailContentTable.add(new Image(mater.material.itemIcon)).size(64);
                    if(player.items.Keys.Where(m=>m.id == mater.material.id).Count() > 0)
                    {
                        var item = player.items.Keys.Where(m => m.id == mater.material.id).First();
                        var label = new Label($"{player.items[item]}/{mater.count}");
                        if (player.items[item] < mater.count)
                        {
                            label.setFontColor(Color.Red);
                            recipeDetailContentTable.add(label);
                            enable = false;
                        }
                        else
                        {
                            recipeDetailContentTable.add(label);
                        }
                    }
                    else
                    {
                        var label = new Label($"0/{mater.count}");
                        label.setFontColor(Color.Red);
                        recipeDetailContentTable.add(label);
                        enable = false;
                    }

                    recipeDetailContentTable.row();
                }
                if (enable)
                {
                    var button = new TextButton("建造",Skin.createDefaultSkin());
                    button.onClicked += btn=> {
                        foreach(var mat in recipe.rawMaterials)
                        {
                            player.throwOut(player.items.Keys.Where(m => m.id == mat.material.id).First(),mat.count);
                        }
                       
                        //Core.startCoroutine(createProcessBar(recipe));
                        player.pickUp(recipe.produce);
                        allContain.clear();
                        initRecipeTable();
                        initRecipeDetailTable(recipe);

                    };
                    recipeDetailContentTable.add(button).width(50).height(20);
                };
                
            }

            recipeDetailScrollPane = new ScrollPane(recipeDetailContentTable,Skin.createDefaultSkin());
            recipeDetailTable.addElement(recipeDetailScrollPane).setPosition(30, 30);
            


        }

        private IEnumerator<object> createProcessBar(Recipe.Recipe recipe)
        {
            int i = 0;
            var table = this.addElement(new Table()).setFillParent(true).center();
            var contentTable = new Dialog("",Skin.createDefaultSkin());
            table.add(contentTable).center();
            var process = new ProgressBar(0, 100, 1, false, new ProgressBarStyle() {background = new PrimitiveDrawable(Color.LightGray) });
            var label = new Label("制作中...");

            contentTable.add(label).center().width(100).height(50);
            contentTable.row();
            contentTable.add(process).center();
            while (true)
            {
                i++;
                if (i < 100)
                {
                    process.setValue(i);
                    yield return null;
                }
                else
                {
                    table.remove();
                    player.pickUp(recipe.produce);
                    allContain.clear();
                    initRecipeTable();
                    initRecipeDetailTable(recipe);
                    yield break;
                }
                   
            }
            
        }

        

        #endregion


    }
}

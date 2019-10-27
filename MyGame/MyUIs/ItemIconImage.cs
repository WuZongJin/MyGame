using Microsoft.Xna.Framework;
using MyGame.GameEntities.Items;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.MyUIs
{
    public class ItemIconButton : ImageButton, IInputListener
    {
        
        #region Properties
        public event Action<Button> OnMouseEntry;
        public event Action<Button> OnMouseExit;
        public Table entryTable;
        public ItemComponent item;
        #endregion

        #region Constrtctor
        public ItemIconButton(Nez.UI.IDrawable imageUp):base(imageUp)
        {

        }
        public ItemIconButton(Nez.UI.IDrawable imageUp, Nez.UI.IDrawable imageDown, Nez.UI.IDrawable imageOver):base(imageUp,imageDown,imageOver)
        {
        }
        #endregion




        #region Input Interface
        public void onMouseEnter()
        {
            OnMouseEntry?.Invoke(this);
        }

        public void onMouseExit()
        {
            OnMouseExit?.Invoke(this);
        }
        #endregion

    }
}

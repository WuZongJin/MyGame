using MyGame.GameEntities.Items;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.MyUIs
{
    public class ItemButton : ImageButton
    {
        #region Properties
        public ItemComponent item { get; set; }
        
        #endregion

        #region Constructor

        public ItemButton(ImageButtonStyle style) : base(style)
        {
        }

        public ItemButton(IDrawable imageUp) : base(imageUp)
        {
        }

        public ItemButton(Skin skin, string styleName = null) : base(skin, styleName)
        {
        }

        public ItemButton(IDrawable imageUp, IDrawable imageDown) : base(imageUp, imageDown)
        {
        }

        public ItemButton(IDrawable imageUp, IDrawable imageDown, IDrawable imageOver) : base(imageUp, imageDown, imageOver)
        {
        }
        #endregion
    }
}

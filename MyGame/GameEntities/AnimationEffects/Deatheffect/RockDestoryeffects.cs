using MyGame.Common;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.AnimationEffects.Deatheffect
{
    public class RockDestoryeffects:Entity
    {
        #region Properties
        Sprite<DeathAnimations> animations;
        #endregion
        public RockDestoryeffects() : base("DeathEffect")
        {

        }

        #region override
        public override void onAddedToScene()
        {
            base.onAddedToScene();
            var texture1 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(460);
            var texture2 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(461);
            var texture3 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(462);
            var texture4 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(463);
            var texture5 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(464);
            var texture6 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(465);
            var texture7 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(466);
            var texture8 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(467);
            var texture9 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(468);
            var texture10 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(469);

            animations = addComponent(new Sprite<DeathAnimations>(texture1));
            animations.setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));

            animations.addAnimation(DeathAnimations.Death, new SpriteAnimation(new List<Subtexture>()
            {
                texture1,
                texture2,
                texture3,
                texture4,
                texture5,
                texture6,
                texture7,
                texture8,
                texture9,
                texture10
            })
            {
                loop = false
            });
            animations.currentAnimation = DeathAnimations.Death;
            animations.onAnimationCompletedEvent += Animations_onAnimationCompletedEvent;
        }

        private void Animations_onAnimationCompletedEvent(DeathAnimations obj)
        {
            this.destroy();
        }
        #endregion

    }
}

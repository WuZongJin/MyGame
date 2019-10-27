﻿using MyGame.Common;
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
    class Deatheffects02:Entity
    {

        #region Properties
        Sprite<DeathAnimations> animations;
        #endregion
        public Deatheffects02() : base("DeathEffect")
        {

        }

        #region override
        public override void onAddedToScene()
        {
            base.onAddedToScene();
            var texture1 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(450);
            var texture2 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(451);
            var texture3 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(452);
            var texture4 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(453);
            var texture5 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(454);
            var texture6 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(455);
            var texture7 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(456);
            var texture8 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(457);
            var texture9 = GameResources.GameTextureResource.xAnimationsTexturePacker.Packer.getSubtexture(458);

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
                texture9
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

using Nez;
using Nez.AI.BehaviorTrees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.GameEntities.Enemys.Slime
{
    public class SlimeBehaviorTree:Component,IUpdatable
    {
        #region Properties
        Slime slime;
        BehaviorTree<SlimeBehaviorTree> tree;
        #endregion

        #region Constructor
        public SlimeBehaviorTree(Slime slime)
        {
            this.slime = slime;
            bulidSelfAbortTree();
        }
        #endregion

        #region Method
        private void bulidSelfAbortTree()
        {
            var builder = BehaviorTreeBuilder<SlimeBehaviorTree>.begin(this);

            builder.selector(AbortTypes.Self);
            builder.conditionalDecorator(m =>
            {
               return m.slime.actorProperty.HP <= 0;
            },false);
            builder.sequence()
                .logAction("-- dead! --")
                .action(m => m.slime.dead())
                .endComposite();

            builder.conditionalDecorator(m =>
            {
                return m.slime.startLostTarget;
            });
            builder.sequence()
                .logAction("-- lostTarget!--")
                .action(m => m.slime.lostTarget())
                .endComposite();

            builder.conditionalDecorator(
                m => m.slime.isLostTarget, false
                );
            builder.sequence()
                .logAction("--back! --")
                .action(m => m.slime.back())
                .endComposite();

            builder.conditionalDecorator(
                m => m.slime.isFindTarget, false
                );
            builder.sequence()
                .logAction("--chase! --")
                .action(m => m.slime.chase())
                .endComposite();

            
            builder.sequence()
                .logAction("--idle --")
                //.action(m => m.slime.idle())
                .waitAction(2f)
                .action(m=>m.slime.patrol())
                .endComposite()
                ;

            builder.endComposite();

            tree = builder.build(1f / 60);
        }
        #endregion

        public void update()
        {
            if (tree != null)
                tree.tick();
        }

        #region condition

        #endregion

    }
}

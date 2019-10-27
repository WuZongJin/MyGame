using Nez.AI.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Player.PlayerStates
{
    public class AttackState : State<Player>
    {
        #region Properties
        bool attacking = false;
        #endregion
        public override void onInitialized()
        {
            _context.animation.onAnimationCompletedEvent += Animation_onAnimationCompletedEvent;

        }

        private void Animation_onAnimationCompletedEvent(Player.PlayerAnimations animations)
        {
            switch (animations)
            {
                case Player.PlayerAnimations.AttackDown:
                case Player.PlayerAnimations.AttackLeft:
                case Player.PlayerAnimations.AttackRight:
                case Player.PlayerAnimations.AttackUp:
                    _context.weapon.isAttackOver = true;
                    _context.weapon.endAttack();
                    break;
            }
        }

        public override void reason()
        {
            if (!attacking && _context.weapon.isAttackOver)
            {
                _machine.changeState<IdleState>();
            }
        }

        public override void begin()
        {
            _context.weapon.beginAttack();

        }

        public override void update(float deltaTime)
        {
            if (_context.attackButton.isDown)
            {
                attacking = true;
            }
            else
            {
                attacking = false;
            }
            if (attacking)
            {
                _context.weapon.update();
            }
        }

        public override void end()
        {
            _context.weapon.isAttacked = false;
            _context.weapon.isAttackOver = false;
            _context.animation.flipX = false;
            attacking = false;
        }
    }
}

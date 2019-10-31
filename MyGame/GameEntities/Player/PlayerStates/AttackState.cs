using Microsoft.Xna.Framework;
using Nez.AI.FSM;
using Nez.Farseer;
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
                case Player.PlayerAnimations.BowAttackDown:
                case Player.PlayerAnimations.BowAttackLeft:
                case Player.PlayerAnimations.BowAttackRight:
                case Player.PlayerAnimations.BowAttackUp:

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
            var moveDir = Vector2.Zero;

            if (_context.upButton.isDown)
            {
                moveDir += new Vector2(0, -1);
                
            }
            if (_context.downButton.isDown)
            {
                moveDir += new Vector2(0, 1); 
            }

            if (_context.rightButton.isDown)
            {
                moveDir += new Vector2(1, 0);
                
            }
            if (_context.leftButton.isDown)
            {
                moveDir += new Vector2(-1, 0);
            }
            
            if (moveDir.Length() > 1)
            {
                moveDir.Normalize();
            }

            if (moveDir != Vector2.Zero)
            {
                var rigidBody = _context.getComponent<FSRigidBody>();
                var movement = moveDir * _context.moveSpeed*0.2f * deltaTime;
                FSCollisionResult fSCollisionResult;
                FixtureExt.collidesWithAnyFixtures(rigidBody.body.fixtureList[0], ref movement, out fSCollisionResult);
                rigidBody.entity.position += movement;
            }

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

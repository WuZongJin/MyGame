using Microsoft.Xna.Framework;
using Nez;
using Nez.AI.FSM;
using Nez.Farseer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Player.PlayerStates
{
    public class WalkState : State<Player>
    {
        #region Properties
        public Vector2 moveDir = Vector2.Zero;
        public bool direLocker = false;
        #endregion

        #region method

        #endregion


        #region override



        public override void onInitialized()
        {
            
        }

        public override void begin()
        {
            direLocker = false;
            switch (_context.currentDirection)
            {
                case Direction.Up:
                    _context.changeAnimationState(Player.PlayerAnimations.WalkUp);
                    break;
                case Direction.Down:
                    _context.changeAnimationState(Player.PlayerAnimations.WalkDown);
                    break;
                case Direction.Left:
                    _context.changeAnimationState(Player.PlayerAnimations.WalkLeft);
                    break;
                case Direction.Right:
                    _context.changeAnimationState(Player.PlayerAnimations.WalkRight);
                    break;
            }
        }

        public override void end()
        {
            direLocker = false;
        }

        public override void reason()
        {
            if (_context.attackButton.isDown)
            {
                if (_context.weapon != null)
                    _machine.changeState<AttackState>();
                
            }

        }

        public override void update(float deltaTime)
        {
            moveDir = Vector2.Zero;
            
            if (_context.upButton.isDown)
            {
                moveDir += new Vector2(0, -1);
                if (!direLocker)
                {
                    direLocker = true;
                    _context.currentDirection = Direction.Up;
                }
            }
            else if (_context.upButton.isReleased)
            {
                if (direLocker && _context.currentDirection == Direction.Up)
                {
                    direLocker = false;
                }
            }

            if (_context.downButton.isDown)
            {
                moveDir += new Vector2(0, 1);

                if (!direLocker)
                {
                    direLocker = true;
                    _context.currentDirection = Direction.Down;
                }
            }
            else if (_context.downButton.isReleased)
            {
                if (direLocker && _context.currentDirection == Direction.Down)
                {
                    direLocker = false;
                }
            }

            if (_context.rightButton.isDown)
            {
                moveDir += new Vector2(1, 0);
                if (!direLocker)
                {
                    direLocker = true;
                    _context.currentDirection = Direction.Right;
                }
            }
            else if (_context.rightButton.isReleased)
            {
                if (direLocker && _context.currentDirection == Direction.Right)
                {
                    direLocker = false;
                }
            }

            if (_context.leftButton.isDown)
            {
                moveDir += new Vector2(-1, 0);
                if (!direLocker)
                {
                    direLocker = true;
                    _context.currentDirection = Direction.Left;
                }
            }
            else if (_context.leftButton.isReleased)
            {
                if (direLocker && _context.currentDirection == Direction.Left)
                {
                    direLocker = false;
                }
            }
            if (moveDir.Length() > 1)
            {
                moveDir.Normalize();
            }
            

            if (moveDir != Vector2.Zero)
            {
                var rigidBody = _context.getComponent<FSRigidBody>();
                var movement = moveDir * _context.moveSpeed * deltaTime;
                FSCollisionResult fSCollisionResult;
                FixtureExt.collidesWithAnyFixtures(rigidBody.body.fixtureList[0], ref movement, out fSCollisionResult);
                rigidBody.entity.position += movement;

                if(_context.preDirection!= _context.currentDirection)
                {
                    begin();
                }
            }
            else
            {
                _machine.changeState<IdleState>();
            }

        }


        #endregion
    }
}

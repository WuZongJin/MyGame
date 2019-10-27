using Microsoft.Xna.Framework;
using MyGame.GlobalManages;
using MyGame.GlobalManages.Notification;
using Nez;
using Nez.AI.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyGame.GameEntities.Player.Player;

namespace MyGame.GameEntities.Player.PlayerStates
{
    public class IdleState:State<Player>
    {
        #region Porperties
        public Vector2 moveDir = Vector2.Zero;

        public override void begin()
        {
            base.begin();
            switch (_context.currentDirection)
            {
                case Direction.Up:
                    _context.changeAnimationState(PlayerAnimations.IdleUp);
                    break;
                case Direction.Down:
                    _context.changeAnimationState(PlayerAnimations.IdleDown);
                    break;
                case Direction.Right:
                    _context.changeAnimationState(PlayerAnimations.IdleRight);
                    break;
                case Direction.Left:
                    _context.changeAnimationState(PlayerAnimations.IdleLeft);
                    break;
            }
        }

        public override void update(float deltaTime)
        {
            _context.updateExecuteAblePropBar();

            moveDir = new Vector2(_context.xVirtualAxis, _context.yVirtualAxis);
            if (moveDir != Vector2.Zero)
            {
                _machine.changeState<WalkState>();
            }

            if (_context.attackButton.isDown)
            {
                if(_context.weapon!=null)
                    _machine.changeState<AttackState>();
                else
                {
                    
                }
            }

           

        }
        #endregion

    }
}

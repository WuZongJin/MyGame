using Nez.AI.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.GameEntities.Player.PlayerStates
{
    public class FallState : State<Player>
    {
        public override void onInitialized()
        {
            base.onInitialized();
        }

        public override void begin()
        {
            base.begin();
            _context.animation.currentAnimation = Player.PlayerAnimations.Fallin;
        }

        public override void reason()
        {
            base.reason();
        }

        public override void update(float deltaTime)
        {
            if(_context.animation.currentFrame == 5)
            {
                _machine.changeState<IdleState>();
                _context.actorProperty.executeDamage(Common.AttackTypes.Physics, 10);
                _context.position = _context.fallinAbleComponent.fallinReturnPosition;
            }
        }

        public override void end()
        {
            base.end();
        }
    }
}

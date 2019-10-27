using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Common;
using MyGame.Common.Game;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GameConponents.UtilityComponents;
using MyGame.GameEntities.Items;
using MyGame.GameEntities.Items.Armors;
using MyGame.GameEntities.Items.Weapon;
using MyGame.GameEntities.Player.PlayerStates;
using MyGame.GameEntities.Player.PlayerUI;
using MyGame.GameEntities.Player.PlayerUI.ScreenUI;
using MyGame.GlobalManages;
using Nez;
using Nez.AI.FSM;
using Nez.Farseer;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nez.VirtualAxis;
using static Nez.VirtualButton;

namespace MyGame.GameEntities.Player
{
    public class Player:Entity,IPauseable
    {
        public enum PlayerAnimations
        {

            IdleDown,
            WalkDown,
            IdleUp,
            WalkUp,
            IdleRight,
            WalkRight,
            IdleLeft,
            WalkLeft,

            AttackDown,
            AttackRight,
            AttackUp,
            AttackLeft,

            BowAttackDown,
            BowAttackUp,
            BowAttackRight,
            BowAttackLeft,

            Fallin,
        }

        public enum PlayerState
        {
            Idle,               //初始状态
            Walk,               //移动状态
        }

        #region Properties

        public Texture2D playerTexture;
        public Texture2D Texture;
        public float Animationfps = 25f;
        public Sprite<PlayerAnimations> animation;
        public PlayerAnimations currentAnimation;
        public PlayerAnimations previewAnimation;
        public Direction currentDirection = Direction.Down;
        public Direction preDirection;
        #endregion

        #region IPauseAble
        public bool couldPause { get; set; }

        public void initCouldPause()
        {
            couldPause = true;
        }
        #endregion

        #region ControlButton

        public VirtualButton upButton;
        public VirtualButton downButton;
        public VirtualButton rightButton;
        public VirtualButton leftButton;

        public VirtualButton attackButton;

        public VirtualButton itemsWindowButton;
        public VirtualButton equitWindowButton;

        public VirtualIntegerAxis xVirtualAxis;
        public VirtualIntegerAxis yVirtualAxis;

        public VirtualButton AButton;
        public VirtualButton SButton;
        public VirtualButton DButton;
        public VirtualButton FButton;
        public VirtualButton GButton;

        #endregion

        #region State Properties
        public float moveSpeed { get { return this.actorProperty.moveSpeed+actorProperty.moveSpeedUpValue+actorProperty.moveSpeed*actorProperty.moveSpeedUpRate; } }
        StateMachine<Player> playerStateMachine;
        IdleState idleState;
        WalkState walkState;
        AttackState attackState;
        FallState fallState;
        #endregion

        #region Collision Properties
        public FSRigidBody rigidBody;
        public FSCollisionShape shape;

        #endregion

        #region ActorProperties
        public ActorPropertyComponent actorProperty { get; set; }
        #endregion

        #region IFallin Interface

        public FallinAbleComponent fallinAbleComponent;
        private void initFallinMethod()
        {
            fallinAbleComponent = addComponent(new FallinAbleComponent());

            fallinAbleComponent.fallin += () =>
            {
                playerStateMachine.changeState<FallState>();
            };
        }


        #endregion

        #region Items Collision
        public Armor armor { get; set; }
        public Weapon weapon { get; set; }
        public Equitment shoes { get; set; }
        public Equitment helmet { get; set; }
        public Equitment shield { get; set; }
        public Equitment wristbands { get; set; }
        public Equitment necklace { get; set; }
        public Equitment ring { get; set; }
       
        public Dictionary<ItemComponent, int> items;
        public List<Recipe.Recipe> recipes;

        #endregion

        #region ExecuteProps
        public ExecuteAbleProps AProps;
        public ExecuteAbleProps SProps;
        public ExecuteAbleProps DProps;
        public ExecuteAbleProps FProps;
        public ExecuteAbleProps GProps;

        public void updateExecuteAblePropBar()
        {
            if (AButton.isPressed)
            {
                if(AProps != null)
                {
                    AProps.excute(this);
                    useProps(AProps);
                }
            }

            if (SButton.isPressed)
            {
                if (SProps != null)
                {
                    SProps.excute(this);
                    useProps(SProps);
                }
            }

            if (DButton.isPressed)
            {
                if (DProps != null)
                {
                    DProps.excute(this);
                    useProps(DProps);
                }
            }

            if (FButton.isPressed)
            {
                if (FProps != null)
                {
                    FProps.excute(this);
                    useProps(FProps);
                }
            }

            if (GButton.isPressed)
            {
                if (GProps != null)
                {
                    GProps.excute(this);
                    useProps(GProps);
                }
            }


        }

        public void useProps(ExecuteAbleProps props)
        {
            var item = items.Keys.Where(m => m.id == props.id).First();
            items[item]--;
            if(items[item] == 0)
            {
                items.Remove(item);
                if(AProps.id == item.id)
                {
                    AProps = null;
                }
                else if(SProps.id == item.id)
                {
                    SProps = null;
                }
                else if (DProps.id == item.id)
                {
                    DProps = null;
                }
                else if (FProps.id == item.id)
                {
                    FProps = null;
                }
                else if (GProps.id == item.id)
                {
                    GProps = null;
                }
            }
        }


        #endregion

        #region Player Screen UI 
        private void initPlayerUI()
        {
            addComponent<PlayerStateUI>().setRenderLayer(GameLayerSetting.playerUiLayer);
            addComponent<executeAblePropsBar>().setRenderLayer(GameLayerSetting.playerUiLayer);
            addComponent<PlayerWeaponUI>().setRenderLayer(GameLayerSetting.playerUiLayer);
        }
        #endregion

        #region Constructor
        public Player() : base("player")
        {
            items = new Dictionary<ItemComponent, int>();
            recipes = new List<Recipe.Recipe>();
            setUpInput();
            InitActorPRoperties();
            initializeAnimation();
            initFallinMethod();
            initializeSateMachine();
            initCouldPause();
            initPlayerUI();
        }
        #endregion

        #region Override Method
        public override void onAddedToScene()
        {
            InitializeCollision();
        }
        public override void onRemovedFromScene()
        {
            base.onRemovedFromScene();
            removeCollision();
        }

        public override void update()
        {
            if (couldPause && GameSetting.isGamePause) return;
            base.update();
            //if (potentialFallin)
            //{
            //    var tposition = this.rigidBody.body.position + new Vector2(0f,0.05f);
            //    if (fallinHole.testPoint(ref tposition))
            //    {
            //        fallin?.Invoke();
            //    }
            //}

            preDirection = currentDirection;
            playerStateMachine.update(Time.deltaTime);
            animation.setLayerDepth(LayerDepthExt.caluelateLayerDepth(this.position.Y));
        }
        #endregion

        #region ActorProperties Method
        private void InitActorPRoperties()
        {
            actorProperty = addComponent<ActorPropertyComponent>();
            actorProperty.MaxHP = 100;
            actorProperty.HP = 100;
            actorProperty.MaxMP = 100;
            actorProperty.MP = 100;
            actorProperty.damage = 4;
            actorProperty.armor = 0;
            actorProperty.fireResistance = 5;
            actorProperty.forzenResistance = 5;
            actorProperty.poisonResistance = 5;
            actorProperty.paralysisResistance = 5;
            actorProperty.magicResistance = 5;
            actorProperty.moveSpeed = 200f;

        }
        #endregion

        #region Item Method

        public void throwOut(ItemComponent item,int count = 1)
        {
            if (!items.ContainsKey(item)) throw  new KeyNotFoundException();
            if (items[item] < count) throw new Exception(" count can't bigger than value");

            items[item] -= count;
            if (items[item] == 0)
                items.Remove(item);

        }
       
        public void pickUp(ItemComponent pickupable,int count = 1)
        {
            var query = this.items.Keys.Where(m => m.id == pickupable.id);
            if (query.Count()>0)
            {
                var item = query.First();
                this.items[item] += count;
            }
            else
            {
                this.items.Add(pickupable, count);
            }

            var notification = Core.getGlobalManager<NotificationManager>();
            notification.getItemNotifacation(pickupable, count);

            var props = pickupable as ExecuteAbleProps;
            if(props!=null)
            {
                equitToPropsTable(props);
            }
        }

        public void equitToPropsTable(ExecuteAbleProps props)
        {
            if ((AProps != null && AProps.id == props.id)
                || (SProps != null && SProps.id == props.id)
                || (DProps != null && DProps.id == props.id)
                || (FProps != null && FProps.id == props.id)
                || (GProps != null && GProps.id == props.id)
                )
                return;

            if(AProps == null)
            {
                AProps = props;
                return;
            }
            else if (SProps == null)
            {
                SProps = props;
                return;
            }
            else if (DProps == null)
            {
                DProps = props;
                return;
            }
            else if(FProps == null)
            {
                FProps = props;
            }
            else if(GProps == null)
            {
                GProps = null;
            }
        }
        #endregion


        #region Aniamtion Method
        public void changeAnimationState(PlayerAnimations playerAnimations)
        {
            currentAnimation = playerAnimations;
            
            animation.play(currentAnimation);
            
        }

        private void initializeAnimation()
        {
            playerTexture = Core.content.Load<Texture2D>("Images/Players/player");
            Texture = Core.content.Load<Texture2D>("Images/Players/Allplayer");
            var subtexture = Subtexture.subtexturesFromAtlas(playerTexture, 24, 32);
            currentAnimation = PlayerAnimations.IdleDown;
            animation = addComponent(new Sprite<PlayerAnimations>(subtexture[0])) ;
            animation.setRenderLayer(GameLayerSetting.actorMoverLayer);
            //animation.setLocalOffset(new Microsoft.Xna.Framework.Vector2(0, -8));

            animation.addAnimation(PlayerAnimations.IdleDown, new SpriteAnimation(new List<Subtexture>()
            {
                subtexture[0]
            }));
            animation.addAnimation(PlayerAnimations.WalkDown, new SpriteAnimation(new List<Subtexture>()
            {
                subtexture[1],
                subtexture[2],
                subtexture[3],
                subtexture[4],
                subtexture[5],
                subtexture[6],
                subtexture[7],
                subtexture[8],
                subtexture[9],
                subtexture[10],
            }));

            animation.addAnimation(PlayerAnimations.IdleRight, new SpriteAnimation(new List<Subtexture>()
            {
                subtexture[11+0]
            }));
            animation.addAnimation(PlayerAnimations.WalkRight, new SpriteAnimation(new List<Subtexture>()
            {
                subtexture[11+1],
                subtexture[11+2],
                subtexture[11+3],
                subtexture[11+4],
                subtexture[11+5],
                subtexture[11+6],
                subtexture[11+7],
                subtexture[11+8],
                subtexture[11+9],
                subtexture[11+10],
            }));

            animation.addAnimation(PlayerAnimations.IdleLeft, new SpriteAnimation(new List<Subtexture>()
            {
                subtexture[22+10]
            }));
            animation.addAnimation(PlayerAnimations.WalkLeft, new SpriteAnimation(new List<Subtexture>()
            {
                subtexture[22+9],
                subtexture[22+8],
                subtexture[22+7],
                subtexture[22+6],
                subtexture[22+5],
                subtexture[22+4],
                subtexture[22+3],
                subtexture[22+2],
                subtexture[22+1],
                subtexture[22+0],
            }));

            animation.addAnimation(PlayerAnimations.IdleUp, new SpriteAnimation(new List<Subtexture>()
            {
                subtexture[33+0]
            }));
            animation.addAnimation(PlayerAnimations.WalkUp, new SpriteAnimation(new List<Subtexture>()
            {
                subtexture[33+1],
                subtexture[33+2],
                subtexture[33+3],
                subtexture[33+4],
                subtexture[33+5],
                subtexture[33+6],
                subtexture[33+7],
                subtexture[33+8],
                subtexture[33+9],
                subtexture[33+10],
            }));

            animation.addAnimation(PlayerAnimations.AttackDown, new SpriteAnimation(LoadAttackDown())
            {
                fps = Animationfps,
                loop = false
            });
            animation.addAnimation(PlayerAnimations.AttackUp, new SpriteAnimation(LoadAttackUp())
            {
                fps = Animationfps,
                loop = false
            });
            animation.addAnimation(PlayerAnimations.AttackRight, new SpriteAnimation(LoadAttackRight())
            {
                fps = Animationfps
                ,
                loop = false
            });
            animation.addAnimation(PlayerAnimations.AttackLeft, new SpriteAnimation(LoadAttackRight())
            {
                fps = Animationfps,

                loop = false
            });

            animation.addAnimation(PlayerAnimations.BowAttackDown, new SpriteAnimation(LoadBowAttackDown()) { fps = Animationfps, loop = false });
            animation.addAnimation(PlayerAnimations.BowAttackUp, new SpriteAnimation(LoadBowAttackUp()) { fps = Animationfps, loop = false });
            animation.addAnimation(PlayerAnimations.BowAttackRight, new SpriteAnimation(LoadBowAttackRight()) { fps = Animationfps, loop = false });
            animation.addAnimation(PlayerAnimations.BowAttackLeft, new SpriteAnimation(LoadBowAttackRight()) { fps = Animationfps, loop = false });

            animation.addAnimation(PlayerAnimations.Fallin, new SpriteAnimation(LoadFallin()) {  loop = false });

        }

        private List<Subtexture> LoadAttackDown()
        {


            Subtexture subtexture = new Subtexture(Texture, new Rectangle(x: 22, y: 131, width: 29, height: 23), new Vector2(32f * 0.65f, 32f * 0.65f));
            Subtexture attack0 = new Subtexture(Texture, new Rectangle(x: 57, y: 130, width: 27, height: 30), new Vector2(27f * 0.65f, 30f * 0.53f));
            Subtexture attack1 = new Subtexture(Texture, new Rectangle(x: 89, y: 131, width: 23, height: 35), new Vector2(23f * 0.62f, 35f * 0.49f));

            Subtexture attack2 = new Subtexture(Texture, new Rectangle(x: 121, y: 132, width: 21, height: 36), new Vector2(21f * 0.47f, 36f * 0.46f));


            Subtexture attack3 = new Subtexture(Texture, new Rectangle(x: 150, y: 132, width: 26, height: 35), new Vector2(26f * 0.38f, 35f * 0.46f));



            Subtexture attack4 = new Subtexture(Texture, new Rectangle(x: 188, y: 131, width: 29, height: 32), new Vector2(29f * 0.32f, 38f * 0.49f));

            Subtexture attack5 = new Subtexture(Texture, new Rectangle(x: 225, y: 130, width: 33, height: 24), new Vector2(33f * 0.27f, 24f * 0.65f));
            Subtexture attack6 = new Subtexture(Texture, new Rectangle(x: 264, y: 130, width: 19, height: 24), new Vector2(19f * 0.47f, 24f * 0.65f));

            


            List<Subtexture> attackAnimation = new List<Subtexture>()
            {
                subtexture,
                attack0,
                attack1,
                attack2,
                attack3,
                attack4,
                attack5,
                attack6,
                //attack7,
            };

            return attackAnimation;

        }
        private List<Subtexture> LoadAttackRight()
        {
            Subtexture attack0 = new Subtexture(Texture, new Rectangle(25, 2630 - 2425 - 33, 18, 33));
            Subtexture attack1 = new Subtexture(Texture, new Rectangle(52, 2630 - 2425 - 32, 28, 32));
            Subtexture attack2 = new Subtexture(Texture, new Rectangle(85, 2630 - 2425 - 28, 33, 28), new Vector2(33 * 0.5f, 28 * 0.44f));
            Subtexture attack3 = new Subtexture(Texture, new Rectangle(123, 2630 - 2425 - 21, 35, 21), new Vector2(35f * 0.48f, 21f * 0.38f));
            Subtexture attack4 = new Subtexture(Texture, new Rectangle(165, 2630 - 2422 - 24, 34, 24), new Vector2(34f * 0.507f, 24f * 0.25f));
            Subtexture attack5 = new Subtexture(Texture, new Rectangle(206, 2630 - 2417 - 30, 30, 30), new Vector2(30f * 0.5f, 30f * 0.19f));
            Subtexture attack6 = new Subtexture(Texture, new Rectangle(244, 2630 - 2414 - 34, 17, 34), new Vector2(17f * 0.73f, 34f * 0.2f));
            Subtexture attack7 = new Subtexture(Texture, new Rectangle(268, 2630 - 2425 - 23, 17, 23), new Vector2(17f * 0.72f, 23f * 0.25f));

            return new List<Subtexture>()
            {
                attack0,
                attack1,
                attack2,
                attack3,
                attack4,
                attack5,
                attack6,
                attack7,
            };
        }
        private List<Subtexture> LoadAttackUp()
        {
            Subtexture attack0 = new Subtexture(Texture, new Rectangle(25, 2630 - 2376 - 24, 25, 24), new Vector2(25f * 0.36f, 24f * 0.27f));
            Subtexture attack1 = new Subtexture(Texture, new Rectangle(55, 2630 - 2376 - 28, 26, 28), new Vector2(26f * 0.36f, 28f * 0.47f));
            Subtexture attack2 = new Subtexture(Texture, new Rectangle(87, 2630 - 2376 - 35, 19, 35));
            Subtexture attack3 = new Subtexture(Texture, new Rectangle(112, 2630 - 2376 - 24, 25, 33), new Vector2(25f * 0.65f, 33f * 0.48f));
            Subtexture attack4 = new Subtexture(Texture, new Rectangle(144, 2630 - 2376 - 29, 29, 29), new Vector2(29f * 0.69f, 29f * 0.43f));
            Subtexture attack5 = new Subtexture(Texture, new Rectangle(180, 2630 - 2376 - 25, 32, 25), new Vector2(32f * 0.71f, 25f * 0.39f));
            Subtexture attack6 = new Subtexture(Texture, new Rectangle(218, 2630 - 2376 - 23, 32, 23), new Vector2(32f * 0.71f, 23f * 0.32f));
            Subtexture attack7 = new Subtexture(Texture, new Rectangle(257, 2630 - 2376 - 23, 18, 23), new Vector2(18f * 0.5f, 23f * 0.35f));

            return new List<Subtexture>()
            {
                attack0,
                attack1,
                attack2,
                attack3,
                attack4,
                attack5,
                attack6,
                attack7,
            };

        }

        private List<Subtexture> LoadFallin()
        {
            Subtexture subtexture1 = new Subtexture(Texture, new Rectangle(62, 2018, 28, 22));
            Subtexture subtexture2 = new Subtexture(Texture, new Rectangle(94, 2022, 24, 17));
            Subtexture subtexture3 = new Subtexture(Texture, new Rectangle(120, 2020, 20, 18));
            Subtexture subtexture4 = new Subtexture(Texture, new Rectangle(143, 2024, 13, 13));
            Subtexture subtexture5 = new Subtexture(Texture, new Rectangle(159, 2027, 7, 7));
            Subtexture subtexture6 = new Subtexture(Texture, new Rectangle(169, 2027, 7, 7));

            return new List<Subtexture>()
            {
                subtexture1,
                subtexture2,
                subtexture3,
                subtexture4,
                subtexture5,
                subtexture6,
            };
        }
        private List<Subtexture> LoadDushDown()
        {
            Subtexture dush0 = new Subtexture(Texture, new Rectangle(331, 2630 - 2464 - 35, 18, 35));
            Subtexture dush1 = new Subtexture(Texture, new Rectangle(352, 2630 - 2464 - 35, 18, 35));
            Subtexture dush2 = new Subtexture(Texture, new Rectangle(374, 2630 - 2464 - 35, 19, 35));
            Subtexture dush3 = new Subtexture(Texture, new Rectangle(397, 2630 - 2464 - 35, 19, 33));
            Subtexture dush4 = new Subtexture(Texture, new Rectangle(419, 2630 - 2464 - 34, 19, 34));

            return new List<Subtexture>()
            {
                dush0,dush1,dush2,dush3,dush4,
            };
        }
        private List<Subtexture> LoadDushRight()
        {
            Subtexture dush0 = new Subtexture(Texture, new Rectangle(330, 2630 - 2426 - 23, 33, 23));
            Subtexture dush1 = new Subtexture(Texture, new Rectangle(367, 2630 - 2426 - 23, 33, 23));
            Subtexture dush2 = new Subtexture(Texture, new Rectangle(408, 2630 - 2426 - 24, 30, 24));
            Subtexture dush3 = new Subtexture(Texture, new Rectangle(446, 2630 - 2426 - 25, 30, 25));
            Subtexture dush4 = new Subtexture(Texture, new Rectangle(483, 2630 - 2426 - 24, 31, 24));

            return new List<Subtexture>()
            {
                dush0,dush1,dush2,dush3,dush4,
            };
        }
        private List<Subtexture> LoadDushUp()
        {
            Subtexture dush0 = new Subtexture(Texture, new Rectangle(332, 2630 - 2384 - 29, 18, 29));
            Subtexture dush1 = new Subtexture(Texture, new Rectangle(361, 2630 - 2384 - 30, 18, 30));
            Subtexture dush2 = new Subtexture(Texture, new Rectangle(388, 2630 - 2384 - 31, 18, 31));
            Subtexture dush3 = new Subtexture(Texture, new Rectangle(416, 2630 - 2384 - 32, 18, 32));
            Subtexture dush4 = new Subtexture(Texture, new Rectangle(443, 2630 - 2384 - 31, 18, 31));

            return new List<Subtexture>()
            {
                dush0,dush1,dush2,dush3,dush4,
            };
        }
        private List<Subtexture> LoadBowAttackDown()
        {
            Subtexture attack0 = new Subtexture(Texture, new Rectangle(32, 2630 - 1294 - 24, 18, 24));
            Subtexture attack1 = new Subtexture(Texture, new Rectangle(60, 2630 - 1293 - 24, 20, 24));
            Subtexture attack2 = new Subtexture(Texture, new Rectangle(89, 2630 - 1290 - 27, 20, 27));
            Subtexture attack3 = new Subtexture(Texture, new Rectangle(112, 2630 - 1287 - 31, 22, 31));
            Subtexture attack4 = new Subtexture(Texture, new Rectangle(143, 2630 - 1289 - 28, 21, 28));
            Subtexture attack5 = new Subtexture(Texture, new Rectangle(170, 2630 - 1283 - 34, 21, 34));
            Subtexture attack6 = new Subtexture(Texture, new Rectangle(198, 2630 - 1294 - 37, 21, 37));
            Subtexture attack7 = new Subtexture(Texture, new Rectangle(227, 2630 - 1291 - 25, 21, 25));
            Subtexture attack8 = new Subtexture(Texture, new Rectangle(254, 2630 - 1294 - 24, 19, 24));
            Subtexture attack9 = new Subtexture(Texture, new Rectangle(280, 2630 - 1292 - 24, 18, 24));

            return new List<Subtexture>()
            {
                attack0,attack1,attack2,attack3,attack4,attack5,attack6,attack7,attack8,attack9,
            };
        }
        private List<Subtexture> LoadBowAttackUp()
        {
            Subtexture attack0 = new Subtexture(Texture, new Rectangle(41, 2630 - 1188 - 24, 17, 24));
            Subtexture attack1 = new Subtexture(Texture, new Rectangle(91, 2630 - 1188 - 25, 22, 25));
            Subtexture attack2 = new Subtexture(Texture, new Rectangle(121, 2630 - 1188 - 29, 22, 29));
            Subtexture attack3 = new Subtexture(Texture, new Rectangle(149, 2630 - 1188 - 24, 23, 24));
            Subtexture attack4 = new Subtexture(Texture, new Rectangle(179, 2630 - 1188 - 33, 23, 33));
            Subtexture attack5 = new Subtexture(Texture, new Rectangle(208, 2630 - 1188 - 21, 23, 21));
            Subtexture attack6 = new Subtexture(Texture, new Rectangle(237, 2630 - 1188 - 22, 18, 22));
            Subtexture attack7 = new Subtexture(Texture, new Rectangle(265, 2630 - 1188 - 23, 18, 23));


            return new List<Subtexture>()
            {
                attack0,attack1,attack2,attack3,attack4,attack5,attack6,attack7,
            };
        }
        private List<Subtexture> LoadBowAttackRight()
        {
            Subtexture attack0 = new Subtexture(Texture, new Rectangle(42, 2630 - 1242 - 24, 19, 23));
            Subtexture attack1 = new Subtexture(Texture, new Rectangle(66, 2630 - 1242 - 23, 22, 23));
            Subtexture attack2 = new Subtexture(Texture, new Rectangle(94, 2630 - 1242 - 22, 26, 22));
            Subtexture attack3 = new Subtexture(Texture, new Rectangle(125, 2630 - 1241 - 23, 32, 23));
            Subtexture attack4 = new Subtexture(Texture, new Rectangle(166, 2630 - 1239 - 23, 21, 23));
            Subtexture attack5 = new Subtexture(Texture, new Rectangle(202, 2630 - 1239 - 23, 22, 23));
            Subtexture attack6 = new Subtexture(Texture, new Rectangle(247, 2630 - 1239 - 23, 17, 23));
            Subtexture attack7 = new Subtexture(Texture, new Rectangle(272, 2630 - 1239 - 23, 17, 23));


            return new List<Subtexture>()
            {
                attack0,attack1,attack2,attack3,attack4,attack5,attack6,attack7,
            };
        }

        #endregion

        #region FSWorld Collision Method
        public void removeCollision()
        {
            var collisionShape = getComponent<FSCollisionShape>();
            components.handleRemove(collisionShape);
            components.components.remove(collisionShape);

            var rigidBody = getComponent<FSRigidBody>();
            components.handleRemove(rigidBody);
            components.components.remove(rigidBody);

            var limited = getComponent<RigidBodyVelocityLimited>();
            components.handleRemove(limited);
            components.components.remove(limited) ;

        }
        private void InitializeCollision()
        {
            rigidBody = addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic)
                .setLinearDamping(0.8f)
                .setMass(2f)
                .setFixedRotation(true)
                .setIsSleepingAllowed(false)
                .setIsBullet(true);


            Vector2[] points = new Vector2[4]{
                new Vector2(5-12,17-18),
                new Vector2(17-12,17-18),
                new Vector2(17-12,26-16),
                new Vector2(5-12,26-16)
            };

            shape = addComponent(new FSCollisionPolygon(points));       

            shape.setCollisionCategories(CollisionSetting.playerCategory);
            shape.setCollidesWith(CollisionSetting.playerShouldTriggerCategory);
               

            addComponent<RigidBodyVelocityLimited>();
        }
        #endregion

        #region State Method
        private void initializeSateMachine()
        {
            idleState = new IdleState();
            walkState = new WalkState();
            attackState = new AttackState();
            fallState = new FallState();

            playerStateMachine = new StateMachine<Player>(this, idleState);
            playerStateMachine.addState(walkState);
            playerStateMachine.addState(attackState);
            playerStateMachine.addState(fallState);
        }
        #endregion

        #region ControlButton Method
        private void setUpInput()
        {
            upButton = new VirtualButton();
            upButton.nodes.Add(new KeyboardKey(Keys.Up));

            downButton = new VirtualButton();
            downButton.nodes.Add(new KeyboardKey(Keys.Down));

            rightButton = new VirtualButton();
            rightButton.nodes.Add(new KeyboardKey(Keys.Right));

            leftButton = new VirtualButton();
            leftButton.nodes.Add(new KeyboardKey(Keys.Left));

            attackButton = new VirtualButton();
            attackButton.nodes.Add(new KeyboardKey(Keys.X));

            itemsWindowButton = new VirtualButton();
            itemsWindowButton.nodes.Add(new KeyboardKey(Keys.Q));

            equitWindowButton = new VirtualButton();
            equitWindowButton.nodes.Add(new KeyboardKey(Keys.W));


            xVirtualAxis = new VirtualIntegerAxis();
            xVirtualAxis.nodes.Add(new KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));

            yVirtualAxis = new VirtualIntegerAxis();
            yVirtualAxis.nodes.Add(new KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Up, Keys.Down));


            AButton = new VirtualButton();
            AButton.nodes.Add(new KeyboardKey(Keys.A));

            SButton = new VirtualButton();
            SButton.nodes.Add(new KeyboardKey(Keys.S));

            DButton = new VirtualButton();
            DButton.nodes.Add(new KeyboardKey(Keys.D));

            FButton = new VirtualButton();
            FButton.nodes.Add(new KeyboardKey(Keys.F));

            GButton = new VirtualButton();
            GButton.nodes.Add(new KeyboardKey(Keys.G));



        }

       


        #endregion
    }
}

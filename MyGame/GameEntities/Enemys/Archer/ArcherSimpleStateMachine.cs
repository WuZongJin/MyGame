using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Common;
using MyGame.GameConponents.ActorPropertyComponents;
using MyGame.GameConponents.SceneObjectTriggerComponents;
using MyGame.GameConponents.UtilityComponents;
using MyGame.GameEntities.Utils;
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
using FSTransform = FarseerPhysics.Common.Transform;

namespace MyGame.GameEntities.Enemys.Archer
{
    public enum ArcherStates
    {
        Idle,
        Move,
        Attack,
        SuperAttack,
    }

    public class ArcherSimpleStateMachine:SimpleStateMachine<ArcherStates>
    {
        #region PRoperties
        Manifold _manifold;

        Archer archer;

        Vector2 direction = Vector2.Zero;
        float arrowSpeed = 150f;

        float moveTime = 1f;
        float moveTimer = 0f;
        float attackfrequency = 0.5f;
        float attackTime = 2f;
        float attackTimer = 0f;
        float superAttackTime = 2f;
        float superAttackTimer = 0f;
        #endregion

        #region Contsrtuctor
        public ArcherSimpleStateMachine(Archer archer)
        {
            this.archer = archer;
            initialState = ArcherStates.Idle;
        }
        #endregion

        #region Idle
        void Idle_Enter()
        {

        }
        void Idle_Tick()
        {
            if (archer.isFindTarget)
            {
                currentState = ArcherStates.Attack;
            }
        }

        void Idle_Exit()
        {

        }
        #endregion


        #region Move
        void Move_Enter()
        {

        }

        void Move_Tick()
        {
            moveTimer += Time.deltaTime;
            if(moveTimer > moveTime)
            {
                moveTimer = 0f;
                var rand = Nez.Random.nextFloat();
                if (rand > 0.5)
                {
                    currentState = ArcherStates.SuperAttack;
                }
                else
                {
                    currentState = ArcherStates.Attack;
                }
                
            }

            var direction = archer.target.position - archer.position;
            direction.Normalize();
            var rigidBody = archer.rigidBody;
            var movement = direction * archer.moveSpeed * Time.deltaTime;
            FSCollisionResult fSCollisionResult;
            FixtureExt.collidesWithAnyFixtures(rigidBody.body.fixtureList[0], ref movement, out fSCollisionResult);
            colliderWithHole(rigidBody.body.fixtureList[0], ref movement, out fSCollisionResult);
            rigidBody.entity.position += movement;
        }

        void Move_Exit()
        {

        }
        #endregion

        #region Attack
        void Attack_Enter()
        {

        }

        void Attack_Tick()
        {
            attackTimer += Time.deltaTime;
            if(attackTimer > attackfrequency)
            {
                attackfrequency += attackfrequency;
                var dir = archer.target.position - archer.position;
                dir.Normalize();
                var degress = (float)Math.Atan2(dir.Y, dir.X) + (float)Math.Atan2(1, 0);
                addArrow(archer.position).setRotation(degress).addComponent(new AutoMoveComponent(dir, arrowSpeed));
                
            }

            if (attackTimer > attackTime)
            {
                attackTimer = 0f;
                currentState = ArcherStates.Move;
            }
        }

        void Attack_Exit()
        {
            attackfrequency = 0.5f;
        }

        #endregion

        #region SuperAttack
        void SuperAttack_Enter()
        {

        }

        void SuperAttack_Tick()
        {
            superAttackTimer += Time.deltaTime;
            if(superAttackTimer > attackfrequency)
            {
                attackfrequency += attackfrequency;
                var dir = archer.target.position - archer.position;
                dir.Normalize();
                var degress = (float)Math.Atan2(dir.Y, dir.X) + (float)Math.Atan2(1, 0);
                addArrow(archer.position).setRotation(degress).addComponent(new AutoMoveComponent(degress,arrowSpeed));
                
                addArrow(archer.position).setRotation(degress+0.5f).addComponent(new AutoMoveComponent(degress+0.5f, arrowSpeed));
                
                addArrow(archer.position).setRotation(degress-0.5f).addComponent(new AutoMoveComponent(degress-0.5f, arrowSpeed));

                    
            }

            if(superAttackTimer > superAttackTime)
            {
                superAttackTimer = 0f;
                currentState = ArcherStates.Move;
            }
        }

        void SuperAttack_Exit()
        {
            attackfrequency = 0.5f;
        }
        #endregion

        private bool colliderWithHole(Fixture self, ref Vector2 motion ,out FSCollisionResult result)
        {
            result = new FSCollisionResult();
            motion *= FSConvert.displayToSim;
            AABB aabb;
            FSTransform xf;
            var didCollide = false;

            self.body.getTransform(out xf);
            xf.p += motion;

            self.shape.computeAABB(out aabb, ref xf, 0);
            var neighbors = ListPool<Fixture>.obtain();
            self.body.world.queryAABB(ref aabb, neighbors);

            if(neighbors.Count > 1)
            {
                for(var i = 0; i < neighbors.Count; i++)
                {
                    if (neighbors[i].fixtureId == self.fixtureId)
                        continue;
                    if (neighbors[i].collisionCategories != CollisionSetting.tiledHoleCategory)
                        continue;

                    if(collideWithHole(self, ref motion, neighbors[i], out result))
                    {
                        xf.p += result.minimumTranslationVector;
                    }

                }
            }

            ListPool<Fixture>.free(neighbors);
            motion *= FSConvert.simToDisplay;

            return didCollide;
        }

        private bool collideWithHole(Fixture fixtureA,ref Vector2 motion,Fixture fixtureB,out FSCollisionResult result)
        {
            FSTransform transformA;
            fixtureA.body.getTransform(out transformA);
            transformA.p += motion;

            FSTransform transformB;
            fixtureB.body.getTransform(out transformB);

            if (collideWithHole(fixtureA, ref transformA, fixtureB, ref transformB, out result))
            {
                motion += result.minimumTranslationVector;
                return true;
            }
            return false;
        }

        private bool collideWithHole(Fixture fixtureA,ref FSTransform transformA, Fixture fixtureB,ref FSTransform transformB, out FSCollisionResult result)
        {
            result = new FSCollisionResult();
            result.fixture = fixtureB;

            if (!ContactManager.shouldCollide(fixtureA, fixtureB))
                return false;

            if (fixtureA.body.world.contactManager.onContactFilter != null && !fixtureA.body.world.contactManager.onContactFilter(fixtureA, fixtureB))
                return false;

            return collidePolygonCircle(fixtureB.shape as PolygonShape, ref transformB, fixtureA.shape as CircleShape, ref transformA, out result);

            
        }

        bool collidePolygonCircle(PolygonShape polygon, ref FSTransform polyTransform, CircleShape circle, ref FSTransform circleTransform, out FSCollisionResult result)
        {
            result = new FSCollisionResult();
            Collision.collidePolygonAndCircle(ref _manifold, polygon, ref polyTransform, circle, ref circleTransform);
            if (_manifold.pointCount > 0)
            {
                FixedArray2<Vector2> points;
                ContactSolver.WorldManifold.initialize(ref _manifold, ref polyTransform, polygon.radius, ref circleTransform, circle.radius, out result.normal, out points);

                //var circleInPolySpace = polygonFixture.Body.GetLocalPoint( circleFixture.Body.Position );
                var circleInPolySpace = MathUtils.mulT(ref polyTransform, circleTransform.p);
                var value1 = circleInPolySpace - _manifold.localPoint;
                var value2 = _manifold.localNormal;
                var separation = circle.radius - (value1.X * value2.X + value1.Y * value2.Y);

                if (separation <= 0)
                    return false;

                result.point = points[0] * FSConvert.simToDisplay;
                result.minimumTranslationVector = result.normal * separation;

#if DEBUG_FSCOLLISIONS
				Debug.drawPixel( result.point, 2, Color.Red, 0.2f );
				Debug.drawLine( result.point, result.point + result.normal * 20, Color.Yellow, 0.2f );
#endif

                return true;
            }
            return false;
        }

        private Entity addArrow(Vector2 position)
        {
            var arrow = archer.scene.addEntity(new MoveAbleEntity()).setPosition(position);
            
            var texture = Core.content.Load<Texture2D>("Images/Players/Allplayer");
            var arrowTexture = new Subtexture(texture, new Rectangle(306, 2630 - 1191 - 16, 5, 16));
            arrow.addComponent(new Sprite(arrowTexture));

           
            arrow.addComponent<FSRigidBody>()
                .setBodyType(BodyType.Dynamic);
            var trigger = arrow.addComponent<SceneObjectTriggerComponentBox>();
            trigger.setSize(5, 16)
                .setIsSensor(true);

            trigger.setCollisionCategories(CollisionSetting.enemyAttackCategory);
            trigger.setCollidesWith(CollisionSetting.wallCategory
                | CollisionSetting.playerCategory
                | CollisionSetting.tiledObjectCategory
                );
            trigger.onAdded += () =>
            {


                trigger.GetFixture().onCollision += (fixtureA, fixtureB, contact) =>
                {
                    var component = fixtureB.userData as Component;

                    if (component == archer.collider)
                        return true;
                    var actorProperty = component.getComponent<ActorPropertyComponent>();
                    if (actorProperty != null)
                    {
                        actorProperty.executeDamage?.Invoke(AttackTypes.Physics, 5);
                        var adir = component.entity.position - arrow.position;
                        adir.Normalize();
                        actorProperty.entity.getComponent<FSRigidBody>().body.applyLinearImpulse(adir*FSConvert.displayToSim);
                    }
                    arrow.destroy();
                    return true;
                };
            };

            return arrow;
        }


    }
}

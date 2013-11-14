using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RunningGame.Role
{
    class Runner
    {
        Texture2D roleTexture;
        List<RoleRun> roleRun = new List<RoleRun>();
        RoleJum roleJum;
        private Rectangle[] runAnimations;
        private Rectangle jumpRect;
        private Vector2 jumpVector;
        private Vector2[] runVector;
        const double FRAME_DELAY = 50.2;
        double elapsedTime;
        bool playedOverOnce;
        double firstHight;
        double secondHight;
        private bool isMovingUp = true;
        bool secondIsMovingUp = true;
        bool isSecondMovingUp = true;
        Vector2 jumpSpeed = new Vector2(0, -2.5f);
        Vector2 downSpeed = new Vector2(0, -2.9f);
        public RoleStatus status = RoleStatus.running;
        private float theHeightJump;
        private Vector2 firstJumpMax;
        private Vector2 SecontJumpMax;

        public enum RoleStatus { 
            running =0,
            jumping = 1,
            secondJumping = 2,
            downMoving = 3,
        }

        public void setSecondJumpStatus()
        {
            status = RoleStatus.secondJumping;
            theHeightJump = roleJum.position.Y - Program.jumpHeight;
            secondIsMovingUp = true;
        }
        public Runner(Texture2D text, Rectangle[] runRect, Rectangle jumpRectangle)
        {
            roleTexture = text;
            runAnimations = runRect;
            jumpRect = jumpRectangle;

            for (int i = 0; i < runAnimations.Length; i++)
            {
                roleRun.Add(new RoleRun() { roleIndex = 0, ISRunning = true, position = new Vector2(200, 350) });
            }

            roleJum = new RoleJum() { index = 0, isJumping = true, position = new Vector2(200, 350) };
        }

        public Rectangle[] RunAnimationRect
        {
            get { return this.runAnimations; }
            set { this.runAnimations = value; }
        }

        public Vector2[] RunVectors
        {
            get { return this.runVector; }
            set { this.runVector = value; }
        }

        public Rectangle JumpRect
        {
            get { return this.jumpRect; }
            set { this.jumpRect = value; }
        }

        public Vector2 JumpVector
        {
            get { return this.jumpVector; }
            set { this.jumpVector = value; }
        }

        
        public void playJumpAnimation()
        { }

        public void playJumpSecondAnimation()
        { }

        public void playRunAnimation()
        { }

        public void Draw(SpriteBatch sb)
        {
            if (status == RoleStatus.running)
            {
                foreach (var r in roleRun)
                {
                    if (r.ISRunning)
                        sb.Draw(roleTexture, r.position, runAnimations[r.roleIndex], Color.White);
                }
            }
            else if (status == RoleStatus.jumping)
            {
               if(roleJum.isJumping)
               {
                   sb.Draw(roleTexture, roleJum.position, jumpRect, Color.White);
                }
            }
            else if (status == RoleStatus.downMoving)
            {
            }
            else // second jump
            {
                if (roleJum.isJumping)
                {
                    sb.Draw(roleTexture, roleJum.position, jumpRect, Color.White);
                }
            }
        }
        public void Update(GameTime gt)
        {
            elapsedTime += gt.ElapsedGameTime.TotalMilliseconds;
            if (status == RoleStatus.jumping)
            {
                if (isMovingUp)
                {
                    if (roleJum.position.Y >= 280)
                    {
                        roleJum.position += jumpSpeed;
                    }
                    else { 
                               isMovingUp = false; 
                    }
                }
                else
                {
                    if (roleJum.position.Y <= 350)
                    {
                        roleJum.position -= downSpeed;
                        if (roleJum.position.Y >= 350)
                        {
                             status = RoleStatus.running;
                            isMovingUp = true;
                        }
                    }
                }
            }
            if(status == RoleStatus.secondJumping)
            {
                if (secondIsMovingUp)
                {
                    isMovingUp = true;
                    if (roleJum.position.Y >= theHeightJump)
                    {
                        roleJum.position += jumpSpeed;
                    }
                    else { secondIsMovingUp = false; }
                }
                else
                {
                    if (roleJum.position.Y <= 350)
                    {
                        roleJum.position -= downSpeed;
                        if (roleJum.position.Y >= 350)
                        {
                            status = RoleStatus.running;
                            secondIsMovingUp = true;
                        }
                    }
                }

            }

            if (elapsedTime > FRAME_DELAY)
            {
                elapsedTime = 0;
                if (status == RoleStatus.running)
                {
                    foreach (var exp in roleRun)
                    {
                        if (exp.ISRunning)
                        {
                            exp.roleIndex++;
                            if (exp.roleIndex == runAnimations.Length)
                            {
                                exp.roleIndex = 0;
                            }
                        }
                    }
                }
                else if (status == RoleStatus.jumping)
                {
                }
                else if (status == RoleStatus.downMoving)
                {
                }
                else // seconde jump
                {
 
                }
            }
        }
    }

    public class RoleRun
    {
        public bool ISRunning;
        public Vector2 position;
        public int roleIndex;
    }

    public class RoleJum
    {
        public bool isJumping;
        public Vector2 position;
        public int index;
    }
}

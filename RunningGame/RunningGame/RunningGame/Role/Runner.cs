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
        const double FRAME_DELAY = 40.2;
        double elapsedTime;
        bool playedOverOnce;

        public enum RoleStatus { 
            running =0,
            jumping = 1,
            secondJumping = 2,
            downMoving = 3,
        }

        public Runner(Texture2D text, Rectangle[] runRect, Rectangle jumpRectangle)
        {
            roleTexture = text;
            runAnimations = runRect;
            jumpRect = jumpRectangle;

            for (int i = 0; i < runAnimations.Length; i++)
            {
                roleRun.Add(new RoleRun() { inedex = i, ISRunning = true, position = new Vector2(200, 300) });
            }

            roleJum = new RoleJum() { index = 0, isJumping = false, position = new Vector2(200, 300) };
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
            foreach (var r in roleRun)
            {
                if(r.ISRunning)
                    sb.Draw(roleTexture, r.position, runAnimations[r.inedex], Color.White);
            }

            if (roleJum.isJumping)
            {
                //play a role jumping
            }
        }
        public void Update(GameTime gt)
        {
            elapsedTime += gt.ElapsedGameTime.TotalMilliseconds;
            if (elapsedTime > FRAME_DELAY)
            {
                elapsedTime = 0;

                foreach (var exp in roleRun)
                {
                    if (exp.ISRunning)
                    {

                        if (exp.inedex < roleRun.Count - 1)
                        {
                            exp.inedex++;
                            exp.ISRunning = false;
                        }
                        else
                        {
                            exp.inedex = 0;
                            playedOverOnce = true;
                            exp.ISRunning = false;
                        }
                    }
                }

                if (playedOverOnce)
                {
                    foreach (var exp in roleRun)
                    { exp.ISRunning = true; }
                }
            }
        }
    }

    public class RoleRun
    {
        public bool ISRunning;
        public Vector2 position;
        public int inedex;
    }

    public class RoleJum
    {
        public bool isJumping;
        public Vector2 position;
        public int index;
    }
}

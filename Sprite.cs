using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Direct2D1;

namespace purpleplatypus
{


    public class Sprite : Object
    {

        private int squareSize = 32;
        public int SquareSize
        {
            get
            {
                return squareSize;
            }
            set
            {
                squareSize = value;
                numberOfFrames = TextureSheet.Height / SquareSize;
                NumberOfStates = TextureSheet.Width / SquareSize;
                CollisionBox = new Rectangle(-SquareSize / 2, -SquareSize / 2, SquareSize / 2, SquareSize / 2);
            }
        }


        public Texture2D TextureSheet; //Organized into 32x32 sections which represent different frames of different animations.
        public String ImageName = "none";
        private int drawCounter; //Increments each time the sprite is drawn.
        private int numberOfFrames; //Calculated according to the height of the TextureSheet.
        public int NumberOfStates; //Calculated according to the width of the TextureSheet.
        public int AnimationSpeed = 20; //How many game frames per animation frame.
        private int animationState = 0; //The column on the TextureSheet which is also a type of animation like standing, running, waving.
        public int AnimationState //This limits animationState to existant states.
        {
            get
            {
                return animationState;
            }
            set
            {
                animationState = value % (NumberOfStates);
            }
        }
        private int animationFrame = 0; //
        public int AnimationFrame
        {
            get
            {


                drawCounter++;
                if (drawCounter % AnimationSpeed == 0) animationFrame++;
                if (animationFrame == numberOfFrames)
                {
                    animationFrame = 0;
                }

                return animationFrame;
            }
            set
            {
                animationFrame = value;
            }

        }
        private Vector2 gameCoordinates = new Vector2(0, 0);
        public Vector2 GameCoordinates;
        //{
        //    get
        //    {
        //        return gameCoordinates;
        //    }
        //    set
        //    {
        //        if (value.X < 0) gameCoordinates = new Vector2(0, value.Y);
        //        if (value.Y < 0) gameCoordinates = new Vector2(value.X, 0);
        //    }
        //}
        public Color Color = Color.White;
        private float rotation = 0F;
        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {

                rotation = (float)value % (float)(Math.PI * 2);
                if (rotation > Math.PI) rotation = rotation - 2 * (float)Math.PI;
                if (rotation < -Math.PI) rotation = rotation + 2 * (float)Math.PI;
            }
        }

        public Rectangle CollisionBox = new Rectangle(0, 0, 0, 0);

        public Sprite(Texture2D texture)
        {
            this.TextureSheet = texture;
            numberOfFrames = TextureSheet.Height / SquareSize;
            NumberOfStates = TextureSheet.Width / SquareSize;
            CollisionBox = new Rectangle(-SquareSize / 2, -SquareSize / 2, SquareSize / 2, SquareSize / 2);

        }

        public bool IsPointInMe(Vector2 point)
        {
            //Rectangle SpriteACollisionArea = new Rectangle
            //    (
            //    (int)SpriteA.GameCoordinates.X + SpriteA.CollisionBox.Left,
            //    (int)SpriteA.GameCoordinates.Y + SpriteA.CollisionBox.Top,
            //    (int)SpriteA.GameCoordinates.X + SpriteA.CollisionBox.Right,
            //    (int)SpriteA.GameCoordinates.Y + SpriteA.CollisionBox.Bottom
            //    );
            Rectangle spriteCollisionArea = new Rectangle
                (
                (int)this.GameCoordinates.X + this.CollisionBox.Left,
                (int)this.GameCoordinates.Y + this.CollisionBox.Top,
                (int)this.GameCoordinates.X + this.CollisionBox.Right,
                (int)this.GameCoordinates.Y + this.CollisionBox.Bottom
                );

            //Vector2[] spriteAEdges = new Vector2[4];
            //spriteAEdges[0] = new Vector2(SpriteACollisionArea.Left, SpriteACollisionArea.Top);
            //spriteAEdges[1] = new Vector2(SpriteACollisionArea.Right, SpriteACollisionArea.Top);
            //spriteAEdges[2] = new Vector2(SpriteACollisionArea.Left, SpriteACollisionArea.Bottom);
            //spriteAEdges[3] = new Vector2(SpriteACollisionArea.Right, SpriteACollisionArea.Bottom);

            //Vector2[] spriteBEdges = new Vector2[4];
            //spriteBEdges[0] = new Vector2(SpriteBCollisionArea.Top, SpriteBCollisionArea.Left);
            //spriteBEdges[1] = new Vector2(SpriteBCollisionArea.Top, SpriteBCollisionArea.Right);
            //spriteBEdges[2] = new Vector2(SpriteBCollisionArea.Bottom, SpriteBCollisionArea.Left);
            //spriteBEdges[3] = new Vector2(SpriteBCollisionArea.Bottom, SpriteBCollisionArea.Right);

            //foreach (Vector2 edgeA in spriteBEdges)
            //{
            if (point.X > spriteCollisionArea.Left &&
                point.X < spriteCollisionArea.Right &&
                point.Y > spriteCollisionArea.Top &&
                point.Y < spriteCollisionArea.Bottom)
                return true;
            //}


            return false;
        }


    }
}
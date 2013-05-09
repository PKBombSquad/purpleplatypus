using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.RawInput;
using SharpDX.Multimedia;

namespace purpleplatypus
{
    class EmeClient : Game
    {
        private string contentPath = "";//Path.GetDirectoryName(Assembly.GetExecutingAssembly().FullName);//"C:/purpleplatypus/content/";

        //General game variables
        private Texture2D pointerTexture;
        private GraphicsDeviceManager myGraphicsDeviceManager;
        private int multiSampleMask = 1;
        private GameTime gameTime = new GameTime();
        private SpriteBatch mySpriteBatch;
        private SpriteFont myFont;
        private SharpDX.Direct3D11.BlendStateDescription blendStateDescription = SharpDX.Direct3D11.BlendStateDescription.Default();
        private BlendState blendState;
        private List<Sprite> spriteList = new List<Sprite>(); //the buildASprite method will fill this up with sprites

        private Vector2 mousePos = Vector2.Zero;
        private Vector2 mouseGamePos = Vector2.Zero;

        private Camera camera = new Camera();


        public EmeClient()
        {
            myGraphicsDeviceManager = new GraphicsDeviceManager(this);

            Device.RegisterDevice(UsagePage.Generic, UsageId.GenericKeyboard, DeviceFlags.None);
            Device.KeyboardInput += (sender, args) => keyboardEvent(args);

            Device.RegisterDevice(UsagePage.Generic, UsageId.GenericMouse, DeviceFlags.None);
            Device.MouseInput += (sender, args) => mouseEvent(args);

            this.Window.AllowUserResizing = true;









        }

        protected override void LoadContent()
        {
            myFont = Content.Load<SpriteFont>(contentPath + "Arial16.tkfnt");

            pointerTexture = Content.Load<Texture2D>(contentPath + "pointer.png");

            mySpriteBatch = new SpriteBatch(GraphicsDevice);

            blendStateDescription.AlphaToCoverageEnable = true;
            blendState = BlendState.New(GraphicsDevice, blendStateDescription, Color.White, multiSampleMask);

            base.LoadContent();
        }

        public void mouseEvent(MouseInputEventArgs args)
        {

        }
        public void keyboardEvent(KeyboardInputEventArgs args)
        {
            if (args.Key == System.Windows.Forms.Keys.Escape) this.Exit();
            camera.keyboardNavigationChecks(args);
        }



        public Sprite buildASprite(string texturePath)
        {
            string fullTexturePath = contentPath + texturePath;
            Texture2D texture;
            texture = Content.Load<Texture2D>(fullTexturePath);
            Sprite builtSprite = new Sprite(texture);
            spriteList.Add(builtSprite);


            //if (texturePath == "demo.png") //set SquareSize and CollisionBox on a per-sprite basis
            //{
            //    builtSprite.SquareSize = 45;
            //    int SquareSize = builtSprite.SquareSize;
            //    builtSprite.CollisionBox = new Rectangle(-SquareSize / 5, -SquareSize / 5, SquareSize / 5, SquareSize / 5);
            //}

            builtSprite.ImageName = texturePath;

            return spriteList[spriteList.Count - 1];

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);

            Sprite obnoctoMan = buildASprite("stick man.png");
            obnoctoMan.GameCoordinates = new Vector2(gameTime.FrameCount, gameTime.FrameCount);

            //mySpriteBatch.Begin(SpriteSortMode.Deferred, blendState);
            foreach (var sprite in spriteList)
            {/////////////////DRAW ALL THE SPRITES/////////////////
                DrawSprite(sprite);
            }
            //mySpriteBatch.End();

            camera.UpdateCamera();




            if (System.Windows.Forms.Form.ActiveForm != null)
            { ////////////////DRAW THE CURSOR/////////////////////
                mousePos.X = SharpDX.Windows.RenderForm.MousePosition.X - System.Windows.Forms.Form.ActiveForm.Left - 8; //The 8 is because the little thing surrounding the window messes this up
                mousePos.Y = SharpDX.Windows.RenderForm.MousePosition.Y - System.Windows.Forms.Form.ActiveForm.Top - 30; //Same for this 30
                mySpriteBatch.Begin(SpriteSortMode.Deferred, blendState);
                mySpriteBatch.Draw(pointerTexture, mousePos, Color.White);
                mySpriteBatch.End();
            }

            base.Draw(gameTime);
        }

        public void DrawSprite(Sprite sprite)
        {
            mySpriteBatch.Begin(SpriteSortMode.Deferred, blendState);
            int top = sprite.AnimationState * sprite.SquareSize; //top, left, bottom, and right of the source rectangle from the texture sheet.
            int left = sprite.AnimationFrame * sprite.SquareSize; //the rows are frames and columns are states.
            int bottom = top + sprite.SquareSize;
            int right = left + sprite.SquareSize;
            Vector2 superScreenCoordinates = new Vector2
                (
                //convert sprite.GameCoordinates to superScreenCoordinates using isometric transformation.  Or not.
                    sprite.GameCoordinates.X,
                    sprite.GameCoordinates.Y
                );
            Vector2 cameraCoordinates = new Vector2
                (//convert superscreen coordinates to camera coordinates (actual display coordinates) by substracting the Camera position.
                superScreenCoordinates.X - camera.CameraX,
                superScreenCoordinates.Y - camera.CameraY
                );
            //mySpriteBatch.Draw(sprite.TextureSheet, cameraCoordinates, new Rectangle(top, left, bottom, right), sprite.Color);
            mySpriteBatch.Draw(
                sprite.TextureSheet,
                cameraCoordinates,
                new Rectangle(top, left, bottom, right),
                sprite.Color,
                sprite.Rotation,
                new Vector2(sprite.SquareSize / 2, sprite.SquareSize / 2),
                1F,
                SpriteEffects.None,
                0F);
            mySpriteBatch.End();
        }





        public void WriteXML(Object objectToSerialize, String filename)
        {
            Type type = objectToSerialize.GetType();
            //String typeName = type.ToString();
            //String filename = objectToSerialize.ToString();
            String filePath = contentPath + filename;

            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(type);

            System.IO.StreamWriter file = new System.IO.StreamWriter(filePath);
            writer.Serialize(file, objectToSerialize);
            file.Close();
        }

        public Object ReadXML(String filepath, Type objectType)
        {
            //String filepath = contentPath + filename;
            System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(objectType);
            System.IO.StreamReader file = new System.IO.StreamReader(filepath);
            //var readObject = Activator.CreateInstance(objectType);
            var readObject = reader.Deserialize(file);
            Console.WriteLine("I read a file named " + filepath + " and now I'm returning a " + readObject.ToString());
            return readObject;
        }
    }
}
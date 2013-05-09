using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.RawInput;

namespace purpleplatypus
{
    class Camera
    {
        private int levelWidth = 900;
        private int levelHeight = 900;

        private int cameraSensitivity = 2;
        private bool cameraGoingDown = false;
        private bool cameraGoingRight = false;
        private bool cameraGoingUp = false;
        private bool cameraGoingLeft = false;
        private int cameraX = 0;
        private int cameraY = 0;
        public int CameraX
        {
            get
            {
                return cameraX;
            }
            set
            {
                cameraX = value;
                int minX = 0;
                if (cameraX < minX) cameraX = minX;
                int maxX = levelWidth - SharpDX.Windows.RenderForm.ActiveForm.Width;
                if (cameraX > maxX) cameraX = maxX;
            }
        }
        public int CameraY
        {
            get
            {
                return cameraY;
            }
            set
            {
                cameraY = value;
                int minY = 0;
                if (cameraY < minY) cameraY = minY;
                int maxY = levelHeight - SharpDX.Windows.RenderForm.ActiveForm.Height;
                if (cameraY > maxY) cameraY = maxY;
            }
        }

        public Camera()
        {
        }

        public void keyboardNavigationChecks(KeyboardInputEventArgs args)
        {
            if (args.State == KeyState.KeyDown)
            {
                switch (args.Key)
                {
                    case System.Windows.Forms.Keys.W:
                        cameraGoingUp = true;
                        break;
                    case System.Windows.Forms.Keys.A:
                        cameraGoingLeft = true;
                        break;
                    case System.Windows.Forms.Keys.S:
                        cameraGoingDown = true;
                        break;
                    case System.Windows.Forms.Keys.D:
                        cameraGoingRight = true;
                        break;
                    default:
                        break;
                }
            }
            if (args.State == KeyState.KeyUp)
            {
                switch (args.Key)
                {
                    case System.Windows.Forms.Keys.W:
                        cameraGoingUp = false;
                        break;
                    case System.Windows.Forms.Keys.A:
                        cameraGoingLeft = false;
                        break;
                    case System.Windows.Forms.Keys.S:
                        cameraGoingDown = false;
                        break;
                    case System.Windows.Forms.Keys.D:
                        cameraGoingRight = false;
                        break;
                    default:
                        break;
                }
            }
        }

        public void UpdateCamera()
        {
            if (cameraGoingRight)
            {
                CameraX += cameraSensitivity;
            }
            if (cameraGoingLeft)
            {
                CameraX -= cameraSensitivity;
            }
            if (cameraGoingDown)
            {
                CameraY += cameraSensitivity;
            }
            if (cameraGoingUp)
            {
                CameraY -= cameraSensitivity;
            }
        }
    }
}

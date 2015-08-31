using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using GlueEngine.World;
using Mogre.PhysX;
using GlueEngine.World.Lights;
using GlueEngine.World.Entities;
using System.Runtime.InteropServices;

namespace GlueEngine.Core
{
    public static class Engine
    {
        private static PhysicsManager physics = new PhysicsManager();
        private static GraphicsManager graphics = new GraphicsManager();
        private static SoundManager sound = new SoundManager();
        private static InputManager input = new InputManager();
        private static WorldManager world = new WorldManager();
        private static IGameState gameState;
        private static int id = 0;

        public static WorldManager World
        {
            get
            {
                return world;
            }
        }

        public static GraphicsManager Graphics
        {
            get
            {
                return graphics;
            }
        }

        public static PhysicsManager Physics
        {
            get
            {
                return physics;
            }
        }

        public static SoundManager Sound
        {
            get
            {
                return sound;
            }
        }

        public static InputManager Input
        {
            get
            {
                return input;
            }
        }

        public static bool Initialise(IntPtr windowHandle, IGameState gameState)
        {
            if (!graphics.Initiliase(windowHandle))
                throw new Exception("Unable to initialise graphics manager");

            if (!physics.Initiliase())
                throw new Exception("Unable to initialise physics manager");

            if(!sound.Initiliase())
                throw new Exception("Unable to initialise sound manager");

            if (!input.Initiliase(graphics.WindowHandle))
                throw new Exception("Unable to initialise input manager");

            if (!gameState.Initiliase())
                return false;

            Engine.gameState = gameState;
            return true;
        }

        internal static void ShutDown()
        {
            graphics.Dispose();
            physics.Dispose();
            sound.Dispose();
            input.Dispose();
        }

        public static bool Update(float deltaTime)
        {
            input.Update(deltaTime);

            if (!gameState.Update(deltaTime))
                return false;

            physics.Update(deltaTime);
            world.Update(deltaTime);
            return true;
        }

        public static void Start(IGameState gameState)
        {
            //try
            //{
                if (Engine.Initialise(IntPtr.Zero, gameState))
                    Engine.Graphics.StartRendering();

                Engine.ShutDown();
            //}
            //catch (SEHException ex)
            //{
            //    if (OgreException.IsThrown)
            //        throw new Exception(OgreException.LastException.FullDescription, ex);
            //}
        }

        public static float MaxAxis(Vector3 vector3)
        {
            float max = vector3.x;

            if (vector3.y > max)
                max = vector3.y;

            if (vector3.z > max)
                max = vector3.z;

            return max;
        }

        public static string UniqueName(string name)
        {
            id++;
            return name + id.ToString();
        }
    }
}

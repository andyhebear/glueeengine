using System;
using System.Collections.Generic;
using System.Text;
using GlueEngine.Core;
using GlueEngine.World;
using Mogre;
using GlueEngine.World.Entities;
using Mogre.PhysX;

namespace GlueGame
{
    public class GameMain : IGameState
    {
        private CharacterController characterController = new CharacterController();

        public bool Initiliase()
        {
            Engine.World.Load("test.world");

            this.characterController.Create(new Vector3(0, 2.5f, 0), Quaternion.IDENTITY);

            Engine.Input.Mouse.MousePressed += new MOIS.MouseListener.MousePressedHandler(Mouse_MousePressed);
            Engine.Input.Keyboard.KeyPressed += new MOIS.KeyListener.KeyPressedHandler(Keyboard_KeyPressed);

            return true;
        }

        private bool Keyboard_KeyPressed(MOIS.KeyEvent arg)
        {
            if (arg.key == MOIS.KeyCode.KC_SPACE)
                this.characterController.Jump();

            return true;
        }

        private bool Mouse_MousePressed(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
        {
            Camera camera = Engine.Graphics.Camera;

            DynamicEntity dynamicEntity = new DynamicEntity("Ball", "ball.mesh");

            dynamicEntity.Velocity = (camera.Direction.NormalisedCopy * 20f) + (Vector3.UNIT_Y * 5);
            dynamicEntity.Density = 1f;
            dynamicEntity.CollisionMode = CollisionMode.BoundingSphere;
            dynamicEntity.EnableCCD = true;
            dynamicEntity.CollisionSound = @"Media\sounds\thud.wav";
            dynamicEntity.Spawn(camera.Position + camera.Direction, camera.Orientation);

            return true;
        }

        private bool ProcessInput(float deltaTime)
        {
            InputManager input = Engine.Input;
            float speed = 8;
            float mouseSensitivity = 0.004f;
            Vector3 move = Vector3.ZERO;

            // rotate camera
            Camera camera = Engine.Graphics.Camera;
            camera.Yaw(new Radian(input.Mouse.MouseState.X.rel * -mouseSensitivity));
            camera.Pitch(new Radian(input.Mouse.MouseState.Y.rel * -mouseSensitivity));

            // test for quit signal
            if (input.Keyboard.IsKeyDown(MOIS.KeyCode.KC_ESCAPE))
                return false;

            // move player
            if (input.Keyboard.IsKeyDown(MOIS.KeyCode.KC_W))
                move += camera.Orientation * -Vector3.UNIT_Z;

            if (input.Keyboard.IsKeyDown(MOIS.KeyCode.KC_S))
                move += camera.Orientation * Vector3.UNIT_Z;

            if (input.Keyboard.IsKeyDown(MOIS.KeyCode.KC_A))
                move += camera.Orientation * -Vector3.UNIT_X;

            if (input.Keyboard.IsKeyDown(MOIS.KeyCode.KC_D))
                move += camera.Orientation * Vector3.UNIT_X;

            this.characterController.Velocity = move * speed;
            return true;
        }

        public bool Update(float deltaTime)
        {
            if (!this.ProcessInput(deltaTime))
                return false;

            this.characterController.Update(deltaTime);
            return true;
        }
    }
}

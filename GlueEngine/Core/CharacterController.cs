using System;
using System.Collections.Generic;
using System.Text;
using Mogre.PhysX;
using Mogre;

namespace GlueEngine.Core
{
    public class CharacterController : IUserControllerHitReport
    {        
        private Controller controller;
        private Vector3 velocity;
        private Camera camera;
        private CapsuleControllerDesc desc;
        private Vector3 jumpVelocity = Vector3.ZERO;

        public float Height
        {
            get
            {
                return desc.Height + (desc.Radius * 2);
            }
        }

        public float Width
        {
            get
            {
                return desc.Radius * 2;
            }
        }

        public Vector3 Velocity
        {
            get
            {
                return this.velocity;
            }
            set
            {
                this.velocity = value;
            }
        }

        public CharacterController()
        {
        }

        public bool Create(Vector3 position, Quaternion orientation)
        {
            desc = new CapsuleControllerDesc();
		    desc.Position = position;            
		    desc.Height = 1.2f;
		    desc.Radius = 0.8f;	
		    desc.SkinWidth = 0.125f;
		    desc.SlopeLimit = Mogre.Math.Cos(new Radian(new Degree(45)));
		    desc.StepOffset = 0.5f;
		    desc.UpDirection = HeightFieldAxes.Y;
		    desc.ClimbingMode = CapsuleClimbingModes.Constrained;
		    desc.Callback = this;

            this.camera = Engine.Graphics.Camera;
            this.camera.Orientation = orientation;

            controller = Engine.Physics.CreateCharacterController(desc);
            return true;
        }

        public void Update(float deltaTime)
        {
            // apply gravity and jumping
            this.velocity += Engine.Physics.Scene.Gravity + jumpVelocity;

            // move the controller
            ControllerFlags flags;
            controller.Move(velocity * deltaTime, uint.MaxValue, 0.001f, out flags);

            // do jumping stuff
            if ((flags & ControllerFlags.Sides) == ControllerFlags.Sides)
            {
                jumpVelocity.x = 0;
                jumpVelocity.z = 0;
            }

            // TODO: add raycast here
            if ((flags & ControllerFlags.Down) == ControllerFlags.Down)
                jumpVelocity = Vector3.ZERO;
            else
                jumpVelocity += Engine.Physics.Scene.Gravity * deltaTime;

            // position head
            camera.Position = controller.Actor.GlobalPosition + (Vector3.UNIT_Y * (this.Height / 2));
            Engine.Sound.SetListenerPosition(camera.Position, -camera.Direction);

        }

        public ControllerActions OnControllerHit(ControllersHit value)
        {
            return ControllerActions.None;
        }

        public ControllerActions OnShapeHit(ControllerShapeHit value)
        {
            return ControllerActions.Push;
        }

        public void Jump()
        {
            if(this.jumpVelocity == Vector3.ZERO)
                this.jumpVelocity = new Vector3(this.velocity.x * 0.5f, 16, this.velocity.z * 0.5f);
        }
    }
}

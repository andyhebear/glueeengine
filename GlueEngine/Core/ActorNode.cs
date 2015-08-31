﻿using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using Mogre.PhysX;

namespace GlueEngine.Core
{
    public class ActorNode
    {
        private SceneNode sceneNode;
        private Actor actor;

        public ActorNode(SceneNode sceneNode, Actor actor)
        {
            this.sceneNode = sceneNode;
            this.actor = actor;
        }

        public void Update(float deltaTime)
        {
            if (!actor.IsSleeping)
            {
                this.sceneNode.Position = actor.GlobalPosition;
                this.sceneNode.Orientation = actor.GlobalOrientationQuaternion;
            }
        }
    }
}

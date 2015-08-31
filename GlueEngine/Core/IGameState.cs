using System;
using System.Collections.Generic;
using System.Text;

namespace GlueEngine.Core
{
    public interface IGameState
    {
        bool Initiliase();
        bool Update(float deltaTime);
    }
}

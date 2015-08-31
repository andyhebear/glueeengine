using System;
using System.Collections.Generic;
using System.Text;
using IrrKlang;
using Mogre;

namespace GlueEngine.Core
{
    public class SoundManager : IDisposable
    {
        private ISoundEngine soundEngine;        

        public SoundManager()
        {
        }

        public bool Initiliase()
        {
            this.soundEngine = new ISoundEngine();
            this.soundEngine.SoundVolume = 1.0f;
            this.SetListenerPosition(Vector3.ZERO, Vector3.UNIT_Z); 
            return true;
        }

        public void Dispose()
        {
        }

        public void SetListenerPosition(Vector3 position, Vector3 direction)
        {
            soundEngine.SetListenerPosition(position.x, position.y, position.z, direction.x, direction.y, direction.z);
        }

        public ISound Play3D(string filename, Vector3 position, float volume)
        {
            if (filename != null && filename != "")
            {
                ISound sound = soundEngine.Play3D(filename, position.x, position.y, position.z);
                sound.Volume = volume;
                sound.MinDistance = 5;
                return sound;
            }

            return null;
        }
    }
}

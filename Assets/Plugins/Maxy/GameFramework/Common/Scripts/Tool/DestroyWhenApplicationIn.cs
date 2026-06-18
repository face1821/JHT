using System;
using UnityEngine;

namespace Maxy.GameFramework.Common.Tool
{
    public class DestroyWhenApplicationIn : MonoBehaviour
    {
        [Flags]
        public enum ApplicationEnviroment
        {
            Editor,

            Window,
            Linux,
            MacOS,

            Android,
            IPhone,

            WebGL,
        }

        public ApplicationEnviroment WhenIn;

        private void Awake()
        {
            bool toDestroy = false;

            if (WhenIn.HasFlag(ApplicationEnviroment.Editor) && Application.isEditor)
            {
                toDestroy = true;
            }
            else if (WhenIn.HasFlag(ApplicationEnviroment.Window))
            {
                if (Application.platform is RuntimePlatform.WindowsPlayer or RuntimePlatform.IPhonePlayer or RuntimePlatform.LinuxPlayer or RuntimePlatform.WebGLPlayer)
                    toDestroy = true;
            }
            else if (WhenIn.HasFlag(ApplicationEnviroment.Linux))
            {
                if (Application.platform is RuntimePlatform.LinuxPlayer)
                    toDestroy = true;
            }
            else if (WhenIn.HasFlag(ApplicationEnviroment.MacOS))
            {
                if (Application.platform == RuntimePlatform.OSXPlayer)
                    toDestroy = true;
            }
            else if (WhenIn.HasFlag(ApplicationEnviroment.Android) && Application.platform == RuntimePlatform.Android)
            {
                toDestroy = true;
            }
            else if (WhenIn.HasFlag(ApplicationEnviroment.IPhone) && Application.platform == RuntimePlatform.IPhonePlayer)
            {
                toDestroy = true;
            }
            else if (WhenIn.HasFlag(ApplicationEnviroment.WebGL) && Application.platform == RuntimePlatform.WebGLPlayer)
            {
                toDestroy = true;
            }

            if (toDestroy)
                Destroy(gameObject);
        }
    }
}
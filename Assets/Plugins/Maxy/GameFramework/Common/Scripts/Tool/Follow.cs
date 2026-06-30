using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Maxy.GameFramework.Common.Tool
{
    public class Follow : MonoBehaviour
    {
        public Transform Target;
        public Vector3 Offset;
        public bool SmoothFollow;
        [EnableIf("SmoothFollow")]
        public float InterpolateRate = 0.5f;

        [Space]
        public bool SyncRotation;
        [EnableIf("SyncRotation")]
        public bool RotationSmoothFollow;
        [EnableIf("RotationSmoothFollow")]
        public float RotationInterpolateRate = 0.5f;

        private void LateUpdate()
        {
            if (Target == null) return;


            if (SyncRotation)
            {
                if (RotationSmoothFollow && Mathf.Abs(transform.eulerAngles.z - Target.eulerAngles.z) > 0.1f)
                {
                    // 帧率无关的指数衰减
                    // Rate 越大，跟随越紧。Rate = 1 时几乎每帧到达
                    float t = 1 - Mathf.Exp(-InterpolateRate * Time.deltaTime * 60f);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Target.rotation, t);
                }
                else
                {
                    transform.rotation = Target.rotation;
                }
            }

            if (SmoothFollow && Vector3.Distance(transform.position, Offset + Target.position) > 0.1f)
            {
                // 帧率无关的指数衰减
                // Rate 越大，跟随越紧。Rate = 1 时几乎每帧到达
                float t = 1 - Mathf.Exp(-InterpolateRate * Time.deltaTime * 60f);
                transform.position = Vector3.Lerp(
                    transform.position,
                    Offset + Target.position,
                    t
                );
            }
            else
            {
                transform.position = Offset + Target.position;
            }
        }
    }
}
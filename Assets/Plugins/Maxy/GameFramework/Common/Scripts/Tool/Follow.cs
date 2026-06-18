using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Maxy.GameFramework.Common.Tool
{
    public class Follow : MonoBehaviour
    {
        public Transform Target;
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
                if (RotationSmoothFollow)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Target.rotation, RotationInterpolateRate);
                }
                else
                {
                    transform.rotation = Target.rotation;
                }
            }

            if (SmoothFollow)
            {
                transform.position = Vector3.Lerp(transform.position, Target.position, InterpolateRate);
            }
            else
            {
                transform.position = Target.position;
            }
        }
    }
}
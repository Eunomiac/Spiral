using System.Runtime.InteropServices;
using UnityEngine;

public class RotationTween : MonoBehaviour
{

    public struct Rotation
    {
        public Vector4 speedVec;
        public float speed;
        private RotationQV rotQV;

        [StructLayout(LayoutKind.Explicit)]
        struct RotationQV
        {
            [FieldOffset(0)]
            public Vector4 dirVec;
            [FieldOffset(0)]
            public Quaternion dirQuat;
        }

        public Quaternion rotation
        {
            get { return rotQV.dirQuat; }
            set { rotQV.dirQuat = value; }
        }

        public Rotation (Quaternion rotation, float speed)
        {
            rotQV.dirVec = Vector4.zero;
            rotQV.dirQuat = rotation;
            speedVec = Vector4.zero;
            this.speed = speed;
        }

        public void Step (Quaternion target)
        {
            float dTime = Time.deltaTime;
            Vector4 targetSpeed = new Vector4(target.x, target.y, target.z, target.w);
            if ( Vector4.Dot(rotQV.dirVec, targetSpeed) < 0 )
                targetSpeed = -targetSpeed;
            float denomRoot = speed * dTime + 1;
            speedVec = (speedVec - (rotQV.dirVec - targetSpeed) * (speed * speed * dTime)) / (denomRoot * denomRoot);
            rotQV.dirVec = (rotQV.dirVec + speedVec * dTime).normalized;
        }

        public static implicit operator Quaternion (Rotation r)
        {
            return r.rotation;
        }
    }
}

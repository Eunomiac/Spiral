using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 1f;

    private RotationTween.Rotation rotation = new RotationTween.Rotation(Quaternion.identity, 1);

    public float TargetAngle { get; set; }

    public void rotate (float? angle = null, float? speed = null)
    {
        rotation.speed = speed ?? rotationSpeed;
        TargetAngle = angle ?? TargetAngle;
        Quaternion newRotation = Quaternion.Euler(0f, TargetAngle, 0f);
        rotation.Step(newRotation);
        transform.rotation = rotation;
    }
}

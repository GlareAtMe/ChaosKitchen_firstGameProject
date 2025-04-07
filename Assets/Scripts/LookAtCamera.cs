using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
        LookAtOnlyY,
        LookAtOnlyYInverted
    }

    [SerializeField] private Mode mode;

    private void LateUpdate() {

        switch (mode) { 
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(dirFromCamera + transform.position);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
            case Mode.LookAtOnlyY:
                Vector3 lookPos = Camera.main.transform.position;
                lookPos.y = transform.position.y; // ігнор Y-нахилу
                transform.LookAt(lookPos);
                break;
            case Mode.LookAtOnlyYInverted:
                lookPos = Camera.main.transform.position;
                lookPos.y = transform.position.y; // ігноруємо нахил камери
                transform.LookAt(lookPos);
                transform.Rotate(0f, 180f, 0f); // дзеркальне відображення
                break;

        }
    }
}

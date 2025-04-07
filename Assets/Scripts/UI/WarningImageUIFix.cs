using UnityEngine;

[ExecuteAlways] // Працює навіть у редакторі
public class WarningImageUIFix : MonoBehaviour
{
    [Tooltip("Зміщення по локальній Y-осі (наприклад, 0.2f для підняття вище)")]
    [SerializeField] private float yOffset = 0.2f;

    private RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        ApplyOffset();
    }

#if UNITY_EDITOR
    private void OnValidate() {
        ApplyOffset(); // Автоматично оновлюється в редакторі
    }
#endif

    private void ApplyOffset() {
        if (rectTransform == null) return;

        Vector3 pos = rectTransform.localPosition;
        pos.y = yOffset;
        rectTransform.localPosition = pos;
    }
}

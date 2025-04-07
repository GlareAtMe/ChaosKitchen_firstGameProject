using UnityEngine;

[ExecuteAlways] // ������ ����� � ��������
public class WarningImageUIFix : MonoBehaviour
{
    [Tooltip("������� �� �������� Y-�� (���������, 0.2f ��� ������� ����)")]
    [SerializeField] private float yOffset = 0.2f;

    private RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        ApplyOffset();
    }

#if UNITY_EDITOR
    private void OnValidate() {
        ApplyOffset(); // ����������� ����������� � ��������
    }
#endif

    private void ApplyOffset() {
        if (rectTransform == null) return;

        Vector3 pos = rectTransform.localPosition;
        pos.y = yOffset;
        rectTransform.localPosition = pos;
    }
}

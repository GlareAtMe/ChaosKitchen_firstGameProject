using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterVolumeManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;

    private const string VOLUME_PARAM = "MasterVolume";

    private void Start() {
        // �������� ��������� ������� ��� ������������� 1f, ���� ���� �������
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_PARAM, 1f);
        masterVolumeSlider.value = savedVolume;

        // ������ ������� ��� �������� (�� ���������� ������)
        masterVolumeSlider.onValueChanged.AddListener(SetVolume);

        // ������ ����������� ��������� ��������
        SetVolume(savedVolume);
    }

    public void SetVolume(float volumeNormalized) {
        // ������������ �������� �������� � �������� 
        float volumeDb = Mathf.Log10(Mathf.Clamp(volumeNormalized, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(VOLUME_PARAM, volumeDb); // ����������� �� AudioMixer

     
        PlayerPrefs.SetFloat(VOLUME_PARAM, volumeNormalized);
        PlayerPrefs.Save();
    }
}

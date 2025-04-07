using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;

    private const string VolumePrefKey = "Volume";

    private void Start() {
        // ����������� ��������� �������
        if (PlayerPrefs.HasKey(VolumePrefKey)) {
            float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey);
            volumeSlider.value = savedVolume;
            SetVolume(savedVolume);
        } else {
            volumeSlider.value = 1f;
            SetVolume(1f);
        }

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume) {
        // ���������� � ����������� ����� (Unity ������ � ����������)
        float volumeDb = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("MusicVolume", volumeDb);

        // �������� ��������
        PlayerPrefs.SetFloat(VolumePrefKey, volume);
        PlayerPrefs.Save();
    }
}
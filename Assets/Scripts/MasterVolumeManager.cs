using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterVolumeManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;

    private const string VOLUME_PARAM = "MasterVolume";

    private void Start() {
        // Отримуємо збережену гучність або використовуємо 1f, якщо ключ відсутній
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_PARAM, 1f);
        masterVolumeSlider.value = savedVolume;

        // Додаємо слухача для слайдера (це відбудеться завжди)
        masterVolumeSlider.onValueChanged.AddListener(SetVolume);

        // Одразу застосовуємо збережене значення
        SetVolume(savedVolume);
    }

    public void SetVolume(float volumeNormalized) {
        // Перетворюємо значення слайдера в децибели 
        float volumeDb = Mathf.Log10(Mathf.Clamp(volumeNormalized, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(VOLUME_PARAM, volumeDb); // Застосовуємо до AudioMixer

     
        PlayerPrefs.SetFloat(VOLUME_PARAM, volumeNormalized);
        PlayerPrefs.Save();
    }
}

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundClipsSO soundsClipsSO;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private SoundManager soundManager;

    private const string VOLUME_PARAM = "SFXVolume";


    private void Awake() {
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_PARAM, 1f);
       SetVolume(savedVolume);
    }

    private void Start() {
        // Підписуємося на події для відтворення звуків
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.OnAnyPickedSomething += Player_OnPickupKitchenObject;
        BaseCounter.OnPlasedKitchenObject += BaseCounter_OnPlasedKitchenObject;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;

        // Якщо слайдер є, прив'язуємо його до функції SetSFXVolume
        if (sfxSlider != null) {
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);

            float savedVolume = PlayerPrefs.GetFloat(VOLUME_PARAM, 1f);
            sfxSlider.value = savedVolume;
        }
    }


    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        GameObject soundGameObject = new GameObject("SFX Sound");
        soundGameObject.transform.position = position;

        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.outputAudioMixerGroup = sfxMixerGroup;
        audioSource.Play();

        Destroy(soundGameObject, audioClip.length); // Clean up after playing
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }


    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e) {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(soundsClipsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnPlasedKitchenObject(object sender, System.EventArgs e) {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(soundsClipsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickupKitchenObject(object sender, System.EventArgs e) {
        Player player = sender as Player;
        PlaySound(soundsClipsSO.objectPickup, player.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e) {
        CuttingCounter cuttingCounter = sender as CuttingCounter; 
        PlaySound(soundsClipsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e) {
        
        PlaySound(soundsClipsSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e) {
        PlaySound(soundsClipsSO.deliveryFail, DeliveryCounter.Instance.transform.position);
    }

    public void FootStepSound(Vector3 position, float volume) {
        PlaySound(soundsClipsSO.footSteps, position, volume);
    }

    public void PlayCountdownSound() {
        PlaySound(soundsClipsSO.warning, Vector3.zero);
    }
    public void PlayWarningSound(Vector3 position) {
        PlaySound(soundsClipsSO.warning, position);
    }

    public void SetVolume(float volumeNormalized) {
        // volumeNormalized — значення від 0 до 1
        float volumeDb = Mathf.Log10(Mathf.Clamp(volumeNormalized, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(VOLUME_PARAM, volumeDb);

        // Якщо хочеш зберігати:
        PlayerPrefs.SetFloat(VOLUME_PARAM, volumeNormalized);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume) {
        float volumeDb = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(VOLUME_PARAM, volumeDb);
        PlayerPrefs.SetFloat(VOLUME_PARAM, volume);
        PlayerPrefs.Save();
    }

    public float GetSFXVolume() {
        return PlayerPrefs.GetFloat(VOLUME_PARAM, 1f);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;

    private Player player;

    private float footStepTimer;
    private float footSteptimerMax = .1f;

    private void Awake() {
        player = gameObject.GetComponent<Player>();
    }

    private void Update() {
         footStepTimer -= Time.deltaTime;

        if (footStepTimer < 0f) {
            footStepTimer = footSteptimerMax;

            if (player.IsWalking()) {
                float volume = 1f;
                //soundManager.FootStepSound(player.transform.position, volume);
            }
        }
    }
}

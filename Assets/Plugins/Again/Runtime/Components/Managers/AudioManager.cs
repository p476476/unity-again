using UnityEngine;

namespace Again.Runtime.Components.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public AudioSource audioSource;

        public void PlaySound(string soundName)
        {
            var audioClip = Resources.Load<AudioClip>($"Sounds/{soundName}");
            if (audioClip == null)
            {
                Debug.Log($"找不到音效：{soundName}");
                return;
            }

            audioSource.PlayOneShot(audioClip);
        }
    }
}
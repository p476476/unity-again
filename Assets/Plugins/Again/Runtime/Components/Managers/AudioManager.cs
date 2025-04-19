using UnityEngine;

namespace Again.Runtime.Components.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public AudioSource audioSource;

        public void PlaySound(string soundName)
        {
            var audioClip = Resources.Load<AudioClip>($"Sounds/{soundName}");
            audioSource.PlayOneShot(audioClip);
        }
    }
}
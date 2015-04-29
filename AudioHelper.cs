using UnityEngine;
using System.Collections;

public class AudioHelper
{
    public static void PlayClip(AudioClip clip, Vector3 position, float pitch, float volume, bool loop)
    {
        GameObject tmpObj = new GameObject("TMP_AUDIO");
        AudioSource tmpAud = tmpObj.AddComponent<AudioSource>();
        tmpObj.AddComponent<AudioKiller>();
        tmpAud.loop = loop;
        tmpAud.clip = clip;
        tmpAud.volume = volume;
        tmpAud.pitch = pitch;

        GameObject.Instantiate(tmpObj, position, Quaternion.identity);
    }

    class AudioKiller : MonoBehaviour
    {
        AudioSource audioSrc;

        void Start()
        {
            this.audioSrc = this.GetComponent<AudioSource>();
            if(audioSrc != null)
            {
                audioSrc.Play();
            } else
            {
                Destroy(this.gameObject);
            }
        }

        void Update()
        {
            if(audioSrc == null || !audioSrc.isPlaying)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

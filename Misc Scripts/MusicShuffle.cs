/* I started from http://answers.unity3d.com/questions/569236/how-to-play-a-playlist-of-mp3s-randomly.html,
 * adapted it to our needs, and made it fail gracefully if no music files could be found.
 * - Jeff
 */

using UnityEngine;
using System.Collections;

public class MusicShuffle : MonoBehaviour
{
    Object[] myMusic; // declare this as Object array
    bool isReady = false;


    void Awake()
    {
        myMusic = Resources.LoadAll("Music", typeof(AudioClip));

        if (myMusic.Length != 0)
        {
            isReady = true;
            audio.clip = myMusic[0] as AudioClip;
        }
        else
            Debug.Log("No music files in Assets\\Music directory");


    }
    void Start()
    {
        if (isReady)
            audio.Play();
    }
    // Update is called once per frame
    void Update()
    {
        if (!audio.isPlaying && isReady)
            playRandomMusic();
    }
    void playRandomMusic()
    {
            audio.clip = myMusic[Random.Range(0, myMusic.Length)] as AudioClip;
            audio.Play();
    }
}
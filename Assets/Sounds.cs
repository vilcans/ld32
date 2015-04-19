using UnityEngine;
using System.Collections.Generic;

public class Sounds : MonoBehaviour {

    public AudioClip[] spawnClips;
    public AudioClip[] secondSpawnClips;

    public void PlaySpawn(Follower follower) {
        AudioClip clip = spawnClips[(int)follower.state];
        spawnClips[(int)follower.state] = secondSpawnClips[(int)follower.state];
        if(!clip) {
            return;
        }
        AudioSource source = follower.GetComponent<AudioSource>();
        if(source == null) {
            source = (AudioSource)follower.gameObject.AddComponent("AudioSource");
        }
        source.PlayOneShot(clip);
    }
}

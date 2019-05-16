using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] tracks;
    // Start is called before the first frame update
    void Start()
    {
        tracks[1].Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playTrack(int n) {
        foreach (AudioSource track in tracks)
        {
            if (track.isPlaying)
                track.Stop();
        }
        tracks[n].Play();
    }
}

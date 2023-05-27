using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AniStop : MonoBehaviour
{
    [SerializeField] private PlayableDirector _playDirector;
    void Start()
    {
        _playDirector = GetComponent<PlayableDirector>();
    }

    public void PauseScene()
    {
        Debug.Log("Signal receiver");
        float delay = 2.0f;
        _playDirector.Pause();

        Invoke("RePlay", delay);
        
    }

    public void RePlay()
    {
        _playDirector.Play();
    }

}

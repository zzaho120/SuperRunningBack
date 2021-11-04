using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource audioSource;
    public PoolName poolName;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            ObjectPool.ReturnObject(poolName, gameObject);
        }
    }
}

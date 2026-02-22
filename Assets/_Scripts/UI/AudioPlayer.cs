using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
  AudioSource audioSource;
  int state = 0;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    audioSource = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update()
  {
    if (!audioSource.isPlaying && audioSource.clip != null && state == 0)
    {
      audioSource.Play();
    }
    if (audioSource.isPlaying && state == 0)
    {
      state = 1;
    }

    if (!audioSource.isPlaying && state == 1)
    {
      Destroy(gameObject);
    }
  }
}

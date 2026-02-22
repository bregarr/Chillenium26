using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public static AudioManager Ref { get; private set; }

  [Header("SFXs")]
  [SerializeField] GameObject audioPrefab;
  [SerializeField] AudioClip[] audioClips;

  void Start()
  {
    if (Ref)
		{
			Debug.LogWarning("There are two wave authorities in the scene!");
		}
		Ref = this;
  }

  public void playSFX(string name)
  {
    GameObject audioPlayer = Instantiate(audioPrefab, WaveAuthority.PlayerRef.transform);
    AudioSource audioSource = audioPlayer.GetComponent<AudioSource>();
    AudioClip audioClip = null;
    for (int i = 0; i < audioClips.Length; i++)
    {
      if (audioClips[i].name == name)
      {
        audioClip = audioClips[i];
      }
    }
    if (audioClip)
    {
      audioSource.clip = audioClip;
    }
    else
    {
      Debug.Log("An audio clip named " + name + " does not exist.");
    }
  }
}

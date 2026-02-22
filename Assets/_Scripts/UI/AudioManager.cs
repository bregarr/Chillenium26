using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public static AudioManager Ref { get; private set; }
  public static MusicHandler MusicRef { get; private set; }

  static float GlobalSfxVolume;
  static float GlobalMusicVolume;

  [SerializeField] GameObject musicHandler;
  [SerializeField] GameObject audioPrefab;
  [SerializeField] AudioClip[] audioClips;

  void Start()
  {
    if (Ref)
    {
      Debug.LogWarning("There are two wave authorities in the scene!");
    }
    Ref = this;

    MusicRef = musicHandler.GetComponent<MusicHandler>();


    if (!PlayerPrefs.HasKey("musicVolume"))
    {
      PlayerPrefs.SetFloat("musicVolume", 100f);
    }
    GlobalMusicVolume = PlayerPrefs.GetFloat("musicVolume") / 100f;
    if (!PlayerPrefs.HasKey("sfxVolume"))
    {
      PlayerPrefs.SetFloat("sfxVolume", 100f);
    }
    GlobalSfxVolume = PlayerPrefs.GetFloat("sfxVolume") / 100f;

    MusicRef.UpdateMusicVolume(GlobalMusicVolume);
  }

  public void playSFX(string name, float volume = 1f, float pitch = 1f)
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
      audioSource.volume = volume * GlobalSfxVolume;
      audioSource.pitch = pitch;
    }
    else
    {
      Debug.Log("An audio clip named " + name + " does not exist.");
    }
  }

  public void UpdateVolume()
  {
    GlobalSfxVolume = PlayerPrefs.GetFloat("sfxVolume") / 100f;
    GlobalMusicVolume = PlayerPrefs.GetFloat("musicVolume") / 100f;
    MusicRef.UpdateMusicVolume(GlobalMusicVolume);
  }

}

using UnityEngine;

public class MusicHandler : MonoBehaviour
{
  [Header("Parts")]
  [SerializeField] AudioSource pluck;
  [SerializeField] AudioSource smoothPluck;
  [SerializeField] AudioSource bass;
  [SerializeField] AudioSource lead;
  [SerializeField] AudioSource horns;
  [SerializeField] AudioSource trumpet;
  [SerializeField] AudioSource bassGuitar;
  [SerializeField] AudioSource drums;
  [SerializeField] AudioSource crash;
  [SerializeField] AudioSource gun;
  
  [Header("Music Stats")]
  [SerializeField] float fadeInSpeed;
  [SerializeField] float fadeOutSpeed;
  
  bool pluckOn;
  bool smoothPluckOn;
  bool bassOn;
  bool leadOn;
  bool hornsOn;
  bool trumpetOn;
  bool bassGuitarOn;
  bool drumsOn;
  bool crashOn;
  bool gunOn;

  public void healthMusic(bool on)
  {
    crashOn = on;
  }

  public void damageMusic(bool on)
  {
    hornsOn = on;
    leadOn = !on;
  }

  public void speedMusic(bool on)
  {
    if (on)
    {
      pluck.pitch = 1.5f;
      smoothPluck.pitch = 1.5f;
      bass.pitch = 1.5f;
      lead.pitch = 1.5f;
      horns.pitch = 1.5f;
      trumpet.pitch = 1.5f;
      bassGuitar.pitch = 1.5f;
      drums.pitch = 1.5f;
      crash.pitch = 1.5f;
      gun.pitch = 1.5f;
    }
    else
    {
      pluck.pitch = 1f;
      smoothPluck.pitch = 1f;
      bass.pitch = 1f;
      lead.pitch = 1f;
      horns.pitch = 1f;
      trumpet.pitch = 1f;
      bassGuitar.pitch = 1f;
      drums.pitch = 1f;
      crash.pitch = 1f;
      gun.pitch = 1f;
    }
  }

  public void defenseMusic(bool on)
  {
    bassGuitarOn = on;
    trumpetOn = !on;
  }

  public void ammoMusic(bool on)
  {
    gunOn = on;
  }

  public void startMusic()
  {
    pluckOn = true;
    smoothPluckOn = true;
    bassOn = true;
    leadOn = true;
    trumpetOn = true;
    drumsOn = true;
  }

  public void stopMusic()
  {
    pluckOn = false;
    smoothPluckOn = false;
    bassOn = false;
    leadOn = false;
    hornsOn = false;
    trumpetOn = false;
    bassGuitarOn = false;
    drumsOn = false;
    crashOn = false;
    gunOn = false;
  }
  
  void Start()
  {
    pluck.Play();
    smoothPluck.Play();
    bass.Play();
    lead.Play();
    horns.Play();
    trumpet.Play();
    bassGuitar.Play();
    drums.Play();
    crash.Play();
    gun.Play();
    startMusic();
  }

  void Update()
  {
    adjustVolumes(Time.deltaTime);
  }

  void adjustVolumes(float amount)
  {
    if (pluckOn && pluck.volume < 1f)
    {
      pluck.volume += amount * fadeInSpeed;
    }
    if (!pluckOn && pluck.volume > 0f)
    {
      pluck.volume -= amount * fadeOutSpeed;
    }

    if (smoothPluckOn && smoothPluck.volume < 1f)
    {
      smoothPluck.volume += amount * fadeInSpeed;
    }
    if (!smoothPluckOn && smoothPluck.volume > 0f)
    {
      smoothPluck.volume -= amount * fadeOutSpeed;
    }
    
    if (bassOn && bass.volume < 1f)
    {
      bass.volume += amount * fadeInSpeed;
    }
    if (!bassOn && bass.volume > 0f)
    {
      bass.volume -= amount * fadeOutSpeed;
    }
    
    if (leadOn && lead.volume < 1f)
    {
      lead.volume += amount * fadeInSpeed;
    }
    if (!leadOn && lead.volume > 0f)
    {
      lead.volume -= amount * fadeOutSpeed;
    }
    
    if (hornsOn && horns.volume < 1f)
    {
      horns.volume += amount * fadeInSpeed;
    }
    if (!hornsOn && horns.volume > 0f)
    {
      horns.volume -= amount * fadeOutSpeed;
    }
    
    if (trumpetOn && trumpet.volume < 1f)
    {
      trumpet.volume += amount * fadeInSpeed;
    }
    if (!trumpetOn && trumpet.volume > 0f)
    {
      trumpet.volume -= amount * fadeOutSpeed;
    }
    
    if (bassGuitarOn && bassGuitar.volume < 1f)
    {
      bassGuitar.volume += amount * fadeInSpeed;
    }
    if (!bassGuitarOn && bassGuitar.volume > 0f)
    {
      bassGuitar.volume -= amount * fadeOutSpeed;
    }
    
    if (drumsOn && drums.volume < 1f)
    {
      drums.volume += amount * fadeInSpeed;
    }
    if (!drumsOn && drums.volume > 0f)
    {
      drums.volume -= amount * fadeOutSpeed;
    }
    
    if (crashOn && crash.volume < 1f)
    {
      crash.volume += amount * fadeInSpeed;
    }
    if (!crashOn && crash.volume > 0f)
    {
      crash.volume -= amount * fadeOutSpeed;
    }
    
    if (gunOn && gun.volume < 1f)
    {
      gun.volume += amount * fadeInSpeed;
    }
    if (!gunOn && gun.volume > 0f)
    {
      gun.volume -= amount * fadeOutSpeed;
    }
  }
}

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
  
  void Play()
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
  }
}

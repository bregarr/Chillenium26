using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIBuffs : MonoBehaviour
{
  [Header("UI Components")]
  [SerializeField] GameObject _buffElementPrefab;
  [SerializeField] float _spacing;
  [SerializeField] float _shiftTime;
  Queue<GameObject> _buffs;
  int _shifting;
  float _travellingTime;
  

  void Start()
  {
    _buffs = new Queue<GameObject>();
    _shifting = 0;
  }

  // Update is called once per frame
  void Update()
  {
    if (_shifting > 0)
    {
      _travellingTime += Time.deltaTime;
      float t = _travellingTime / _shiftTime;
      t = t > 1 ? 1 : t;
      foreach (Transform child in transform)
      {
        child.transform.position -= new Vector3(0f, _spacing * t * _shifting, 0f);
      }
      if (t >= 1)
      {
        _travellingTime = 0;
        _shifting = 0;
      }
    }
  }
  
  public void AddBuff(Dice buff)
  {
    if (WaveAuthority.PlayerRef.GetMaxDice() < _buffs.Count + 1)
    {
      RemoveBuff();
    }
    GameObject newBuffElement = Instantiate(_buffElementPrefab, transform);
    newBuffElement.GetComponent<BuffElement>().InitializeBuff(buff, new Vector3(transform.position.x, transform.position.y + _spacing * _buffs.Count, 1f));
    _buffs.Enqueue(newBuffElement);
  }
  public void RemoveBuff()
  {
    GameObject oldBuff = _buffs.Dequeue();
    Destroy(oldBuff);
    _travellingTime = 0;
    _shifting += 1;
  }
}

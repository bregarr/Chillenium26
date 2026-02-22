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
  GameObject buff1 = null;
  GameObject buff2 = null;
  int spaceCount = 0;

  void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
  {
    
  }
  
  public void AddBuff(Dice buff)
  {
    GameObject newBuffElement = Instantiate(_buffElementPrefab, transform);
    newBuffElement.GetComponent<BuffElement>().InitializeBuff(buff, new Vector3(transform.position.x, transform.position.y + _spacing * spaceCount, 1f));
    if (buff1 == null)
    {
      buff1 = newBuffElement;
      spaceCount = 1;
    }
    else if (buff2 == null)
    {
      buff2 = newBuffElement;
    }
    else
    {
      buff1.GetComponent<BuffElement>().ReplaceBuff(buff2.GetComponent<BuffElement>().dice);
      Destroy(buff2);
      buff2 = newBuffElement;
    }
  }
}

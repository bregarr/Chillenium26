using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class BuffElement : MonoBehaviour
{
  [Header("Element Values")]
  [SerializeField] float _centerScreenTime;
  [SerializeField] float _travelTime;
  [SerializeField] float _shiftTime;
  [SerializeField] float _endScale;
  float _travellingTime;
  Vector3 _startPosition;
  Vector3 _endPosition;
  float _startScale;

  [Header("UI Components")]
  [SerializeField] Image _icon;
  [SerializeField] TextMeshProUGUI _text;

  [Header("Dice Icons")]
  [SerializeField] Material _d4;
  [SerializeField] Material _d6;
  [SerializeField] Material _d8;
  [SerializeField] Material _d12;
  [SerializeField] Material _d20;
  
  // Buff font colors for reference
  // Health Regen = 0xD96767
  // Damage Up = 0x770000
  // Speed Up = 0x00670F
  // Defense Up = 0x323232
  // More Ammo = 0xB16A00
  
  public void InitializeBuff(Dice buff, Vector3 travelToPosition)
  {
    _endPosition = travelToPosition;
    switch (buff.GetBuffType())
    {
      case eBuffType.Health:
        _text.text = "Health Regen";
        _text.color = new Color(0xD9, 0x67, 0x67);
        _icon.material = _d4;
        _icon.material.SetInt("SelectNumber", buff.sideNum);
        break;
      case eBuffType.Damage:
        _text.text = "Damage Up";
        _text.color = new Color(0x77, 0x00, 0x00);
        _icon.material = _d6;
        _icon.material.SetInt("SelectNumber", buff.sideNum);
        break;
      case eBuffType.Speed:
        _text.text = "Speed Up";
        _text.color = new Color(0x00, 0x67, 0x0F);
        _icon.material = _d8;
        _icon.material.SetInt("SelectNumber", buff.sideNum);
        break;
      case eBuffType.Defense:
        _text.text = "Defense Up";
        _text.color = new Color(0x32, 0x32, 0x32);
        _icon.material = _d12;
        _icon.material.SetInt("SelectNumber", buff.sideNum);
        break;
      case eBuffType.Ammo:
        _text.text = "More Ammo";
        _text.color = new Color(0xB1, 0x6A, 0x00);
        _icon.material = _d20;
        _icon.material.SetInt("SelectNumber", buff.sideNum);
        break;
    }
  }

  public void EndCenterScreenTime()
  {
    _centerScreenTime = 0;
  }

  void Start()
  {
    _travellingTime = 0;
    _startPosition = transform.position;
    _startScale = transform.localScale.x;
  }

  // Update is called once per frame
  void Update()
  {
    if (_centerScreenTime <= 0)
    {
      _travellingTime += Time.deltaTime;
      float t = _travellingTime / _travelTime;
      transform.position = Vector3.Lerp(_startPosition, _endPosition, t);

      float newScale = Mathf.Lerp(_startScale, _endScale, t);
      transform.localScale = new Vector3(newScale, newScale, 1f);
    }
    else
    {
      _centerScreenTime -= Time.deltaTime;
    }
  }
}

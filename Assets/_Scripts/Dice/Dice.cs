using UnityEngine;

public enum eBuffType
{
	None = 0, Health = 4, Damage = 6, Speed = 8, Defense = 12, Ammo = 20
}

public class Dice : MonoBehaviour
{

	[Header("(Optional)")]
	[SerializeField] int _sideCount;
	[SerializeField] eBuffType _buffType;

	public int sideNum { get; private set; }

	public Dice(eBuffType type)
	{
		InitializeDice(type);
	}

	void OnEnable()
	{
		if (_buffType == eBuffType.None)
		{
			_buffType = (eBuffType)_sideCount;
		}
	}

	public Dice InitializeDice(eBuffType buffType)
	{
		_buffType = buffType;
		if (_buffType == eBuffType.None)
		{
			buffType = (eBuffType)_sideCount;
			Debug.Log(_sideCount);
		}

		GameObject newDice = DiceAuthority.Ref.GetDiceByBuff(buffType);
		GetComponent<MeshFilter>().mesh = newDice.GetComponent<MeshFilter>().mesh;
		GetComponent<MeshRenderer>().material = newDice.GetComponent<MeshRenderer>().material;

		RollDice();
		return this;
	}

	public Dice InitializeDice()
	{
		RollDice();
		return this;
	}

	public void RollDice()
	{
		sideNum = (int)Mathf.Round(Random.Range(1, (int)_buffType));
	}

	public int GetSideCount()
	{
		return (int)_buffType;
	}

	public eBuffType GetBuffType()
	{
		return _buffType;
	}

}
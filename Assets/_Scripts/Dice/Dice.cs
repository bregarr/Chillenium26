using UnityEngine;

public enum eBuffType
{
	None = 0, Health = 4, Damage = 6, Speed = 8, Defense = 12, Ammo = 20
}

public class Dice : MonoBehaviour
{

	public int sideNum { get; private set; }
	public eBuffType buffType { get; private set; }

	public Dice(eBuffType type)
	{
		InitializeDice(type);
	}

	public Dice InitializeDice(eBuffType buffType)
	{
		this.buffType = buffType;

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
		sideNum = (int)Mathf.Round(Random.Range(1, (int)buffType));
	}

}
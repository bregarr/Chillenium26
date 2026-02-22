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
	[SerializeField] bool _floating;
	float _lifespan;
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

	public Dice InitializeDice(bool floating)
	{
		if (floating)
		{
			_lifespan = 10f;
			_floating = floating;
			transform.position += new Vector3(0f, 1f, 0f);
		}
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

	void Update()
	{
		if (_floating)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y * 0.1f * Mathf.Sin(Time.time * 0.2f), transform.position.z);
			transform.Rotate(new Vector3(0f, 0.5f, 0f));

			_lifespan -= Time.deltaTime;
			if (_lifespan <= 0)
			{
				Destroy(gameObject);
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (!_floating)
		{
			return;
		}

		GameObject player = other.gameObject;

		if (player.GetComponent<PlayerControl>())
		{
			player.GetComponent<PlayerControl>().AddDice(this);
			Destroy(gameObject);
		}
	}
}
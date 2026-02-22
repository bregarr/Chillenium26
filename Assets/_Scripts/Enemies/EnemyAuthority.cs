using System.Collections.Generic;
using UnityEngine;

public class EnemyAuthority : MonoBehaviour
{

	public static EnemyAuthority Ref { get; private set; }

	[Header("Enemy Attack Data")]
	[SerializeField] List<GameObject> _validProjectiles = new();
	[SerializeField] float _heightVariance;

	void OnEnable()
	{
		if (Ref)
		{
			Debug.LogWarning("Two enemy authorities in the scene!");
		}
		Ref = this;
	}

	public GameObject GetRandomProjectile()
	{
		return _validProjectiles[Random.Range(0, _validProjectiles.Count)];
	}

	public float GetHeightVariance()
	{
		return Random.Range(-_heightVariance, _heightVariance);
	}

}
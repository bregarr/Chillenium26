using System.Collections.Generic;
using UnityEngine;

// Holds the model info for the dice
public class DiceAuthority : MonoBehaviour
{

    public static DiceAuthority Ref { get; private set; }

    [Header("Dice Statics")]
    [SerializeField] List<GameObject> _diceList;

    void Start()
    {
        if (Ref)
        {
            Debug.LogWarning("Two dice authority in the scene!");
        }
        Ref = this;
    }

    public GameObject GetDiceBySides(int sides)
    {
        foreach (GameObject dice in _diceList)
        {
            if (dice.GetComponent<Dice>().GetSideCount() == sides)
            {
                return dice;
            }
        }
        return null;
    }

    public GameObject GetDiceByBuff(eBuffType type)
    {
        foreach (GameObject dice in _diceList)
        {
            if (dice.GetComponent<Dice>().GetBuffType() == type)
            {
                return dice;
            }
        }
        return null;
    }

    public eBuffType GetRandomDiceType()
    {
        return _diceList[Random.Range(0, _diceList.Count)].GetComponent<Dice>().GetBuffType();
    }

}
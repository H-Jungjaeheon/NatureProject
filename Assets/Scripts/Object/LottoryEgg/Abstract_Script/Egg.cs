using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Egg : MonoBehaviour
{
    [Header("Egg���_����")]
    public int BuyMoney;

    [SerializeField]
    List<LottoryPersent> lottoryPersents;
}

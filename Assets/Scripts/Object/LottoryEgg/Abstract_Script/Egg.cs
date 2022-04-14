using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Egg : MonoBehaviour
{
    [Header("Egg상속_변수")]
    public int BuyMoney;

    public List<LottoryPersent> lottoryPersents;
}

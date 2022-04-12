using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Brush : MonoBehaviour
{
    [SerializeField]
    float DestroyTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyObj", DestroyTime);
    }

    void DestroyObj()
    {
        Destroy(this.gameObject);
    }
}

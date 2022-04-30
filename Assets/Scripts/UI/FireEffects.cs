using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffects : MonoBehaviour
{
    [SerializeField] private SpriteRenderer SR;
    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        Effecting();
    }
    void Effecting()
    {
        Color color = SR.color;
        color.a -= Time.deltaTime / 2.5f;
        GetComponent<SpriteRenderer>().color = color;
        transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime / 5, transform.localScale.y + Time.deltaTime / 5, transform.localScale.z);
        if(color.a <= 0)       
            Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeComponent : MonoBehaviour
{
    public GameManager gameManager;
    public new Transform transform;
    public Animator animator;
    public MeshRenderer[] renderers;
    public GameObject hat;
    public Transform firePoint;

    public int health;
    public float healthRegen;
    public int attackPoint;
    public float attackSpeed;
    public float attackDelay;
    public float moveSpeed;
    public float rotSpeed;
    public float JumpAmount;
    public bool IsJumping;
    public new string name;
    public string desc;
    public float resizeAmoint;
    public Vector3 minSize;
    public Vector3 maxSize;
    [TextArea(3, 5)]
    public string none;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

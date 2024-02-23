using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float vida;
    [SerializeField] private GameObject efectoMuerte;
    // Start is called before the first frame update
    
    public void TomarDa�o(float da�o)
    {
        vida -= da�o;
        if(vida <= 0)
        {
            Muerte();
        }
    }

    public void Muerte()
    {
        Instantiate(efectoMuerte, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

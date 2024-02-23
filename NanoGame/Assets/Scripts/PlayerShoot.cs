using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private GameObject bala;
    [SerializeField] private float tiempoEntreDisparos;
    private float cooldown;

    private void Update()
    {
        if (Input.GetButtonDown("Fire2") && Time.time >= cooldown){

            Shoot();
            cooldown = Time.time + tiempoEntreDisparos;
        }
    }

    private void Shoot()
    {
        Instantiate(bala, controladorDisparo.position, controladorDisparo.rotation);
    }
}

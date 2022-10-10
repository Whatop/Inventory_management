using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
              var direction = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            var bullet = ScrollViewController.SpawnFromPool<Bullet>("Bullet", transform.position + direction.normalized);//Instantiate(bulletPrefab, transform.position + direction.normalized, Quaternion.identity).GetComponent<Bullet>();
            bullet.Shoot(direction.normalized);
        }
    }
}

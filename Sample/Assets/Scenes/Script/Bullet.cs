using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;

    public void Shoot(Vector3 dir)
    {
        direction = dir;
        //Destroy(gameObject, 5f);
    }

    private void OnEnable()
    {
        Invoke(nameof(DeactiveDelay), 5f);
    }

    private void OnDisable()
    {
        ScrollViewController.ReturnToPool(gameObject);
        CancelInvoke();
    }

    void DeactiveDelay() => gameObject.SetActive(false);

    void Update()
    {
        transform.Translate(direction.x * 3 *Time.deltaTime, direction.y * 3 * Time.deltaTime, 0);   
    }
}

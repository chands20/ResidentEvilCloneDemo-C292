using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{

    [SerializeField] protected int ammoCapacity;
    [SerializeField] protected int currentLoadedAmmo;
    [SerializeField] protected int currentSpareAmmo;
    [SerializeField] protected bool canFire;

    [Tooltip("The point in the barrel where the bullet spawns")]
    [SerializeField] protected Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void Reload()
    {
        if (currentLoadedAmmo < ammoCapacity && currentSpareAmmo > 0)
        {
            int bulletsToLoad = ammoCapacity - currentLoadedAmmo;
            if (currentSpareAmmo >= bulletsToLoad)
            {
                currentLoadedAmmo = ammoCapacity;
                currentSpareAmmo -= bulletsToLoad;
            }
            else
            {
                currentLoadedAmmo = currentLoadedAmmo + currentSpareAmmo;
            }
        }
    }
    
    protected virtual void Fire()
    {
        if (canFire)
        {
            RaycastHit hit;
            if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, 500))
            {
                Debug.DrawRay(firePoint.position, firePoint.forward * hit.distance, Color.red, 2f);
                if (hit.transform.CompareTag("Zombie"))
                {
                    hit.transform.GetComponent<Enemy>().TakeDamage(1);
                }
            }
        }
    }



}

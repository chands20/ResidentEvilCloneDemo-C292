using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Fire()
    {
        base.Fire();


    }

    public override void Reload(Magazine newMag)
    {
        base.Reload(newMag); 
    }
}

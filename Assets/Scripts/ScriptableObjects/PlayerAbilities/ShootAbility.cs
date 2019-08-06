using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ProjectileAbility")]
public class ShootAbility : PlayerAbility
{
    [Tooltip("The bullet that the player will fire.")]
    [SerializeField]
    protected GameObject bullet;
    [Tooltip("The amount of hit point damage this attack will do.")]
    [SerializeField]
    protected int power = 1;
    [Tooltip("The speed this bullet will move at.")]
    [SerializeField]
    protected float speed = 10f;
    [Tooltip("The name of the child game object to be used as the origin of the bullet. Leave blank to specify the player's origin instead.")]
    [SerializeField]
    protected string firePointName = "FirePoint";

    protected Transform bulletOrigin;
    protected PlayerAim aimDir;
    protected Health hp;

    public override void Initialize(){
        hp = player.GetComponent<Health>();
        aimDir = player.GetComponent<PlayerAim>();
        bulletOrigin = player.transform.Find(firePointName);
        
        // Given fire point name is invalid, default the bullet origin to the player's transform
        if(!bulletOrigin)
            bulletOrigin = player.transform;
    }

    public override void ActivateAbility(){
        // Create a bullet and set its properties
        GameObject b = Instantiate(bullet, bulletOrigin.position, Quaternion.Euler(aimDir.GetAimDirection()));
        b.GetComponent<Projectile>().SetDirection(aimDir.GetAimDirection());
        b.GetComponent<Projectile>().SetPower(power);
        b.GetComponent<Projectile>().SetShooter(player);
        b.GetComponent<Projectile>().SetSpeed(speed);
        
        hp.TakeDamage(cost);
    }
}

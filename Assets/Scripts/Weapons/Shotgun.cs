﻿

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{

    [SerializeField] Camera rifle;
    [SerializeField] ParticleSystem shootParticle;
    [SerializeField] ParticleSystem bloodParticle;
    [SerializeField] Transform shootPos;
    [SerializeField] Animator shotgunAnimator;
    [SerializeField] bool isReloaded = true;

    [SerializeField] AudioSource shotAudio;
    [SerializeField] AudioSource reloadAudio;
    [SerializeField] AudioSource bloodAudio;

    bool isActive = true;

    public void ActivateShotgun()
    {
        isActive = true;
        this.gameObject.SetActive(true);
    }

    public void DeactivateShotgun()
    {
        isActive = false;
        this.gameObject.SetActive(false);
    }

    public bool IsShotgunActive()
    {
        return isActive;
    }

    public override string NameOfTheWeapon()
    {
        return Keys.Weapons.SHOTGUN;
    }

    public override void Shoot()
    {
        isReloaded = false;

        shotAudio.Play();

        Debug.Log("ShootingFromShotgun");
        shootParticle.transform.position = shootPos.position;
        shootParticle.Play();
        Debug.DrawRay(rifle.transform.position, rifle.transform.forward, Color.green);
        RaycastHit hitOut;
        if (Physics.Raycast(rifle.transform.position, rifle.transform.forward, out hitOut, 50f, 1024))
        {
            hitOut.collider.gameObject.GetComponent<BaseEnemy>().Hit();
            Instantiate(bloodParticle, hitOut.collider.transform.position + Vector3.up, Quaternion.identity);
            bloodAudio.Play();
            Debug.Log("Damage!");
        }
    }

    public override void Reload()
    {
        if(!isReloaded)
            StartCoroutine(ReloadWeapon());
    }

    public IEnumerator ReloadWeapon()
    {
        reloadAudio.Play();
        shotgunAnimator.SetBool(Keys.WeaponsAnimations.RELOAD, true);
        yield return new WaitForSeconds(0.5f);
        shotgunAnimator.SetBool(Keys.WeaponsAnimations.RELOAD, false);
        isReloaded = true;
    }
}

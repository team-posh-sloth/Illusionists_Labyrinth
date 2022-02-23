using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float iTime, blinkTime;
    bool iFrames, iFramesPlayer;

    [SerializeField] Material flashMaterial;
    Renderer[] renderersToFlash;
    Material[] ogMaterials;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MazeEnemy enemy) && gameObject.tag == "Player Weapon" && !iFrames)
        {
            if (enemy.isReal)
            {
                enemy.hitPoints -= 1;
                if (enemy.hitPoints <= 0)
                {
                    Destroy(enemy.gameObject);
                }
                else
                {
                    iFrames = true;
                    StartCoroutine(FlashEnemy(enemy.gameObject));
                }
            }
            else
            {
                enemy.TeleportHome();
            }
        }
        if (other.name == "Player" && gameObject.tag != "Player Weapon" && !iFramesPlayer)
        {
            iFramesPlayer = true;
            StartCoroutine(FlashEnemy(other.gameObject));
            other.GetComponent<Player>().takeDamage();
        }
    }

    IEnumerator FlashEnemy(GameObject enemy)
    {
        // Save the flashing states of enemy's renderer materials
        if (enemy.GetComponentsInChildren<SkinnedMeshRenderer>().Length < 1)
        {
            renderersToFlash = new MeshRenderer[1]; // Set size of the array to fit amount of renderers
            ogMaterials = new Material[1]; // Set size of the array to fit amount of materials
            for (int i = 0; i < renderersToFlash.Length; i++)
            {
                renderersToFlash.SetValue(enemy.GetComponent<MeshRenderer>(), i); // Save each of the enemy's skinned mesh renderers
                ogMaterials.SetValue(renderersToFlash[i].material, i); // Save each of the enemy's materials from the renderers we will flash
            }
        }
        else
        {
            renderersToFlash = new SkinnedMeshRenderer[enemy.GetComponentsInChildren<SkinnedMeshRenderer>().Length]; // Set size of the array to fit amount of renderers
            ogMaterials = new Material[renderersToFlash.Length]; // Set size of the array to fit amount of materials
            for (int i = 0; i < renderersToFlash.Length; i++)
            {
                renderersToFlash.SetValue(enemy.GetComponentsInChildren<SkinnedMeshRenderer>()[i], i); // Save each of the enemy's skinned mesh renderers
                ogMaterials.SetValue(renderersToFlash[i].material, i); // Save each of the enemy's materials from the renderers we will flash
            }
        }

        // Flashes for an amount of seconds equal to iTimer
        float blinkDelay = 0;
        for (float i = 0; i <= iTime; i += Time.deltaTime)
        {
            if (blinkDelay <= 0)
            {
                for (int j = 0; j < renderersToFlash.Length; j++) // Check each of the renderers
                {
                    if (renderersToFlash[j].material.color != flashMaterial.color) { renderersToFlash[j].material = flashMaterial; } // Blink to the flash material
                    else { renderersToFlash[j].material = ogMaterials[j]; } // Blink to the original material
                }
                blinkDelay = blinkTime;
            }
            else { blinkDelay -= Time.deltaTime; }
            yield return null;
        }
        // Return things to the way they should be
        for (int i = 0; i < renderersToFlash.Length; i++)
        {
            renderersToFlash[i].material = ogMaterials[i]; // Blink to the original material
        }
        iFrames = false;
        iFramesPlayer = false;
    }
}
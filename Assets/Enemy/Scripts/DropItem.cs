using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    private Rigidbody2D itemRb;
    public float dropForce = 5;
    public AudioClip dropSound;
    private AudioSource audioSource;

    void Start()
    {
        itemRb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        PlayDropSound();

        itemRb.AddForce(Vector2.up * dropForce, ForceMode2D.Impulse);
    }

    private void PlayDropSound()
    {
        if (dropSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(dropSound);
        }
    }
}

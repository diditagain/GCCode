using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSpriteLayer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<SpriteRenderer> spritesInFront = new List<SpriteRenderer>();
    private void OnEnable()
    {
        spriteRenderer.sortingOrder = (int)(spriteRenderer.transform.position.y * -100);

        foreach (var sprite in spritesInFront)
        {
            sprite.sortingOrder = spriteRenderer.sortingOrder + 1;
        }
    }

    private void OnValidate()
    {
        if(spriteRenderer != null)
            spriteRenderer.sortingOrder = (int)(spriteRenderer.transform.position.y * -100);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    [SerializeField] private InputScriptableObject input;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    [SerializeField] private List<SpriteRenderer> spritesInFrontOfPlayer = new List<SpriteRenderer>();
    [SerializeField] private List<SpriteRenderer> spritesBehindPlayer = new List<SpriteRenderer>();

    private void OnValidate()
    {
        if (playerSpriteRenderer != null)
            playerSpriteRenderer.sortingOrder = (int)(playerSpriteRenderer.transform.position.y * -100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Updates sprite look direction and sprite layer.
    /// </summary>
    /// <param name="inputX"></param>
    public void UpdateSprite(float inputX)
    {
        SetSpriteLookDirection(inputX);
        SetSpriteLayerOrder();
    }

    private void SetSpriteLayerOrder()
    {
        playerSpriteRenderer.sortingOrder = (int)(playerSpriteRenderer.transform.position.y * -100);

        foreach (var sprite in spritesInFrontOfPlayer)
        {
            sprite.sortingOrder = playerSpriteRenderer.sortingOrder + 1;
        }
        foreach (var sprite in spritesBehindPlayer)
        {
            sprite.sortingOrder = playerSpriteRenderer.sortingOrder - 1;
        }
    }

    private void SetSpriteLookDirection(float inputX)
    {
        Vector3 characterScale = playerSpriteRenderer.transform.localScale;

        //default look direction is left. multiply by -1 to look right.
        if (inputX > 0)
            characterScale.x = Mathf.Abs(characterScale.x) * -1;

        else if (inputX < 0)
            characterScale.x = Mathf.Abs(characterScale.x);

        playerSpriteRenderer.transform.localScale = characterScale;
    }

    public void LookRight()
    {
        Vector3 characterScale = playerSpriteRenderer.transform.localScale;
        characterScale.x = Mathf.Abs(characterScale.x) * -1;
        playerSpriteRenderer.transform.localScale = characterScale;
    }
    public void LookLeft()
    {
        Vector3 characterScale = playerSpriteRenderer.transform.localScale;
        characterScale.x = Mathf.Abs(characterScale.x);
        playerSpriteRenderer.transform.localScale = characterScale;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField] private Area nextArea;
    public bool Primed = true;

    [SerializeField] private Transform startPos;
    public Vector2 StartPos { get => startPos.position; }
    [SerializeField] private Transform endPos;
    public Vector2 EndPos { get => endPos.position; }

    [SerializeField] private PolygonCollider2D cameraBoundsCollider;
    [SerializeField] private AudioClip backgroundMusic;

    private Player player;
    [SerializeField] private LayerMask playerLayer;

    private void Awake()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        int isPlayer = (int)Mathf.Pow(2, collision.gameObject.layer) & playerLayer;
        if (isPlayer == 0)
            return;
        
        player = collision.GetComponent<Player>();
        nextArea.Primed = false;

        if (!Primed)
        {
            AudioManager.Instance.SetBackgroundMusic(backgroundMusic);
            cameraBoundsCollider.gameObject.SetActive(true);
            LevelLoader.Instance.FadeIn();
            Primed = true;
            CameraController.Instance.SetBounds(cameraBoundsCollider);
            player.GoToNewArea(this);
        }
        else
        {
            LevelLoader.Instance.FadeOut();
            LevelLoader.FadeOutCompleted += FadeOutThenGoToNextArea;
        }

        
    }

    private void FadeOutThenGoToNextArea()
    {
        if (player == null)
            return;

        cameraBoundsCollider.gameObject.SetActive(false);

        player.GoToNewArea(nextArea);

        LevelLoader.FadeOutCompleted -= FadeOutThenGoToNextArea;
    }

}

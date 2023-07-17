using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [SerializeField] private CinemachineVirtualCamera zoomedInCamera;
    [SerializeField] private CinemachineVirtualCamera zoomedOutCamera;

    private CinemachineConfiner zoomedInCameraConfiner;
    private CinemachineConfiner zoomedOutCameraConfiner;

    private Transform player;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;


        zoomedInCameraConfiner = zoomedInCamera.GetComponent<CinemachineConfiner>();
        zoomedOutCameraConfiner = zoomedOutCamera.GetComponent<CinemachineConfiner>();

        ZoomOut();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        zoomedInCamera.Follow = player;
        zoomedOutCamera.Follow = player;
    }

    public void ZoomIn(Transform target)
    {
        zoomedInCamera.Follow = target;
        zoomedInCamera.Priority = 1;
        zoomedOutCamera.Priority = 0;
    }
    public void ZoomOut()
    {
        zoomedInCamera.Priority = 0;
        zoomedOutCamera.Priority = 1;
    }

    public void SetBounds(PolygonCollider2D boundsCollider)
    {
        zoomedInCameraConfiner.m_BoundingShape2D = boundsCollider;
        zoomedOutCameraConfiner.m_BoundingShape2D = boundsCollider;
    }
}

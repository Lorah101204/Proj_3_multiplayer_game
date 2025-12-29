using UnityEngine;
using DG.Tweening;

public class SlideDoor : DoorBase
{
    [SerializeField] private Transform door;
    [SerializeField] private Vector2 openOffset;

    private Vector2 closedPos;
    private Vector2 openPos;

    private void Awake()
    {
        closedPos = door.position;
        openPos = closedPos + openOffset;
    }

    public override void Open()
    {
        base.Open();
        door.DOMove(openPos, 0.5f).SetEase(Ease.OutExpo);
    }

    public override void Close()
    {
        base.Close();
        door.DOMove(closedPos, 0.5f).SetEase(Ease.OutExpo);
    }
}

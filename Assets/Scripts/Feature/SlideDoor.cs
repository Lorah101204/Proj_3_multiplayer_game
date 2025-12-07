using UnityEngine;

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
        door.position = openPos;
    }

    public override void Close()
    {
        base.Close();
        door.position = closedPos;
    }
}

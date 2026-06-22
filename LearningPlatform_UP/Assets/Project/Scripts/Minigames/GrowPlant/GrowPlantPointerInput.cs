using UnityEngine;

public readonly struct GrowPlantPointerState
{
    public GrowPlantPointerState(Vector3 worldPosition, bool down, bool held, bool up)
    {
        WorldPosition = worldPosition;
        Down = down;
        Held = held;
        Up = up;
    }

    public Vector3 WorldPosition { get; }
    public bool Down { get; }
    public bool Held { get; }
    public bool Up { get; }
}

public static class GrowPlantPointerInput
{
    public static GrowPlantPointerState Get(Camera camera)
    {
        if (camera == null)
        {
            return new GrowPlantPointerState(Vector3.zero, false, false, false);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            return new GrowPlantPointerState(
                ScreenToWorld(camera, touch.position),
                touch.phase == TouchPhase.Began,
                touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary,
                touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled);
        }

        return new GrowPlantPointerState(
            ScreenToWorld(camera, Input.mousePosition),
            Input.GetMouseButtonDown(0),
            Input.GetMouseButton(0),
            Input.GetMouseButtonUp(0));
    }

    private static Vector3 ScreenToWorld(Camera camera, Vector2 screenPosition)
    {
        Vector3 worldPosition = camera.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0f;
        return worldPosition;
    }
}

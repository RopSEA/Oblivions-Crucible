using UnityEngine;
using UnityEngine.Events;

public class MovementTracker : MonoBehaviour
{
    public UnityEvent OnAllDirectionsMoved;

    private bool movedUp = false;
    private bool movedDown = false;
    private bool movedLeft = false;
    private bool movedRight = false;
    private bool completed = false;

    void Update()
    {
        if (completed) return;

        if (Input.GetKeyDown(KeyCode.W)) movedUp = true;
        if (Input.GetKeyDown(KeyCode.S)) movedDown = true;
        if (Input.GetKeyDown(KeyCode.A)) movedLeft = true;
        if (Input.GetKeyDown(KeyCode.D)) movedRight = true;

        if (movedUp && movedDown && movedLeft && movedRight)
        {
            completed = true;
            Debug.Log("Moved in all directions!");
            OnAllDirectionsMoved?.Invoke();
        }
    }

    public void ResetTracker()
    {
        movedUp = movedDown = movedLeft = movedRight = completed = false;
    }
}

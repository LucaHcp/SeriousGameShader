using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    IMover mover;

    void Start()
    {
        mover = GetComponent<IMover>();
        LockMouseOnCameraCenter();
        Invoke("UnLockMouseOnCamera", 0.1f);
    }

    void Update()
    {
        MoveToMousePosition();
    }

    void MoveToMousePosition()
    {
        // Erhalte die Mausposition
        Vector3 mousePosition = Input.mousePosition;

        // Setze die Z-Komponente auf die gewünschte Ebene (z.B., Z = 0)
        mousePosition.z = Camera.main.transform.position.z;

        // Wandle die Mausposition in Weltkoordinaten um
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Setze die Z-Komponente auf die gewünschte Ebene (Z = 0)
        worldPosition.z = 0;

        // Berechne die Richtung zum gewünschten Punkt
        Vector3 direction = worldPosition - transform.position;
        direction.z = 0;
        if (direction.magnitude >= 0.75f)
        {
            direction.Normalize();

            mover.MoveToPosition(new Vector2(direction.x, direction.y));
            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
            {
                mover.Sprint();
            }
        }
    }

    void LockMouseOnCameraCenter()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void UnLockMouseOnCamera()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}

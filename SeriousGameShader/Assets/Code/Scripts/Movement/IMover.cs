using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMover
{
    void MoveToPosition(Vector2 inputDirection);
    void Sprint();
}

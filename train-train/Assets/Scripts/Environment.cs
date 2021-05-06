using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour {

    public FreeParallax parallax;

	public void SetMoveSpeed(float speed)
    {
        parallax.Speed = speed;
    }
}

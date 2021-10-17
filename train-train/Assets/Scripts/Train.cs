using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Train : MonoBehaviour
{
    public CanvasRenderer c_renderer;
    public List<Seat> seats;
    public Transform middle;
    public Image driverImage;
    public GameObject arrow;
    public GameObject arrow2;
    public GameObject move;
    public GameObject move2;
    public Texture2D driver {  get { return driverImage.sprite.texture; } set {  driverImage.sprite = Sprite.Create(value, new Rect(0, 0, value.width, value.height), new Vector2(0, 0)); } }
    public Seat FreeSeat()
    {
        var seat = seats.Find(s => s.isEmpty());
        return seat;
    }

    public float AccelerationSpeed = 8;
    public float DecelerationSpeed = 6;
    public float BreakSpeed = 20;
    public float SpeedLimit = 500;

    private float _speed = 0;
    public float Speed {
        get { return _speed; }
        set {
            _speed = Mathf.Clamp(value, 0f, SpeedLimit);
        }
    }

    public void Accelerate()
    {
        Speed += Time.deltaTime * AccelerationSpeed;
    }

    public void Decelerate()
    {
        Speed -= Time.deltaTime * SpeedLimit / 7;
    }

    public void Break()
    {
        Speed -= Time.deltaTime * BreakSpeed;
    }

    public void playLeave()
    {
        GetComponent<Animator>().Play("train_leave");
    }

    private void Start()
    {
        c_renderer = GetComponent<CanvasRenderer>();
    }

}

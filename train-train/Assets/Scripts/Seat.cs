using System.Collections;
using UnityEngine;

public class Seat : MonoBehaviour
{
    public BoxCollider Collider;
    public Passenger passenger;
    public bool blocked = false;

    public bool isEmpty() { return passenger == null && !blocked; }
    public void Place(Passenger passenger)
    {

        passenger.transform.SetParent(transform);
        this.passenger = passenger;

        var p = passenger.GetComponent<RectTransform>();
        var my = GetComponent<RectTransform>();
        p.position = my.position;
    }

    public Passenger Remove()
    {
        var tmp = passenger;
        passenger = null;
        return tmp;
    }

    public void leaveSeat()
    {
        if (passenger) {
            StartCoroutine(Leave());
        }
    }


    private IEnumerator Leave()
    {
        blocked = true;
        passenger.playLeave();
        var go = passenger.gameObject;
        passenger = null;

        yield return new WaitForSeconds(1f);
        blocked = false;
        yield return new WaitForSeconds(2f);
        Destroy(go);
    }

    public void setActive(bool active)
    {
        if (passenger) {
            Collider.enabled = active;
            passenger.active = active;
        }
    }

}
using UnityEngine;
using UnityEngine.UI;

public class Passenger : MonoBehaviour
{
    public Animator animator;
    public RawImage image;
    public SymbolRepresentation symbolRepresentation;
    private Symbol _symbol;
    public Symbol symbol { get { return _symbol; } set { _symbol = value;  symbolRepresentation.setSymbol(value); } }
    public Texture2D texture { get { return image.texture as Texture2D; } set { image.texture = value; } }
    private bool _active = false;
    public bool active {
        get { return _active; }
        set {
            _active = value;
            var color = image.color;
            color.a = active ? 1f : 0.5f;
            image.color = color;
        }
    }

    public static Passenger GetPassenger(Symbol s, Texture2D t)
    {
        var p = Instantiate(Resources.Load<GameObject>("Passenger")).GetComponent<Passenger>();
        p.texture = t;
        p.symbol = s;
        return p;
    }

    public void playHappy()
    {
        animator.Play("passenger_happy");
    }

    public void playSad()
    {
        animator.Play("passenger_unhappy");
    }

    public void playLeave()
    {
        animator.Play("passenger_leave");
    }
}

using UnityEngine;

public class EntityAudioController : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event[] swingEvents;
    [SerializeField] private AK.Wwise.Event hitEvent;
    [SerializeField] private AK.Wwise.Event hurtEvent;
    [SerializeField] private AK.Wwise.Event dashEvent;
    [SerializeField] private AK.Wwise.Event walkEvent;
    [SerializeField] private AK.Wwise.Event spinEvent;
    [SerializeField] private AK.Wwise.Event sweepEvent;
    [SerializeField] private AK.Wwise.Event slipEvent;
    [SerializeField] private AK.Wwise.Event raveEvent;

    private float walkID;

    public void PlaySwing(int index = 0)
    {
        swingEvents[index].Post(gameObject);
    }

    public void PlayHit()
    {
        hitEvent.Post(gameObject);
    }

    public void PlayHurt()
    {
        hurtEvent.Post(gameObject);
    }

    public void PlayDash()
    {
        dashEvent.Post(gameObject);
    }

    public void PlayWalk()
    {
        if(walkID == 0f)
            walkID = walkEvent.Post(gameObject);
    }

    public void StopWalk()
    {
        if(walkID != 0f)
        {
            walkEvent.Stop(gameObject);
            walkID = 0f;
        }
    }

    public void PlaySpin()
    {
        spinEvent.Post(gameObject);
    }

    public void StopSpin()
    {
        spinEvent.Stop(gameObject);
    }

    public void PlaySweep()
    {
        sweepEvent.Post(gameObject);
    }

    public void PlaySlip()
    {
        slipEvent.Post(gameObject);
    }

    public void PlayRave()
    {
        raveEvent.Post(gameObject);
    }
}

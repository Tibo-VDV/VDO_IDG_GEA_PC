using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AI_Behavior : MonoBehaviour
{
    public enum Behavior { wandering, searching, hunting, attacking }
    public Behavior currentBehavior = Behavior.wandering;

    [SerializeField] Transform target;
    NavMeshAgent agent => GetComponent<NavMeshAgent>();

    [Header("Navigation settings")]
    [SerializeField] Transform[] wayPoints;
    [SerializeField] int wayPointIndex = 0;
    [SerializeField] float minimumDistance = 2f;

    void Start()
    {

        if (wayPoints.Length == 0)
        {
            Debug.LogError("Set at least one waypoint in the waypoint list.");
            return;
        }

        SetBehavior(Behavior.wandering);
    }


    #region Coroutines
    IEnumerator Wander()
    {
        while (true)
        {
            UpdateTarget(FindVialableWaypoint());
            yield return new WaitForSeconds(0.1f); // update niet per frame maar per seconden

        }
    }
    

    IEnumerator HuntPlayer()
    {
        while (true)
        {
            yield return null;
        }
    }

    IEnumerator SearchingPlayer()
    {
        while (true)
        {
            yield return null;
        }
    }
    #endregion

    #region logic methods
    public void SetBehavior(Behavior behaviorToSet)
    {
        StopAllCoroutines();
        currentBehavior = behaviorToSet;
        UpdateBehavior();
    }

    void UpdateBehavior()
    {
        switch (currentBehavior)
        {
            case Behavior.wandering:
                StartCoroutine(Wander());
                break;

            case Behavior.searching:

                break;

            case Behavior.hunting:

                break;

            case Behavior.attacking:
                break;

            default:
                SetBehavior(Behavior.wandering);
                break;


        }
    }

    void UpdateTarget(Transform setTarget)
    {
        target = setTarget;
        agent.destination = target.position;

    }

    Transform FindVialableWaypoint()
    {
        //als we dichtbij een waypoint zijn ga naar de volgende, of wanneer deze volledig onbereikbaar is.
        if (agent.remainingDistance <= minimumDistance || agent.remainingDistance >= Mathf.Infinity)
        {
            wayPointIndex++;
            wayPointIndex = wayPointIndex % wayPoints.Length; //loop index with modulo(%)
            return wayPoints[wayPointIndex];
        }
        return wayPoints[wayPointIndex];
    }
    
    #endregion
}

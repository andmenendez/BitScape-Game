using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicAI : MonoBehaviour {
    public enum AIState {
        Patrol,
        ChasePlayer,
        UseBattery,
        Activate
    }
    public List<GameObject> waypoints;

    private StateVariable<AIState> state = new StateVariable<AIState>(AIState.Patrol);
    private NavMeshAgent navMesh;
    private int currWayPoint = -1;
    private float distanceTo;
    private Transform battery;
    private BatteryController batteryControl;
    private Transform activator;
    private Transform player;
    private AgentBatteryController internalBattery;

    private AudioSource audioSource;
    private AudioClip audio_talk;
    private AudioClip audio_done;
    private AudioClip audio_dead;
    private AudioClip audio_activate;
    private AudioClip audio_battery;

    private bool audioPlayed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        navMesh = GetComponent<NavMeshAgent>();
        battery = GameObject.Find("battery3").transform;
        batteryControl = battery.GetComponent<BatteryController>();
        activator = GameObject.Find("activator3").transform;
        player = GameObject.Find("player").transform;
        internalBattery = GetComponent<AgentBatteryController>();
        setNextWaypoint();

        audio_talk = (AudioClip)Resources.Load($"Sounds/robot_talk");
        audio_done = (AudioClip)Resources.Load($"Sounds/robot_done");
        audio_dead = (AudioClip)Resources.Load($"Sounds/robot_dead");
        audio_activate = (AudioClip)Resources.Load($"Sounds/robot_activate");
        audio_battery = (AudioClip)Resources.Load($"Sounds/robot_battery");
        audioPlayed = false;
    }

    void Update() {
        CalculateNearObject();
        ChangeAgentState();

        audioSource.loop = false;
    }

    private void ChangeAgentState() {
        if (state.HasChanged)
        {
            Debug.Log($"agent state:{state.Value}");
            if (!audioPlayed)
            {
                audioSource.Stop();
                switch (state.Value)
                {
                    case AIState.Patrol:
                        audioSource.PlayOneShot(audio_dead, 1.0F);
                        break;
                    case AIState.ChasePlayer: 
                        audioSource.PlayOneShot(audio_done, 1.0F);
                        break;
                    case AIState.Activate:
                        audioSource.PlayOneShot(audio_activate, 1.0F);
                        break;
                    case AIState.UseBattery:
                        audioSource.PlayOneShot(audio_battery, 1.0F);
                        break;
                }
                audioPlayed = true;
            }
        }
        switch (state.Value) {
            case AIState.Patrol:
                if (navMesh.remainingDistance < 1)
                    setNextWaypoint();
                break;

            case AIState.ChasePlayer:
                if (Vector3.Distance(transform.position, player.position) < 4) {
                    navMesh.SetDestination(transform.position);
                } else {
                    navMesh.SetDestination(player.position);
                    GameObject.Find("LevelState").GetComponent<LevelState>().currentMaxInstruction = 6;
                }
                break;

            case AIState.Activate:
                navMesh.SetDestination(activator.position);
                break;

            case AIState.UseBattery:
                navMesh.SetDestination(battery.position);
                break;
        }
    }

    private void CalculateNearObject() {
        NavMeshHit hit;

        //if the agent can see the battery and is on the floor
        //and distance to the battery is less than threshold
        //go to the battery location
        if (!navMesh.Raycast(battery.position, out hit) &&
            Vector3.Distance(transform.position, battery.position) < 4f &&
            battery.position.y < 1f &&
            batteryControl.batteryLevel.Value > batteryControl.minimumLevel) {
            state.SetValue(AIState.UseBattery);
            audioPlayed = false;
            return;
        }

        //if the agent has battery greather than the minimun go to the activator
        if (internalBattery.batteryLevel > internalBattery.minimumLevel) {
            if (state.SetValue(AIState.Activate)) {
                LevelState.Instance.PlaySound("battery_full", gameObject);
                audioPlayed = false;

            }
            return;
        }

        //if player is visible and not battery is available go to the player position
        if (!navMesh.Raycast(player.position, out hit) &&
            Vector3.Distance(transform.position, player.position) < 10f) {
            state.SetValue(AIState.ChasePlayer);
            audioPlayed = false;

            return;
        }

        state.SetValue(AIState.Patrol);
        audioPlayed = false;
    }

    private void setNextWaypoint() {
        currWayPoint += 1;
        if (currWayPoint >= waypoints.Count)
            currWayPoint = 0;
        navMesh.SetDestination(waypoints[currWayPoint].transform.position);
    }
}
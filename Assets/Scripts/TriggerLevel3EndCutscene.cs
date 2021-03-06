﻿using UnityEngine;
using System.Collections;

public class TriggerLevel3EndCutscene : MonoBehaviour
{

    private Transform target;
    //all targets along the cutscene path
    public Transform[] allTargets;
    public float[] speedToTarget;
    private int targetIndex = 0;
    private GameObject _player;
    //private bool _reachedDestination = false;
    private float speed = 10;
    private bool _wasTriggered = false;

    private GameObject mapIcon;
    private bool movePlayer = false;
    private float distToTarget;
    private const float arrivalDist = 0.5f;

    public float lookRotation = -90.0f;
    private Quaternion dockRotation;
    private Quaternion playerRotation;
    private float rotationSpeed = 0.1f;

    private bool reachedTarget = false;
    private float pauseTime = 5.0f;

    private LoadingTransition loadingTransition;
    public GameObject transitionBox;

    private AudioSource audioSrc;

    // Use this for initialization
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        audioSrc = GetComponent<AudioSource>();
        //subscribe map icon function to part pickup event
        TractorBeamControls.partPickupDelegate += ActivateMapIcon;
        //get map icon components and set inactive at start
        if (mapIcon = transform.FindChild("MapIcon").gameObject)
        {
            mapIcon.SetActive(false);
        }
        if (transitionBox != null)
        {
            loadingTransition = transitionBox.GetComponent<LoadingTransition>();
        }
        target = allTargets[0];
        dockRotation = Quaternion.identity * Quaternion.Euler(0, 0, lookRotation);
    }

    void OnDisable()
    {
        TractorBeamControls.partPickupDelegate -= ActivateMapIcon;
    }
    // Update is called once per frame
    void Update()
    {
        if (movePlayer == true && _player != null)
        {
            distToTarget = Vector2.Distance(_player.transform.position, target.transform.position);
            //if player has reached point
            if (distToTarget <= arrivalDist)
            {
                _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                if (playerRotation == null)
                { playerRotation = _player.transform.rotation; }

                _player.transform.rotation = Quaternion.Lerp(playerRotation, dockRotation, Time.deltaTime * rotationSpeed);

                //rotate to proper facing direction
                if (_player.transform.rotation == dockRotation)
                {
                    movePlayer = false;
                    StartCoroutine(ReachedTarget());
                }
            }
            else
            {
                //move player to point
                _player.SendMessage("cutSceneMovePlayer", speed);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "TetheredPart" && other.GetComponent<TetheredObject>().tetherOn && !_wasTriggered)
        {
            //set triggered to true (to prevent multiple calls)
            _wasTriggered = true;
            TetheredObject tether = other.GetComponent<TetheredObject>();
            tether.tetherOn = false;
            tether.ShrinkAndDestroy();
            //disable collisions
            _player.GetComponent<CircleCollider2D>().enabled = false;
            //disable fuel loss
            _player.GetComponent<Player>().DisableFuelLoss();
            //disable camera follow and center camera
            //StartCoroutine(CenterCamera());
            BeginCutscene();
        }
       
    }

    void BeginCutscene()
    {
        //target.GetComponent<EndCutScene>().SetEndpointActive();
        //setReachedDestination(false);

        //set destination for player to go       
        _player.SendMessage("setPlayerDestination", (Vector2)target.position);
        speed = speedToTarget[targetIndex];

        //start player movement
        movePlayer = true;

    }
   
    IEnumerator ReachedTarget()
    {

        targetIndex++;
        if (targetIndex < allTargets.Length)
        {
            //get next target

            //swap layer
            SpriteRenderer playerRend = _player.GetComponent<SpriteRenderer>();
            playerRend.sortingLayerName = "Parallax2";
            playerRend.sortingOrder = -2;

            //start shrinking player
            StartCoroutine(ShrinkPlayer());

            target = allTargets[targetIndex];  
            BeginCutscene();
        }
        else
        {
            //end level
            //show ending dialogue if this is an end trigger
            if (transitionBox != null)
            {
                loadingTransition.startCommentaryDialogue();
            }
        }
        yield return null;
    }

    IEnumerator ShrinkPlayer()
    {
        Vector3 shrinkVec = new Vector3(0.05f,0.05f,0);
        while (_player.transform.localScale.x > 0.1)
        {
            _player.transform.localScale -= shrinkVec;
            yield return new WaitForSeconds(1);
        }
        yield return null;
       
    }

    void ActivateMapIcon()
    {
        if (mapIcon != null)
        {
            mapIcon.SetActive(true);
        }
    }

    IEnumerator PlayAudio()
    {
        audioSrc.Play();
        yield return new WaitForSeconds(2.0f);
        while (audioSrc.volume > 0)
        {
            audioSrc.volume -= 0.1f;
            yield return new WaitForSeconds(0.3f);
        }
        audioSrc.Stop();
        yield return null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Entities;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Controller : MonoBehaviour
{
    private Race _race;
    private bool isPlayer;
    private float maxSpeed = 10.0f;
    public int currentCheckpoint;
    public int loops;
    
    //Player
    private AudioManager _audioManager;
    private float accel = 2.0f;
    private float deaccel = -5f;
    private Vector3 steeling;
    private Vector3 speed;
    private Text speedShowUI;
    private Rigidbody2D _rigidbody;
    private float penalty = 1f;
    private int Layers;
    private Text LugarUI;
    private Text TiempoUI;
    public float currentTime;
    private float lastTime;

    //AI
    public int currentStatus = 0;
    private AudioSource _audioSource;
    private Quaternion targetRotation;

    public float getPoints()
    {
        return loops * 1000f + currentCheckpoint*10f + 10f*(1f-Vector3.Distance(gameObject.transform.position, _race.checkpoint[
                                                                (currentCheckpoint + 1)%(_race.checkpoint.Length)].transform.position)/100f);
    }

    private void Awake()
    {
        loops = 1;
        currentCheckpoint = 0;
        Layers = LayerMask.GetMask(new string[] {"Road"});
        // Physics2D.IgnoreLayerCollision(8, 13, true);
        Random.InitState((int) DateTime.Now.Ticks);
        _rigidbody = GetComponent<Rigidbody2D>();
        speed = new Vector3();
        _race = GameObject.Find("GlobalScript").GetComponent<Race>();
        
        //
        if (isPlayer = (gameObject.tag == "Player"))
        {
            deaccel -= (1.5f *Game.CurrentGame.upBrake);
            accel += Game.CurrentGame.upAccel;
            maxSpeed += Game.CurrentGame.upSpeed * 1.1f;
            steeling = new Vector3();
            steeling.z = 60f + 1.5f*Game.CurrentGame.upSteeling;
            transform.Find("Body").gameObject.GetComponent<SpriteRenderer>().sprite =
                Resources.Load<Sprite>(string.Format("Sprites/Cars/{0}{1}", Game.CurrentGame.car+1,Game.CurrentGame.color+1));
            _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            speedShowUI = GameObject.Find("Canvas").transform.Find("Speed").GetComponent<Text>();
            LugarUI = GameObject.Find("Canvas").transform.Find("Lugar").GetComponent<Text>();
            TiempoUI = GameObject.Find("Canvas").transform.Find("Tiempo").GetComponent<Text>();
        }
        else
        {
            transform.Find("Body").gameObject.GetComponent<SpriteRenderer>().sprite =
                Resources.Load<Sprite>(string.Format("Sprites/Cars/{0}{1}", Random.Range(1, 5), Random.Range(1, 5)));
            maxSpeed = Race.botSpeed + 2f*(int)Game.CurrentGame.Difficulty;
            _audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Wall":
                speed.y = 0f;
                _audioManager.play("wallhit");
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other.transform.tag);
        if (other.transform.CompareTag("Checkpoint") &&
            int.Parse(other.transform.name) == (currentCheckpoint + 1) % _race.checkpoint.Length && loops <= 3)
        {
            if (++currentCheckpoint == _race.checkpoint.Length)
            {
                currentCheckpoint = 0;
                loops++;
            }
        }
    }

    public static string formatTime(float time)
    {
        return string.Format("{0:00}:{1:00}.{2:000} ", ((int) time / 60) % 60,
            ((int) time) % 60, (int) (time * 1000) % 1000);
    }

    // Update is called once per frame
    void Update()
    {
        if (_race.state == ePhase.Loading)
        {
            return;
        }

        if (!Game.CurrentGame.camaraRotation)
        {
            Camera.main.transform.rotation = Quaternion.identity;
        }

        if (isPlayer && _race.state != ePhase.Finished)
        {
            penalty = _rigidbody.IsTouchingLayers(Layers) ? 1f : Race.penalty + 0.5f*(int)Game.CurrentGame.Difficulty;
            // Debug.Log(maxSpeed* (1f/penalty) + " "+ penalty);
            
            if (speed.y != 0f)
            {
                if (Time.time - lastTime > maxSpeed/ (10f*speed.y) || lastTime==0f)
                {
                    // Debug.Log("h");
                    _audioManager.play("speed");
                    lastTime = Time.time + maxSpeed/(10f*speed.y);
                }
                transform.Rotate(steeling * -Input.GetAxis("Horizontal") * Time.deltaTime);
            }

            if (Input.GetButton("Fire2"))
            {
                if (speed.y > 0f)
                {
                    if (Input.GetButtonDown("Fire2"))
                    {
                        _audioManager.play("brake");
                    }
                    speed.y = Mathf.Max(0f, speed.y + deaccel * Time.deltaTime);
                }
                else
                {
                    speed.y = Mathf.Max(-10f, speed.y - accel * Time.deltaTime);
                }
            }
            else if (Input.GetButton("Fire1") && speed.y < maxSpeed* (1f/penalty))
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    _audioManager.play("accel");
                }
                speed.y += accel * Time.deltaTime;
            }

            if (penalty > 1f && speed.y > maxSpeed * (1f / penalty))
                penalty = 10f;

            //Esto sería uk
            if (speed.y > 0f)
            {
                speed.y = Mathf.Max(0f, speed.y - penalty * 0.5f * Time.deltaTime);
            }
            else
            {
                speed.y = Mathf.Min(0f, speed.y + penalty * 0.5f * Time.deltaTime);
            }

            currentTime = Time.time - _race.startTime;
            speedShowUI.text = string.Format(" Speed: {0:0.00}", speed.y);
            LugarUI.text = string.Format(" Lugar: {0}\n Lap: {1}/3", _race.getPlayerPos(), (loops <= 3 ? loops : 3));
            TiempoUI.text = formatTime(currentTime);

            transform.Translate(speed * Time.deltaTime);
        }
        else
        {
            if (Vector3.Distance(transform.position, _race.points[currentStatus]) < 0.5f)
            {
                currentStatus = (currentStatus + 1) % _race.points.Count;
            }
            
            if(!isPlayer)
            _audioSource.volume = 0.3f *
                Mathf.Max(0, 1f - Vector3.Distance(transform.position, _race._playerGO.transform.position)/5f) *
                 Game.CurrentGame.EffectsVolume / 100f;
            
            // Debug.Log((transform.rotation.eulerAngles.z -
            //            Vector3.SignedAngle((_race.points[currentStatus] - transform.position), Vector3.up,
            //                Vector3.back)) % 360);

            if (speed.y < maxSpeed)
            {
                speed.y += Random.Range(-1f,2f) * Time.deltaTime;
            }
            else
            {
                speed.y -= Random.Range(0f,3f/(float) (Game.CurrentGame.Difficulty)) * Time.deltaTime;
            }

            if (speed.y > 5f && speed.y > 4f && Vector3.Distance(transform.position, _race.points[currentStatus]) < 3.5f)
            {
                speed.y -= Random.Range(3f+1.5f*(float) (Game.CurrentGame.Difficulty),10f) * Time.deltaTime;
            }
            
            targetRotation = Quaternion.Euler(0f, 0f,
                Vector3.SignedAngle((_race.points[currentStatus] - transform.position), Vector3.up, Vector3.back));

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 180f*Time.deltaTime);
            // transform.position =
            //     Vector3.MoveTowards(transform.position, _race.points[currentStatus], speed.y * Time.deltaTime);
            transform.Translate(speed * Time.deltaTime);
        }
    }
}
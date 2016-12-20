using UnityEngine;
using System.Collections;

public class UnitychanAnimationControll : MonoBehaviour
{
    AnimatorStateInfo animInfo;
    Animator animator;
    AudioSource audiosource;
    bool isLooked = false;
    public bool IsLooked { get { return isLooked; }
        set { isLooked = value; } }
    float speed = 50.0f;
    float normalTimer = 0.0f;
    float normalInterval = 15.0f;
    float signTimer = 0.0f;
    float signInterval = 6.5f;
    public GameObject cube;

    public AudioClip voice_Yes;
    public AudioClip voice_Syukkin;
    public AudioClip voice_ok;

    enum State
    {
        NORMAL, WAVE, RUN, SIGN
    }
    State state;
    bool isEndRotate = true;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        animInfo = animator.GetCurrentAnimatorStateInfo(0);
        switch (state)
        {
            case State.NORMAL:
                Normal();
                break;
            case State.WAVE:
                //Wave();
                break;
            case State.RUN:
                Run();
                break;
            case State.SIGN:
                Sign();
                break;
        }
    }

    void Normal()
    {
        if (isLooked)
        {
            animator.SetTrigger("sign");
            audiosource.clip = voice_Yes;
            audiosource.Play();
            state = State.SIGN;
        }
        else if (normalTimer >= normalInterval && !isLooked)
        {
            int rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    animator.SetTrigger("wave");
                    audiosource.clip = voice_ok;
                    audiosource.Play();
                    state = State.WAVE;
                    break;
                case 1:
                    animator.SetBool("run", true);
                    audiosource.clip = voice_Syukkin;
                    audiosource.Play();
                    state = State.RUN;
                    break;
            }
            normalTimer = 0.0f;
        }
        normalTimer += Time.deltaTime;
    }

    public void EndWave()
    {/*
        if (animInfo.IsName("Base Layer.WaveHands") && animInfo.normalizedTime < 1.0f)
        {
            Debug.Log("end wave");
            state = State.NORMAL;
        }*/
        state = State.NORMAL;
    }

    void Run()
    {
        cube.transform.Rotate(Vector3.up,Time.deltaTime*speed);
        //Debug.Log("run");
        //Debug.Log(cube.transform.rotation.y);
    }

    void OnTriggerExit(Collider other)
    {
        if (isEndRotate && state == State.RUN)
        {
            isEndRotate = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isEndRotate && state == State.RUN)
        {
            isEndRotate = true;
            state = State.NORMAL;
            animator.SetBool("run", false);
            Debug.Log("Exit");
        }
    }

    void Sign()
    {
        signTimer += Time.deltaTime;
        if (signTimer >= signInterval)
        {
            state = State.NORMAL;
            signTimer = 0.0f;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using rnd = UnityEngine.Random;
using KModkit;

public class REPSEQ : MonoBehaviour {

    public KMAudio audio;
    public KMBombInfo bomb;
    public KMBombModule module;

    public KMSelectable[] buttons;
    public GameObject[] buttonobjects;
    public GameObject background;
    public GameObject renderjudgement;
    public GameObject[] stattexts;
    public GameObject buttonrotate;
    public SpriteRenderer judgement;

    public Material[] backgroundcolors;
    public TextMesh stat;
    public TextMesh score;
    public Sprite[] judgementlist;
    public Animator anim;
    public Animator beats;
    public Animator middle;

    int totalflashes;
    int totalinputs;
    int correctinputs;
    int incorrectinputs;
    int extrainputs;
    int onbeatinputs;
    int perfectsequences;
    int totalscore;

    int goalscore;
    bool perfect;

    bool audioon;
    bool flashing;

    List<int> flashes = new List<int>();
    List<float> timedpresses = new List<float>();

    float rotator;
    bool clockwise;
    int rotations;
    float timing;
    float animtiming;
    float currentsize;
    bool parttwo;
    int inputpart;
    int sequencesplayed = 1;
    int sequence = 0;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        stat.text = "";
        score.text = "";
        renderjudgement.transform.localScale = new Vector3(-0.05f, -0.05f, -0.05f); //Semi-debug code, just in case something stupid happens
        goalscore = rnd.Range(11, 21) * 20;
        flashing = true;
        anim = renderjudgement.GetComponent<Animator>(); //Nabs the animator components from the corresponding assets, so animations can be played correctly
        beats = stattexts[1].GetComponent<Animator>();
        middle = buttonobjects[8].GetComponent<Animator>();
        for (byte i = 0; i < buttons.Length; i++)
        {
            KMSelectable inputs = buttons[i];
            inputs.OnInteract += delegate
            {
                Inputting(inputs);
                return false;
            };
        }
    }
    void Start ()
    {
        stat.text = "Goal score";
        score.text = goalscore.ToString();
        StartCoroutine(Startup());
    }

    void Inputting(KMSelectable inputs)
    {
        audioon = true; //Enables audio since the mod now knows you've interacted
        int press = Array.IndexOf(buttons, inputs);
        buttons[press].AddInteractionPunch(0.2f);
        if (!flashing && !moduleSolved)
        {
            totalinputs++;
            if (inputpart >= flashes.Count)
            {
                Debug.LogFormat("[REPSEQ #{1}] Pressed {0} when no button was expected", press, moduleId);
                extrainputs++;
                perfect = false;
                judgement.sprite = judgementlist[9]; //MISS
            }
            else if (flashes[inputpart] == press)
            {
                Debug.LogFormat("[REPSEQ #{2}] Pressed {0} when expecting {1}, that's correct", press, flashes[inputpart], moduleId);
                correctinputs++;
                int temp = 0;
                if (Math.Abs(timing - timedpresses[inputpart]) < 0.023f) { } //FANTASTIC!
                else if (Math.Abs(timing - timedpresses[inputpart]) < 0.0445f) temp++; //EXCELLENT
                else if (Math.Abs(timing - timedpresses[inputpart]) < 0.1035f) temp += 3; //GREAT
                else if (Math.Abs(timing - timedpresses[inputpart]) < 0.1365f) temp += 5; //DECENT
                else temp += 7; //WAY OFF
                if ((timing - timedpresses[inputpart] > 0) && temp != 0) temp++; //Early/late check
                if (temp < 5) { onbeatinputs++; Debug.LogFormat("[REPSEQ #{0}] That press was also sufficiently on-beat!", moduleId); } //A GREAT or better keeps your combo in ITG, same goes here but with giving you a bonus point
                judgement.sprite = judgementlist[temp]; //Actually sets the sprite lmao
            }
            else
            {
                perfect = false;
                Debug.LogFormat("[REPSEQ #{2}] Pressed {0} when expecting {1}, that's incorrect", press, flashes[inputpart], moduleId);
                incorrectinputs++;
                judgement.sprite = judgementlist[9]; //MISS
            }
            inputpart++;
            anim.Play("Base Layer.Judgements", -1, 0f); //This animation flashes the judgement at you like in ITG, which is where all these judgements come from
        }
    }

    IEnumerator Flash0() //Sequences 1 and 5, 12 flashes, no rotations, the absolute basics
    {
        for (int i = 0; i < 12; i++)
        {
            flashes.Add(rnd.Range(0, 8));
        }
        totalflashes += 12;
        timedpresses.Add(0f);
        buttonobjects[flashes[0] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[0] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(0.3813559f);
        buttonobjects[flashes[1] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[1] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(0.6355931f);
        buttonobjects[flashes[2] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[2] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(0.8898303f);
        buttonobjects[flashes[3] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[3] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(1.1440675f);
        buttonobjects[flashes[4] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[4] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(1.5254234f);
        buttonobjects[flashes[5] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[5] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(1.7796606f);
        buttonobjects[flashes[6] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[6] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.0338978f);
        buttonobjects[flashes[7] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[7] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(2.4152537f);
        buttonobjects[flashes[8] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[8] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.6694909f);
        buttonobjects[flashes[9] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[9] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.9237281f);
        buttonobjects[flashes[10] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[10] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(3.1779653f);
        buttonobjects[flashes[11] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[11] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.5084744f;
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        StartCoroutine(InputSeq());
        yield return new WaitWhile(() => timing < -0.1542372f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0f);
    }

    IEnumerator Flash1() //Sequence 2, 14/15 flashes, no rotations
    {
        if (!parttwo)
            totalflashes++;
        for (int i = 0; i < 15; i++) //Can just ignore the last one if it's the second part, no need to add more code to just remove one entry
        {
            flashes.Add(rnd.Range(0, 8));
        }
        totalflashes += 14;
        timedpresses.Add(0f);
        buttonobjects[flashes[0] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[0] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(0.2542372f);
        buttonobjects[flashes[1] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[1] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.1271186f);
        timing -= 0.1271186f;
        timedpresses.Add(0.3813559f);
        buttonobjects[flashes[2] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[2] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.1271186f);
        timing -= 0.1271186f;
        timedpresses.Add(0.5084744f);
        buttonobjects[flashes[3] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[3] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.1271186f);
        timing -= 0.1271186f;
        timedpresses.Add(0.6355931f);
        buttonobjects[flashes[4] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[4] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(0.8898303f);
        buttonobjects[flashes[5] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[5] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(1.1440675f);
        buttonobjects[flashes[6] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[6] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(1.5254234f);
        buttonobjects[flashes[7] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[7] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(1.7796606f);
        buttonobjects[flashes[8] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[8] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.1271186f);
        timing -= 0.1271186f;
        if (!parttwo)
        {
            timedpresses.Add(1.9067792f);
            buttonobjects[flashes[9] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
            yield return new WaitWhile(() => timing < 0.1f);
            buttonobjects[flashes[9] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
            yield return new WaitWhile(() => timing < 0.1271186f);
            timing -= 0.1271186f;
            timedpresses.Add(2.0338978f);
            buttonobjects[flashes[10] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
            yield return new WaitWhile(() => timing < 0.1f);
            buttonobjects[flashes[10] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
            yield return new WaitWhile(() => timing < 0.3813559f);
            timing -= 0.3813559f;
            timedpresses.Add(2.4152537f);
            buttonobjects[flashes[11] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
            yield return new WaitWhile(() => timing < 0.1f);
            buttonobjects[flashes[11] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
            yield return new WaitWhile(() => timing < 0.2542372f);
            timing -= 0.2542372f;
            timedpresses.Add(2.6694909f);
            buttonobjects[flashes[12] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
            yield return new WaitWhile(() => timing < 0.1f);
            buttonobjects[flashes[12] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
            yield return new WaitWhile(() => timing < 0.2542372f);
            timing -= 0.2542372f;
            timedpresses.Add(2.9237281f);
            buttonobjects[flashes[13] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
            yield return new WaitWhile(() => timing < 0.1f);
            buttonobjects[flashes[13] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
            yield return new WaitWhile(() => timing < 0.2542372f);
            timing -= 0.2542372f;
            timedpresses.Add(3.1779653f);
            buttonobjects[flashes[14] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
            yield return new WaitWhile(() => timing < 0.1f);
            buttonobjects[flashes[14] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
            yield return new WaitWhile(() => timing < 0.3813559f);
            timing -= 0.3813559f;
        }
        else
        {
            yield return new WaitWhile(() => timing < 0.1271186f);
            timing -= 0.1271186f;
            timedpresses.Add(2.0338978f);
            buttonobjects[flashes[9] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
            yield return new WaitWhile(() => timing < 0.1f);
            buttonobjects[flashes[9] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
            yield return new WaitWhile(() => timing < 0.3813559f);
            timing -= 0.3813559f;
            timedpresses.Add(2.4152537f);
            buttonobjects[flashes[10] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
            yield return new WaitWhile(() => timing < 0.1f);
            buttonobjects[flashes[10] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
            yield return new WaitWhile(() => timing < 0.2542372f);
            timing -= 0.2542372f;
            timedpresses.Add(2.6694909f);
            buttonobjects[flashes[11] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
            yield return new WaitWhile(() => timing < 0.1f);
            buttonobjects[flashes[11] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
            yield return new WaitWhile(() => timing < 0.2542372f);
            timing -= 0.2542372f;
            timedpresses.Add(2.9237281f);
            buttonobjects[flashes[12] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
            yield return new WaitWhile(() => timing < 0.1f);
            buttonobjects[flashes[12] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
            yield return new WaitWhile(() => timing < 0.2542372f);
            timing -= 0.2542372f;
            timedpresses.Add(3.1779653f);
            buttonobjects[flashes[13] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
            yield return new WaitWhile(() => timing < 0.1f);
            buttonobjects[flashes[13] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
            yield return new WaitWhile(() => timing < 0.3813559f);
            timing -= 0.3813559f;
        }
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.5084744f;
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        StartCoroutine(InputSeq());
        yield return new WaitWhile(() => timing < -0.1542372f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < -0.1271186f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < -0.0271186f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0f);
    }

    IEnumerator Flash2() //Sequences 3 and 9, 12 moves, no rotations
    {
        for (int i = 0; i < 12; i++)
        {
            flashes.Add(rnd.Range(0, 8));
        }
        totalflashes += 12;
        timedpresses.Add(0f);
        middle.Play("Base Layer." + flashes[0].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(0.3813559f);
        middle.Play("Base Layer." + flashes[1].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(0.6355931f);
        middle.Play("Base Layer." + flashes[2].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(0.8898303f);
        middle.Play("Base Layer." + flashes[3].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(1.1440675f);
        middle.Play("Base Layer." + flashes[4].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(1.5254234f);
        middle.Play("Base Layer." + flashes[5].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(1.7796606f);
        middle.Play("Base Layer." + flashes[6].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.0338978f);
        middle.Play("Base Layer." + flashes[7].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(2.4152537f);
        middle.Play("Base Layer." + flashes[8].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.6694909f);
        middle.Play("Base Layer." + flashes[9].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.9237281f);
        middle.Play("Base Layer." + flashes[10].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(3.1779653f);
        middle.Play("Base Layer." + flashes[11].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        for (int i = 0; i < 9; i++)
        {
            if (i != 4) buttonobjects[i * 2].transform.localScale = new Vector3(0.025f, 0.01f, 0.025f);
        }
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.5084744f;
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        StartCoroutine(InputSeq());
        yield return new WaitWhile(() => timing < -0.1542372f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0f);
    }

    IEnumerator Flash3() //Sequence 4, 13 moves, no rotations
    {
        for (int i = 0; i < 13; i++)
        {
            flashes.Add(rnd.Range(0, 8));
        }
        totalflashes += 13;
        timedpresses.Add(0f);
        middle.Play("Base Layer." + flashes[0].ToString(), -1, 0f);
        if (parttwo)
        {
            yield return new WaitWhile(() => timing < 0.3813559f);
            timing -= 0.3813559f;
            timedpresses.Add(0.3813559f);
            middle.Play("Base Layer." + flashes[1].ToString(), -1, 0f);
            yield return new WaitWhile(() => timing < 0.2542372f);
            timing -= 0.2542372f;
        }
        else
        {
            yield return new WaitWhile(() => timing < 0.5084745f);
            timing -= 0.5084745f;
            timedpresses.Add(0.5084745f);
            middle.Play("Base Layer." + flashes[1].ToString(), -1, 0f);
            yield return new WaitWhile(() => timing < 0.1271186f);
            timing -= 0.1271186f;
        }
        timedpresses.Add(0.6355931f);
        middle.Play("Base Layer." + flashes[2].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(0.8898303f);
        middle.Play("Base Layer." + flashes[3].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(1.1440675f);
        middle.Play("Base Layer." + flashes[4].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(1.5254234f);
        middle.Play("Base Layer." + flashes[5].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(1.7796606f);
        middle.Play("Base Layer." + flashes[6].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.0338978f);
        middle.Play("Base Layer." + flashes[7].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(2.4152537f);
        middle.Play("Base Layer." + flashes[8].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.6694909f);
        middle.Play("Base Layer." + flashes[9].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.9237281f);
        middle.Play("Base Layer." + flashes[10].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(3.1779653f);
        middle.Play("Base Layer." + flashes[11].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.1271186f);
        timing -= 0.1271186f;
        timedpresses.Add(3.3050839f);
        middle.Play("Base Layer." + flashes[12].ToString(), -1, 0f);
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        for (int i = 0; i < 9; i++)
        {
            if (i != 4) buttonobjects[i * 2].transform.localScale = new Vector3(0.025f, 0.01f, 0.025f);
        }
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.5084744f;
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        StartCoroutine(InputSeq());
        yield return new WaitWhile(() => timing < -0.1542372f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        if (parttwo)
        {
            yield return new WaitWhile(() => timing < -0.1271186f);
            for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
            yield return new WaitWhile(() => timing < -0.0271186f);
            for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        }
        yield return new WaitWhile(() => timing < 0f);
    }

    IEnumerator Flash4() //Sequence 6, 12 flashes with 8 consistent rotations
    {
        for (int i = 0; i < 12; i++)
        {
            flashes.Add(rnd.Range(0, 8));
        }
        totalflashes += 12;
        timedpresses.Add(0f);
        buttonobjects[flashes[0] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[0] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(0.3813559f);
        buttonobjects[flashes[1] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[1] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(0.6355931f);
        buttonobjects[flashes[2] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[2] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.1271186f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(0.8898303f);
        buttonobjects[flashes[3] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[3] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(1.1440675f);
        buttonobjects[flashes[4] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[4] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.1271186f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(1.5254234f);
        buttonobjects[flashes[5] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[5] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        timing -= 0.2542372f;
        timedpresses.Add(1.7796606f);
        buttonobjects[flashes[6] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[6] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.0338978f);
        buttonobjects[flashes[7] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[7] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(2.4152537f);
        buttonobjects[flashes[8] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[8] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.6694909f);
        buttonobjects[flashes[9] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[9] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.1271186f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.9237281f);
        buttonobjects[flashes[10] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[10] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(3.1779653f);
        buttonobjects[flashes[11] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[11] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.1271186f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        timing -= 0.5084744f;
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        StartCoroutine(InputSeq());
        yield return new WaitWhile(() => timing < -0.1542372f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0f);
    }

    IEnumerator Flash5() //Sequence 7, 12 inverted flashes, no rotations
    {
        for (int i = 0; i < 12; i++)
        {
            flashes.Add(rnd.Range(0, 8));
        }
        totalflashes += 12;
        timedpresses.Add(0f);
        for (int i = 0; i < 9; i++) if (i != flashes[0]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(0.3813559f);
        for (int i = 0; i < 9; i++) if (i != flashes[1]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(0.6355931f);
        for (int i = 0; i < 9; i++) if (i != flashes[2]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(0.8898303f);
        for (int i = 0; i < 9; i++) if (i != flashes[3]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(1.1440675f);
        for (int i = 0; i < 9; i++) if (i != flashes[4]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(1.5254234f);
        for (int i = 0; i < 9; i++) if (i != flashes[5]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(1.7796606f);
        for (int i = 0; i < 9; i++) if (i != flashes[6]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.0338978f);
        for (int i = 0; i < 9; i++) if (i != flashes[7]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(2.4152537f);
        for (int i = 0; i < 9; i++) if (i != flashes[8]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.6694909f);
        for (int i = 0; i < 9; i++) if (i != flashes[9]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.9237281f);
        for (int i = 0; i < 9; i++) if (i != flashes[10]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(3.1779653f);
        for (int i = 0; i < 9; i++) if (i != flashes[11]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.5084744f;
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        StartCoroutine(InputSeq());
        yield return new WaitWhile(() => timing < -0.1542372f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0f);
    }

    IEnumerator Flash6() //Sequence 8, 12 inverted flashes with 8 consistent rotations
    {
        for (int i = 0; i < 12; i++)
        {
            flashes.Add(rnd.Range(0, 8));
        }
        totalflashes += 12;
        timedpresses.Add(0f);
        for (int i = 0; i < 9; i++) if (i != flashes[0]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(0.3813559f);
        for (int i = 0; i < 9; i++) if (i != flashes[1]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(0.6355931f);
        for (int i = 0; i < 9; i++) if (i != flashes[2]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.1271186f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(0.8898303f);
        for (int i = 0; i < 9; i++) if (i != flashes[3]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(1.1440675f);
        for (int i = 0; i < 9; i++) if (i != flashes[4]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.1271186f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(1.5254234f);
        for (int i = 0; i < 9; i++) if (i != flashes[5]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        timing -= 0.2542372f;
        timedpresses.Add(1.7796606f);
        for (int i = 0; i < 9; i++) if (i != flashes[6]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.0338978f);
        for (int i = 0; i < 9; i++) if (i != flashes[7]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(2.4152537f);
        for (int i = 0; i < 9; i++) if (i != flashes[8]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.6694909f);
        for (int i = 0; i < 9; i++) if (i != flashes[9]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.1271186f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.9237281f);
        for (int i = 0; i < 9; i++) if (i != flashes[10]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(3.1779653f);
        for (int i = 0; i < 9; i++) if (i != flashes[11]) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.1271186f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        timing -= 0.5084744f;
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        StartCoroutine(InputSeq());
        yield return new WaitWhile(() => timing < -0.1542372f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0f);
    }

    IEnumerator Flash7() //Sequence 10, 12 flashes with 14 inconsistent rotations, hardest one to read
    {
        for (int i = 0; i < 12; i++)
        {
            flashes.Add(rnd.Range(0, 8));
        }
        totalflashes += 12;
        timedpresses.Add(0f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        buttonobjects[flashes[0] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[0] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(0.3813559f);
        buttonobjects[flashes[1] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[1] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(0.6355931f);
        buttonobjects[flashes[2] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[2] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.1271186f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(0.8898303f);
        buttonobjects[flashes[3] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[3] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(1.1440675f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        buttonobjects[flashes[4] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[4] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(1.5254234f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        buttonobjects[flashes[5] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[5] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(1.7796606f);
        buttonobjects[flashes[6] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[6] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.1271186f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.0338978f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        buttonobjects[flashes[7] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[7] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        timedpresses.Add(2.4152537f);
        buttonobjects[flashes[8] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[8] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.6694909f);
        buttonobjects[flashes[9] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[9] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.1271186f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(2.9237281f);
        buttonobjects[flashes[10] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[10] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.2542372f;
        timedpresses.Add(3.1779653f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        buttonobjects[flashes[11] * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        buttonobjects[flashes[11] * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0.3813559f);
        timing -= 0.3813559f;
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.2542372f);
        timing -= 0.5084744f;
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
        StartCoroutine(InputSeq());
        yield return new WaitWhile(() => timing < -0.1542372f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < -0.1271186f);
        if (rnd.Range(0, 2) == 1) clockwise = true;
        StartCoroutine(Rotating());
        yield return new WaitWhile(() => timing < 0f);
    }

    IEnumerator Rotating() //In code instead of with animations, because I haven't found if you can make animations use positions relative to its position at the start of the animation. Also each rotation is 45 degrees
    {
        if (clockwise) rotator = 1f; else rotator = -1f;
        rotations += Convert.ToInt32(rotator);
        clockwise = false;
        animtiming = 0f;
        buttonrotate.transform.Rotate(0f, 9f * rotator, 0f, Space.Self);
        buttonobjects[8].transform.Rotate(0f, 9f * rotator, 0f, Space.Self); //Had to make it like this so I don't need to redo EVERY single animation clip I made (the middle button is misaligned)
        buttonobjects[9].transform.Rotate(0f, 9f * rotator, 0f, Space.Self);
        yield return new WaitWhile(() => animtiming < 0.01f);
        buttonrotate.transform.Rotate(0f, 8f * rotator, 0f, Space.Self);
        buttonobjects[8].transform.Rotate(0f, 8f * rotator, 0f, Space.Self);
        buttonobjects[9].transform.Rotate(0f, 8f * rotator, 0f, Space.Self);
        yield return new WaitWhile(() => animtiming < 0.02f);
        buttonrotate.transform.Rotate(0f, 7f * rotator, 0f, Space.Self);
        buttonobjects[8].transform.Rotate(0f, 7f * rotator, 0f, Space.Self);
        buttonobjects[9].transform.Rotate(0f, 7f * rotator, 0f, Space.Self);
        yield return new WaitWhile(() => animtiming < 0.03f);
        buttonrotate.transform.Rotate(0f, 6f * rotator, 0f, Space.Self);
        buttonobjects[8].transform.Rotate(0f, 6f * rotator, 0f, Space.Self);
        buttonobjects[9].transform.Rotate(0f, 6f * rotator, 0f, Space.Self);
        yield return new WaitWhile(() => animtiming < 0.04f);
        buttonrotate.transform.Rotate(0f, 5f * rotator, 0f, Space.Self);
        buttonobjects[8].transform.Rotate(0f, 5f * rotator, 0f, Space.Self);
        buttonobjects[9].transform.Rotate(0f, 5f * rotator, 0f, Space.Self);
        yield return new WaitWhile(() => animtiming < 0.05f);
        buttonrotate.transform.Rotate(0f, 4f * rotator, 0f, Space.Self);
        buttonobjects[8].transform.Rotate(0f, 4f * rotator, 0f, Space.Self);
        buttonobjects[9].transform.Rotate(0f, 4f * rotator, 0f, Space.Self);
        yield return new WaitWhile(() => animtiming < 0.06f);
        buttonrotate.transform.Rotate(0f, 3f * rotator, 0f, Space.Self);
        buttonobjects[8].transform.Rotate(0f, 3f * rotator, 0f, Space.Self);
        buttonobjects[9].transform.Rotate(0f, 3f * rotator, 0f, Space.Self);
        yield return new WaitWhile(() => animtiming < 0.07f);
        buttonrotate.transform.Rotate(0f, 2f * rotator, 0f, Space.Self);
        buttonobjects[8].transform.Rotate(0f, 2f * rotator, 0f, Space.Self);
        buttonobjects[9].transform.Rotate(0f, 2f * rotator, 0f, Space.Self);
        yield return new WaitWhile(() => animtiming < 0.08f);
        buttonrotate.transform.Rotate(0f, 1f * rotator, 0f, Space.Self);
        buttonobjects[8].transform.Rotate(0f, 1f * rotator, 0f, Space.Self);
        buttonobjects[9].transform.Rotate(0f, 1f * rotator, 0f, Space.Self);
        yield return new WaitWhile(() => animtiming < 0.09f);
    }

    IEnumerator InputSeq()
    {
        perfect = true;
        inputpart = 0;
        flashing = false;
        yield return new WaitWhile(() => timing < 3.559321f);
        if (inputpart != flashes.Count) perfect = false; //Perfect sequences only occur if you pressed EVERY button that flashed
        flashing = true;
        flashes.Clear(); //Empties the lists so the next sequence can start fresh
        timedpresses.Clear();
        if (perfect) { Debug.LogFormat("[REPSEQ #{0}] Perfect sequence!", moduleId); perfectsequences++; }
        if (audioon) Debug.LogFormat("[REPSEQ #{1}] Total score after that sequence is {0}", totalscore, moduleId); //A sequence without interactions doesn't need to update the score
        timing -= 3.559321f;
        buttonrotate.transform.Rotate(0f, -45f * rotations, 0f, Space.Self); //Resets rotations for sequences that manipulate it, to keep things accurate
        buttonobjects[8].transform.Rotate(0f, -45f * rotations, 0f, Space.Self);
        buttonobjects[9].transform.Rotate(0f, -45f * rotations, 0f, Space.Self);
        rotations = 0;
        if (!parttwo) //This entire if-else statement determines whether or not this is the first or second time passing through this, and responds appropriately
        {
            parttwo = true; //Next time around, it'll go through the other one
            switch (sequence)
            {
                case 0: case 4: case 5: case 6: case 7: case 9:
                    for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
                    yield return new WaitWhile(() => timing < 0.1f);
                    for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
                    yield return new WaitWhile(() => timing < 0.2542372f);
                    timing -= 0.5084744f;
                    for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
                    yield return new WaitWhile(() => timing < -0.1542372f);
                    for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
                    yield return new WaitWhile(() => timing < 0f);
                    break;
                case 1:
                    for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
                    yield return new WaitWhile(() => timing < 0.1f);
                    for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
                    yield return new WaitWhile(() => timing < 0.2542372f);
                    timing -= 0.5084744f;
                    for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
                    yield return new WaitWhile(() => timing < -0.1542372f);
                    for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
                    yield return new WaitWhile(() => timing < -0.1271186f);
                    for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[1];
                    yield return new WaitWhile(() => timing < -0.0271186f);
                    for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
                    yield return new WaitWhile(() => timing < 0f);
                    break;
                case 2: case 8:
                    for (int i = 0; i < 9; i++)
                    {
                        if (i != 4) buttonobjects[i * 2].transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
                    }
                    yield return new WaitWhile(() => timing < 0.2542372f);
                    timing -= 0.5084744f;
                    middle.Play("Base Layer.4", -1, 0f);
                    yield return new WaitWhile(() => timing < 0f);
                    break;
                case 3:
                    for (int i = 0; i < 9; i++)
                    {
                        if (i != 4) buttonobjects[i * 2].transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
                    }
                    yield return new WaitWhile(() => timing < 0.2542372f);
                    timing -= 0.5084744f;
                    middle.Play("Base Layer.4", -1, 0f);
                    for (int i = 0; i < 9; i++)
                    {
                        if (i != 4) buttonobjects[i * 2].transform.localScale = new Vector3(0.025f, 0.01f, 0.025f);
                    }
                    yield return new WaitWhile(() => timing < -0.2042372f);
                    for (int i = 0; i < 9; i++)
                    {
                        if (i != 4) buttonobjects[i * 2].transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
                    }
                    yield return new WaitWhile(() => timing < 0f);
                    break;
                default: break;
            }
            switch (sequence) //Goes back to the same sequence it started from
            {
                case 0: case 4: StartCoroutine(Flash0()); break;
                case 1: StartCoroutine(Flash1()); break;
                case 2: case 8: StartCoroutine(Flash2()); break;
                case 3: StartCoroutine(Flash3()); break;
                case 5: StartCoroutine(Flash4()); break;
                case 6: StartCoroutine(Flash5()); break;
                case 7: StartCoroutine(Flash6()); break;
                case 9: StartCoroutine(Flash7()); break;
                default: break;
            }
            audioon = false;
        }
        else //Deciding the next one to play
        {
            parttwo = false;
            if (totalscore >= goalscore) { StartCoroutine(EndAnim()); StopCoroutine(InputSeq()); } //WIN
            else if (sequencesplayed == 4) { StartCoroutine(Breaktime()); StopCoroutine(InputSeq()); } //4 sequences played, therefore it's break time
            else
            {
                if (totalscore <= 0) sequence = 0;
                else sequence = totalscore / (goalscore / 10);
                switch (sequence)
                {
                    case 0:
                    case 4:
                        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[4];
                        yield return new WaitWhile(() => timing < 0.1f);
                        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
                        yield return new WaitWhile(() => timing < 0.5084745f);
                        timing = 0f;
                        StartCoroutine(Flash0());
                        background.GetComponent<Renderer>().material = backgroundcolors[0];
                        break;
                    case 1:
                        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[4];
                        yield return new WaitWhile(() => timing < 0.1f);
                        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
                        yield return new WaitWhile(() => timing < 0.5084745f);
                        timing = 0f;
                        StartCoroutine(Flash1());
                        background.GetComponent<Renderer>().material = backgroundcolors[0];
                        break;
                    case 2:
                    case 8:
                        for (int i = 0; i < 9; i++)
                        {
                            if (i != 4) buttonobjects[i * 2].transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
                        }
                        yield return new WaitWhile(() => timing < 0.5084745f);
                        timing = 0f;
                        StartCoroutine(Flash2());
                        background.GetComponent<Renderer>().material = backgroundcolors[1];
                        break;
                    case 3:
                        for (int i = 0; i < 9; i++)
                        {
                            if (i != 4) buttonobjects[i * 2].transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
                        }
                        yield return new WaitWhile(() => timing < 0.5084745f);
                        timing = 0f;
                        StartCoroutine(Flash3());
                        background.GetComponent<Renderer>().material = backgroundcolors[1];
                        break;
                    case 5:
                        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[4];
                        yield return new WaitWhile(() => timing < 0.1f);
                        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
                        yield return new WaitWhile(() => timing < 0.5084745f);
                        timing = 0f;
                        StartCoroutine(Flash4());
                        background.GetComponent<Renderer>().material = backgroundcolors[2];
                        break;
                    case 6:
                        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[4];
                        yield return new WaitWhile(() => timing < 0.1f);
                        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
                        yield return new WaitWhile(() => timing < 0.5084745f);
                        timing = 0f;
                        StartCoroutine(Flash5());
                        background.GetComponent<Renderer>().material = backgroundcolors[5];
                        break;
                    case 7:
                        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[4];
                        yield return new WaitWhile(() => timing < 0.1f);
                        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
                        yield return new WaitWhile(() => timing < 0.5084745f);
                        timing = 0f;
                        StartCoroutine(Flash6());
                        background.GetComponent<Renderer>().material = backgroundcolors[4];
                        break;
                    case 9:
                        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[4];
                        yield return new WaitWhile(() => timing < 0.1f);
                        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
                        yield return new WaitWhile(() => timing < 0.5084745f);
                        timing = 0f;
                        StartCoroutine(Flash7());
                        background.GetComponent<Renderer>().material = backgroundcolors[3];
                        break;
                    default: break;
                }
                if (audioon) audio.PlaySoundAtTransform("SEQ" + (sequence + 1).ToString(), transform); //No interactions in the second half makes the audio go silent
                if (audioon) sequencesplayed++; //It also delays the breaktime
                audioon = false;
            }
        }
    }

    IEnumerator Startup() //Yeah this is the startup animation
    {
        audio.PlaySoundAtTransform("Start1", transform);
        for (int i = 0; i < 8; i++)
        {
            animtiming = 0f;
            beats.Play("Base Layer.beats", -1, 0f);
            yield return new WaitWhile(() => animtiming < 1.016949f);
        }
        audio.PlaySoundAtTransform("Start2", transform);
        stat.text = "Remember the";
        score.text = "SEQ";
        for (int i = 0; i < 8; i++)
        {
            animtiming = 0f;
            beats.Play("Base Layer.beats", -1, 0f);
            yield return new WaitWhile(() => animtiming < 1.016949f);
        }
        audio.PlaySoundAtTransform("Start3", transform);
        stat.text = "Excessive presses";
        score.text = "BAD";
        for (int i = 0; i < 8; i++)
        {
            animtiming = 0f;
            beats.Play("Base Layer.beats", -1, 0f);
            yield return new WaitWhile(() => animtiming < 1.016949f);
        }
        audio.PlaySoundAtTransform("Start4", transform);
        stat.text = "Good luck!";
        score.text = "HF";
        for (int i = 0; i < 7; i++)
        {
            animtiming = 0f;
            beats.Play("Base Layer.beats", -1, 0f);
            yield return new WaitWhile(() => animtiming < 1.016949f);
        }
        animtiming = 0f;
        beats.Play("Base Layer.beats", -1, 0f);
        yield return new WaitWhile(() => animtiming < 0.5084745f);
        stat.text = "";
        score.text = "";
        yield return new WaitWhile(() => animtiming < 1.016949f);
        timing = 0f;
        audio.PlaySoundAtTransform("SEQ1", transform);
        StartCoroutine(Flash0()); //First sequence will ALWAYS be the first one, since your score starts at 0
    }

    IEnumerator Breaktime() //Plays every 4 sequences (AKA every 8 input sections)
    {
        if (totalscore <= 0) sequence = 0;
        else sequence = totalscore / (goalscore / 10);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[4];
        yield return new WaitWhile(() => timing < 0.1f);
        for (int i = 0; i < 9; i++) buttonobjects[i * 2].GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => timing < 0.5084745f);
        audio.PlaySoundAtTransform("Break", transform);
        stat.text = "Current score:";
        score.text = totalscore.ToString();
        for (int i = 0; i < 32; i++)
        {
            animtiming = 0f;
            beats.Play("Base Layer.beats", -1, 0f);
            yield return new WaitWhile(() => animtiming < 1.016949f);
            stat.color = new Color(1f, 1f, 1f, (32f - Convert.ToSingle(i))/32f); //Fades the text out over time to transition better into the next sequence
            score.color = new Color(1f, 1f, 1f, (32f - Convert.ToSingle(i))/32f);
        }
        stat.text = "";
        score.text = "";
        stat.color = new Color(1f, 1f, 1f, 1f); //So I don't need to reset the transparency anywhere else
        score.color = new Color(1f, 1f, 1f, 1f);
        timing = 0f;
        switch (sequence) //Definitely coulda made this more optimal, but it works
        {
            case 0: case 4: StartCoroutine(Flash0()); break;
            case 1: StartCoroutine(Flash1()); break;
            case 2: case 8: StartCoroutine(Flash2()); for (int i = 0; i < 9; i++) if (i != 4) buttonobjects[i * 2].transform.localScale = new Vector3(0.001f, 0.001f, 0.001f); break;
            case 3: StartCoroutine(Flash3()); for (int i = 0; i < 9; i++) if (i != 4) buttonobjects[i * 2].transform.localScale = new Vector3(0.001f, 0.001f, 0.001f); break;
            case 5: StartCoroutine(Flash4()); break;
            case 6: StartCoroutine(Flash5()); break;
            case 7: StartCoroutine(Flash6()); break;
            case 9: StartCoroutine(Flash7()); break;
            default: break;
        }
        audio.PlaySoundAtTransform("SEQ" + (sequence + 1).ToString(), transform);
        sequencesplayed = 1; //Resets the seq counter so that the next 4th sequence will call back to this function
    }

    IEnumerator EndAnim() //Self-explanatory, animation for when your score exceeds the goal
    {
        yield return new WaitWhile(() => timing < 0.5084745f); //Almost made the sounds overlap before I added this line, don't remove this
        audio.PlaySoundAtTransform("End", transform);
        animtiming = 0f;
        stat.text = "Total flashes:";
        score.text = totalflashes.ToString();
        for (int i = 0; i < 31; i++)
        {
            switch (i) //Displays stats for every 1/8 of this part
            {
                case 4: stat.text = "Total inputs:"; score.text = totalinputs.ToString(); break;
                case 8: stat.text = "Correct inputs:"; score.text = correctinputs.ToString(); break;
                case 12: stat.text = "Incorrect inputs:"; score.text = incorrectinputs.ToString(); break;
                case 16: stat.text = "Extraneous inputs:"; score.text = extrainputs.ToString(); break;
                case 20: stat.text = "On-beat inputs:"; score.text = onbeatinputs.ToString(); break;
                case 24: stat.text = "PERFECT SEQUENCES:"; score.text = perfectsequences.ToString(); break;
                case 28: stat.text = "TOTAL SCORE:"; score.text = totalscore.ToString(); break;
                default: break; //If it's not a multiple of 4, just skip by this section as if nothing's here
            }
            beats.Play("Base Layer.beats", -1, 0f);
            yield return new WaitWhile(() => animtiming < 1.016949f * Convert.ToSingle(i+1));
        }
        beats.Play("Base Layer.beats", -1, 0f);
        yield return new WaitWhile(() => animtiming < 32.0338983f);
        stat.text = "";
        score.text = "";
        background.GetComponent<Renderer>().material = backgroundcolors[6];
        yield return new WaitWhile(() => animtiming < 32.5423728f);
        score.text = "END"; //Somewhat inspired by the end of Mawaru and the Magician's Curse, taro please stop making bangers
        moduleSolved = true;
        flashing = true;
        module.HandlePass();
    }

    void Update ()
    {
        timing += Time.deltaTime;
        animtiming += Time.deltaTime;
        totalscore = (correctinputs*2) + onbeatinputs + (perfectsequences * 20) - incorrectinputs - (extrainputs * 4); //Constantly updates score so it doesn't need to change it at specific moments
	}
}

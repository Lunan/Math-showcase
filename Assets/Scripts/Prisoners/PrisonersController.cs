using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrisonersController : MonoBehaviour
{
    public GameObject room;

    public GameObject canvas;

    public GameObject boxPrefab;
    public GameObject prisonerPrefab;

    public Slider simulationSpeed;
    public GameObject startUI;
    public TMP_InputField numberOfPrisoners;
    public TMP_InputField guessesPerPrisoner;
    public TMP_Text guessesUsedText;
    public TMP_Text cyclesText;
    public GameObject resetButton;

    private BoxControl[] boxes;
    private GameObject[] prisoners;

    private int guesses;
    private bool simulating;

    int sqr;
    int number;

    public void StartGame()
    {
        number = int.Parse(numberOfPrisoners.text);
        guesses = int.Parse(guessesPerPrisoner.text);
        sqr = Mathf.CeilToInt(Mathf.Sqrt(number));
        boxes = new BoxControl[number];
        prisoners = new GameObject[number];
        List<int> numbersToAssign = new List<int>();
        for(int n = 0; n < number; n++)
        {
            numbersToAssign.Add(n);
        }
        for(int n = 0; n < number; n++)
        {
            GameObject box = Instantiate(boxPrefab,canvas.transform);
            boxes[n] = box.GetComponent<BoxControl>();
            box.transform.localPosition = new Vector3(-200 + (n % sqr) * (400f / sqr), -200 + (n / sqr) * (400f / sqr));
            box.transform.localScale = new Vector3(4f / sqr, 4f / sqr, 4f / sqr);
            boxes[n].SetBoxNumber(n);
            int random = Random.Range(0, numbersToAssign.Count);
            boxes[n].SetPaperNumber(numbersToAssign[random]);
            numbersToAssign.RemoveAt(random);
            GameObject prisoner = Instantiate(prisonerPrefab,canvas.transform);
            prisoners[n] = prisoner;
            prisoner.transform.localPosition = new Vector3(-600 + (n % sqr) * (400f / sqr), -200 + (n / sqr) * (400f / sqr));
            prisoner.transform.localScale = new Vector3(4f / sqr, 4f / sqr, 4f / sqr);
            prisoner.GetComponentInChildren<TMP_Text>().text = n.ToString();
        }
        startUI.SetActive(false);
        room.SetActive(true);
    }

    public void ResetSimulation()
    {
        resetButton.SetActive(false);
        inKnownCycle = new List<int>();
        cyclesText.text = "";
        doingPrisoner = -1;
        startNextPrisoner = false;
        resetRoom = true;
        openBox = 0;
        usedGuesses = 0;
        guessesUsedText.text = "Guesses used: " + usedGuesses;
        List<int> numbersToAssign = new List<int>();
        for (int n = 0; n < number; n++)
        {
            numbersToAssign.Add(n);
        }
        for (int n = 0; n < number; n++)
        {
            int random = Random.Range(0, numbersToAssign.Count);
            boxes[n].SetPaperNumber(numbersToAssign[random]);
            boxes[n].CloseBox();
            numbersToAssign.RemoveAt(random);
            prisoners[n].transform.localPosition = new Vector3(-600 + (n % sqr) * (400f / sqr), -200 + (n / sqr) * (400f / sqr));
        }
    }

    public void ToMenuClick()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Simulate()
    {
        simulating = true;
        StartCoroutine(Simulation()); 
    }

    IEnumerator Simulation()
    {
        while (simulating)
        {
            yield return new WaitForSeconds(2-simulationSpeed.value);
            NextStep();
        }
    }

    int doingPrisoner = -1;
    bool startNextPrisoner = false;
    bool resetRoom = true;
    int openBox;
    int usedGuesses = 0;

    List<int> inKnownCycle = new List<int>();
    List<int> currectCycle = new List<int>();

    void NextStep()
    {
        if (resetRoom)
        {
            foreach(BoxControl box in boxes)
            {
                box.CloseBox();
            }
            resetRoom = false;
            startNextPrisoner = true;
            currectCycle = new List<int>();
        }
        else if (startNextPrisoner)
        {
            doingPrisoner++;
            if (doingPrisoner == prisoners.Length)
            {
                simulating = false;
                Debug.Log("Simulation done! \n The prisoners win!");
                resetButton.SetActive(true);
                return;
            }
            else
            {
                prisoners[doingPrisoner].transform.localPosition = new Vector3(0, 215);
                openBox = doingPrisoner;
                startNextPrisoner = false;
                usedGuesses = 0;
            }
        }
        else
        {
            boxes[openBox].OpenBox();
            currectCycle.Add(openBox);
            if (boxes[openBox].paperNumber == doingPrisoner)
            {
                resetRoom = true;
                prisoners[doingPrisoner].transform.localPosition = new Vector3(200 + (doingPrisoner % sqr) * (400f / sqr), -200 + (doingPrisoner / sqr) * (400f / sqr));
                guessesUsedText.text = "Guesses used: " + (usedGuesses+1);
                if (!inKnownCycle.Contains(currectCycle[0]))
                {
                    string toAdd = "";
                    foreach (int n in currectCycle)
                    {
                        toAdd += n + " --> ";
                        inKnownCycle.Add(n);
                    }
                    cyclesText.text += toAdd+"\n\n";
                }
            }
            else
            {
                usedGuesses++;
                openBox = boxes[openBox].paperNumber;
                guessesUsedText.text = "Guesses used: " + usedGuesses;
                if (usedGuesses >= guesses)
                {
                    simulating = false;
                    Debug.Log("Simulation done! \n The prisoners lose!");
                    resetButton.SetActive(true);
                    return;
                }
            }
        }
    }

}

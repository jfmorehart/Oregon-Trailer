using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HuntManager : MonoBehaviour
{
    [SerializeField]
    private GameObject huntScene;
    public static HuntManager instance;

    public TMP_Text threeStarTimeText;
    public TMP_Text twoStarTimeText;

    public TMP_Text timeEarnedText;
    public GameObject oneStar, twoStar, ThreeStar;

    bool goToGarageScreenOnComplete = false;
    int starsFromLevel = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //instantiate from this prefab
    public StarEarnedVFX starEarnedVFX_Prefab;
    public GameObject starResourceUIObj;
    public void createStarVFX(GameObject star)
    {
        //instantiate it under the mom
        GameObject starVfx = Instantiate(starEarnedVFX_Prefab.gameObject, starResourceUIObj.transform);
        starVfx.GetComponent<Transform>().position = star.transform.position;
        starVfx.GetComponent<StarEarnedVFX>().startPos = star.transform.position;
        starVfx.GetComponent<StarEarnedVFX>().endPos = starResourceUIObj.transform.position;
        //Instantiate(new GameObject("Start"+ star.transform.parent.name), starVfx.GetComponent<StarEarnedVFX>().startPos, quaternion.identity);
        //Instantiate(new GameObject("End " + star.transform.parent.name), starVfx.GetComponent<StarEarnedVFX>().endPos, quaternion.identity);

    }


    IEnumerator ButtonTimer(float seconds)
    {
        buttonUnavailableObject.SetActive(true);
        float timer = 0;
        while (timer < seconds)
        {
            timer+=Time.deltaTime;
            if (timer >= seconds)
            {
                buttonUnavailableObject.SetActive(false);
                break;
            }
            yield return null;
        }

    }

    public GameObject buttonUnavailableObject; // the object will set active on initiation, will turn off after 5 seconds
    public void displayHunt(bool goToGarageScreenOnComplete, float timeEarned, float twoStarTime, float threeStarTime, int starsEarned)
    {
        //--------------------------------------------------------level has ended, show stats

        huntScene.SetActive(true);
        // button unavailable
        StartCoroutine(ButtonTimer(4));
        
        //one star is active automatically
        Debug.Log("Stars earned " + starsEarned);
        oneStar.SetActive(true);
        // create vfx here
        //createStarVFX(oneStar);
        if(starsEarned==1)
            StartCoroutine(starBehaviorRoutine(1));
        if (starsEarned == 2)
        {
            StartCoroutine(starBehaviorRoutine(2));
            //create vfx here
        }
        if (starsEarned == 3)
        {
            StartCoroutine(starBehaviorRoutine(3));
            // create vfx here
        }
        
        this.goToGarageScreenOnComplete = goToGarageScreenOnComplete;
        starsFromLevel = starsEarned;
        timeEarnedText.text = (Math.Floor(timeEarned / 60) % 60).ToString("00") + ":" + Convert.ToInt32(timeEarned % 60).ToString("00") ; //TimeSpan.FromSeconds(timeEarned).ToString("hh':'mm':'ss");//string.Format("{0:t}", TimeSpan.FromSeconds(timeEarned));

        twoStarTimeText.text = (Math.Floor(twoStarTime / 60) % 60).ToString("00") + ":" + Convert.ToInt32(twoStarTime % 60).ToString("00");//string.Format("Two Star Time: {0:t}", twoStarTime );
        threeStarTimeText.text = (Math.Floor(threeStarTime / 60) % 60).ToString("00") + ":" + Convert.ToInt32(threeStarTime % 60).ToString("00");//string.Format("Three Star Time: {0:t}", threeStarTime);


        //hideHunt();
    }

    IEnumerator starBehaviorRoutine(int starsEarned)
    {
        oneStar.SetActive(true);
        yield return new WaitForSeconds(0.25f);

        createStarVFX(oneStar);
        if (starsEarned==1)
        {
            ThreeStar.transform.GetComponent<Image>().color = new Color(0.9882354f, 0.6941177f, 0.2313726f, 0);//orange: 0.9882354f 0.6941177f 0.2313726f
            twoStar.transform.GetComponent<Image>().color = new Color(0.9882354f, 0.6941177f, 0.2313726f, 0);
        }
        if (starsEarned == 2)
        {
            ThreeStar.transform.GetComponent<Image>().color = new Color(0.9882354f, 0.6941177f, 0.2313726f, 0);//orange: 0.9882354f 0.6941177f 0.2313726f
            twoStar.transform.GetComponent<Image>().color = new Color(0.9882354f, 0.6941177f, 0.2313726f, 1);
            twoStar.SetActive(true);         
            yield return new WaitForSeconds(0.25f);
            createStarVFX(twoStar);



        }
        else
        {
            //three stars
            ThreeStar.SetActive(true);
            twoStar.SetActive(true);//both should be set active
            //PLEASE DONT CHANGE THE ORDER OF THE STAR PREFAB PLEASE PLEASE PLEASE I AM BEGGING YOU 
            ThreeStar.transform.GetComponent<Image>().color = new Color(0.9882354f, 0.6941177f, 0.2313726f, 1);//orange: 0.9882354f 0.6941177f 0.2313726f
            twoStar.transform.GetComponent<Image>().color = new Color(0.9882354f, 0.6941177f, 0.2313726f, 1);//orange: 0.9882354f 0.6941177f 0.2313726f
            yield return new WaitForSeconds(0.25f);
            createStarVFX(twoStar);
            yield return new WaitForSeconds(0.25f);
            createStarVFX(ThreeStar);
        }
        
    }
    public void hideHunt()
    {
        //only work if all stars has gone to the end
        huntScene.SetActive(false);


        oneStar.SetActive(false);
        twoStar.SetActive(false);
        ThreeStar.SetActive(false);


        timeEarnedText.text = "";
        twoStarTimeText.text = "";
        threeStarTimeText.text = "";

        //communicate with map manager to display the map
        MapManager.instance.nodeActivityDone(goToGarageScreenOnComplete, 0);

    }

}

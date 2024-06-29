using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public enum GameMod {
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    [Header("Set in Inspector")]
    public Text             uitlevel;
    public Text             uitShots;
    public Text             uitButton;
    public Vector3          castlePos;
    public GameObject[]       castles;

    [Header("Set Dynamically")]
    public int          level;
    public int          levelMax;
    public int          shotsTaken;
    public GameObject   castle;
    public GameMod      mode = GameMod.idle;
    public string       showing =  "Show Slingshot";

    void Start() {
        S = this;
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel() {
        if (castle != null) { Destroy(castle); }
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos) {
            Destroy(pTemp);
        }

        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMod.playing;
    }

    void UpdateGUI() {
        uitlevel.text = "Level: " + (level+1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update() {
        UpdateGUI();

        if ((mode == GameMod.playing) && Goal.goalMet) {
            mode = GameMod.levelEnd;
            SwitchView("Show both");
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel() {
        level++;
        if (level == levelMax) {
            level = 0;
        }
        StartLevel();
    }

    public void SwitchView(string eView = "") {
        if (eView == "") { eView = uitButton.text; }
        showing = eView;
        switch (showing) {
            case "Show Slinghot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slinshot";
                break;
        }
    }

    public static void ShotFired() {
        S.shotsTaken ++;
    }
}

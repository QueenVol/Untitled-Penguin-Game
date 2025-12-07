using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public PlayerController player;

    void Start()
    {
        LoadPlayer();
    }
    public void SavePlayer()
    {
        PlayerPrefs.SetInt("isLeg", player.isLeg ? 1 : 0);
        PlayerPrefs.SetInt("isWing", player.isWing ? 1 : 0);

        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.transform.position.z);

        PlayerPrefs.SetFloat("jumpSpeed", player.jumpSpeed);

        PlayerPrefs.SetInt("penguinActive", player.penguin.activeSelf ? 1 : 0);

        PlayerPrefs.SetInt("musicTrigger1", player.musicTrigger == null ? 0 : 1);
        PlayerPrefs.SetInt("musicTrigger2", player.musicTrigger2 == null ? 0 : 1);
        PlayerPrefs.SetInt("musicTrigger3", player.musicTrigger3 == null ? 0 : 1);

        PlayerPrefs.Save();

        Debug.Log("Game Saved!");
    }
    public void LoadPlayer()
    {
        if (!PlayerPrefs.HasKey("isLeg"))
            return;

        player.isLeg = PlayerPrefs.GetInt("isLeg") == 1;
        player.leg1.SetActive(player.isLeg);
        player.leg2.SetActive(player.isLeg);

        player.isWing = PlayerPrefs.GetInt("isWing") == 1;
        player.wing1.SetActive(player.isWing);
        player.wing2.SetActive(player.isWing);

        player.jumpSpeed = PlayerPrefs.GetFloat("jumpSpeed", player.jumpSpeed);

        float x = PlayerPrefs.GetFloat("PlayerX", player.transform.position.x);
        float y = PlayerPrefs.GetFloat("PlayerY", player.transform.position.y);
        float z = PlayerPrefs.GetFloat("PlayerZ", player.transform.position.z);
        player.transform.position = new Vector3(x, y, z);

        bool penguinActive = PlayerPrefs.GetInt("penguinActive", 1) == 1;
        player.penguin.SetActive(penguinActive);

        if (PlayerPrefs.GetInt("musicTrigger1", 1) == 0 && player.musicTrigger != null)
            Destroy(player.musicTrigger);

        if (PlayerPrefs.GetInt("musicTrigger2", 1) == 0 && player.musicTrigger2 != null)
            Destroy(player.musicTrigger2);

        if (PlayerPrefs.GetInt("musicTrigger3", 1) == 0 && player.musicTrigger3 != null)
            Destroy(player.musicTrigger3);

        Debug.Log("Game Loaded!");
    }
}

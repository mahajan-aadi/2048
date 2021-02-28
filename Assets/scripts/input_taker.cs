using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum direction
{
    right,left,up,down
};

public class input_taker : MonoBehaviour
{
    public GameObject gameover;
    public Text gameoverscoretext;
    private game_manager movement;
    public Text highscore;
    void Awake()
    {
        movement = GameObject.FindObjectOfType<game_manager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        highscore.text = PlayerPrefs.GetInt("highscore").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        bool move = movement.check_move();
        if (move &&movement.game==game_manager.gamestate.playing)
        {
            get_input();
        }
        if(!move)
        {
            movement.game = game_manager.gamestate.not_playing;
            gameoverscoretext.text = movement.scoretext.text;
            gameover.SetActive(true);
            int scoretext;
            int highscoretext;
            int.TryParse(movement.scoretext.text,out scoretext);
            int.TryParse(highscore.text, out highscoretext);
            if (scoretext > highscoretext)
            {
                PlayerPrefs.SetInt("highscore", scoretext);
            }
        }
    }
    void get_input()
    {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                StartCoroutine(movement.move(direction.right));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
               StartCoroutine(movement.move(direction.left));
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
               StartCoroutine(movement.move(direction.up));
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
               StartCoroutine(movement.move(direction.down));
            };
        }
    
}


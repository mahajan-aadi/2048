using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class game_manager : MonoBehaviour
{
    public enum gamestate { playing,not_playing}
    private List<tile_behaviour> empty_list = new List<tile_behaviour>();
    private tile_behaviour [,] all_list = new tile_behaviour[4, 4];
    private List<tile_behaviour[]> column = new List<tile_behaviour[]>();
    private List<tile_behaviour[]> row = new List<tile_behaviour[]>();
    private int score=0;
    public Text scoretext;
    public GameObject gamewon;
    public GameObject screen;
    private bool move_available;
    public gamestate game;
    private bool[] all_directions = new bool[4] { true, true, true, true };
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {   
        tile_behaviour[] tilelist = GameObject.FindObjectsOfType<tile_behaviour>();
        foreach(tile_behaviour i in tilelist)
        {
            i.number = 0;
            all_list[i.row, i.column] = i;
            empty_list.Add(i);
        }
        for (int i = 0; i < 4; i++)
        {
            column.Add(new tile_behaviour[] { all_list[0, i], all_list[1, i], all_list[2, i], all_list[3, i] });
        }
        for (int i = 0; i < 4; i++)
        {
            row.Add(new tile_behaviour[] { all_list[i, 0], all_list[i, 1], all_list[i, 2], all_list[i, 3] });
        }
        update_score();
        generate_empty();
        generate_empty();
    }
    public void new_game()
    {
        Application.LoadLevel(Application.loadedLevel);
        game = gamestate.playing;
    }
    void update_score()
    {
        scoretext.text = score.ToString();
    }
    bool move_up(tile_behaviour[] list)
    {
        for(int i = 0; i < list.Length-1; i++)
        {

            if (list[i+1].number!=0 && list[i].number == 0)
            {

                list[i].number = list[i+1].number;
                list[i+1].number = 0;

                return true;
            }
            if (list[i].number == list[i + 1].number && !list[i].merged && !list[i + 1].merged && list[i].number != 0)
            {
                list[i].number *= 2;
                list[i+1].number = 0;
                list[i].merged = true;
                score += list[i].number;
                update_score();
                list[i].merge_animation();
                return true;
            }
        }
        return false;
    }
    bool move_down(tile_behaviour[] list)
    {
        for (int i = list.Length-1; i > 0; i--)
        {
            if (list[i].number == 0 && list[i - 1].number != 0)
            {
                list[i].number = list[i-1].number;
                list[i-1].number = 0;
                return true;
            }
            if (list[i].number == list[i - 1].number && !list[i].merged && !list[i - 1].merged && list[i].number != 0)
            {
                list[i].number *= 2;
                list[i-1].number = 0;
                list[i].merged = true;
                score += list[i].number;
                update_score();
                list[i].merge_animation();
                return true;
            }
        }
        return false;
    }
    void merge_correct()
    {
        empty_list.Clear();
        foreach(tile_behaviour t in all_list)
        {
            t.merged = false;
            if (t.number == 0)
            {
                empty_list.Add(t);
            }
            if (t.number == 2048 && count==0)
            {
                game = gamestate.not_playing;
                screen.SetActive(false);
                gamewon.SetActive(true);
                count += 1;
            }
        }
    }
    public void won()
    {
        game = gamestate.playing;
        screen.SetActive(true);
        gamewon.SetActive(false);
    }
    public void generate_empty()
    {
        if (empty_list.Count > 0)
        {
            int new_position = Random.Range(0,empty_list.Count);
            int prob = Random.Range(0, 10);
            int num = 2;
            if (prob == 9)
            {
                num = 4;
            }
            empty_list[new_position].number = num;
            empty_list[new_position].create_animation();
            empty_list.RemoveAt(new_position);
        }
    }
    public bool check_move()
    {
        if (empty_list.Count > 0)
        {
            return true;
        }
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (all_list[i, j].number == all_list[i + 1, j].number)
                {
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (all_list[i, j].number == all_list[i, j+1].number)
                {
                    return true;
                }
            }
        }
       
        return false;

    }
    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            generate_empty();
        }
    }*/
    IEnumerator coroutine_move_up(tile_behaviour[] line,int i)
    {
        all_directions[i] = false;
        while (move_up(line))
        {
            move_available = true;
            yield return new WaitForSeconds(0.03f);
        }
        all_directions[i] = true;
    }
    IEnumerator coroutine_move_down(tile_behaviour[] line, int i)
    {
        all_directions[i] = false;
        while (move_down(line))
        {
            move_available = true;
            yield return new WaitForSeconds(0.03f);
        }
        all_directions[i] = true;
    }
    public IEnumerator move(direction movement)
    {
        game = gamestate.not_playing;
        move_available = false;
        for (int i = 0; i < 4; i++)
        {
            switch (movement)
            {
                case direction.up:
                    StartCoroutine(coroutine_move_up(column[i], i));
                    break;
                case direction.down:
                    StartCoroutine(coroutine_move_down(column[i], i));
                    break;
                case direction.right:
                    StartCoroutine(coroutine_move_down(row[i], i));
                    break;
                case direction.left:
                    StartCoroutine(coroutine_move_up(row[i], i));
                    break;
            }
        }
        while (!(all_directions[0]&&all_directions[1]&&all_directions[2]&&all_directions[3])) { yield return null; }
        game = gamestate.playing;
        if (move_available)
        {
            merge_correct();
            generate_empty();
        }

        StopAllCoroutines();
    }
}

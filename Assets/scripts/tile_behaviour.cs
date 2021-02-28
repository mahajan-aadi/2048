using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class tile_behaviour : MonoBehaviour
{
    public bool merged = false;
    public int row;
    public int column;
    public int number
    {
        get
        {
            return Number;
        }
        set
        {
            Number = value;
            if (number == 0){
                set_empty();
            }
            else
            {
                ApplyStyles(number);
                set_visible();
            }
        }
    }
    private int Number;
    private Text text_number;
    private Image image;
    private Animator anim;
    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        image = transform.Find("numbered_tile").GetComponent<Image>();
        text_number = GetComponentInChildren<Text>();
    }
    public void create_animation()
    {
        anim.SetTrigger("create_apply");
    }
    public void merge_animation()
    {
        anim.SetTrigger("merge_apply");
    }
    public void ApplyStyles(int num)
    {
        int index = (int)math.log2(num) - 1;
        if (index <= 10)
        {
            text_number.text = tile_style.ins.tilestyles[index].number.ToString();
            text_number.color = tile_style.ins.tilestyles[index].textcolor;
            image.color = tile_style.ins.tilestyles[index].tilecolor;
        }
        else
        {
            text_number.text = number.ToString();
            text_number.color = tile_style.ins.tilestyles[11].textcolor;
            image.color = tile_style.ins.tilestyles[11].tilecolor;
        }
    }
    private void set_visible()
    {
        image.enabled = true;
        text_number.enabled = true;
    }
    private void set_empty()
    {
        image.enabled = false;
        text_number.enabled = false;
    }
    void Start()
    {
        
    }

}

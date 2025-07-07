using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class ButtonSelectUI : MonoBehaviour
{
    public Button[] buttons;
    public int selectedButton = 0;
    public float firstKeyDelay;
    private float timeForDelay;

    // Start is called before the first frame update
    void Start()
    {
        selectedButton = -1;
        timeForDelay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeForDelay += Time.deltaTime;
        if (timeForDelay < firstKeyDelay)
        {
            return;
        }
        if (Input.GetButtonDown("Attack"))
        {
            if (selectedButton < 0)
            {
                selectedButton = 0;
                ButtonHighlight(selectedButton);
            }
            else if (selectedButton < buttons.Length)
            {
                buttons[selectedButton].onClick.Invoke();
            }
        }
        if(Input.GetButtonDown("LR") || Input.GetButtonDown("Middle"))
        {
            if (selectedButton < 0)
            {
                selectedButton = 0;
                ButtonHighlight(selectedButton);
            }
            else if (Input.GetAxisRaw("LR") > 0 || Input.GetKeyDown(KeyCode.S))
            {
                selectedButton++;
                if (selectedButton >= buttons.Length)
                {
                    selectedButton = 0;
                }
                ButtonHighlight(selectedButton);
            }
            else if(Input.GetAxisRaw("LR") < 0 || Input.GetKeyDown(KeyCode.W))
            {
                selectedButton--;
                if (selectedButton < 0)
                {
                    selectedButton = buttons.Length - 1;
                }
                ButtonHighlight(selectedButton);
            }
        }
    }

    private void ButtonHighlight(int _index)
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.InGameButtonChange);
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == _index)
            {
                buttons[i].transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                buttons[i].image.color = new Color(1f, 1f, 1f);
            }
            else
            {
                buttons[i].transform.localScale = new Vector3(1, 1, 1);
                buttons[i].image.color = new Color(0.75f, 0.75f, 0.75f);
            }
        }
    }
}

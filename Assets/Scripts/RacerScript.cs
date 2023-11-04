using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RacerScript : MonoBehaviour
{
    [SerializeField] private Text SpeedText;

    public float laptime = 0;
    public float besttime = 0;
    private bool startTimer = false;

    private bool checkpoint1 = false;
    public bool checkpoint2 = false;
    public UnityEngine.UI.Text Ltime;
    public UnityEngine.UI.Text Btime;



    void Update()
    {
        if(startTimer == true)
        {
            laptime =  laptime + Time.deltaTime;

            Ltime.text = "Time: " + laptime.ToString("F2");
        }
        SpeedText.text = (m_Rigidbody.Velocity.Magnitude * 2.23693629f).ToString("0")+ (" MPH");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "StartFinish")
        {
           if(startTimer == false)
            {
                startTimer = true;
                laptime = 0;
                checkpoint1 = false;
                checkpoint2 = false;
            }

            if (checkpoint1 == true && checkpoint2 == true)
            {
                startTimer = false;
                
                if(besttime == 0)
                {
                    besttime = laptime;
                }
                if(laptime < besttime)
                {
                    besttime = laptime;
                }

                Btime.text = "Best: " + besttime.ToString("F2");

            }
            
        }

        if(other.gameObject.name == "Checkpoint1")
        {
            checkpoint1 = true;
        }

        if (other.gameObject.name == "Checkpoint2")
        {
            checkpoint2 = true;
        }
    }
}

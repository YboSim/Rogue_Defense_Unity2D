﻿using PathCreation;
using UnityEngine;


// Moves along a path at constant speed.
// Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
public class PathFollower : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float speed = 3;
    float distanceTravelled;

    void Start()
    {
        endOfPathInstruction = EndOfPathInstruction.Stop;

        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
        }
    }

    private void OnEnable()
    {
        // 속도 조정
        if (this.gameObject.name.Contains("01") == true)
        {
            Rogue_1_Ctrl a_RogueCtrl = GetComponent<Rogue_1_Ctrl>();
            speed = a_RogueCtrl.m_MvSpeed;

        }
        else if (this.gameObject.name.Contains("02") == true)
        {
            Rogue_2_Ctrl a_RogueCtrl = GetComponent<Rogue_2_Ctrl>();
            speed = a_RogueCtrl.m_MvSpeed;
        }
        else if (this.gameObject.name.Contains("03") == true)
        {
            Rogue_3_Ctrl a_RogueCtrl = GetComponent<Rogue_3_Ctrl>();
            speed = a_RogueCtrl.m_MvSpeed;
        }
        else if (this.gameObject.name.Contains("04") == true)
        {
            Rogue_4_Ctrl a_RogueCtrl = GetComponent<Rogue_4_Ctrl>();
            speed = a_RogueCtrl.m_MvSpeed;
        }
        else if (this.gameObject.name.Contains("MB") == true)
        {
            Rogue_MB_Ctrl a_RogueCtrl = GetComponent<Rogue_MB_Ctrl>();
            speed = a_RogueCtrl.m_MvSpeed;
        }
        else if (this.gameObject.name.Contains("FB") == true)
        {
            Rogue_FB_Ctrl a_RogueCtrl = GetComponent<Rogue_FB_Ctrl>();
            speed = a_RogueCtrl.m_MvSpeed;
        }
        // 속도 조정
    }

    void Update()
    {
        if (pathCreator != null)
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        }
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}

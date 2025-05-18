
using System;
using System.Collections;
using System.Collections.Generic;
using br.com.bonus630.thefrog.Manager;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
namespace br.com.bonus630.thefrog.Caracters
{
    public class NPCWoman : NPCBase, INPC
    {
        [SerializeField] GameObject[] EntraceList;
        [SerializeField] MusicSource musicSource;
        [SerializeField] MazeBuilder mazeBuilder;
        [SerializeField] int mazeSteps = 5;
        List<int> mazeDirections = null;
        string path = string.Empty;
        public void CheckInitialDialogue(int dialogue)
        {
        }
        
        public override Transform GetTransform()
        {
            return transform;
        }

        public override void Interact()
        {
        }
       
        public override void SetFinishDialogue()
        {
            dialogueCounter = 0;
            musicSource.Play(BackgroundMusic.Ignition, true);
            mazeBuilder.ActiveEntrace();
            
        }
        public override Dictionary<string, string> GetDialogueVariables()
        {
            if (mazeDirections == null)
            {
                mazeDirections = new List<int>();
                for (int i = 0; i < mazeSteps; i++)
                {

                    mazeDirections.Add(UnityEngine.Random.Range(0, 4));
                    path += ((MazeDirections)mazeDirections[i]).ToString() + ", ";
                }
                mazeBuilder.CorrectPath = mazeDirections;
            }  


            return new Dictionary<string, string>() { { "{entrada}", path } };
        }
    }
}


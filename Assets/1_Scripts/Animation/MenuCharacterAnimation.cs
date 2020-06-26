using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCharacterAnimation : MonoBehaviour
{
    [SerializeField] private CharacterNames characterName;
    [SerializeField] private Animator animationController;

    public CharacterNames CharacterName => characterName;

    private void Start()
    {
        if (animationController)
        {
            switch (characterName)
            {
                case CharacterNames.Russell:
                    animationController.SetTrigger("Russell Idle");
                    break;
                case CharacterNames.Jojo:
                    animationController.SetTrigger("Jojo Idle");
                    break;
                case CharacterNames.Kiki:
                    animationController.SetTrigger("Kiki Idle");
                    break;
                case CharacterNames.Momo:
                    animationController.SetTrigger("Momo Idle");
                    break;
                case CharacterNames.None:
                default:
                    break;
            }
        }
    }

    public void Dance()
    {
        if (animationController)
        {
            switch (characterName)
            {
                case CharacterNames.Russell:
                    animationController.SetTrigger("Russell Dance");
                    break;
                case CharacterNames.Jojo:
                    animationController.SetTrigger("Jojo Dance");
                    break;
                case CharacterNames.Kiki:
                    animationController.SetTrigger("Kiki Dance");
                    break;
                case CharacterNames.Momo:
                    animationController.SetTrigger("Momo Dance");
                    break;
                case CharacterNames.None:
                default:
                    break;
            }
        }
    }
}

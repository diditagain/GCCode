using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName ="Story Text")]
public class TextScriptableObject : ScriptableObject
{
    [TextArea(20,40)]
    public string Text;
    public AudioClip VoiceNarration;
    
}


using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "MusicConfig", menuName = "Data/MusicConfig", order = 0)]
    public sealed class MusicConfig : ScriptableObject
    {
        public AudioClip ButtonsSound;
        public AudioClip QuestStartSound;
        public AudioClip QuestListSound;
        public AudioClip QuestdoneSound;
        public AudioClip WinGameSound;
        public AudioClip LoseGameSound;
        public AudioClip MainThemeSound;
        public AudioClip PickUpSound;
        public AudioClip GETRewardSound;
        public AudioClip BuldSound;


        [SerializeField] private float _volume = 0.1f;

        public float Volume => _volume;
    }
}

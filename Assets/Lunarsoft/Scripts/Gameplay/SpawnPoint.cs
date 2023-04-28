using UnityEngine;

namespace Lunarsoft
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private string doorID;

        public string DoorID
        {
            get { return doorID; }
        }
    }
}

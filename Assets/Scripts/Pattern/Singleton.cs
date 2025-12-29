using UnityEngine;
namespace DesignPattern
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>, new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindFirstObjectByType<T>();
                }
                return instance;
            }
        }

        void OnDestroy()
        {
            instance = null;
        }
    }
}

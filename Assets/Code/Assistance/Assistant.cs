using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Assistance
{
    public static class Assistant
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            var result = gameObject.GetComponent<T>();
            if (!result)
            {
                result = gameObject.AddComponent<T>();
            }

            return result;
        }
        
        public static Vector3 Change(this Vector3 org, object x = null, object y = null, object z = null)
        {
            return new Vector3(x == null ? org.x : (float) x, y == null ? org.y : (float) y,
                z == null ? org.z : (float) z);
        }
        
        public static T Load<T>(string resourcesPath) where T : Object =>
            Resources.Load<T>(Path.ChangeExtension(resourcesPath, null));
        
        public static T[] LoadAll<T>(string resourcesPath) where T : Object =>
            Resources.LoadAll<T>(Path.ChangeExtension(resourcesPath, null));
    }
}

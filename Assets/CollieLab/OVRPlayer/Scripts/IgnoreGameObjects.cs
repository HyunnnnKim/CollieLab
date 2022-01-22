using System.Collections.Generic;
using UnityEngine;

namespace CollieLab.Assets
{
    public class IgnoreGameObjects : MonoBehaviour
    {
        #region Serialize Field
        [SerializeField] private List<GameObject> ignoreList = new List<GameObject>();
        public List<GameObject> IgnoreList
        {
            get
            {
                InitIgnoreList();
                return ignoreList;
            }
        }
        #endregion

        #region Private Field
        private GameObject[] ignoreArray = null;
        private Collider[] colliders = null;
        public Collider[] Colliders
        {
            get
            {
                InitIgnoreList();
                InitColliders();
                return colliders;
            }
        }
        #endregion

        #region Initialize
        private void InitIgnoreList()
        {
            if (ignoreArray != null) return;

            ignoreArray = ignoreList.ToArray();
        }

        private void InitColliders()
        {
            if (colliders != null) return;

            List<Collider> ignoreColliders = new List<Collider>();
            if (ignoreArray != null)
            {
                for (int i = 0; i < ignoreArray.Length; i++)
                    ignoreColliders.Add(ignoreArray[i].GetComponent<Collider>());
            }
            colliders = ignoreColliders.ToArray();
        }
        #endregion

        #region Check Functions
        /// <summary>
        /// Check if the gameObject is in the ignore list.
        /// </summary>
        public bool Has(GameObject gameObject)
        {
            if (ignoreArray == null) return false;

            bool result = false;
            for (int i = 0; i < ignoreArray.Length; i++)
            {
                if (gameObject == ignoreArray[i])
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        #endregion
    }
}

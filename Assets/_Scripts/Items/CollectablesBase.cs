using br.com.bonus630.thefrog.Manager;
using UnityEditor;
using UnityEngine;
namespace br.com.bonus630.thefrog.Items
{
    public abstract class CollectablesBase : MonoBehaviour
    {
        [SerializeField] private int amount;
        [SerializeField] protected string itemID;

        public int Amount { get { return amount; } }

        private void Awake()
        {
        }

        protected virtual void Start()
        {
            gameObject.SetActive(!GameManager.Instance.IsCollected(itemID));

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }
        protected void Reset()
        {
            GenID();
        }
        protected void OnValidate()
        {
            if (string.IsNullOrEmpty(itemID))
                GenID();
        }
        protected void GenID()
        {

#if UNITY_EDITOR
            // Cria um ID baseado no nome e um GUID curto
            itemID = gameObject.name + "_" + System.Guid.NewGuid().ToString().Substring(0, 8);

            // Marca a cena como suja (precisa salvar depois)
            EditorUtility.SetDirty(this);
#endif

        }

    }
}
 
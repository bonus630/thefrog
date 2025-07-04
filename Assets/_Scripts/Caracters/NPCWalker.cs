using UnityEngine;

namespace br.com.bonus630.thefrog.Caracters
{
    [Tooltip("A generic walker NPC")]
    public class NpcWalker : NPCBase, INPC
    {
        [SerializeField] float leftDistance = 0f;
        [SerializeField] float rightDistance = 0f;
        [SerializeField] float speed = 10f;
        [SerializeField] bool isWalker = false;
        bool walking = false;
        Vector3 initialPosition;
        Vector3 leftPosition;
        Vector3 rightPosition;
        int direction = 1;
        float timeToInteract = 2f;
        float lastInteractTime = 0f;
        bool interactTimeRunning = false;
        Animator m_Animator;
        private readonly int Walking = Animator.StringToHash("Walking");
        private void Start()
        {
            walking = isWalker;
            initialPosition = transform.position;
            leftPosition = new Vector3(initialPosition.x - leftDistance, initialPosition.y);
            rightPosition = new Vector3(initialPosition.x + rightDistance, initialPosition.y);
            m_Animator = GetComponent<Animator>();
            m_Animator.SetBool(Walking, walking);
        }
      
        protected override void Update()
        {
            
            if (!walking)
                return;
            if (interactTimeRunning)
            {
                lastInteractTime += Time.deltaTime;
                if(lastInteractTime >  timeToInteract)
                {
                    lastInteractTime = 0f;
                    interactTimeRunning = false;
                }
            }
            if(direction == 1)
            {
              // transform.position = Vector3.Lerp(initialPosition, rightPosition, time);
               transform.position = Vector3.MoveTowards(transform.position, rightPosition, speed * Time.deltaTime);
               if(transform.position == rightPosition)
                {
                    direction = -1;
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
                }
            }
            if (direction == -1)
            {
                // transform.position = Vector3.Lerp(initialPosition, rightPosition, time);
                transform.position = Vector3.MoveTowards(transform.position, leftPosition, speed * Time.deltaTime);
                if (transform.position == leftPosition)
                {
                    direction = 1;
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
                }
            }

        }
        public override bool ReadyToInteract(bool lookFor)
        {
            Debug.Log("walker interaction");
            bool result = lookFor && playerTriggerEnter;
            TalkIcon.SetActive(result);
            if (isWalker && !(lastInteractTime > 0))
            {
                walking = !result;
                m_Animator.SetBool(Walking, walking);
                interactTimeRunning = true;
            }
            return result;
        }

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
            walking = true;
            m_Animator.SetBool(Walking, walking);
        }
    }
}

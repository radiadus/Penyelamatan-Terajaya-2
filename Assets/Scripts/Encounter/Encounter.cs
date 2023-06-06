using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Encounter : MonoBehaviour
{
    private float[][] characterPositions = new float[][]
    {
        new float[] {0f, 0f},
        new float[] {3f, 0f},
        new float[] {-3f, 0f}
    };
    private float[][] enemyPositions = new float[][]
    {
        new float[] {0f, 5f},
        new float[] {3f, 5f},
        new float[] {-3f, 5f},
        new float[] {1f, 7f},
        new float[] {-1f, 7f}
    };

    [SerializeField] private Button attack, item, flee;
    public GameObject textBox;
    private GameObject skillChoice;
    private GameObject itemChoice;
    public TextMeshProUGUI textBoxText;
    public GameObject[] friendlyPrefabs;
    public GameObject[] enemyPrefabs;
    private Friendly[] friendlies;
    private Enemy[] enemies;


    private enum BattleState
    {
        PLAYER_TURN,
        ATTACK_PHASE,
        STATUS_CHECK
    }
    private enum ActionType
    {
        SKILL,
        ITEM,
        FLEE
    }

    private class Action
    {
        public ActionType type;
        public delegate void Function(CombatUnit user, List<CombatUnit> targets);
        public Function func;
        public CombatUnit user;
        public List<CombatUnit> targets;
        public int fleeRoll;
        public bool FleeSuccesful()
        {
            if (fleeRoll > 191) return true;
            return false;
        }

        public Action(CombatUnit caster, Skill skill, List<CombatUnit> targets)
        {
            this.type = ActionType.SKILL;
            this.func = skill.Cast;
            this.user = caster;
            this.targets = targets;
        }

        public Action(CombatUnit user, Consumable consumable, List<CombatUnit> targets)
        {
            this.type = ActionType.ITEM;
            this.func = consumable.Use;
            this.user = user;
            this.targets = targets;
        }

        public Action(Enemy enemy, List<CombatUnit> targets)
        {
            this.type = ActionType.SKILL;
            this.func = enemy.Attack;
            this.user = enemy;
            this.targets = targets;
        }

        public Action(Friendly friendly)
        {
            this.type = ActionType.FLEE;
            this.user = friendly;
            this.fleeRoll = Random.Range(0, 256);
        }

        public Action(Friendly friendly, bool failed)
        {
            this.type = ActionType.SKILL;
            this.user = friendly;
            this.targets = new List<CombatUnit>();
            this.func = delegate { };
        }
    }

    private class Sorter : IComparer<Action>
    {
        public int Compare(Action x, Action y)
        {
            int speedX = x.user.speed;
            int speedY = y.user.speed;
            return speedY.CompareTo(speedX);
        }
    }

    private BattleState state;
    private Sorter sorter;
    private List<Action> actions;
    private int characterTurn;
    private int enemyRemaining;
    private int friendlyRemaining;
    [SerializeField] private GameObject canvas, attackPanel, itemPanel;

    // Start is called before the first frame update
    void Start()
    {
        skillChoice = Resources.Load<GameObject>("Prefab/UI/Encounter/Attack");
        itemChoice = Resources.Load<GameObject>("Prefab/UI/Encounter/Item");

        state = BattleState.PLAYER_TURN;
        characterTurn = 0;
        enemyRemaining = enemies.Count();
        friendlyRemaining = friendlies.Count(f => !f.IsDead());

        attack.onClick.AddListener(delegate { OpenAttackPanel(); });
        item.onClick.AddListener(delegate { OpenItemPanel(); });
        flee.onClick.AddListener(delegate { Flee(); });

    }

    public void FetchFriendlies()
    {
        friendlyPrefabs = new GameObject[]
        {
            Resources.Load<GameObject>("Prefab/Character rig and animations/Assassin"),
            Resources.Load<GameObject>("Prefab/Character rig and animations/Mage"),
            Resources.Load<GameObject>("Prefab/Character rig and animations/Swordsman")
        };
    }

    public void StartEncounter()
    {
        foreach (Friendly friendly in friendlies)
        {
            if (friendly.IsDead()) friendly.PlayDeadAnimation();
        }
        StartCoroutine(PlayerTurn());
        StartCoroutine(AttackPhase());
        StartCoroutine(StatusCheck());
    }

    IEnumerator PlayerTurn()
    {
        while(enemyRemaining > 0)
        {
            while (state != BattleState.PLAYER_TURN) yield return null;
            while (characterTurn < 3) yield return null;
            state = BattleState.ATTACK_PHASE;
            characterTurn = 0;
            yield return null;
        }
    }

    IEnumerator AttackPhase()
    {
        while(enemyRemaining > 0)
        {
            while (state != BattleState.ATTACK_PHASE) yield return null;
            RandomizeEnemyAction();
            actions.Sort(sorter);
            foreach (Action action in actions)
            {
                action.func(action.user, action.targets);
            }
            actions.Clear();
            yield return null;
        }
    }

    IEnumerator StatusCheck()
    {
        while (enemyRemaining > 0)
        {
            while (state != BattleState.STATUS_CHECK) yield return null;
            foreach (Friendly friendly in friendlies)
            {
                foreach(StatusEffect effect in friendly.statusEffectList)
                {

                }
            }
            foreach (Enemy enemy in enemies)
            {
                foreach(StatusEffect effect in enemy.statusEffectList)
                {

                }
            }
            state = BattleState.PLAYER_TURN;
            yield return null;
        }
    }

    public void RandomizeEnemyAction()
    {
        List<CombatUnit> targets = friendlies.Where<CombatUnit>(f => !f.IsDead()).ToList();
        foreach (Enemy enemy in enemies)
        {
            actions.Add(new Action(enemy, targets));
        }
    }

    public void StartTurn()
    {
        canvas.SetActive(true);
        InstantiatePanels(friendlies[0]);
    }

    public void InstantiateClasses()
    {
        this.friendlies = new Friendly[]
        {
            friendlyPrefabs[0].GetComponent<Friendly>(),
            friendlyPrefabs[1].GetComponent<Friendly>(),
            friendlyPrefabs[2].GetComponent<Friendly>()
        };

        this.enemies = this.enemyPrefabs.Select(e => e.GetComponent<Enemy>()).ToArray();
    }

    public void SpawnPositions()
    {
        for (int i = 0; i < 3; i++)
        {
            float[] position = characterPositions[i];
            Vector3 spawnVect = new Vector3(position[0], 0, position[1]);
            Debug.Log(position.ToString());
            Instantiate(friendlyPrefabs[i], spawnVect, Quaternion.identity);
        }
        int enemySize = enemyPrefabs.Length;
        for (int i = 0; i < enemySize; i++)
        {
            float[] position = enemyPositions[i];
            Vector3 spawnVect = new Vector3(position[0], 0, position[1]);
            Debug.Log(position.ToString());
            Instantiate(enemyPrefabs[i], spawnVect, Quaternion.Euler(0, 180, 0));
        }
    }
    
    private void InstantiatePanels(Friendly friendly)
    {
        List<Skill> skills = friendly.stats.skillList;
        int x = 0;
        int y = 0;
        for (int i = 0; i < skills.Count; i++)
        {
            x = i / 2 * 150;
            y = i % 2 * 150;
            Vector2 position = new Vector2(x, y);
            RectTransform rtf = skillChoice.GetComponent<RectTransform>();
            rtf.position = position;
            skillChoice.GetComponent<Button>().onClick.AddListener(delegate { ChooseTarget(skills[i]); });
            Instantiate(skillChoice, attackPanel.transform);
        }

    }

    void ChooseTarget(Skill skill)
    {
        switch (skill.target)
        {
            case Skill.Target.ALLY:
                break;
            case Skill.Target.ENEMY:
                break;
            case Skill.Target.ALL_ALLY:
                break;
            case Skill.Target.ALL_ENEMY:
                break;
            case Skill.Target.NONE:
                break;
            default:
                break;
        }
    }

    void UseSkill(Skill skill, List<CombatUnit> target)
    {
        actions.Add(new Action(friendlies[characterTurn], skill, target));
    }

    private void OpenBattlePanel(Friendly character)
    {
        canvas.SetActive(true);
        attackPanel.SetActive(false);
        itemPanel.SetActive(false);
    }

    private void OpenAttackPanel()
    {
        attackPanel.SetActive(true);
        itemPanel.SetActive(false);
    }

    private void OpenItemPanel()
    {
        itemPanel.SetActive(true);
        attackPanel.SetActive(false);
    }

    private void Flee()
    {
        actions.Add(new Action(friendlies[characterTurn]));
    }
}

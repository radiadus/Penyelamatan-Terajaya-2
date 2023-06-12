using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

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
        new float[] {-3f, 5f},
        new float[] {0f, 5f},
        new float[] {3f, 5f},
        new float[] {-1f, 7f},
        new float[] {1f, 7f}
    };

    public Button attack, item, flee, undoFriendly, undoEnemy, friendlyConfirm, enemyConfirm;
    public Button[] attackDifficulty, answerButton, enemyButton, friendlyButton;
    public Slider[] hpBar, mpBar;
    public GameObject textBox, battleOptions, skillChoice, itemChoice, itemPanel, friendlySelect, questionCanvas, enemySelect;
    public GameObject[] attackPanel;
    public GameObject[] friendlyTarget, enemyTarget;
    public TextMeshProUGUI textBoxText, questionText;
    public TextMeshProUGUI[] answerText;
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
        FLEE,
        ENEMY
    }

    private class Action
    {
        public ActionType type;
        public delegate int Function(CombatUnit user, List<CombatUnit> targets);
        public Function func;
        public CombatUnit user;
        public List<CombatUnit> targets;
        public string actionName;
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
            this.actionName = skill.skillName;
            this.user = caster;
            this.targets = targets;
        }

        public Action(CombatUnit user, Consumable consumable, List<CombatUnit> targets)
        {
            this.type = ActionType.ITEM;
            this.func = consumable.Use;
            this.actionName = consumable.itemName;
            this.user = user;
            this.targets = targets;
        }

        public Action(Enemy enemy, List<CombatUnit> targets)
        {
            this.type = ActionType.ENEMY;
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
    private Sorter sorter = new Sorter();
    private List<Action> actions = new List<Action>();
    private int characterTurn;
    private int enemyRemaining;
    private int friendlyRemaining;

    // Start is called before the first frame update
    void Start()
    {
        FetchFriendlies();
        SpawnPositionsAndInstantiateClasses();

        state = BattleState.PLAYER_TURN;
        characterTurn = 0;
        enemyRemaining = enemies.Count();

        attack.onClick.AddListener(delegate { OpenAttackPanel(); });
        item.onClick.AddListener(delegate { OpenItemPanel(); });
        flee.onClick.AddListener(delegate { Flee(); });

        undoFriendly.onClick.AddListener(delegate { UndoFriendlySelect(); });
        undoEnemy.onClick.AddListener(delegate { UndoEnemySelect(); });

        for(int i = 0; i < 3; i++)
        {
            int index = i;
            attackDifficulty[index].onClick.AddListener(delegate { Debug.Log(index); SwitchAttackPanel(index); });
        }

        for(int i = enemyRemaining; i < 5; i++)
        {
            enemyButton[i].gameObject.SetActive(false);
        }
        StartCoroutine(StartEncounter());
    }

    public void FetchFriendlies()
    {
        friendlyPrefabs = new GameObject[]
        {
            Resources.Load<GameObject>("Prefab/Character rig and animations/Mage"),
            Resources.Load<GameObject>("Prefab/Character rig and animations/Swordsman"),
            Resources.Load<GameObject>("Prefab/Character rig and animations/Assassin")
        };
    }

    IEnumerator StartEncounter()
    {
        yield return new WaitForSecondsRealtime(1);
        foreach (Friendly friendly in friendlies)
        {
            if (friendly.IsDead()) friendly.PlayDeadAnimation();
        }
        StartTurn(characterTurn);
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
            Debug.Log(actions.Count);
            RandomizeEnemyAction();
            actions.Sort(sorter);
            Debug.Log(actions.Count);
            foreach (Action action in actions)
            {
                Debug.Log(action.user.name);
                int number = action.func(action.user, action.targets);
                if (number != -2)
                {
                    textBox.SetActive(true);
                    string text = action.user.name;
                    if (action.type == ActionType.ENEMY)
                    {
                        text += " menyerang ";
                        if (((Enemy)(action.user)).attackType == Enemy.AttackType.SINGLE) text += friendlies[((Enemy)action.user).attackTarget].name + "!";
                        else text += "semua karakter!";
                    }
                    else
                    {
                        text += " menggunakan " + action.actionName + "!";
                    }
                    if (number != -1) text += " (" + number + " total kerusakan)";
                    textBoxText.text = text;
                    UpdateHPMPBar(0);
                    UpdateHPMPBar(1);
                    UpdateHPMPBar(2);
                    yield return new WaitForSecondsRealtime(action.user.animator.GetCurrentAnimatorStateInfo(0).length);
                }
            }
            textBox.SetActive(false);
            actions.Clear();
            state = BattleState.STATUS_CHECK;
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
                    effect.DecreaseTurn();
                    if (effect.remainingTurn == 0) friendly.statusEffectList.Remove(effect);
                }
            }
            foreach (Enemy enemy in enemies)
            {
                foreach(StatusEffect effect in enemy.statusEffectList)
                {
                    effect.DecreaseTurn();
                    if (effect.remainingTurn == 0) enemy.statusEffectList.Remove(effect);
                }
            }
            state = BattleState.PLAYER_TURN;
            StartTurn(characterTurn);

            yield return null;
        }
    }

    public void RandomizeEnemyAction()
    {
        List<CombatUnit> targets = friendlies.ToList<CombatUnit>();
        foreach (Enemy enemy in enemies)
        {
            actions.Add(new Action(enemy, targets));
        }
    }

    public void StartTurn(int turn)
    {
        if (characterTurn > 2)
            return;
        if (friendlies[turn].IsDead())
        {
            characterTurn++;
            StartTurn(characterTurn);
            return;
        }
        InstantiatePanels(friendlies[turn]);
        OpenBattlePanel(friendlies[turn]);
    }

    public void SpawnPositionsAndInstantiateClasses()
    {
        friendlies = new Friendly[3];
        for (int i = 0; i < 3; i++)
        {
            float[] position = characterPositions[i];
            Vector3 spawnVect = new Vector3(position[0], 0, position[1]);
            GameObject friendly = Instantiate(friendlyPrefabs[i], spawnVect, Quaternion.identity);
            friendlies[i] = friendly.GetComponent<Friendly>();
            friendlies[i].InitializeStats();
            hpBar[i].maxValue = friendlies[i].maxHP;
            hpBar[i].value = friendlies[i].HP;
            mpBar[i].maxValue = friendlies[i].maxMP;
            mpBar[i].value = friendlies[i].MP;
        }
        int enemySize = enemyPrefabs.Length;
        enemies = new Enemy[enemySize];
        for (int i = 0; i < enemySize; i++)
        {
            float[] position = enemyPositions[i];
            Vector3 spawnVect = new Vector3(position[0], 0, position[1]);
            GameObject enemy = Instantiate(enemyPrefabs[i], spawnVect, Quaternion.Euler(0, 180, 0));
            enemies[i] = enemy.GetComponent<Enemy>().InitializeStats();
        }
    }
    
    private void InstantiatePanels(Friendly friendly)
    {
        foreach(GameObject panel in attackPanel)
        {
            AttackButton[] attacks = panel.GetComponentsInChildren<AttackButton>();
            foreach (AttackButton att in attacks)
            {
                att.DestroySelf();
            }
        }
        List<Skill> skills = friendly.stats.skillList;
        int x = 0;
        int y = 0;
        int[] count = new int[] { 0, 0, 0};
        for (int i = 0; i < skills.Count; i++)
        {
            int index = i;
            Skill skill = skills[index];
            int difficulty = skill.difficulty;
            x = count[difficulty-1] % 2 * 250;
            y = count[difficulty-1] / 2 * -100;
            count[difficulty-1]++;
            Vector2 position = new Vector2(x, y);
            RectTransform rtf = skillChoice.GetComponent<RectTransform>();
            rtf.anchoredPosition = new Vector2(0, 0);
            rtf.anchoredPosition = position;
            GameObject obj = Instantiate(skillChoice, attackPanel[difficulty-1].transform);
            AttackButton script = obj.GetComponent<AttackButton>();
            script.skill = skill;
            script.InitializeText();
            Button button = obj.GetComponent<Button>();
            button.onClick.AddListener(delegate { if (friendly.MP >= skill.mpCost) ChooseTarget(skill); });
        }

    }

    void UpdateHPMPBar(int index)
    {
        hpBar[index].value = friendlies[index].HP;
        mpBar[index].value = friendlies[index].MP;
    }

    void ChooseTarget(Skill skill)
    {
        List<CombatUnit> target = new List<CombatUnit>();
        switch (skill.target)
        {
            case Skill.Target.ALLY:
                OpenFriendlySelect();
                InitializeFriendlyListener(skill, false, false);
                break;
            case Skill.Target.ENEMY:
                OpenEnemySelect();
                InitializeEnemyListener(skill, false);
                break;
            case Skill.Target.ALL_ALLY:
                OpenFriendlySelect();
                InitializeFriendlyListener(skill, true, false);
                break;
            case Skill.Target.ALL_ENEMY:
                OpenEnemySelect();
                InitializeEnemyListener(skill, true);
                break;
            case Skill.Target.SELF:
                OpenFriendlySelect();
                InitializeFriendlyListener(skill, false, true);
                break;
            default:
                break;
        }
    }
    void InitializeFriendlyListener(Skill skill, bool all, bool self)
    {
        bool single = !all && !self;
        for (int i = 0; i < friendlies.Length; i++)
        {
            int index = i;
            friendlyButton[index].onClick.RemoveAllListeners();
            if (single)
            {
                friendlyButton[index].onClick.AddListener(delegate
                {
                    ResetFriendlyChoice();
                    friendlyTarget[index].SetActive(true);
                });
            }
        }
        if (all)
        {
            for (int j = 0; j < 3; j++)
            {
                friendlyTarget[j].SetActive(true);
            }
        }
        else if (self)
        {
            friendlyTarget[characterTurn].SetActive(true);
        }
        friendlyConfirm.onClick.RemoveAllListeners();
        friendlyConfirm.onClick.AddListener(delegate { OpenQuestionPanel(skill, GetSelectedFriendly()); });
    }
    void InitializeFriendlyListener(Item item)
    {

    }

    List<CombatUnit> GetSelectedFriendly()
    {
        List<CombatUnit> target = new List<CombatUnit>();
        for (int i = 0; i < 3; i++)
        {
            if (friendlyTarget[i].gameObject.activeSelf) target.Add(friendlies[i]);
        }
        return target;
    }
    List<CombatUnit> GetSelectedEnemy()
    {
        List<CombatUnit> target = new List<CombatUnit>();
        for(int i = 0; i < 5; i++)
        {
            if (enemyTarget[i].gameObject.activeSelf) target.Add(enemies[i]);
        }
        return target;
    }
    void InitializeEnemyListener(Skill skill, bool all)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            int index = i;
            enemyButton[index].onClick.RemoveAllListeners();
            if (!all)
            {
                enemyButton[index].onClick.AddListener(delegate 
                {
                    ResetEnemyChoice();
                    enemyTarget[index].SetActive(true);
                });
            }
        }
        if (all)
        {
            for (int j = 0; j < enemies.Length; j++)
            {
                Debug.Log(j);
                Debug.Log(enemies[j]);
                if (!enemies[j].IsDead())
                    enemyTarget[j].SetActive(true);
            }
        }
        enemyConfirm.onClick.RemoveAllListeners();
        enemyConfirm.onClick.AddListener(delegate { Debug.Log(GetSelectedEnemy().Count()); if (GetSelectedEnemy().Count() != 0) OpenQuestionPanel(skill, GetSelectedEnemy()); });
    }
    void ResetEnemyChoice()
    {
        for (int j = 0; j < 5; j++)
        {
            enemyTarget[j].SetActive(false);
        }
    }
    void ResetFriendlyChoice()
    {
        for (int j = 0; j < 3; j++)
        {
            friendlyTarget[j].SetActive(false);
        }
    }
    void OpenQuestionPanel(Skill skill, List<CombatUnit> target)
    {
        questionCanvas.SetActive(true);
        battleOptions.SetActive(false);
        friendlySelect.SetActive(false);
        enemySelect.SetActive(false);
        SetQuestion(skill, target);
    }
    void SetQuestion(Skill skill, List<CombatUnit> target)
    {
        Question question = QuestionReader.Instance.GetQuestionByDifficulty(skill.difficulty);
        if (question != null)
        {
            questionText.text = question.question;
            foreach(Button button in answerButton)
            {
                button.gameObject.SetActive(false);
            }
            for(int i = 0; i < question.answerCount; i++)
            {
                int index = i;
                answerButton[index].gameObject.SetActive(true);
                answerText[index].text = question.answer[i];
                answerButton[index].onClick.RemoveAllListeners();
                answerButton[index].onClick.AddListener(delegate { AnswerQuestion(skill, target, question, index); });
            }
        }
    }
    void AnswerQuestion(Skill skill, List<CombatUnit> target, Question question, int answer)
    {
        bool correct = answer == question.key - 'A';
        if (correct)
        {
            actions.Add(new Action(friendlies[characterTurn], skill, target));
        }
        questionCanvas.SetActive(false);
        characterTurn++;
        StartTurn(characterTurn);
    }
    void OpenFriendlySelect()
    {
        friendlySelect.SetActive(true);
        battleOptions.SetActive(false);
        ResetFriendlyChoice();
    }
    void OpenEnemySelect()
    {
        enemySelect.SetActive(true);
        battleOptions.SetActive(false);
        ResetEnemyChoice();
    }
    void UndoEnemySelect()
    {
        enemySelect.SetActive(false);
        battleOptions.SetActive(true);
    }
    void UndoFriendlySelect()
    {
        friendlySelect.SetActive(false);
        battleOptions.SetActive(true);
    }
    void UseSkill(Skill skill, List<CombatUnit> target)
    {
        actions.Add(new Action(friendlies[characterTurn], skill, target));
    }
    private void OpenBattlePanel(Friendly character)
    {
        battleOptions.SetActive(true);
        foreach(GameObject panel in attackPanel)
        {
            panel.SetActive(false);
        }
        foreach(Button button in attackDifficulty)
        {
            button.gameObject.SetActive(false);
        }
        itemPanel.SetActive(false);
    }
    private void OpenAttackPanel()
    {
        foreach(Button button in attackDifficulty)
        {
            button.gameObject.SetActive(true);
        }
        attackPanel[0].SetActive(true);
        itemPanel.SetActive(false);
    }
    private void SwitchAttackPanel(int difficulty)
    {
        for(int i = 0; i < 3; i++)
        {
            attackPanel[i].SetActive(i == difficulty ? true : false);
        }
    }
    private void OpenItemPanel()
    {
        itemPanel.SetActive(true);
        foreach (GameObject panel in attackPanel)
        {
            panel.SetActive(false);
        }
        foreach (Button button in attackDifficulty)
        {
            button.gameObject.SetActive(false);
        }
    }
    private void Flee()
    {
        actions.Add(new Action(friendlies[characterTurn]));
    }
}

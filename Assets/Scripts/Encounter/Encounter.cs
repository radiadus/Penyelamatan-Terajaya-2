using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

using Random = UnityEngine.Random;
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

    public Button attack, item, flee, undoFriendly, undoEnemy, friendlyConfirm, enemyConfirm, winButton, hpPotion, mpPotion;
    public Button[] attackDifficulty, answerButton, enemyButton, friendlyButton;
    public Slider[] hpBar, mpBar;
    public GameObject textBox, battleOptions, skillChoice, itemChoice, itemPanel, friendlySelect, questionCanvas, correctImage, incorrectImage, enemySelect, winPanel;
    public GameObject[] attackPanel;
    public GameObject[] friendlyTarget, enemyTarget;
    public TextMeshProUGUI textBoxText, questionText, goldText, hpPotionRemainingText, mpPotionRemainingText;
    public TextMeshProUGUI[] answerText, levelText, expText;
    public GameObject[] friendlyPrefabs;
    public GameObject[] enemyPrefabs;
    private List<GameObject> instantiatedEffects = new List<GameObject>();
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
        public AudioClip clip;

        public string actionName;
        public int fleeRoll, priority;
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
            this.priority = skill.priority;
            this.clip = skill.clip;
        }

        public Action(CombatUnit user, Consumable consumable, List<CombatUnit> targets)
        {
            this.type = ActionType.ITEM;
            this.func = consumable.Use;
            this.actionName = consumable.itemName;
            this.user = user;
            this.targets = targets;
            this.priority = 0;
            this.clip = consumable.clip;
        }

        public Action(Enemy enemy, List<CombatUnit> targets)
        {
            this.type = ActionType.ENEMY;
            this.func = enemy.Attack;
            this.user = enemy;
            this.targets = targets;
            this.priority = 0;
            this.clip = Enemy.clip;
        }

        public Action(Friendly friendly)
        {
            this.type = ActionType.FLEE;
            this.user = friendly;
            this.targets = new List<CombatUnit>();
            this.fleeRoll = Random.Range(0, 256);
            this.priority = 0;
            this.func = delegate (CombatUnit unit, List<CombatUnit> targets)
            {
                if (this.fleeRoll > 0) return 1;
                return 0;
            };
        }

    }

    private class Sorter : IComparer<Action>
    {
        public int Compare(Action x, Action y)
        {
            int priority = y.priority.CompareTo(x.priority);
            if (priority == 0)
            {
                int speedX = x.user.speed;
                int speedY = y.user.speed;
                return speedY.CompareTo(speedX);
            }
            return priority;
        }
    }

    private BattleState state;
    private Sorter sorter = new Sorter();
    private List<Action> actions = new List<Action>();
    private int characterTurn;
    private int enemyRemaining;
    private int friendlyRemaining;
    private int hpPotionRemaining, mpPotionRemaining;
    private ItemInstance hpPotions, mpPotions;

    // Start is called before the first frame update
    void Start()
    {
        FetchFriendlies();
        SpawnPositionsAndInstantiateClasses();

        state = BattleState.PLAYER_TURN;
        characterTurn = 0;
        enemyRemaining = enemies.Count();
        friendlyRemaining = friendlies.ToList().FindAll(f => !f.IsDead()).Count();

        attack.onClick.AddListener(delegate { OpenAttackPanel(); });
        item.onClick.AddListener(delegate { OpenItemPanel(); });
        flee.onClick.AddListener(delegate { Flee(); });

        hpPotions = GameManager.Instance.inventory.FindItemInstance(typeof(Jamu));
        mpPotions = GameManager.Instance.inventory.FindItemInstance(typeof(JamuEnergi));

        hpPotionRemaining = hpPotions != null ? hpPotions.quantity : 0;
        mpPotionRemaining = mpPotions != null ? mpPotions.quantity : 0;

        hpPotion.onClick.AddListener(delegate 
        {
            if (hpPotionRemaining > 0)
            {
                OpenFriendlySelect();
                InitializeFriendlyListener((Consumable)hpPotions.item);
            }
        });
        mpPotion.onClick.AddListener(delegate
        {
            if (mpPotionRemaining > 0)
            {
                OpenFriendlySelect();
                InitializeFriendlyListener((Consumable)mpPotions.item);
            }
        });


        undoFriendly.onClick.AddListener(delegate { UndoFriendlySelect(); textBox.SetActive(false); });
        undoEnemy.onClick.AddListener(delegate { UndoEnemySelect(); textBox.SetActive(false); });

        for(int i = 0; i < 3; i++)
        {
            int index = i;
            attackDifficulty[index].onClick.AddListener(delegate { SwitchAttackPanel(index); });
        }

        for(int i = enemyRemaining; i < 5; i++)
        {
            enemyButton[i].gameObject.SetActive(false);
        }
        StartCoroutine(StartEncounter());
        Debug.Log(friendlies[1].name + " " + friendlies[1].statusEffectList.Count);
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
        Debug.Log(friendlies[1].name + " " + friendlies[1].statusEffectList.Count);
        yield return new WaitForSecondsRealtime(1);
        Debug.Log(friendlies[1].name + " " + friendlies[1].statusEffectList.Count);
        foreach (Friendly friendly in friendlies)
        {
            if (friendly.IsDead()) friendly.PlayDeadAnimation();
        }
        Debug.Log(friendlies[1].name + " " + friendlies[1].statusEffectList.Count);
        StartTurn(characterTurn);
        StartCoroutine(PlayerTurn());
        StartCoroutine(AttackPhase());
        StartCoroutine(StatusCheck());
    }

    IEnumerator PlayerTurn()
    {
        while(enemyRemaining > 0 && friendlyRemaining > 0)
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
        while(enemyRemaining > 0 && friendlyRemaining > 0)
        {
            while (state != BattleState.ATTACK_PHASE) yield return null;
            RandomizeEnemyAction();
            actions.Sort(sorter);
            foreach (Action action in actions)
            {
                if (!action.user.IsDead())
                {
                    Debug.Log(action.user.name);
                    int number = action.func(action.user, action.targets);
                    if (action.type == ActionType.FLEE)
                    {
                        textBox.SetActive(true);
                        textBoxText.text = action.user.name + " mencoba untuk kabur!";
                        yield return new WaitForSecondsRealtime(2);
                        if (number == 1)
                        {
                            textBoxText.text = "Percobaan kabur berhasil!";
                            EncounterManager.Instance.FleeEncounter();
                            yield break;
                        }
                        else
                        {
                            textBoxText.text = "Percobaan kabur gagal!";
                            yield return new WaitForSecondsRealtime(2);
                        }
                    }
                    else if (number != -2)
                    {
                        textBox.SetActive(true);
                        string text = action.user.name;
                        if (action.type == ActionType.ENEMY)
                        {
                            text += " menyerang ";
                            if (((Enemy)(action.user)).attackType == Enemy.AttackType.SINGLE) text += ((Enemy)action.user).attackTarget + "!";
                            else text += "semua karakter!";
                        }
                        else
                        {
                            text += " menggunakan " + action.actionName + "!";
                        }
                        if (number == -3) text += " Meleset!";
                        else if (number != -1) text += " (" + number + " total poin kesehatan)";
                        textBoxText.text = text;
                        UpdateHPMPBar(0);
                        UpdateHPMPBar(1);
                        UpdateHPMPBar(2);
                        UpdateRemainingUnits();
                        yield return new WaitForSecondsRealtime(action.user.animator.GetCurrentAnimatorStateInfo(0).length + 1);
                        if (enemyRemaining == 0 || friendlyRemaining == 0)
                        {
                            BattleStatus();
                            textBox.SetActive(false);
                            yield break;
                        }
                    }
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
        while (enemyRemaining > 0 && friendlyRemaining > 0)
        {
            while (state != BattleState.STATUS_CHECK) yield return null;
            foreach (Friendly friendly in friendlies)
            {
                for (int i = friendly.statusEffectList.Count-1; i > -1; i--)
                {
                    StatusEffect effect = friendly.statusEffectList[i];
                    int number = effect.DecreaseTurn();
                    UpdateHPMPBar(0);
                    UpdateHPMPBar(1);
                    UpdateHPMPBar(2);
                    UpdateRemainingUnits();
                    if (number != -1)
                    {
                        textBox.SetActive(true);
                        string text = friendly.name;
                        if(number == -2)
                        {
                            text += " ditaklukkan oleh efek sakratul maut!";
                        }
                        else
                        {
                            text += " tersakiti oleh ";
                            if (effect.GetType() == typeof(Poison)) text += "racun!";
                            else if (effect.GetType() == typeof(Burn)) text += "luka bakar!";
                            text += " (" + number + " poin kesehatan)";
                        }
                        textBoxText.text = text;
                        yield return new WaitForSecondsRealtime(2);
                    }
                    if (enemyRemaining == 0 || friendlyRemaining == 0)
                    {
                        BattleStatus();
                        textBox.SetActive(false);
                        yield break;
                    }
                    if (friendly.IsDead())
                    {
                        break;
                    }
                }
            }
            foreach (Enemy enemy in enemies)
            {
                for(int i = enemy.statusEffectList.Count-1; i > -1; i--)
                {
                    StatusEffect effect = enemy.statusEffectList[i];
                    Debug.Log(effect.GetType().Name);
                    int number = effect.DecreaseTurn();
                    UpdateRemainingUnits();
                    if (number != -1)
                    {
                        textBox.SetActive(true);
                        string text = enemy.name;
                        if (number == -2)
                        {
                            text += " ditaklukkan oleh efek sakratul maut!";
                        }
                        else
                        {
                            text += " tersakiti oleh ";
                            if (effect.GetType() == typeof(Poison)) text += "racun!";
                            else if (effect.GetType() == typeof(Burn)) text += "luka bakar!";
                            text += " (" + number + " poin kesehatan)";
                        }
                        textBoxText.text = text;
                        yield return new WaitForSecondsRealtime(2);
                    }
                    if (enemyRemaining == 0 || friendlyRemaining == 0)
                    {
                        BattleStatus();
                        textBox.SetActive(false);
                        yield break;
                    }
                    if (enemy.IsDead())
                    {
                        break;
                    }
                }
            }
            state = BattleState.PLAYER_TURN;
            textBox.SetActive(false);
            StartTurn(characterTurn);

            yield return null;
        }
    }

    void BattleStatus()
    {
        if (enemyRemaining == 0)
        {
            int gold = 0;
            int exp = 0;
            enemies.ToList().ForEach(e => { gold += e.goldGain; exp += e.expGain; });
            List<Friendly> alive = friendlies.ToList().FindAll(f => !f.IsDead());
            List<Friendly> dead = friendlies.ToList().FindAll(f => f.IsDead());
            foreach (Friendly friendly in alive)
            {
                friendly.GainExp(exp);
            }
            winPanel.SetActive(true);
            winButton.onClick.AddListener(delegate { EncounterManager.Instance.WinEncounter(); });
            for (int i = 0; i < 3; i++)
            {
                levelText[i].text = friendlies[i].stats.level.ToString();
                string text = "Poin pengalaman + ";
                if (friendlies[i].IsDead()) text += "0 Poin";
                else
                {
                    text += exp + " Poin" + (friendlies[i].stats.exp < exp ? "\nNaik level!" : "");
                }
                expText[i].text = text;
            }
            goldText.text = "Rp " + gold;
            GameManager.Instance.inventory.money += gold;
            foreach(Friendly friendly in dead)
            {
                friendly.HP = 1;
            }
            foreach (Friendly friendly in friendlies)
            {
                friendly.SetStats();
            }
        }
        if (friendlyRemaining == 0)
        {
            foreach (Friendly friendly in friendlies)
            {
                friendly.HP = (int)(0.25f * friendly.maxHP);
                friendly.SetStats();
            }
            EncounterManager.Instance.LoseEncounter();
        }
    }

    public void UpdateRemainingUnits()
    {
        enemyRemaining = enemies.ToList().FindAll(e => !e.IsDead()).Count();
        friendlyRemaining = friendlies.ToList().FindAll(f => !f.IsDead()).Count();
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
        Debug.Log(friendlies[1].name + " " + friendlies[1].statusEffectList.Count);
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
        foreach(GameObject effect in instantiatedEffects)
        {
            Destroy(effect);
        }
        instantiatedEffects.Clear();
        foreach(GameObject panel in attackPanel)
        {
            AttackButton[] attacks = panel.GetComponentsInChildren<AttackButton>();
            foreach (AttackButton att in attacks)
            {
                att.DestroySelf();
            }
        }
        List<Skill> skills = friendly.stats.skillList;
        float x = 0;
        float y = 0;
        int[] count = new int[] { 0, 0, 0 };
        for (int i = 0; i < skills.Count; i++)
        {
            int index = i;
            Skill skill = skills[index];
            int difficulty = skill.difficulty;
            x = count[difficulty-1] % 2 * 485.8f + 273.2f;
            y = count[difficulty-1] / 2 * -115 - 325;
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
        for (int i = 0; i < 3; i++)
        {
            if (!friendlies[i].IsDead())
            {
                List<StatusEffect> effects = friendlies[i].statusEffectList;
                for(int j = 0; j < effects.Count; j++)
                {
                    GameObject prefab = effects[j].statusPrefab;
                    x = -70 + 35 * j;
                    y = -25;
                    prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                    GameObject instantiatedEffect = Instantiate(prefab, friendlyButton[i].transform);
                    instantiatedEffects.Add(instantiatedEffect);
                }
            }
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            if (!enemies[i].IsDead())
            {
                List<StatusEffect> effects = enemies[i].statusEffectList;
                for (int j = 0; j < effects.Count; j++)
                {
                    GameObject prefab = effects[j].statusPrefab;
                    x = -70 + 35 * j;
                    y = -25;
                    prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                    GameObject instantiatedEffect = Instantiate(prefab, enemyButton[i].transform);
                    instantiatedEffects.Add(instantiatedEffect);
                }
            }
        }
    }

    void UpdateHPMPBar(int index)
    {
        hpBar[index].value = friendlies[index].HP;
        mpBar[index].value = friendlies[index].MP;
    }
    void ChooseTarget(Skill skill)
    {
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
        textBox.SetActive(true);
        textBoxText.text = skill.skillDescription;
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
        friendlyConfirm.onClick.AddListener(delegate { OpenQuestionPanel(skill, GetSelectedFriendly()); textBox.SetActive(false); });
    }
    void InitializeFriendlyListener(Consumable item)
    {
        for (int i = 0; i < friendlies.Length; i++)
        {
            int index = i;
            friendlyButton[index].onClick.RemoveAllListeners();
            friendlyButton[index].onClick.AddListener(delegate
            {
                ResetFriendlyChoice();
                friendlyTarget[index].SetActive(true);
            });
        }
        friendlyConfirm.onClick.RemoveAllListeners();
        friendlyConfirm.onClick.AddListener(delegate
        {
            actions.Add(new Action(friendlies[characterTurn], item, GetSelectedFriendly()));
            characterTurn++;
            StartTurn(characterTurn);
        });
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
            enemyButton[index].gameObject.SetActive(!enemies[index].IsDead());
        }
        if (all)
        {
            for (int j = 0; j < enemies.Length; j++)
            {
                if (!enemies[j].IsDead())
                    enemyTarget[j].SetActive(true);
            }
        }
        enemyConfirm.onClick.RemoveAllListeners();
        enemyConfirm.onClick.AddListener(delegate { if (GetSelectedEnemy().Count() != 0) OpenQuestionPanel(skill, GetSelectedEnemy()); textBox.SetActive(false); });
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
        foreach(Button button in answerButton)
        {
            button.onClick.RemoveAllListeners();
        }
        bool correct = answer == question.key - 'A';
        if (correct)
        {
            actions.Add(new Action(friendlies[characterTurn], skill, target));
        }
        StartCoroutine(ShowResult(correct));
    }
    IEnumerator ShowResult(bool correct)
    {
        if (correct) correctImage.SetActive(true);
        else incorrectImage.SetActive(true);
        for (int i = 0; i < 60; i++)
        {
            yield return null;
        }
        correctImage.SetActive(false);
        incorrectImage.SetActive(false);
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
        battleOptions.SetActive(false);
        characterTurn++;
        StartTurn(characterTurn);
    }
}

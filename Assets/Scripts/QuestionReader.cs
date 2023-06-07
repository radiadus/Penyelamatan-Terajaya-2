using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class QuestionReader : MonoBehaviour
{
    public QuestionBank questionBank;
    public static QuestionReader Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (questionBank.questions == null)
        {
            questionBank.questions = ReadQuestions();
        }
    }

    private Dictionary<QuestionCategory, List<Question>> ReadQuestions()
    {
        Dictionary<QuestionCategory, List<Question>> questions = new Dictionary<QuestionCategory, List<Question>>();
        foreach(QuestionCategory category in Enum.GetValues(typeof(QuestionCategory)))
        {
            questions.Add(category, new List<Question>());    
        }
        StreamReader file = File.OpenText("Assets/Resources/SoalTerajaya4(1).csv");
        file.ReadLine();
        string line = "";
        while ((line = file.ReadLine()) != null)
        {
            string[] data = line.Split(';');
            Question question = new Question();
            question.question = System.Text.RegularExpressions.Regex.Unescape(data[0]);
            question.answerA = data[1];
            question.answerB = data[2];
            question.answerC = data[3];
            question.answerD = data[4];
            question.key = data[5];
            question.difficulty = int.Parse(data[6]);
            switch (data[7])
            {
                case "PG2":
                    question.answerCount = 2;
                    break;
                case "PG4":
                    question.answerCount = 4;
                    break;
                default:
                    break;
            }
            switch (data[8])
            {
                case "Soal Bacaan":
                    question.category = QuestionCategory.PARAGRAPH;
                    break;
                case "Soal Sinonim/Antonim":
                    question.category = QuestionCategory.SYNONYM_ANTONYM;
                    break;
                case "Peri Bahasa":
                    question.category = QuestionCategory.PROVERB;
                    break;
                case "Huruf Kapital":
                    question.category = QuestionCategory.CAPITAL;
                    break;
                case "Tanda Baca":
                    question.category = QuestionCategory.PUNCTUATION;
                    break;
                case "Kata Baku":
                    question.category = QuestionCategory.SPELLING;
                    break;
                case "Struktur Kalimat":
                    question.category = QuestionCategory.WORD_STRUCTURE;
                    break;
                default:
                    break;
            }
            question.solved = false;
            questions[question.category].Add(question);
        }
        file.Close();
        return questions;
    }

    public Question GetQuestionByCategoryAndDifficulty(QuestionCategory category, int difficulty)
    {
        List<Question> fetched = questionBank.questions[category].FindAll(q => q.difficulty == difficulty && !q.solved);
        if (fetched.Count == 0)
        {
            ResetByCategoryAndDifficulty(category, difficulty);
            return GetQuestionByCategoryAndDifficulty(category, difficulty);
        }
        return fetched[Random.Range(0, fetched.Count)];
    }

    private void ResetByCategoryAndDifficulty(QuestionCategory category, int difficulty)
    {
        questionBank.questions[category].FindAll(q => q.difficulty == difficulty).ForEach(q => q.solved = false);
    }

}

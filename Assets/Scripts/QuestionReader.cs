using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    private Dictionary<int, List<Question>> ReadQuestions()
    {
        Dictionary<int, List<Question>> questions = new Dictionary<int, List<Question>>();
        questions.Add(1, new List<Question>());
        questions.Add(2, new List<Question>());
        questions.Add(3, new List<Question>());
        StreamReader file = File.OpenText("Assets/Resources/Soal/SoalTerajaya7.csv");
        file.ReadLine();
        string line = "";
        while ((line = file.ReadLine()) != null)
        {
            string[] data = line.Split(';');
            Question question = new Question();
            question.question = System.Text.RegularExpressions.Regex.Unescape(data[0]);
            string[] answers = new string[4];
            answers[0] = data[1];
            answers[1] = data[2];
            answers[2] = data[3];
            answers[3] = data[4];
            question.answer = answers;
            question.key = data[5][0];
            Debug.Log(data[6]);
            question.difficulty = int.Parse(data[6]);
            Debug.Log(question.difficulty);
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
            questions[question.difficulty].Add(question);
        }
        file.Close();
        return questions;
    }

    public Question GetQuestionByDifficulty(int difficulty)
    {
        List<Question> fetched = questionBank.questions[difficulty].FindAll(q => q.solved == false);
        if (fetched.Count == 0)
        {
            ResetByDifficulty(difficulty);
            return GetQuestionByDifficulty(difficulty);
        }
        return fetched[Random.Range(0, fetched.Count)];
    }

    private void ResetByDifficulty(int difficulty)
    {
        questionBank.questions[difficulty].ForEach(q => q.solved = false);
    }

}

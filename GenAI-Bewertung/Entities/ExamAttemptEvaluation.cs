﻿namespace GenAI_Bewertung.Entities;

public class ExamAttemptEvaluation
{
    public int ExamAttemptEvaluationId { get; set; }
    public int ExamAttemptId { get; set; }
    public ExamAttempt ExamAttempt { get; set; } = null!;

    public double Score { get; set; }
    public bool IsPassed { get; set; }
    public string FeedbackSummary { get; set; } = string.Empty;
    public DateTime EvaluatedAt { get; set; }
}
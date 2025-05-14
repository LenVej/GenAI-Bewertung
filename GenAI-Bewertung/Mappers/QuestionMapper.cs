using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Entities.QuestionTypes;
using GenAI_Bewertung.Enums;

namespace GenAI_Bewertung.Mappers;

public static class QuestionMapper
{
    public static QuestionDto ToDto(Question q)
    {
        var dto = new QuestionDto
        {
            QuestionId = q.QuestionId,
            QuestionText = q.QuestionText,
            QuestionType = q.QuestionType,
            Subject = q.Subject,
            CreatedBy = q.CreatedBy,
            CreatedAt = q.CreatedAt
        };

        switch (q)
        {
            case MultipleChoiceQuestion mc:
                dto.Choices = mc.Choices;
                dto.CorrectIndices = mc.CorrectIndices;
                break;
            case EitherOrQuestion eo:
                dto.OptionA = eo.OptionA;
                dto.OptionB = eo.OptionB;
                dto.CorrectAnswer = eo.CorrectAnswer;
                break;
            case OneWordQuestion ow:
                dto.ExpectedAnswer = ow.ExpectedAnswer;
                break;
            case MathQuestion m:
                dto.ExpectedResult = m.ExpectedResult;
                break;
            case EstimationQuestion est:
                dto.CorrectValue = est.CorrectValue;
                break;
            case FillInTheBlankQuestion fib:
                dto.ClozeText = fib.ClozeText;
                dto.Gaps = fib.Gaps.Select(g => new BlankGapDto
                {
                    Index = g.Index,
                    Solutions = g.Solutions
                }).ToList();
                break;

            case FreeTextQuestion ft:
                dto.ExpectedKeywords = ft.ExpectedKeywords;
                break;
        }

        return dto;
    }

    public static Question FromCreateDto(CreateQuestionDto dto, int userId)
    {
        var createdAt = DateTime.UtcNow;

        return dto.QuestionType switch
        {
            QuestionType.MultipleChoice => new MultipleChoiceQuestion
            {
                QuestionText = dto.QuestionText,
                Subject = dto.Subject,
                QuestionType = dto.QuestionType,
                CreatedBy = userId,
                CreatedAt = createdAt,
                Choices = dto.Choices ?? new(),
                CorrectIndices = dto.CorrectIndices ?? new()
            },
            QuestionType.EitherOr => new EitherOrQuestion
            {
                QuestionText = dto.QuestionText,
                Subject = dto.Subject,
                QuestionType = dto.QuestionType,
                CreatedBy = userId,
                CreatedAt = createdAt,
                OptionA = dto.OptionA ?? "",
                OptionB = dto.OptionB ?? "",
                CorrectAnswer = dto.CorrectAnswer ?? ""
            },
            QuestionType.OneWord => new OneWordQuestion
            {
                QuestionText = dto.QuestionText,
                Subject = dto.Subject,
                QuestionType = dto.QuestionType,
                CreatedBy = userId,
                CreatedAt = createdAt,
                ExpectedAnswer = dto.ExpectedAnswer ?? ""
            },
            QuestionType.Math => new MathQuestion
            {
                QuestionText = dto.QuestionText,
                Subject = dto.Subject,
                QuestionType = dto.QuestionType,
                CreatedBy = userId,
                CreatedAt = createdAt,
                ExpectedResult = dto.ExpectedResult ?? 0
            },
            QuestionType.Estimation => new EstimationQuestion
            {
                QuestionText = dto.QuestionText,
                Subject = dto.Subject,
                QuestionType = dto.QuestionType,
                CreatedBy = userId,
                CreatedAt = createdAt,
                CorrectValue = dto.CorrectValue ?? 0,
            },
            QuestionType.FillInTheBlank => new FillInTheBlankQuestion
            {
                QuestionText = dto.QuestionText,
                Subject = dto.Subject,
                QuestionType = dto.QuestionType,
                CreatedBy = userId,
                CreatedAt = createdAt,
                ClozeText = dto.ClozeText ?? "",
                Gaps = dto.Gaps?.Select(g => new Entities.QuestionTypes.BlankGap
                {
                    Index = g.Index,
                    Solutions = g.Solutions ?? new()
                }).ToList() ?? new()
            },
            QuestionType.FreeText => new FreeTextQuestion
            {
                QuestionText = dto.QuestionText,
                Subject = dto.Subject,
                QuestionType = dto.QuestionType,
                CreatedBy = userId,
                CreatedAt = createdAt,
                ExpectedKeywords = dto.ExpectedKeywords ?? ""
            },
            _ => throw new ArgumentException("Unbekannter Fragentyp")
        };
    }
    
    public static Question FromUpdateDto(UpdateQuestionDto dto, Question existing)
    {
        existing.QuestionText = dto.QuestionText;
        existing.Subject = dto.Subject;

        switch (existing)
        {
            case MultipleChoiceQuestion mc:
                mc.Choices = dto.Choices ?? new();
                mc.CorrectIndices = dto.CorrectIndices ?? new();
                break;

            case EitherOrQuestion eo:
                eo.OptionA = dto.OptionA ?? "";
                eo.OptionB = dto.OptionB ?? "";
                eo.CorrectAnswer = dto.CorrectAnswer ?? "";
                break;

            case OneWordQuestion ow:
                ow.ExpectedAnswer = dto.ExpectedAnswer ?? "";
                break;

            case MathQuestion m:
                m.ExpectedResult = dto.ExpectedResult ?? 0;
                break;

            case EstimationQuestion est:
                est.CorrectValue = dto.CorrectValue ?? 0;
                break;

            case FillInTheBlankQuestion fib:
                fib.ClozeText = dto.ClozeText ?? "";
                // Optional: alte Gaps ersetzen
                fib.Gaps = dto.Gaps?.Select(g => new BlankGap
                {
                    Index = g.Index,
                    Solutions = g.Solutions ?? new(),
                    FillInTheBlankQuestionId = existing.QuestionId
                }).ToList() ?? new();
                break;

            case FreeTextQuestion ft:
                ft.ExpectedKeywords = dto.ExpectedKeywords ?? "";
                break;
        }

        return existing;
    }

}
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
                dto.Solutions = fib.Solutions;
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
            Solutions = dto.Solutions ?? new()
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

}
import {BlankGap} from "./blank-gap.model";

export interface Question {
  questionId: number;
  questionText: string;
  questionType: string;
  subject: string;
  createdBy: number;
  createdAt: string;

  // Multiple Choice
  choices?: string[];
  correctIndices?: number[];

  // EitherOr
  optionA?: string;
  optionB?: string;
  correctAnswer?: string;

  // OneWord
  expectedAnswer?: string;

  // Math
  expectedResult?: number;

  // Estimation
  correctValue?: number;

  // FillInTheBlank
  clozeText?: string;
  gaps?: BlankGap[];

  // FreeText
  expectedKeywords?: string;
}

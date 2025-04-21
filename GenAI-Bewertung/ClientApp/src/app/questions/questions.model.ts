export interface Question {
  questionId: number;
  questionText: string;
  questionType: string;
  subject: string;
  createdBy: number;
  createdAt: string;

  // typ-spezifische Felder (alle optional)
  choices?: string[];
  correctIndices?: number[];

  optionA?: string;
  optionB?: string;
  correctAnswer?: string;

  expectedAnswer?: string;

  expectedResult?: number;

  correctValue?: number;

  clozeText?: string;
  solutions?: string[];

  expectedKeywords?: string;
}

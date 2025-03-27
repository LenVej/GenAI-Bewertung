export interface Question {
  questionId: number;
  questionText: string;
  questionType: string;
  subject: string;
  createdBy: number;
  createdAt: string; // or Date
}

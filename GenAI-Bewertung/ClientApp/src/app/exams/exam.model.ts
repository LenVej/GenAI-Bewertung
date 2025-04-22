export interface Exam {
  examId: number;
  title: string;
  description: string;
  createdBy: number;
  createdAt: string;
  timeLimitMinutes?: number;
  questions: {
    id: number;
    order: number;
    question: any; // or Question
  }[];
}

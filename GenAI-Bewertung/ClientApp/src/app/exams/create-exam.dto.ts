export interface CreateExamDto {
  title: string;
  description: string;
  timeLimitMinutes?: number;
  questionIds: number[];
}

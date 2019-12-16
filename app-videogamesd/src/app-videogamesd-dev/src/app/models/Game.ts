export class Game{
    _id?: string;
    title: string;
    score: [
        {
        date: Date;
        int_score: number;
        }
    ]
}
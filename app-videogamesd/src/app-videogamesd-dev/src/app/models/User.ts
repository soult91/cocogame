export class User {
    _id?: string;
    activeConnection: number;
    name: string;
    username: string;
    password: string;
    email: string;
    init_date: Date;
    birthdate: Date;
    discriminator: string;
    status: any;
    token: string;
    LastLogin: Date;
    games: {
        planificacion: {
            subscribed: boolean;
            bathroom:{
                _score0: {
                    score: Array<number>;
                    date: Array<Date>;
                    time: Array<number>;

                },
                _score1: {
                    score: Array<number>;
                    date: Array<Date>;
                    time: Array<number>;

                },
                _score2: {
                    score: Array<number>;
                    date: Array<Date>;
                    time: Array<number>;

                }
            },
            dress:{
                _score0: {
                    score: Array<number>;
                    date: Array<Date>;
                    time: Array<number>;

                },
                _score1: {
                    score: Array<number>;
                    date: Array<Date>;
                    time: Array<number>;

                },
                _score2: {
                    score: Array<number>;
                    date: Array<Date>;
                    time: Array<number>;

                }
            }
           
        }
    };
    total_score: number;
    n?: number;
}
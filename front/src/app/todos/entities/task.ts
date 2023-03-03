export interface ITask {
    id: string | null;
    name: string;
    status: string;
    createAt: Date;
    deadline: Date | null;
}

export class Task implements ITask {
    constructor(){}
    
    id!: string | null;
    name!: string;
    status!: string;
    createAt!: Date;
    deadline!: Date | null;
}
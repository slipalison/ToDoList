export interface ITask {
    id: string | null;
    name: string;
    status: string;
    createAt: Date;
    deadline: Date | null;

    convertDate():any;
}

export class Task implements ITask {
    constructor() { }

    id!: string | null;
    name!: string;
    status!: string;
    createAt!: Date;
    deadline!: Date | null;

    convertDate() : any {
        let date = this.deadline?.toUTCString();
        if (date)
            return new Date(date).toString();
        return this.deadline?.toString()
    }
}
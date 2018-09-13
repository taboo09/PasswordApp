export interface Password{
    id: number;
    userId: number;
    name: string;
    username: string;
    hint: string;
    comment: string;
    secretQuestion: string;
    answer: string;
    other: string;
    passwordEncrypt: string;
    created: Date;
    updated: Date;
}
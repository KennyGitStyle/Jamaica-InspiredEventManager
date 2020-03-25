export interface IEventActivity {
    id: string;
    title: string;
    description: string;
    category: string;
    date: Date;
    city: string;
    venue: string;

}

export interface IEventActivityFormValues extends Partial<IEventActivity> {
    time?: Date
}

export class EventActivityFormValues implements IEventActivityFormValues {
    id?: string = undefined;
    title: string = '';
    category: string = '';
    description: string = '';
    date?: Date = undefined;
    time?: Date = undefined;
    city: string = '';
    venue: string = '';

    /**
     *
     */
    constructor(init?: EventActivityFormValues ) {
        if(init && init.date){
            init.time = init.date
        }
        Object.assign(this, init)
    }
}
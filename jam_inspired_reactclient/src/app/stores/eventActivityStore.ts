import {observable, action, computed, runInAction} from 'mobx';
import { SyntheticEvent} from 'react';
import {IEventActivity} from '../models/event-activity';
import agent from '../api/agent';
import { history } from '../..';
import { toast } from 'react-toastify';
import {RootStore} from './rootStore'
import { setActivityProps, createAttendee } from '../common/util/Util';



export default class EventActivityStore {
    rootStore: RootStore
    constructor(rootStore: RootStore) {
        this.rootStore = rootStore
    }

    @observable eventActivityRegistry = new Map();
    @observable activity: IEventActivity | null = null;
    @observable loadingInitial = false;
    @observable submitting = false;
    @observable target= '';
    @observable loading = false;

    @computed get eventActivitiesByDate() {
        return this.groupEventActivitiesByDate(Array.from(this.eventActivityRegistry.values()))

    }

    groupEventActivitiesByDate = (events: IEventActivity[]) => {
        const sortedEvents = events.sort(
            (a, b)=> a.date.getTime() - b.date.getTime()
        )
        return Object.entries(sortedEvents.reduce((events, event) => {
            const date = event.date.toISOString().split('T')[0]
            events[date] = events[date] ? [...events[date], event] : [event];
            return events
        }, {} as {[key: string]: IEventActivity[]}));
    }

    @action loadEventActivities = async () => {
        this.loadingInitial = true;
        try {
            const eventActivities = await agent.EventActivities.list();
            runInAction('loading event activities', () => {
                eventActivities.forEach((event) => {
                    setActivityProps(event, this.rootStore.userStore.user!)
                    this.eventActivityRegistry.set(event.id, event);
                  });
                  this.loadingInitial = false;
            });
        } catch (err) {
            runInAction('loading activities error', ()=> {
                this.loadingInitial = false;
            })
            console.log(err);
        }
    }

    @action loadEventActivity = async (id: string) => {
        let eventActivity = this.getEventActivity(id);
        if(eventActivity){
            this.activity = eventActivity;
            return eventActivity;
        } else {
            this.loadingInitial = true;
            try {
                eventActivity = await agent.EventActivities.details(id);
                runInAction('loading event', () => {
                    setActivityProps(eventActivity, this.rootStore.userStore.user!)
                    this.activity = eventActivity;
                    this.eventActivityRegistry.set(eventActivity.id, eventActivity);
                    this.loadingInitial = false;
                })
                return eventActivity;
            } catch (er) {
                runInAction('catching event error', () => {
                    this.loadingInitial = false;
                })
                console.log(er);
            }
        }
    }

    @action clearEventActivity = () => {
        this.activity = null
    }
    getEventActivity(id: string) {
        return this.eventActivityRegistry.get(id);
    }

    @action createEventActivity = async (event: IEventActivity) => {
        this.submitting = true;
        try {
            await agent.EventActivities.create(event);
            const attendee = createAttendee(this.rootStore.userStore.user!)
            attendee.isHost = true
            let attendees = []
            attendees.push(attendee)
            event.attendees = attendees
            event.isHost = true
            runInAction('creating event activity', () => {
                this.eventActivityRegistry.set(event.id, event);
                this.submitting = false;
            })
            history.push(`/events/${event.id}`)
        } catch (err) {
            runInAction('catching error creating activity', () => {
                this.submitting = true;
            })
            toast.error('Problem sumitting data');
            console.log(err.response);
        }
    }

    @action editEventActivity = async (event: IEventActivity) => {
        this.submitting = true
        try {
           await agent.EventActivities.update(event);
           runInAction('editing event activity', () => {
            this.eventActivityRegistry.set(event.id, event);
            this.activity = event;
            this.submitting = false;
           })
           history.push(`/events/${event.id}`)
        }catch (err) {
            runInAction('catch error editing activity', () => {
                this.submitting = false;
            })
            toast.error('Problem submitting data');
            console.log(err.response)
        }
    }

    @action deleteEventActivity = async (event: SyntheticEvent<HTMLButtonElement>, id: string) => {
        this.submitting = true;
        this.target = event.currentTarget.name;
        try {
            await agent.EventActivities.delete(id);
            runInAction('deleting event activity', () => {
                this.eventActivityRegistry.delete(id);
                this.submitting =false;
                this.target = '';
            })
        }catch (err) {
            runInAction('catching delete error event activity', () => {
                this.submitting = false;
                this.target = '';
            })
            
            console.log(err)
        }
        
    }

    @action attendEventActivity = async () => {
        const attendee = createAttendee(this.rootStore.userStore.user!)
        this.loading = true
        try {
            await agent.EventActivities.attend(this.activity!.id)
            runInAction(() => {
                if(this.activity) {
                    this.activity.attendees.push(attendee)
                    this.activity.isGoing = true
                    this.eventActivityRegistry.set(this.activity.id, this.activity);
                    this.loading = false
                 }
            })
        }catch (error) {
            runInAction(() => {
                this.loading = false
            })
            toast.error("Problem signing up to  event")
        }
    }

    @action cancelAttendance = async () => {
        this.loading = true
        try {
            await agent.EventActivities.unattend(this.activity!.id)
            runInAction(() => {
                if(this.activity) {
                    this.activity.attendees = this.activity.attendees.filter(x => 
                        x.username !== this.rootStore.userStore.user!.username)
                    this.activity.isGoing = false
                    this.eventActivityRegistry.set(this.activity.id, this.activity)
                    this.loading = false
                }
            })
            
        } catch (error) {
            runInAction(() => {
                this.loading = false
            })
            toast.error("Problem cancelling attendance")
        }
        
    }

}


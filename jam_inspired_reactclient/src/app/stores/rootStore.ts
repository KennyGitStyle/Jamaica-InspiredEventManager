import EventActivityStore from './eventActivityStore'
import UserStore from './userStore'
import { createContext } from 'react'
import { configure } from 'mobx';
import CommonStore from './commonStore'
import Modalstore from './modalStore'

configure({enforceActions: 'always'});

export class RootStore {
    eventActivityStore: EventActivityStore
    commonStore: CommonStore
    userStore: UserStore
    modalStore: Modalstore

    constructor() {
        this.eventActivityStore = new EventActivityStore(this)
        this.userStore = new UserStore(this)
        this.userStore = new UserStore(this)
        this.commonStore = new CommonStore(this)
        this.modalStore = new Modalstore(this)
        
    }
    
}

export const RootStoreContext = createContext(new RootStore())
import { IEventActivity, IEventAttendee } from "../../models/event-activity"
import { IUser } from "../../models/user"

export const combineDateAndTime = (date: Date, time: Date) => {
    const timeString = time.getHours() + ':' + time.getMinutes() + ':00' 
    
    const year = date.getFullYear()
    const month = date.getMonth() + 1
    const day = date.getDate()

    const dateString = `${year}-${month}-${day }`

    return new Date(dateString + ' ' + timeString)
}

export const setActivityProps = (event: IEventActivity, user: IUser) => {
    event.date = new Date(event.date)
    event.isGoing = event.attendees.some(
        a => a.username === user.username
    )
    event.isHost = event.attendees.some(
        a => a.username === user.username && a.isHost
    )

    return event
}

export const createAttendee = (user: IUser): IEventAttendee => {
    return {
        displayName: user.displayName,
        isHost: false,
        username: user.username,
        image: user.image!
        
    }
}
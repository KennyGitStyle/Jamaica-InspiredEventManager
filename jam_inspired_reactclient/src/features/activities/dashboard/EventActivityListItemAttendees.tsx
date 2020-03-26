import React from 'react'
import { List, Image, Popup} from 'semantic-ui-react'
import { IEventAttendee } from '../../../app/models/event-activity'

interface IProps {
    attendees: IEventAttendee[]
}

const EventActivityListItemAttendees: React.FC<IProps> = ({ attendees }) => {
    return (
        <List horizontal>
            {attendees.map((attendee) => (
                <List.Item key={attendee.username}>
                    <Popup header={attendee.displayName} trigger={<Image size='mini' rounded src={attendee.image || '/assets/user.png'} />}/>
                </List.Item>
            ))}
        </List>
    )
}

export default EventActivityListItemAttendees

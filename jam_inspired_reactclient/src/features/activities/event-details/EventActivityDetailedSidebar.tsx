import React, { Fragment } from 'react';
import { Segment, List, Item, Label, Image } from 'semantic-ui-react';
import { Link } from 'react-router-dom';
import { IEventAttendee } from '../../../app/models/event-activity';
import { observer } from 'mobx-react-lite';

interface IProps {
    attendees: IEventAttendee[]
}

const EventActivityDetailedSidebar: React.FC<IProps> = ({ attendees }) => {

    return (
        <Fragment>
            <Segment
                textAlign='center'
                style={{ border: 'none' }}
                attached='top'
                inverted
                color='black'>
                {attendees.length} {attendees.length === 1 ? 'Person' : 'People'} going
              </Segment>
            <Segment attached>
                <List relaxed divided>
                    {attendees.map((attendee) => (
                        <Item key={attendee.username} style={{ position: 'relative' }}>
                            {attendee.isHost &&
                                <Label
                                    style={{ position: 'absolute' }}
                                    color='yellow'
                                    ribbon='right'>
                                    Host
                                </Label>}
                            <Image size='tiny' src={attendee.image || '/assets/user.png'} />
                            <Item.Content verticalAlign='middle'>
                                <Item.Header as='h3'>
                                    <Link to={`/profile/${attendee.username}`}>{attendee.displayName}</Link>
                                </Item.Header>
                                <Item.Extra style={{ color: 'green' }}>Following</Item.Extra>
                            </Item.Content>
                        </Item>
                    ))}
                </List>
            </Segment>
        </Fragment>
    )
}

export default observer(EventActivityDetailedSidebar);
